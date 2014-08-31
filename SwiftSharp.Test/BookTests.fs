namespace SwiftSharp.Test

open System
open System.IO
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftParser
open SwiftSharp.SwiftCompiler

type BookTests () =

    member this.Test (code) =
        let ast = 
            match parseCode code with
            | Some x -> x
            | x -> failwith (sprintf "%A" x)
        Assert.Greater (ast.Length, 0)
