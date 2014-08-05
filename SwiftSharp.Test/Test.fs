namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp.SwiftParser

[<TestFixture>]
type ExpressionTests () =

    let parse code =
        match parseText code with
        | Some ([ExpressionStatement x], p) -> x
        | _ -> failwith "Parse didn't return anything"

    [<Test>]
    member x.Funcall() =
        let e = parse "f(x)"
        Assert.AreEqual (Funcall (Variable "f",[(None, Variable "x")],None), e)

    