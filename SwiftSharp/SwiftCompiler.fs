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
//        CorlibPath: string
//        ErrorOutput: System.IO.TextWriter
    }

type ClrType = IKVM.Reflection.Type
        
type Env (config) =
    let dir = System.IO.Path.GetDirectoryName (config.OutputPath)
    let name = new AssemblyName (System.IO.Path.GetFileNameWithoutExtension (config.OutputPath))
    let u = new Universe ()
    let mscorlib = u.Load ("mscorlib")
    let StringType = mscorlib.GetType ("System.String")
    let voidType = mscorlib.GetType ("System.Void")
    let objectType = mscorlib.GetType ("System.Object")
    let asm = u.DefineDynamicAssembly (name, AssemblyBuilderAccess.Save, dir)
    let modl = asm.DefineDynamicModule (name.Name, config.OutputPath)
    let mainType = modl.DefineType (name.Name + ".Globals", TypeAttributes.Public)
    let mainMethod = mainType.DefineMethod ("Main", MethodAttributes.Public ||| MethodAttributes.Static ||| MethodAttributes.HideBySig)

    let knownTypes = new Dictionary<SwiftType, ClrType> ()

    member this.VoidType = voidType

    member this.DefineType name = 
        let t = modl.DefineType (name, TypeAttributes.Public)
        knownTypes.[IdentifierType (name, [])] <- t
        t

    member this.GetClrType t =
        match t with
        | IdentifierType ("String", []) -> StringType
        | IdentifierType ("Void", []) -> voidType
        | _ ->
            match knownTypes.TryGetValue (t) with
            | (true, y) -> y
            | _ -> objectType
//            | _ -> failwith (sprintf "LookupType failed for %A" t)

    member this.GetClrTypeOrVoid t =
        match t with
        | Some x -> this.GetClrType x
        | _ -> voidType



let rec compileFunc func (typ : TypeBuilder) (env : Env) =
    match func with
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
        let il = builder.GetILGenerator ()
        builder
    | _ -> failwith (sprintf "compileFunc called with %A" func)


and compileClass cls (env : Env) =
    match cls with
    | ClassDeclaration (name, inheritance, decls) ->
        let typ = env.DefineType name
        let methods =
            decls
            |> Seq.choose (function
                | FunctionDeclaration _ as d -> Some (compileFunc d typ env)
                | _ -> None)
            |> Seq.toArray        
        typ
    | _ -> failwith (sprintf "compileClass called with %A" cls)



let compileTypes env stmts =
    stmts
    |> List.choose (function
        | DeclarationStatement (ClassDeclaration _ as d) -> Some (compileClass d env)
        | _ -> None)

let compileFile file =
    let config =
        {
            InputUrls = [file]
            OutputPath = System.IO.Path.ChangeExtension (file, ".dll")
        }
    let env = new Env (config)
    match parseFile file with
    | Some (ss, e) -> compileTypes env ss
    | None -> failwith "parse failed"




//compileFile "/Users/fak/Dropbox/Projects/SwiftSharp/SwiftSharp.Test/TestFiles/SODAClient.swift"


