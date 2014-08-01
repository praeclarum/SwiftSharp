namespace Swiften
module SwiftParser

open System
open System.Text

let invariantCulture = System.Globalization.CultureInfo.InvariantCulture

type Type =
    | IdentifierType of string

type Pattern =
    | IdentifierPattern of string * (Type option)


type Statement =
    | ExpressionStatement of Expression
    | VariableDeclaration of (Pattern * (Expression option)) list
    | ConstantDeclaration of (Pattern * (Expression option)) list

and Expression =
    | Number of float
    | Str of string
    | Variable of string
    | Compound of Expression * (Binary list)
    | Funcall of Expression * (((string option) * Expression) list) * ((Statement list) option)

and Binary = string * Expression


type Document =
    {
        Name : string
        Body : string
    }

type Position =
    {
        Document : Document
        Index : int
    }
    static member Beginning doc = { Document = doc; Index = 0 }
    member this.Advance count = { Document = this.Document; Index = this.Index + count; }
    member this.Eof = this.Index >= this.Document.Body.Length

let ws (i : Position) =
    if i.Eof then i
    else
        let b = i.Document.Body
        let n = b.Length
        if not (Char.IsWhiteSpace (b.[i.Index])) then i
        else
            let mutable e = i.Index + 1
            while e < n && Char.IsWhiteSpace (b.[e]) do e <- e + 1
            i.Advance (e - i.Index)


let (|Identifier|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if Char.IsLetter (ch) then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && Char.IsLetter (body.[e]) do e <- e + 1
            Some (body.Substring (i.Index, e - i.Index), i.Advance (e - i.Index))
        else None

let (|String|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if ch = '"' then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && body.[e] <> '"' do e <- e + 1
            Some (body.Substring (i.Index + 1, e - i.Index - 1), i.Advance (e - i.Index + 1))
        else None

let (|Token|) (text : string) i =
    let b = i.Document.Body
    let n = b.Length
    if i.Index + text.Length > n then None
    else
        match String.CompareOrdinal (text, 0, b, i.Index, text.Length) with
        | 0 -> Some (i.Advance text.Length)
        | _ -> None

let isNum c = Char.IsDigit (c) || c = '.'

let (|Number|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if isNum ch then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && isNum body.[e] do e <- e + 1
            Some (Double.Parse (body.Substring (i.Index, e - i.Index)), i.Advance (e - i.Index))
        else None

let (|Binary_operator|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if ch = '+' then Some (ch.ToString (), i.Advance 1)
        else None

let rec (|Type|) i =
    match i with
    | Type_identifier (Some r) -> Some r
    | _ -> None

and (|Type_identifier|) i =
    match i with
    | Identifier (Some (y, j)) -> Some (IdentifierType (y), j)
    | _ -> None


let (|Type_annotation|) i =
    match i with
    | Token ":" (Some j) ->
        match ws j with
        | Type (Some r) -> Some r
        | _ -> None
    | _ -> None

let rec (|Expression|) i =
    match i with
    | Prefix_expression (Some (pe, j)) ->
        match ws j with
        | Binary_expressions (Some (bes, k)) -> Some (Compound (pe, bes), k)
        | _ -> Some (pe, j)
    | _ -> None

and (|Primary_expression|) i =
    match i with
    | Identifier (Some (id, j)) -> Some (Variable id, j)
    | Literal (Some r) -> Some r
    | _ -> None

and (|Literal|) i =
    match i with
    | Number (Some (v, j)) -> Some (Number v, j)
    | String (Some (v, j)) -> Some (Str v, j)
    | _ -> None

and (|Trailing_closure|) i : (Statement list * Position) option = None

and (|Expression_element|) i =
    match i with
    | Identifier (Some (id, j)) ->
        match ws j with
        | Token ":" (Some k) ->
            match ws k with
            | Expression (Some (w, l)) -> Some ((Some id, w), l)
            | _ -> None
        | _ -> Some ((None, Variable id), j)
    | Expression (Some (y, j)) -> Some ((None, y), j)
    | _ -> None


and (|Expression_element_list|) i =
    match i with
    | Expression_element (Some (y,j)) ->
        match j with
        | Token "," (Some k) ->
            match k with
            | Expression_element_list (Some (w, l)) -> Some (y :: w, l)
            | _ -> None
        | _ -> Some ([y], j)
    | _ -> None

and (|Parenthesized_expression|) i =
    match i with
    | Token "(" (Some j) ->
        match j with
        | Token ")" (Some k) -> Some ([], k)
        | Expression_element_list (Some (z,k)) ->
            match k with
            | Token ")" (Some l) -> Some (z, l)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Function_call_expression|) i =
    match i with
    | Parenthesized_expression (Some (pae, j)) ->
        match j with
        | Trailing_closure (Some (tc, k)) -> Some ((fun pe -> Funcall (pe, pae, Some tc)), k)
        | _ -> Some ((fun pe -> Funcall (pe, pae, None)), j)
    | _ ->
        match i with
        | Trailing_closure (Some (tc, j)) -> Some ((fun pe -> Funcall (pe, [], Some tc)), j)
        | _ -> None

and (|Postfix_expression|) i =
    let postfix_expression_internal i =        
        match i with
        | Primary_expression (Some (y, j)) ->
            match j with
            | Function_call_expression (Some (z, k)) -> Some (z y, k) 
            | _ -> Some (y, j)
        | _ -> None
    postfix_expression_internal i


and (|Prefix_expression|) i =
    match i with
    | Postfix_expression (Some r) -> Some r
    | _ -> None


and (|Binary_expression|) i : (Binary * Position) option =
    match i with
    | Token "=" (Some j) ->
        match ws j with
        | Prefix_expression (Some (z, k)) -> Some (("=", z), k)
        | _ -> None
    | Binary_operator (Some (y, j)) ->
        match ws j with
        | Prefix_expression (Some (z, k)) -> Some ((y, z), k)
        | _ -> None
    | _ -> None

and (|Binary_expressions|) i =
    match i with
    | Binary_expression (Some (be, j)) ->
        match ws j with
        | Binary_expressions (Some (bes, k)) -> Some (be :: bes, k)
        | _ -> Some ([be], j)
    | _ -> None

let rec (|Pattern|) i =
    match i with
    | Identifier_pattern (Some (y, j)) ->
        match ws j with
        | Type_annotation (Some (z, k)) -> Some (IdentifierPattern (y, Some z), k)
        | _ -> Some (IdentifierPattern (y, None), j)
    | _ -> None

and (|Identifier_pattern|) i =
    match i with
    | Identifier (Some r) -> Some r
    | _ -> None

let rec (|Statement|) i =
    match i with
    | Declaration (Some r) -> Some r
    | Expression (Some (y,j)) -> Some (ExpressionStatement y, j)
    | _ -> None

and (|Declaration|) i =
    match i with
    | Constant_declaration (Some r) -> Some r
    | Variable_declaration (Some r) -> Some r
    | _ -> None

and (|Constant_declaration_head|) i =
    match i with
    | Token "let" (Some j) -> Some j
    | _ -> None


and (|Constant_declaration|) i =
    match i with
    | Constant_declaration_head (Some j) ->
        match ws j with
        | Pattern_initializer_list (Some (z, k)) -> Some (ConstantDeclaration z, k)            
        | _ -> None
    | _ -> None


and (|Variable_declaration_head|) i =
    match i with
    | Token "var" (Some j) -> Some j
    | _ -> None


and (|Variable_declaration|) i =
    match i with
    | Variable_declaration_head (Some j) ->
        match ws j with
        | Pattern_initializer_list (Some (z, k)) -> Some (VariableDeclaration z, k)            
        | _ -> None
    | _ -> None
        
and (|Pattern_initializer_list|) i =
    match i with
    | Pattern_initializer (Some (y,j)) ->
        match j with
        | Token "," (Some k) ->
            match k with
            | Pattern_initializer_list (Some (w, l)) -> Some (y :: w, l)
            | _ -> None
        | _ -> Some ([y], j)
    | _ -> None
        
and (|Pattern_initializer|) i =
    match i with
    | Pattern (Some (y,j)) ->
        match ws j with
        | Initializer (Some (z, k)) -> Some ((y, Some z), k)
        | _ -> Some ((y, None), j)
    | _ -> None

and (|Initializer|) i =
    match i with
    | Token "=" (Some j) ->
        match ws j with
        | Expression (Some (z, k)) -> Some (z, k)
        | _ -> None
    | _ -> None

let rec (|Statements|) i =
    match i with
    | Statement (Some (y, j)) ->
        match ws j with
        | Statements (Some (z, k)) -> Some (y :: z, k)
        | _ -> Some ([y], j)
    | _ -> None


let parseDocument document =
    match Position.Beginning document |> ws with
    | Statements (Some r) -> Some r
    | _ -> None
let parseFile path = parseDocument { Name = path; Body = System.IO.File.ReadAllText (path) }
let parseText text = parseDocument { Name = "<text>"; Body = text }
let toPosition text = Position.Beginning { Name = "<text>"; Body = text }



module Tests =

    //#load "../Swiften/SwiftParser.fs";;

    let SimpleValues =
        [
            parseText "var myVariable = 42\nmyVariable = 50\nlet myConstant = 42"

        ]


    let pl = 
        [
            parseText """println"""
            parseText """println()"""
            parseText """println(3)"""
            parseText """println(x:3)"""
            parseText """println("Hello, world")"""
        ]


    let abx = parseText "Aaa+B+Ccc"
    let a2 = parseText "Aaa+2"
    let tb = parseText "33+B"
    let t2 = parseText "33+2"
