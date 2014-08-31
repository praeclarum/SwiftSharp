module SwiftSharp.SwiftCompiler

// #r "../Lib/IKVM.Reflection.dll";;

open System
open System.Text
open System.Collections.Generic

open SwiftParser

open IKVM.Reflection
open IKVM.Reflection.Emit

let notSupported x = failwith (sprintf "Not supported: %A" x)

let omg msg x = failwith (sprintf msg x)

let pass msg x = failwith (sprintf msg x)

type Config =
    {
        OutputPath: string
        References: string list
    }

type ClrType = IKVM.Reflection.Type

type DefinedMembers = ResizeArray<MemberInfo>

type DefinedType =
    {
        Builder : TypeBuilder
        GenericParameters : GenericTypeParameterBuilder array
        Members : DefinedMembers
    }
    with
        static member Create (t, gps) = { Builder = t; GenericParameters = gps; Members = new DefinedMembers () }

type TypeId = (string * int) list

let strToId n = [(n, 0)]
let swiftToId ((n, g) : SwiftTypeElement) : TypeId = [(n, g.Length)]
let rec clrToId (clrType : ClrType) : TypeId =
    let g =
        if clrType.ContainsGenericParameters then
            (clrType.GetGenericArguments ()).Length
        else 0
    let ti = clrType.Name.IndexOf ('`')
    let name = if ti > 0 then clrType.Name.Substring (0, ti) else clrType.Name
    if clrType.IsNested then
        let nid = clrToId clrType.DeclaringType
        List.append nid [(name, g)]
    else
        [(name, g)]

type TypeRef =
    | ClrTypeRef of ClrType
    | DefinedTypeRef of DefinedType
    with
        member this.Members : MemberInfo seq =
            match this with
            | ClrTypeRef t -> t.GetMembers () :> MemberInfo seq
            | DefinedTypeRef t -> t.Members :> MemberInfo seq
        member this.GetMember (name) =
            match this with
            | ClrTypeRef t -> t.GetMember (name)
            | DefinedTypeRef t -> t.Members |> Seq.filter (fun x -> x.Name = name) |> Seq.toArray
        member this.Methods : MethodInfo seq =
            match this with
            | ClrTypeRef t -> t.GetMethods () |> Array.toSeq
            | DefinedTypeRef t -> t.Members |> Seq.choose (function | :? MethodInfo as m -> Some m | _ -> None)
        member this.Constructors : ConstructorInfo seq =
            match this with
            | ClrTypeRef t -> t.GetConstructors () |> Array.toSeq
            | DefinedTypeRef t -> t.Members |> Seq.choose (function | :? ConstructorInfo as c -> Some c | _ -> None)
        member this.Name =
            match this with
            | ClrTypeRef t -> t.Name
            | DefinedTypeRef t -> t.Builder.Name
        member this.FullName =
            match this with
            | ClrTypeRef t -> t.FullName
            | DefinedTypeRef t -> t.Builder.FullName
        member this.ClrType =
            match this with
            | ClrTypeRef t -> t
            | DefinedTypeRef t -> t.Builder :> ClrType
        member this.MakeGenericType (targs : TypeRef array) =
            match this with
            | ClrTypeRef t -> ClrTypeRef (t.MakeGenericType (targs |> Array.map (fun x -> x.ClrType)))
            | DefinedTypeRef t -> ClrTypeRef (t.Builder.MakeGenericType (targs |> Array.map (fun x -> x.ClrType)))
        member this.IsAssignableFrom (other : TypeRef) =
            match this with
            | ClrTypeRef t -> t.IsAssignableFrom (other.ClrType)
            | _ -> omg "IsAssignableFrom not implemented for %A" (this, other)

type TypeMap = Dictionary<TypeId, TypeRef>

type Env (config) =
    let dir = System.IO.Path.GetDirectoryName (config.OutputPath)
    let name = new AssemblyName (System.IO.Path.GetFileNameWithoutExtension (config.OutputPath))
    let defaultNamespace = name.Name
    let u = new Universe ()
    let dirs = dir :: (config.References |> List.map (fun x -> System.IO.Path.GetDirectoryName (x)))
    do
        u.add_AssemblyResolve (new ResolveEventHandler (fun sender (e : ResolveEventArgs) ->
            let n = new AssemblyName (e.Name)
            match dirs |> Seq.map (fun d -> System.IO.Path.Combine (d, n.Name + ".dll")) |> Seq.tryFind (fun p -> System.IO.File.Exists (p)) with
            | Some p -> u.LoadFile (p)
            | _ -> failwith (sprintf "Cannot find %A" e.Name)
            ))

    let refs = config.References |> List.map u.LoadFile
    let mscorlib = refs |> List.find (fun x -> x.GetType ("System.String") <> null)
    let monotouch = refs |> List.tryFind (fun x -> x.GetType ("MonoTouch.Foundation.NSString") <> null)
    let errors = new ResizeArray<string> ()

    //
    // Add basic types
    //
    let stringType = mscorlib.GetType ("System.String")
    let intType = mscorlib.GetType ("System.Int32")
    let voidType = mscorlib.GetType ("System.Void")
    let objectType = mscorlib.GetType ("System.Object")
    let exportAttributeType = match monotouch with | Some x -> Some (x.GetType ("MonoTouch.Foundation.ExportAttribute")) | _ -> None
    let registerAttributeType = match monotouch with | Some x -> Some (x.GetType ("MonoTouch.Foundation.RegisterAttribute")) | _ -> None
    let asm = u.DefineDynamicAssembly (name, AssemblyBuilderAccess.Save, dir)
    let modl = asm.DefineDynamicModule (name.Name, config.OutputPath)
    do
        refs
        |> List.iter (fun a ->
            modl.__AddAssemblyReference (a.GetName (), a))

    let getRegisteredName (tr : TypeRef) =
        match tr with
        | ClrTypeRef t ->
            match registerAttributeType with
            | Some r ->
                match t.__GetCustomAttributes (r, true) |> Seq.toList with
                | [] -> None
                | x :: _ -> Some (x.ConstructorArguments.[0].Value.ToString ())
            | _ -> None
        | _ -> None

    //
    // Read the namespaces
    //
    let namespaces = new Dictionary<string list, TypeMap> ()
    do
        refs
        |> List.iter (fun a ->
            let types = a.GetTypes () |> Array.choose (fun t ->
                let k = if t.Namespace <> null then t.Namespace.Split ('.') |> Array.toList else []
                let ns =
                    match namespaces.TryGetValue (k) with
                    | (true, x) -> x
                    | (false, _) ->
                        let x = new TypeMap ()
                        namespaces.Add (k, x)
                        x
                if t.IsPublic then
                    let tr = ClrTypeRef t
                    ns.Add (clrToId t, tr)
                    Some (ns, tr)
                else None)
            types |> Array.iter (fun (ns, t) ->                
                match getRegisteredName t with
                | Some rname when rname <> t.Name ->
                    let rid = strToId rname
                    if not (ns.ContainsKey (rid)) then
                        ns.Add (rid, t)
                | _ -> ()))

    let definedTypes = new TypeMap ()

    let coreTypes = new TypeMap ()

    let learnType id t =
        coreTypes.[[id]] <- ClrTypeRef t

    do
        learnType ("AnyObject", 0) objectType
        learnType ("Int", 0) intType
        learnType ("String", 0) stringType
        learnType ("Void", 0) voidType
        learnType ("Dictionary", 2) (mscorlib.GetType ("System.Collections.Generic.Dictionary`2"))
        learnType ("Array", 1) (mscorlib.GetType ("System.Collections.Generic.List`1"))
        learnType ("Action", 1) (mscorlib.GetType ("System.Action`1"))

    let methodReplacements =
        Map.ofList
            [
                (("System.String", "hasPrefix"), "StartsWith")
            ]

    member this.Assembly = asm

    member this.CoreTypes = coreTypes
    member this.VoidType = ClrTypeRef voidType
    member this.ObjectType = ClrTypeRef objectType
    member this.StringType = ClrTypeRef stringType
    member this.ExportAttributeType = exportAttributeType

    member this.GetMemberReplacement (fullTypeName, methodName) =
        match methodReplacements.TryFind (fullTypeName, methodName) with
        | Some n -> n
        | _ -> methodName

    member this.FindGlobalMethods (methodName : string) : MethodInfo list =
        match methodName with
        | "println" ->
            match objectType.Assembly.GetType ("System.Console") with
            | null -> []
            | t -> [t.GetMethod ("WriteLine", [|objectType|])]
        | _ -> []

    member this.DefinedTypes = definedTypes

    member this.DefineType name generics attribs (parent : TypeRef) =
        let t = modl.DefineType (defaultNamespace + "." + name, attribs, parent.ClrType)
        let g = 
            match generics with
            | [] -> [||]
            | _ -> t.DefineGenericParameters (generics |> List.toArray)
        let id = [(name, g.Length)]
        let d = DefinedType.Create (t, g)
        definedTypes.[id] <- DefinedTypeRef d
        d

    member this.GetNamespace (parts) =
        match namespaces.TryGetValue (parts) with
        | (true, ns) -> ns
        | (false, _) ->
            match namespaces.TryGetValue ("MonoTouch" :: parts) with
            | (true, ns) -> ns
            | (false, _) -> failwith (sprintf "Cannot find namespace %A" parts)

    member this.GetOperatorPrecedence op = failwith (sprintf "Unknown operator %s" op)


type TranslationUnit (env : Env, stmts : Statement list) =
    let typeAliases = new TypeMap ()
    let importedTypes = new TypeMap ()
    do
        stmts
        |> List.iter (function
            | DeclarationStatement(ImportDeclaration path) ->
                let ns = env.GetNamespace path
                ns |> Seq.iter (fun x -> importedTypes.Add (x.Key, x.Value))
            | _ -> ())

    member this.Env = env
    member this.Statements = stmts

    member private this.LookupKnownType id : TypeRef option =
        match typeAliases.TryGetValue id with
        | (true, t) -> Some t
        | _ ->
            match env.DefinedTypes.TryGetValue id with
            | (true, t) -> Some t
            | _ ->
                match importedTypes.TryGetValue id with
                | (true, t) -> Some t
                | _ ->
                    match env.CoreTypes.TryGetValue id with
                    | (true, t) -> Some t
                    | _ -> None

    member this.DefineType = env.DefineType

    member this.DefineTypeAlias (name, swiftType) =
        let clrType = this.GetClrType (swiftType)
        typeAliases.[[(name, 0)]] <- clrType
        clrType

    member this.FindClrType (SwiftType es : SwiftType) : TypeRef option =
        // TODO: This ignores nested types
        let e = es.Head
        let id = e |> swiftToId
        match this.LookupKnownType id, id with
        | Some t, [(_, 0)] -> Some t
        | Some t, _ ->
            // Great, let's try to make a concrete one
            let targs = snd e |> Seq.map this.GetClrType |> Seq.toArray
            Some (t.MakeGenericType (targs))
        | _ -> None

    member this.FindClrType (name : string) =
        this.FindClrType (SwiftType [(name, [])])

    member this.GetClrType (swiftType) =
        match this.FindClrType swiftType with
        | Some x -> x
        | _ -> failwith (sprintf "GetClrType failed for %A" swiftType)

    member this.GetClrTypeOrVoid optionalSwiftType =
        match optionalSwiftType with
        | Some x -> this.GetClrType x
        | _ -> env.VoidType

    member this.InferType (expr) : TypeRef =
        match expr with
        | _ -> failwith (sprintf "Cannot infer type of: %A" expr)

let compileMethod (tu : TranslationUnit) (typ : DefinedType) (methodName : string) (ps : (ParameterBuilder * TypeRef) list) (body : Statement list) (il : ILGenerator) =

    let cid = ref 1
    let locals = ref []

    let declareLocal name clrType =
        let loc = il.DeclareLocal clrType
        loc.SetLocalSymInfo (name)
        locals := (name, loc) :: !locals
        loc

    let (|NamedType|_|) e =
        match e with
        | Variable name -> tu.FindClrType (name)
        | _ -> None

    let resolveOverload xs args getAttrs =
        match tu.Env.ExportAttributeType with
        | Some ex ->
            let mats = xs |> Seq.choose (fun x -> 
                match getAttrs x ex |> Seq.toList with
                | [e : CustomAttributeData] when e.ConstructorArguments.Count = 1 -> 
                    let n = e.ConstructorArguments.[0].Value.ToString ()
                    let ans = n.Split ([|':'|], StringSplitOptions.RemoveEmptyEntries)
                    let a0 = ans.[0]
                    if a0.StartsWith ("initWith") && a0.Length > 8 then
                        ans.[0] <-
                            if a0.Length > 9 && Char.IsUpper (a0.[9]) then
                                a0.Substring (8)
                            else
                                (Char.ToLowerInvariant (a0.[8])).ToString () + a0.Substring (9)
                    Some (ans, x)
                | _ -> None)
            let anames = args |> Seq.map fst |> Seq.toArray
            let argMatch m a =
                match a with
                | Some x -> x = m
                | None -> true
            let argsMatch ((mnames : string array), m) =
                anames |> Array.map2 argMatch mnames |> Array.forall (id)
            match mats |> Seq.tryFind argsMatch with
            | Some (_, x) -> Some x
            | None -> omg "I can't find a ctor %A" (mats, args)
        | _ -> omg "I don't know how to choose an overload of init %A" (xs, args)

    let matchMethod (methods : MethodInfo seq) (args : Argument list) =
        match methods |> Seq.filter (fun x -> (x.GetParameters ()).Length = args.Length) |> Seq.toList with
        | [] -> None
        | [x] -> Some x
        | xs -> resolveOverload xs args (fun x ex -> x.__GetCustomAttributes (ex, true))

    let findMethod (typ : TypeRef) methodName (args : Argument list) =
        let name = tu.Env.GetMemberReplacement (typ.FullName, methodName)
        matchMethod (typ.Methods |> Seq.filter (fun x -> x.Name = name)) args

    let findGlobalMethod methodName (args : Argument list) =
        matchMethod (tu.Env.FindGlobalMethods (methodName)) args

    let findCtor (typ : TypeRef) (args : Argument list) =
        match typ.Constructors |> Seq.filter (fun x -> (x.GetParameters ()).Length = args.Length) |> Seq.toList with
        | [] -> None
        | [x] -> Some x
        | xs -> resolveOverload xs args (fun x ex -> x.__GetCustomAttributes (ex, true))

    let canAssignType (sourceType : TypeRef, destType : TypeRef) =
        destType.IsAssignableFrom (sourceType)

    let typesCompatible (sourceTypes : TypeRef seq, destTypes : TypeRef seq) =
        sourceTypes |> Seq.zip destTypes |> Seq.forall canAssignType

    let findMethodWithParamTypes (typ : TypeRef) name (argTypes : TypeRef list) =
        match typ.Methods |> Seq.filter (fun x ->
            let ps = x.GetParameters ()
            x.Name = name &&
                ps.Length = argTypes.Length &&
                typesCompatible (argTypes, ps |> Seq.map (fun x -> ClrTypeRef x.ParameterType))) |> Seq.toList with
        | [] -> None
        | [x] -> Some x
        | xs -> omg "Cannot determine overload %A" (typ, name, argTypes, xs)

    let rec findBinaryOp (typ : TypeRef) opName arg : MethodInfo option =
        let n =
            match opName with
            | "+" -> "op_Addition"
            | _ -> "op_" + opName
        match typ.Methods |> Seq.filter (fun x -> x.Name = n) |> Seq.toList with
        | [x] -> Some x
        | [] ->
            match typ.FullName, opName with
            | "System.String", "+" -> findMethodWithParamTypes typ "Concat" [typ; typeof arg]
            | _ -> omg "I don't know the operator %A" (typ, opName)
        | xs -> omg "I don't know how to overload op %A" (opName, n, xs)

    and typeof e : TypeRef =
        match e with
        | Str _ -> tu.Env.StringType
        | FlatBinary _ -> flatten e tu.Env.GetOperatorPrecedence |> typeof
        | Binary (_, TernaryConditionalBinary (e, _)) -> typeof e
        | Variable name ->
            match !locals |> List.tryFind (fun (n,_) -> n = name) with
            | Some (_, loc) -> ClrTypeRef loc.LocalType
            | None ->
                match ps |> List.tryFind (fun (x, _) -> x.Name = name) with
                | Some (_, t) -> t
                | _ -> omg "Can't find the type of variable %s" name
        | Funcall (NamedType x, args) -> x
        | Funcall (Member (Some (NamedType t), name), args) ->
            match findMethod t name args with
            | Some x -> ClrTypeRef x.ReturnType
            | _ -> omg "Could not find method on type %A" (t, name, args)
        | _ -> failwith (sprintf "Don't know the type of %A" e)

    let rec apply f args =
        let emitCall (meth : MethodInfo) =
            args |> List.iter (snd >> eval)
            if meth.Attributes.HasFlag (MethodAttributes.Virtual) then
                il.EmitCall (OpCodes.Callvirt, meth, ClrType.EmptyTypes)
            else
                il.EmitCall (OpCodes.Call, meth, ClrType.EmptyTypes)
        match f with
        | Member (Some o, name) ->
            let ot = typeof o
            match findMethod ot name args with
            | Some meth ->
                eval o
                emitCall meth
            | _ -> omg "Cannot find method %A" (ot, name)
        | Variable name ->
            match findGlobalMethod name args with
            | Some meth -> emitCall meth
            | _ -> omg "Cannot find function %A" name
        | _ -> omg "Don't know how to apply %A" f

    and eval e =
        match e with
        | Str s -> il.Emit (OpCodes.Ldstr, s)
        | FlatBinary _ -> flatten e tu.Env.GetOperatorPrecedence |> eval
        | Variable name ->
            match !locals |> List.tryFind (fun (n,_) -> n = name) with
            | Some (_, loc) -> il.Emit (OpCodes.Ldloc, loc.LocalIndex)
            | None ->
                match ps |> List.tryFindIndex (fun (x,_) -> x.Name = name) with
                | Some i -> il.Emit (OpCodes.Ldarg, i)
                | _ -> omg "Can't find variable %s for eval" name
        | Binary (c, TernaryConditionalBinary (t, f)) ->
            let falseLabel = il.DefineLabel ()
            let endLabel = il.DefineLabel ()
            eval c
            il.Emit (OpCodes.Brfalse, falseLabel)
            eval t
            il.Emit (OpCodes.Br, endLabel)
            il.MarkLabel (falseLabel)
            eval f
            il.MarkLabel (endLabel)
        | Binary (x, OpBinary (op, y)) ->
            let xt = typeof x
            match findBinaryOp xt op y with
            | Some meth ->
                eval y
                eval x
                il.Emit (OpCodes.Callvirt, meth)
            | _ ->
                match xt.FullName, op with
                | ("System.String", "+") -> omg "Don't know how to add string %A" (xt, op)
                | _ -> omg "Cannot find operator %A for eval" (xt, x, op, y)
        | Closure (head, body) ->
            let ctypeName = sprintf "%sClosure%d" methodName !cid
            cid := !cid + 1
            pass "Closures, closures!! %A" head
        | Funcall ((NamedType t), args) ->
            match findCtor t args with
            | None -> omg "Couldn't find constructor for %A" args
            | Some ctor ->
                args |> List.iter (snd >> eval)
                il.Emit (OpCodes.Newobj, ctor)
        | Funcall (Member (Some (NamedType t), name), args) ->
            match findMethod t name args with
            | Some meth ->
                args |> List.iter (snd >> eval)
                il.EmitCall (OpCodes.Callvirt, meth, [||])
            | _ -> omg "Could not find method on type %A" (t, name, args)
        | Funcall (Variable name as f, args) ->
            match findMethod (DefinedTypeRef typ) name args with
            | Some meth ->
                args |> List.iter (snd >> eval)
                il.EmitCall (OpCodes.Callvirt, meth, [||])
            | _ -> apply f args
        | Funcall (f, args) -> apply f args
        | _ -> failwith (sprintf "Can't eval %A" e)

    let assign left right =
        match left with
        | Member (Some (Variable "self"), name) ->
            match (DefinedTypeRef typ).GetMember (name) with
            | [||] -> failwith (sprintf "Can't find member %A" name)
            | [|:? FieldBuilder as field|] ->
                eval right
                il.Emit (OpCodes.Ldloc_0);
                il.Emit (OpCodes.Stfld, field)
            | [|m|] -> failwith (sprintf "Don't know how to assign %A" m)
            | _ -> failwith (sprintf "Ambiguous member %A" name)
        | _ -> pass "Can't assign %A" left

    let rec run s =
        match s with
        | ExpressionStatement e ->
            match (flatten e tu.Env.GetOperatorPrecedence) with
            | Binary (left, OpBinary ("=", right)) -> assign left right
            | x ->
                let h = il.__StackHeight
                eval x
                if il.__StackHeight > h then
                    il.Emit (OpCodes.Pop)
        | DeclarationStatement (ConstantDeclaration [(IdentifierPattern (name, None), Some e)]) ->
            let loc = declareLocal name (typeof e).ClrType
            loc.SetLocalSymInfo (name)
            eval e
            il.Emit (OpCodes.Stloc, loc.LocalIndex)
        | x -> failwith (sprintf "Don't know how to compile statement %A" x)

    body |> List.iter run
    il.Emit (OpCodes.Ret)

let declareField (tu : TranslationUnit) (typ : DefinedType) decl =

    let getType optType optInit =
        match optType with
        | Some t -> tu.GetClrType (t)
        | _ ->
            match optInit with
            | Some e -> tu.InferType (e)
            | _ -> failwith "let statements must have a type or initializer"                

    match decl with
        | ConstantDeclaration specs ->
            let attribs = FieldAttributes.Public
            let builders = specs |> List.map (function
                | (IdentifierPattern (name, ot), oi) ->
                    typ.Builder.DefineField (name, (getType ot oi).ClrType, attribs)
                | x -> notSupported x)                
            Some (fun () -> ())
        | _ -> None

let declareMethod (tu : TranslationUnit) (typ : DefinedType) decl =
    match decl with
        | FunctionDeclaration (dspecs, name : string, parameters: Parameter list list, res, body) ->
            if parameters.Length > 1 then failwith "Curried function declarations not supported"
            let returnType =
                match res with
                | Some (resAttrs, resType) -> tu.GetClrType (resType)
                | None -> tu.Env.VoidType
            let paramTypes =
                parameters.Head
                |> Seq.map (function
                    | (a,e,l,Some t,d) -> tu.GetClrType (t)
                    | _ -> tu.Env.ObjectType)
                |> Seq.toArray
            let attribs = MethodAttributes.Public
            let builder = typ.Builder.DefineMethod (name, attribs, returnType.ClrType, paramTypes |> Array.map (fun x -> x.ClrType))
            let ps = parameters.Head |> List.mapi (fun i (_, _, ploc, _, _) -> builder.DefineParameter (i + 1, ParameterAttributes.None, ploc))
            let psts = (List.zip ps (paramTypes |> Array.toList))
            Some (fun () -> compileMethod tu typ name psts body (builder.GetILGenerator ()))
        | InitializerDeclaration (parameters: Parameter list, body) ->
            let paramTypes =
                parameters
                |> Seq.map (function
                    | (a,e,l,Some t,d) -> tu.GetClrType (t)
                    | _ -> tu.Env.ObjectType)
                |> Seq.toArray
            let attribs = MethodAttributes.Public
            let builder = typ.Builder.DefineConstructor (attribs, CallingConventions.Standard, paramTypes |> Array.map (fun x -> x.ClrType))
            let ps = parameters |> List.mapi (fun i (_, _, ploc, _, _) -> builder.DefineParameter (i + 1, ParameterAttributes.None, ploc))
            let psts = (List.zip ps (paramTypes |> Array.toList))
            Some (fun () -> compileMethod tu typ "init" psts body (builder.GetILGenerator ()))
        | _ -> None

type DeclaredType = DefinedType * (Declaration list)

let declareType (tu : TranslationUnit) stmt : DeclaredType list =
    match stmt with
    | DeclarationStatement (ClassDeclaration (name, generics, inheritance, decls) as d) ->
        [((tu.DefineType name generics TypeAttributes.Public tu.Env.ObjectType), decls)]
    | DeclarationStatement (UnionEnumDeclaration (name, generics, inheritance, cases) as d) ->
        let d = tu.DefineType name generics (TypeAttributes.Public ||| TypeAttributes.Abstract) tu.Env.ObjectType
        let cbs : DeclaredType list = cases |> List.collect (fun ccases ->
            ccases |> List.map (fun (cn, oct) ->
                let tattribs = TypeAttributes.Class ||| TypeAttributes.Public
                let fattribs = FieldAttributes.Public
                let cd = tu.DefineType (name + cn) [] tattribs (DefinedTypeRef d)
                match oct with
                | Some (SwiftType [("Tuple", ts)]) ->
                    ts |> List.iteri (fun i tt ->
                        let fb = cd.Builder.DefineField (sprintf "Item%d" (i+1), (tu.GetClrType (tt)).ClrType, fattribs)
                        ())
                | _ -> ()
                (cd, [])))
        (d, []) :: cbs
    | DeclarationStatement (TypealiasDeclaration (name, typ)) ->
        let ct = tu.DefineTypeAlias (name, typ)
        []
    | _ -> []

let isTopLevelStatement stmt =
    match stmt with
    | DeclarationStatement (ConstantDeclaration _) -> true
    | ReturnStatement _
    | DeclarationStatement _ -> false
    | ExpressionStatement _
    | IfStatement _
    | SwitchStatement _
    | ForInStatement _ -> true


let rec compileUrls inputUrls config =
    let asts = inputUrls |> List.choose parseFile
    compile asts config

and compile asts config =
    let env = new Env (config)

    // Create translation units for each AST
    let tus = asts |> List.map (fun x -> new TranslationUnit (env, x))

    // First pass: Declare the types
    let typeDecls = tus |> List.map (fun x -> (x, x.Statements |> List.collect (declareType x)))

    // Second pass: Declare methods - these state their types explicitely
    let methodCompilers =
        typeDecls |> List.collect (fun (tu, tdecls) ->
            tdecls |> List.collect (fun (typ, decls) ->
                decls |> List.choose (declareMethod tu typ)))

    // Third pass: Declare fields and properties
    let fieldCompilers =
        typeDecls |> List.collect (fun (tu, tdecls) ->
            tdecls |> List.collect (fun (typ, decls) ->
                decls |> List.choose (declareField tu typ)))

    // Create the top level type
    let tattribs = TypeAttributes.Public
    let topLevelType = env.DefineType "Globals" [] tattribs env.ObjectType
    let topLevelExe = tus |> List.map (fun x -> (x, x.Statements |> List.filter isTopLevelStatement))
    let topLevelCompilers = topLevelExe |> List.mapi (fun i (tu, body) ->
        let name = sprintf "Initialize%d" i
        let attribs = MethodAttributes.Public ||| MethodAttributes.Static
        let builder = topLevelType.Builder.DefineMethod (name, attribs, env.VoidType.ClrType, [||])
        (builder, fun () -> compileMethod tu topLevelType name [] body (builder.GetILGenerator ())))

    if config.OutputPath.EndsWith (".exe") then
        let name = sprintf "Main"
        let attribs = MethodAttributes.Public ||| MethodAttributes.Static ||| MethodAttributes.HideBySig
        let builder = topLevelType.Builder.DefineMethod (name, attribs, env.VoidType.ClrType, [||])
        let il = builder.GetILGenerator ()
        for (b, c) in topLevelCompilers do
            il.EmitCall (OpCodes.Call, b, ClrType.EmptyTypes)
        il.Emit (OpCodes.Ret)
        env.Assembly.SetEntryPoint (builder)

    // Fourth pass: Compile code, finally
    for fc in fieldCompilers do fc ()
    for mc in methodCompilers do mc ()
    for mc in topLevelCompilers do (snd mc) ()

    // Bake the types
    let types =
        env.DefinedTypes.Values
        |> Seq.iter (function
            | DefinedTypeRef t -> if not (t.Builder.IsCreated ()) then t.Builder.CreateType () |> ignore
            | _ -> ())

    // Save it
    env.Assembly


let compileFile file =
    let config =
        {
            OutputPath = System.IO.Path.ChangeExtension (file, ".dll")
            References = [ typeof<String>.Assembly.Location ]
        }
    compileUrls [file] config

//compileFile "/Users/fak/Dropbox/Projects/SwiftSharp/SwiftSharp.Test/TestFiles/SODAClient.swift"


