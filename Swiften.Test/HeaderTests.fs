namespace Swiften.Test

open System
open NUnit.Framework

open Swiften.SwiftParser

[<TestFixture>]
type HeaderTests () =

    let parse path =
        match parseFile ("TestHeaders/" + path + ".swift") with
        | Some ([ExpressionStatement x], p) -> x
        | x -> failwith (sprintf "Parse returned: %A" x)

    [<Test>]
    member x.UIView () =
        let e = parse "UIKit.UIView"
        Assert.AreEqual (Funcall (Variable "f",[(None, Variable "x")],None), e)

    [<Test>]
    member x.UIViewController () =
        let e = parse "UIKit.UIViewController"
        Assert.AreEqual (Funcall (Variable "f",[(None, Variable "x")],None), e)

    [<Test>]
    member x.NSString () =
        let e = parse "CoreFoundation.NSString"
        Assert.AreEqual (Funcall (Variable "f",[(None, Variable "x")],None), e)

    