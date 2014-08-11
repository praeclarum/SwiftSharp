namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftParser

[<TestFixture>]
type ExpressionTests () =

    let parseCode code =
        match parseText code with
        | Some x -> x
        | x -> failwith (sprintf "Parse returned: %A" x)


    [<Test>]
    member x.Subscript() =
        let ast, e = parseCode """
        rows[0]
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Subscript _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.EnumCtor() =
        let ast, e = parseCode """
        .Row (0)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s)


    [<Test>]
    member x.Funcall() =
        let ast, e = parseCode "f(x)"
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.ClosureExpression() =
        let ast, e = parseCode """
            session.dataTaskWithRequest(request, completionHandler: {data, response, error -> Void in
             2 + 2
                })
 """
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s);

    [<Test>]
    member x.OptionalChaining() =
        let ast, e = parseCode """
self.delegate?.didCompleteWithError(error.localizedDescription)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s);


    [<Test>]
    member x.AsCast() =
        let ast, e = parseCode """
JSONObjectWithData as NSDictionary
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Compound (Variable _, [AsBinary _]))] -> ()
        | _ -> Assert.Fail (s);

    [<Test>]
    member x.InOut() =
        let ast, e = parseCode """
&err
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (InOut "err")] -> ()
        | _ -> Assert.Fail (s);


    [<Test>]
    member x.Tuple1Expr() =
        let ast, e = parseCode """
                (5)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement _] -> ()
        | _ -> Assert.Fail (s);

    [<Test>]
    member x.BinaryOperators() =
        let ast, e = parseCode """
            switch APIHost {
            case "http://api.dol.gov":
                queryString += "&$"
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

    