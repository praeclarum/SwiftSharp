namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp.SwiftParser

[<TestFixture>]
type HeaderTests () =

    let parse path =
        match parseFile ("TestHeaders/" + path + ".swift") with
        | Some x -> x
        | x -> failwith (sprintf "Parse returned: %A" x)
    
    let parseCode code =
        match parseText code with
        | Some x -> x
        | x -> failwith (sprintf "Parse returned: %A" x)

        //#load "../SwiftSharp/SwiftParser.fs";;open SiftParser;;


    [<Test>]
    member x.NewlineAtEndOfFuncDeclWithType() =
        let ast, e = parseCode """
class NSString {
    func characterAtIndex(index: Int) -> unichar
}
    """
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.DotDotDotParams() =
        let ast, e = parseCode """
extension NSString {
    convenience init(format: NSString, _ args: CVarArg...)
}"""
        let r = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.NewlineAtEndOfFuncDecl() =
        let ast, e = parseCode """
extension NSString {    
    func getCharacters(buffer: UnsafePointer<unichar>, range aRange: NSRange)
}
    """
        let r = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.ArrayType() =
        let ast, e = parseCode """
extension NSString {
    func componentsSeparatedByString(separator: String!) -> [AnyObject]!
}   
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.FunctionType() =
        let ast, e = parseCode """
extension NSString {
    func enumerateLinesUsingBlock(block: ((String!, UnsafePointer<ObjCBool>) -> Void)!)
}   
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.UIView () =
        let ast, e = parse "UIKit.UIView"
        Assert.AreEqual (0, ast.Length)

    [<Test>]
    member x.UIViewController () =
        let ast, e = parse "UIKit.UIViewController"
        Assert.AreEqual (0, ast.Length)

    [<Test>]
    member x.NSString () =
        let ast, e = parse "CoreFoundation.NSString"
        let r = (sprintf "%A" ast)
        Assert.AreEqual (56, ast.Length)
    