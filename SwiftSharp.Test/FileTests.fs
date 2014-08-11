namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftParser

[<TestFixture>]
type HeaderTests () =

    let parseFile path =
        match parseFile ("TestFiles/" + path + ".swift") with
        | Some x -> x
        | x -> failwith (sprintf "Parse returned: %A" x)
    
    [<Test>]
    member x.SODAClient () =
        let ast, e = parseFile "SODAClient"
        let s = (sprintf "%A" ast)
        Assert.AreEqual (7, ast.Length)

    [<Test>]
    member x.GovDataRequest () =
        let ast, e = parseFile "GovDataRequest"
        let s = (sprintf "%A" ast)
        Assert.AreEqual (3, ast.Length)

    [<Test>]
    member x.UIView () =
        let ast, e = parseFile "UIKit.UIView"
        let s = (sprintf "%A" ast)
        Assert.AreEqual (33, ast.Length)

    [<Test>]
    member x.UIViewController () =
        let ast, e = parseFile "UIKit.UIViewController"
        let s = (sprintf "%A" ast)
        Assert.AreEqual (19, ast.Length)

    [<Test>]
    member x.NSString () =
        let ast, e = parseFile "CoreFoundation.NSString"
        let r = (sprintf "%A" ast)
        Assert.AreEqual (56, ast.Length)
    