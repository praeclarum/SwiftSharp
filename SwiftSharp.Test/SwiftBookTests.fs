namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp.SwiftParser

[<TestFixture>]
type SwiftBookTests () =

    let parse code =
        match parseText code with
        | Some x -> x
        | _ -> failwith "Parse didn't return anything"

        //#load "../SwiftSharp/SwiftParser.fs";;open SiftParser;;


    [<Test>]
    member x.SimpleValue4() =
        let ast = parse """
let apples = 3
let oranges = 5
let appleSummary = "I have \(apples) apples."
let fruitSummary = "I have \(apples + oranges) pieces of fruit."""
        Assert.AreEqual (4, ast.Length)


    [<Test>]
    member x.SimpleValue3() =
        let ast = parse """
let label = "The width is "
let width = 94
let widthLabel = label + String(width)"""
        Assert.AreEqual (3, ast.Length)

    [<Test>]
    member x.SimpleValue2() =
        let ast = parse """
let implicitInteger = 70
let implicitDouble = 70.0
let explicitDouble: Double = 70"""
        Assert.AreEqual (3, ast.Length)

    [<Test>]
    member x.SimpleValue1() =
        let ast = parse """
    var myVariable = 42
    myVariable = 50
    let myConstant = 42"""
        Assert.AreEqual (3, ast.Length)
