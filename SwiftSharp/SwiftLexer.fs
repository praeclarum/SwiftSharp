
module SwiftSharp.SwiftLexer

open System
open System.Text

let invariantCulture = System.Globalization.CultureInfo.InvariantCulture


//
// Lexer
//

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
        let body = this.Document.Body
        let e = if this.Index < body.Length then this.Index + 1 else body.Length
        let b = if e - 32 >= 0 then e - 32 else 0
        body.Substring (b, e - b)

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

let isOperatorCharacter ch =
    match ch with
    | '/' | '=' | '-' | '+' | '!' | '*' | '%' | '<' | '>' | '&' | '|' | '^' | '~' | '.' -> true
    | _ -> false

let (|Operator|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if isOperatorCharacter ch then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && (isOperatorCharacter body.[e]) do e <- e + 1
            Some (body.Substring (i.Index, e - i.Index), i.Advance (e - i.Index))
        else None

let isTrailIdent ch = Char.IsLetterOrDigit (ch) || ch = '_'

let isKeyword = function
    | "class" | "deinit" | "enum" | "extension" | "func" | "import" | "init" | "internal" | "let" | "operator" | "private" | "protocol" | "public" | "static" | "struct" | "subscript" | "typealias" | "var"
    | "break" | "case" | "continue" | "default" | "do" | "else" | "fallthrough" | "for" | "if" | "in" | "return" | "switch" | "where" | "while" -> true
    | _ -> false

let (|Identifier|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if Char.IsLetter (ch) || ch = '_' then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && isTrailIdent body.[e] do e <- e + 1
            let name = body.Substring (i.Index, e - i.Index)
            if isKeyword name then None else Some (name, i.Advance (e - i.Index))
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

let (|Newline|) p1 =
    let b = p1.Document.Body
    let n = b.Length
    let p2 = ws p1
    match b.IndexOf ('\n', p1.Index, p2.Index - p1.Index) with
    | x when x < 0 -> None
    | _ -> Some p2

let (|Token|) (text : string) i =
    let b = i.Document.Body
    let n = b.Length
    if i.Index + text.Length > n then None
    else
        let sss = b.Substring (i.Index, text.Length)
        match String.CompareOrdinal (text, 0, b, i.Index, text.Length) with
        | 0 -> Some (i.Advance text.Length)
        | _ -> None

let (|TokenValue|) (text : string) p1 =
    match p1 with
    | Token text (Some p2) -> Some (text, p2)
    | _ -> None

let kwd x = (|TokenValue|) x

let br x = (|TokenValue|) x

let isNum c = Char.IsDigit (c) || c = '.'

let isLeadNum c = Char.IsDigit (c)

let (|Number|) (i : Position) =
    if i.Eof then None
    else
        let ch = i.Document.Body.[i.Index]
        if isLeadNum ch then
            let body = i.Document.Body
            let n = body.Length
            let mutable e = i.Index + 1
            while e < n && isNum body.[e] do e <- e + 1
            let numStr = body.Substring (i.Index, e - i.Index)
            Some (Double.Parse (numStr), i.Advance (e - i.Index))
        else None

let (|Binary_operator|) = (|Operator|)

//
// Combinators
//

let opt pattern p1 =
    match pattern p1 with
    | Some (v1, p2) -> Some (Some v1, p2)
    | _ -> Some (None, p1)

let rec oneOrMore pattern p1 =
    match pattern p1 with
    | Some (v1, p2) ->
        match oneOrMore pattern (ws p2) with
        | Some (v2, p3) -> Some (v1 :: v2, p3)
        | _ -> Some ([v1], p2)
    | _ -> None

let rec zeroOrMore pattern p1 =
    match pattern p1 with
    | Some (v1, p2) ->
        match zeroOrMore pattern (ws p2) with
        | Some (v2, p3) -> Some (v1 :: v2, p3)
        | _ -> Some ([v1], p2)
    | _ -> Some ([], p1)

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

let (|||) x y p1 =
    match x p1 with
    | Some (v1, p2) -> Some (v1, p2)
    | _ ->
        match y p1 with
        | Some (v1, p2) -> Some (v1, p2)
        | _ -> None

let (&&&) x y p1 =
    match x p1 with
    | Some (v1, p2) ->
        match y (ws p2) with
        | Some (v2, p3) -> Some ((v1, v2), p3)
        | _ -> None
    | _ -> None

let okwd x = x |> kwd |> opt

let nokwd x p1 =
    match kwd x p1 with
    | Some (v2, p2) -> Some (Some v2, p2)
    | _ -> None
