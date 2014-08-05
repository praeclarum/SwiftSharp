
module SwiftSharp.SwiftParser

open System
open System.Text

let invariantCulture = System.Globalization.CultureInfo.InvariantCulture

type Type =
    | IdentifierType of string
    | ImplicitlyUnwrappedOptionalType of Type

type Pattern =
    | IdentifierPattern of string * (Type option)


type Statement =
    | ExpressionStatement of Expression
    | DeclarationStatement of Declaration

and Declaration =
    | ImportDeclaration of string list
    | VariableDeclaration of (Pattern * (Expression option)) list
    | ConstantDeclaration of (Pattern * (Expression option)) list
    | TypealiasDeclaration of string * Type
    | StructDeclaration of string * (Type list) * (Declaration list)
    | InitializerDeclaration of string list

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
    member this.PreviousText =
        let n = Math.Min (this.Index, 32)
        this.Document.Body.Substring (this.Index - n, n)

//
// Lexing
//

let rec ws (i : Position) : Position =
    if i.Eof then i
    else
        let b = i.Document.Body
        let n = b.Length
        match b.[i.Index] with
        | '/' ->
            if i.Index + 1 < n && b.[i.Index + 1] = '*' then
                i |> comment
            else
                if i.Index + 1 < n && b.[i.Index + 1] = '/' then
                    i |> lineComment
                else i
        | x when not (Char.IsWhiteSpace (x)) -> i
        | _ ->
            let mutable e = i.Index + 1
            while e < n && Char.IsWhiteSpace (b.[e]) do e <- e + 1
            i.Advance (e - i.Index) |> ws

and comment (i : Position) : Position =
    let b = i.Document.Body
    let n = b.Length
    let mutable e = i.Index + 2
    while e < n && not (b.[e] = '*' && e + 1 < n && b.[e+1] = '/') do e <- e + 1
    e <- e + 2 // Skip the star slash
    i.Advance (e - i.Index) |> ws

and lineComment (i : Position) : Position =
    let b = i.Document.Body
    let n = b.Length
    let mutable e = i.Index + 2
    while e < n && b.[e] <> '\n' do e <- e + 1
    e <- e + 1 // Skip the \n
    i.Advance (e - i.Index) |> ws

let isTrailIdent ch = Char.IsLetterOrDigit (ch) || ch = '_'

let (|Identifier|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if Char.IsLetter (ch) || ch = '_' then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && isTrailIdent body.[e] do e <- e + 1
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

//
// Combinators
//

let rec oneOrMore pattern p1 =
    match pattern p1 with
    | Some (v1, p2) ->
        match oneOrMore pattern (ws p2) with
        | Some (v2, p3) -> Some (v1 :: v2, p3)
        | _ -> Some ([v1], p2)
    | _ -> None

let rec oneOrMoreSep pattern sep p1 =
    match pattern p1 with
    | Some (v1, p2) ->
        match ws p2 with
        | Token sep (Some p3) ->
            match oneOrMoreSep pattern sep (ws p3) with
            | Some (v2, p3) -> Some (v1 :: v2, p3)
            | _ -> None
        | _ -> Some ([v1], p2)
    | _ -> None


//
// Grammar
//

let rec (|Type|) i =
    let matchType = function
        | Type_identifier (Some r) -> Some r
        | _ -> None

    // Look for '!'
    match matchType i with
    | Some (v1, p2) ->
        match ws p2 with
        | Token "!" (Some p3) -> Some (ImplicitlyUnwrappedOptionalType v1, p3)
        | _ -> Some (v1, p2)
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

and (|Expression_element_list|) = oneOrMoreSep (|Expression_element|) ","

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

let rec (|Statement|) p1 =
    match p1 with
    | Declaration (Some (v1, p2)) -> Some (DeclarationStatement v1, p2)
    | Expression (Some (v1, p2)) -> Some (ExpressionStatement v1, p2)
    | _ -> None

and (|Declaration|) p1 =
    match p1 with
    | Import_declaration (Some r) -> Some r
    | Constant_declaration (Some r) -> Some r
    | Variable_declaration (Some r) -> Some r
    | Typealias_declaration (Some r) -> Some r
    | Struct_declaration (Some r) -> Some r
//    | Initializer_declaration (Some r) -> Some r
    | _ -> None



//struct_declaration
//    : attributes STRUCT struct_name generic_parameter_clause type_inheritance_clause struct_body
//    | attributes STRUCT struct_name generic_parameter_clause struct_body
//    | attributes STRUCT struct_name type_inheritance_clause struct_body
//    | attributes STRUCT struct_name struct_body
//    | STRUCT struct_name generic_parameter_clause type_inheritance_clause struct_body
//    | STRUCT struct_name generic_parameter_clause struct_body
//    | STRUCT struct_name type_inheritance_clause struct_body
//    | STRUCT struct_name struct_body
//    ;
and (|Struct_declaration|) i =
    match i with
    | Token "struct" (Some p2) ->
        match ws p2 with
        | Struct_name (Some (v2, p3)) ->
            match ws p3 with
            | Type_inheritance_clause (Some (v3, p4)) ->
                match ws p4 with
                | Struct_body (Some (v4, p5)) -> Some (StructDeclaration (v2, v3, v4), p5)
                | _ -> None
            | Struct_body (Some (v3, p4)) -> Some (StructDeclaration (v2, [], v3), p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Struct_name|) = (|Identifier|)

and (|Type_inheritance_clause|) p1 =
    match p1 with
    | Token ":" (Some p2) ->
        match ws p2 with
        | Type_inheritance_list (Some n2) -> Some n2
        | _ -> None
    | _ -> None

and (|Type_inheritance_list|) = oneOrMoreSep (|Type_identifier|) ","

and (|Struct_body|) p1 =
    match p1 with
    | Token "{" (Some p2) ->
        match ws p2 with
        | Token "}" (Some p3) -> Some ([], p3)
        | Declarations (Some n2) -> Some n2
        | _ -> None
    | _ -> None

and (|Import_declaration|) = function
    | Token "import" (Some j) ->
        match ws j with
        | Import_path (Some (z, k)) -> Some (ImportDeclaration z, k)
        | _ -> None
    | _ -> None


and (|Declarations|) = oneOrMore (|Declaration|)

and (|Import_path_identifier|) = (|Identifier|)

and (|Import_path|) = oneOrMoreSep (|Import_path_identifier|) ","

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

and (|Typealias_head|) i =
    match i with
    | Token "typealias" (Some p1) ->
        match ws p1 with
        | Identifier (Some n2) -> Some n2
        | _ -> None
    | _ -> None

and (|Typealias_assignment|) = function
    | Token "=" (Some p1) ->
        match ws p1 with
        | Type (Some n2) -> Some n2
        | _ -> None
    | _ -> None

/// typealias-declaration → typealias-head typealias-assignment
and (|Typealias_declaration|) = function
    | Typealias_head (Some (v1, p2)) ->
        match ws p2 with
        | Typealias_assignment (Some (v2, p3)) -> Some (TypealiasDeclaration (v1, v2), p3)
        | _ -> None
    | _ -> None
        
and (|Pattern_initializer|) i =
    match i with
    | Pattern (Some (y,j)) ->
        match ws j with
        | Initializer (Some (z, k)) -> Some ((y, Some z), k)
        | _ -> Some ((y, None), j)
    | _ -> None

and (|Pattern_initializer_list|) = oneOrMoreSep (|Pattern_initializer|) ","

and (|Initializer|) i =
    match i with
    | Token "=" (Some j) ->
        match ws j with
        | Expression (Some (z, k)) -> Some (z, k)
        | _ -> None
    | _ -> None

let (|Statements|) = oneOrMore (|Statement|)

let parseDocument document =
    match Position.Beginning document |> ws with
    | Statements (Some r) -> Some r
    | _ -> None
let parseFile path = parseDocument { Name = path; Body = System.IO.File.ReadAllText (path) }
let parseText text = parseDocument { Name = "<text>"; Body = text }
let toPosition text = Position.Beginning { Name = "<text>"; Body = text }

