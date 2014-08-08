namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp.SwiftParser

[<TestFixture>]
type HeaderTests () =

    let parseFile path =
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
    member x.SelfPropAssign() =
        let ast, e = parseCode """
class GovDataRequest {   
    init(APIKey: String, APIHost: String, APIURL:String) {
        self.foo.APIKey = APIKey
    }
}
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.Switch() =
        let ast, e = parseCode """
class GovDataRequest {      
    func callAPIMethod () {
        switch APIHost {
        case "http://api.dol.gov":
            queryString = "?KEY=" + APIKey
        default:
            println("doing nothing for now")
        }
    }
}
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.HashParam() =
        let ast, e = parseCode """
class GovDataRequest {   
    func callAPIMethod (#method: String, arguments: Dictionary<String,String>) {
    }
}
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)


    [<Test>]
    member x.ForIn() =
        let ast, e = parseCode """
class GovDataRequest {   
    func callAPIMethod () {
        for (argKey, argValue) in arguments {
            x = 42
        }
    }
}
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    [<Test>]
    member x.GovDataRequest () =
        let ast, e = parseFile "GovDataRequest"
        let s = (sprintf "%A" ast)
        Assert.AreEqual (5, ast.Length)

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
    