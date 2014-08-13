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
    member x.ArrayLiteral() =
        let ast = parseCode """
            [a]
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (ArrayLiteral _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.DictionaryLiteral() =
        let ast = parseCode """
            [a: b]
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (DictionaryLiteral _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.AsOptional() =
        let ast = parseCode """
            jsonResult as? String
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Compound _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.TernaryConditional() =
        let ast = parseCode """
a ? b : c
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Compound _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.Subscript() =
        let ast = parseCode """
        rows[0]
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Subscript _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.EnumCtor() =
        let ast = parseCode """
        .Row (0)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s)


    [<Test>]
    member x.Funcall() =
        let ast = parseCode "f(x)"
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s)

    [<Test>]
    member x.ClosureExpression() =
        let ast = parseCode """
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
        let ast = parseCode """
self.delegate?.didCompleteWithError(error.localizedDescription)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Funcall _)] -> ()
        | _ -> Assert.Fail (s);


    [<Test>]
    member x.AsCast() =
        let ast = parseCode """
JSONObjectWithData as NSDictionary
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (Compound (Variable _, [AsCastBinary _]))] -> ()
        | _ -> Assert.Fail (s);

    [<Test>]
    member x.InOut() =
        let ast = parseCode """
&err
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement (InOut "err")] -> ()
        | _ -> Assert.Fail (s);


    [<Test>]
    member x.Tuple1Expr() =
        let ast = parseCode """
                (5)
"""
        let s = (sprintf "%A" ast)
        match ast with
        | [ExpressionStatement _] -> ()
        | _ -> Assert.Fail (s);

    [<Test>]
    member x.BinaryOperators() =
        let ast = parseCode """
            switch APIHost {
            case "http://api.dol.gov":
                queryString += "&$"
            }
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)



    [<Test>]
    member x.SelfPropAssign() =
        let ast = parseCode """
class GovDataRequest {   
    init(APIKey: String, APIHost: String, APIURL:String) {
        self.foo.APIKey = APIKey
    }
}
"""
        let s = (sprintf "%A" ast)
        Assert.AreEqual (1, ast.Length)

    