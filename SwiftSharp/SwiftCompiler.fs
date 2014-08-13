module SwiftSharp.SwiftCompiler

// #r "../Lib/IKVM.Reflection.dll";;

open System
open System.Text
open System.Collections.Generic

open SwiftParser

open IKVM.Reflection
open IKVM.Reflection.Emit

type Config =
    {
        InputUrls: string list
        OutputPath: string
        CorlibPath: string
    }

type ClrType = IKVM.Reflection.Type
        
type Env (config) =
    let dir = System.IO.Path.GetDirectoryName (config.OutputPath)
    let name = new AssemblyName (System.IO.Path.GetFileNameWithoutExtension (config.OutputPath))
    let u = new Universe ()
    let mscorlib = u.Load (config.CorlibPath)
    let stringType = mscorlib.GetType ("System.String")
    let intType = mscorlib.GetType ("System.Int32")
    let voidType = mscorlib.GetType ("System.Void")
    let objectType = mscorlib.GetType ("System.Object")
    let asm = u.DefineDynamicAssembly (name, AssemblyBuilderAccess.Save, dir)
    let modl = asm.DefineDynamicModule (name.Name, config.OutputPath)

    let knownTypes = new Dictionary<SwiftType, ClrType> ()

    let learnType name t =
        knownTypes.[IdentifierType (name, [])] <- t

    let defineType name = 
        let t = modl.DefineType (name, TypeAttributes.Public)
        learnType name t
        t

    do
        learnType "AnyObject" objectType
        learnType "Int" intType
        learnType "String" stringType
        learnType "Void" voidType
        learnType "Dictionary" (mscorlib.GetType ("System.Collections.Generic.Dictionary`2"))
        learnType "List" (mscorlib.GetType ("System.Collections.Generic.List`1"))

    let mainType = defineType (name.Name + "Globals")
    let mainMethod = mainType.DefineMethod ("Main", MethodAttributes.Public ||| MethodAttributes.Static ||| MethodAttributes.HideBySig)

    member this.VoidType = voidType

    member this.DefinedTypes = knownTypes.Values |> Seq.choose (function | :? TypeBuilder as t -> Some t | _ -> None) |> Seq.toList
    member this.MainType = mainType

    member this.DefineType name = defineType name

    member this.GetClrType swiftType =
        match knownTypes.TryGetValue (swiftType) with
        | (true, t) -> t
        | _ ->
            match swiftType with
            | IdentifierType (name, generics) when generics.Length > 0 ->
                // Can we get the generic version?
                match knownTypes.TryGetValue (IdentifierType (name, [])) with
                | (true, g) ->
                    // Great, let's try to make a concrete one
                    let targs = generics |> Seq.map this.GetClrType |> Seq.toArray
                    let t = g.MakeGenericType (targs)
                    knownTypes.[swiftType] <- t
                    t
                | _ -> failwith (sprintf "GetClrType failed for %A" swiftType)
            | _ -> failwith (sprintf "GetClrType failed for %A" swiftType)

    member this.GetClrTypeOrVoid optionalSwiftType =
        match optionalSwiftType with
        | Some x -> this.GetClrType x
        | _ -> voidType


let declareMember (env : Env) (typ : TypeBuilder) decl =
    match decl with
        | FunctionDeclaration (dspecs, name : string, parameters: Parameter list list, res, body) ->
            if parameters.Length > 1 then failwith "Curried function declarations not supported"
            let returnType =
                match res with
                | Some (resAttrs, resType) -> env.GetClrType (resType)
                | None -> env.VoidType
            let paramTypes = parameters.Head |> Seq.map (fun (a,e,l,t,d) -> env.GetClrTypeOrVoid (t)) |> Seq.toArray
            let attribs = MethodAttributes.Public
            let builder = typ.DefineMethod (name, attribs, returnType, paramTypes)
            let ps = parameters.Head |> List.mapi (fun i p -> builder.DefineParameter (i + 1, ParameterAttributes.None, "args"))
            Some (fun () -> ())
        | _ -> None


let declareType (env : Env) stmt =
    match stmt with
    | DeclarationStatement (ClassDeclaration (name, inheritance, decls) as d) -> Some (env.DefineType name, decls)
    | DeclarationStatement (UnionEnumDeclaration (name, generics, inheritance, cases) as d) -> Some (env.DefineType name, [])
    | DeclarationStatement (TypealiasDeclaration (name, typ) as d) -> Some (env.DefineType name, [])
    | _ -> None


let compile config =
    let env = new Env (config)

    // Parse
    let stmts = config.InputUrls |> List.choose parseFile |> List.collect (fun x -> x)

    // First pass: Declare the types
    let typeDecls = stmts |> List.choose (declareType env)

    // Second pass: Declare members
    let memberCompilers = typeDecls |> List.collect (fun (typ, decls) -> (decls |> List.choose (declareMember env typ)))

    // Third pass: Compile code
    for mc in memberCompilers do mc ()

    // Hey, there they are
    env.MainType :: env.DefinedTypes


let compileFile file =
    let config =
        {
            InputUrls = [file]
            OutputPath = System.IO.Path.ChangeExtension (file, ".dll")
            CorlibPath = "mscorlib"
        }
    compile config


//compileFile "/Users/fak/Dropbox/Projects/SwiftSharp/SwiftSharp.Test/TestFiles/SODAClient.swift"


