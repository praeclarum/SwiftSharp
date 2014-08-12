namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftCompiler

[<TestFixture>]
type CompilerTests () =

    let compileFile path = compileFile ("TestFiles/" + path + ".swift")

    [<Test>]
    member x.SODAClient() =
        let r = compileFile "SODAClient"
        match r with
        | [t] -> Assert.AreEqual ("SODAClient", t.Name)
        | _ -> failwith "Expected 1 class"
