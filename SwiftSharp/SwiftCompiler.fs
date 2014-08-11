module SwiftSharp.SwiftCompiler

// #r "../Lib/IKVM.Reflection.dll";;

open System
open System.Text

open SwiftParser

open IKVM.Reflection
open IKVM.Reflection.Emit



let workingDir = System.IO.Directory.GetCurrentDirectory ()
let u = new Universe ()

let asm = u.DefineDynamicAssembly (new AssemblyName ("SwiftSharpTest.dll"), AssemblyBuilderAccess.Save, workingDir)

let m = asm.DefineDynamicModule ("sdf", "sdf", false)

let typeName d =
    match d with
    | ClassDeclaration (name, _, _) -> name
    | StructDeclaration (name, _, _) -> name
    | RawValueEnumDeclaration (name, _, _) -> name
    | _ -> failwith (sprintf "No name for: %A" d)

let compileTypes stmts =
    let decls = stmts |> Seq.choose (function
        | DeclarationStatement (ClassDeclaration _ as d)
        | DeclarationStatement (StructDeclaration _ as d)
        | DeclarationStatement (RawValueEnumDeclaration _ as d) -> Some d
        | _ -> None)
    for d in decls do
        let t = m.DefineType (typeName d)
        printfn "%A" t

let compileFile file =
    let stmts =
        match parseFile file with
        | Some (x, e) -> x
        | x -> failwith (sprintf "Parse returned: %A" x)
    stmts |> compileTypes

compileFile "/Users/fak/Dropbox/Projects/SwiftSharp/SwiftSharp.Test/TestHeaders/GovDataRequest.swift"


