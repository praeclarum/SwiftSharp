namespace Swiften.Test

open System
open NUnit.Framework

open Swiften.SwiftParser

[<TestFixture>]
type SwiftBookTests () =

    let parse code =
        match parseText code with
        | Some (x, p) -> x
        | _ -> failwith "Parse didn't return anything"

        //#load "../Swiften/SwiftParser.fs";;open SiftParser;;


    [<Test>]
    member x.SimpleValue4() =
        let ast = parse """
let apples = 3
let oranges = 5
let appleSummary = "I have \(apples) apples."
let fruitSummary = "I have \(apples + oranges) pieces of fruit."""

        let ex =
            [ConstantDeclaration
                 [(IdentifierPattern ("apples",None), Some (Number 3.0))];
               ConstantDeclaration
                 [(IdentifierPattern ("oranges",None), Some (Number 5.0))];
               ConstantDeclaration
                 [(IdentifierPattern ("appleSummary",None),
                   Some (Str "I have \(apples) apples."))];
               ConstantDeclaration
                 [(IdentifierPattern ("fruitSummary",None),
                   Some (Str "I have \(apples + oranges) pieces of fruit."))]]

        Assert.AreEqual (ex, ast)


    [<Test>]
    member x.SimpleValue3() =
        let ast = parse """
let label = "The width is "
let width = 94
let widthLabel = label + String(width)"""

        let ex =
            [ConstantDeclaration
                 [(IdentifierPattern ("label",None), Some (Str "The width is "))];
               ConstantDeclaration
                 [(IdentifierPattern ("width",None), Some (Number 94.0))];
               ConstantDeclaration
                 [(IdentifierPattern ("widthLabel",None),
                   Some
                     (Compound
                        (Variable "label",
                         [("+",
                           Funcall (Variable "String",[(None, Variable "width")],None))])))]]

        Assert.AreEqual (ex, ast)

    [<Test>]
    member x.SimpleValue2() =
        let ast = parse """
let implicitInteger = 70
let implicitDouble = 70.0
let explicitDouble: Double = 70"""

        Assert.AreEqual (
            [ConstantDeclaration
                 [(IdentifierPattern ("implicitInteger",None), Some (Number 70.0))];
               ConstantDeclaration
                 [(IdentifierPattern ("implicitDouble",None), Some (Number 70.0))];
               ConstantDeclaration
                 [(IdentifierPattern ("explicitDouble",Some (IdentifierType "Double")),
                   Some (Number 70.0))]],
            ast)

    [<Test>]
    member x.SimpleValue1() =
        let ast = parse """
    var myVariable = 42
    myVariable = 50
    let myConstant = 42"""

        Assert.AreEqual ([],
            ast)
