
module SwiftSharp.SwiftParser

open System
open System.Text

let invariantCulture = System.Globalization.CultureInfo.InvariantCulture

type Type =
    | IdentifierType of string * (Type list)
    | NestedType of Type list
    | ImplicitlyUnwrappedOptionalType of Type
    | OptionalType of Type
    | FunctionType of Type * Type
    | TupleType of (Type list) * bool
    | ArrayType of Type
    | DictionaryType of Type * Type

type Attribute = string * string
type Parameter = string * string * (Type option)

type DeclarationSpecifier =
    | Static
    | Override
    | Class

type Statement =
    | ExpressionStatement of Expression
    | DeclarationStatement of Declaration
    | SwitchStatement of Expression * (SwitchCase list)
    | ForInStatement of Pattern * Expression * (Statement list)
    | IfStatement of Expression * (Statement list) * ((Statement list) option)

and SwitchCase = (Pattern list) * (Statement list)

and Declaration =
    | ImportDeclaration of string list
    | GetterSetterVariableDeclaration of (DeclarationSpecifier list) * (string * Type) * ((Statement list) * ((Statement list) option))
    | PatternVariableDeclaration of (DeclarationSpecifier list) * ((Pattern * (Expression option)) list)
    | ConstantDeclaration of (Pattern * (Expression option)) list
    | TypealiasDeclaration of string * Type
    | StructDeclaration of string * (Type list) * (Declaration list)
    | ClassDeclaration of string * (Type list) * (Declaration list)
    | InitializerDeclaration of (Parameter list) * (Statement list)
    | FunctionDeclaration of (DeclarationSpecifier list) * string * ((Parameter list) list) * (FunctionResult option) * (Statement list)
    | ExtensionDeclaration of string * Type * ((Type list) option) * (Declaration list)
    | ProtocolDeclaration of string * string * ((Type list) option) * (Declaration list)
    | RawValueEnumDeclaration of string * (Type list) * ((string list) list)

and Expression =
    | Number of float
    | Str of string
    | Variable of string
    | Compound of Expression * (Binary list)
    | Funcall of Expression * (((string option) * Expression) list) * ((Statement list) option)
    | ExplicitMember of Expression * string
    | Closure of (((Parameter list) * (FunctionResult option)) option) * (Statement list)
    | OptionalChaining of Expression
    | InOut of string
    | TupleExpr of ((string option) * Expression) list

and FunctionResult = ((Attribute list) * Type)

and Binary =
    | OpBinary of string * Expression
    | AsBinary of Type

and Pattern =
    | IdentifierPattern of string * (Type option)
    | ExpressionPattern of Expression
    | TuplePattern of (Pattern list) * (Type option)


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
        this.Document.Body.Substring (this.Index - n, n + 1)

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


//
// Grammar
//

let rec (|Type|) i =
    let matchType = function
        | Array_type (Some r) -> Some r
        | Dictionary_type (Some r) -> Some r
        | Type_identifier (Some r) -> Some r
        | Tuple_type (Some r) -> Some r
        | _ -> None

    // Look for '!' or '?'
    match matchType i with
    | Some (v1, p2) ->
        match ws p2 with
        | Token "!" (Some p3) -> Some (ImplicitlyUnwrappedOptionalType v1, p3)
        | Token "?" (Some p3) -> Some (OptionalType v1, p3)
        | Token "->" (Some p3) ->
            match ws p3 with
            | Type (Some (v3, p4)) -> Some (FunctionType (v1, v3), p4)
            | _ -> None
        | _ -> Some (v1, p2)
    | _ -> None


and (|Type_name|) = (|Identifier|)

and (|Generic_argument|) = (|Type|)

and (|Generic_argument_list|) = oneOrMoreSep (|Generic_argument|) ","

and (|Tuple_type_element|) = (|Type|)

and (|Tuple_type_element_list|) = oneOrMoreSep (|Tuple_type_element|) ","

and (|Tuple_type_body|) p1 =
    match p1 with
    | Tuple_type_element_list (Some (v1, p2)) ->
        match ws p2 with
        | Token "..." (Some p3) -> Some ((v1, true), p3)
        | _ -> Some ((v1, false), p2)
    | _ -> None

and (|Tuple_type|) p1 =
    match ((br "(") &&& (opt (|Tuple_type_body|)) &&& (br ")")) p1 with
    | Some (((v1, Some v2), v3), p4) -> Some (TupleType v2, p4)
    | Some (((v1, None), v3), p4) -> Some (TupleType ([], false), p4)
    | _ -> None

and (|Array_type|) p1 =
    match ((br "[") &&& (|Type|) &&& (br "]")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (ArrayType v2, p4)
    | _ -> None

and (|Dictionary_type|) p1 =
    match ((br "[") &&& (|Type|) &&& (br ":") &&& (|Type|) &&& (br "]")) p1 with
    | Some (((((v1, v2), v3), v4), v5), p4) -> Some (DictionaryType (v2, v4), p4)
    | _ -> None

and (|Generic_argument_clause|) p1 =
    match ((br "<") &&& (|Generic_argument_list|) &&& (br ">")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

and (|Type_identifier|) p1 =
    let justId =
        match p1 with
        | Type_name (Some (v1, p2)) ->
            match ws p2 with
            | Generic_argument_clause (Some (v2, p3)) ->
                Some (IdentifierType (v1, v2), p3)
            | _ -> Some (IdentifierType (v1, []), p2)
        | _ -> None

    match justId with
    | Some (idt, p3) ->
        match ws p3 with
        | Token "." (Some p4) ->
            match ws p4 with
            | Type_identifier (Some (NestedType v4, p5)) ->
                Some (NestedType (idt :: v4), p5)
            | Type_identifier (Some (IdentifierType (v4, v5), p6)) ->
                Some (NestedType (idt :: [IdentifierType (v4, v5)]), p6)
            | _ -> Some (idt, p3)
        | _ -> Some (idt, p3)
    | _ -> None


and (|Type_annotation|) i =
    match i with
    | Token ":" (Some j) ->
        match ws j with
        | Type (Some r) -> Some r
        | _ -> None
    | _ -> None

and (|Expression|) i =
    match i with
    | Prefix_expression (Some (pe, j)) ->
        match ws j with
        | Binary_expressions (Some (bes, k)) -> Some (Compound (pe, bes), k)
        | _ -> Some (pe, j)
    | _ -> None

and (|Identifier_list|) = oneOrMoreSep (|Identifier|) ","

and (|Closure_signature|) p1 =
    let tail e p1 = 
        match ((opt (|Function_result|)) &&& (kwd "in")) p1 with
        | Some ((v1, v2), p4) -> Some ((e, v1), p4)
        | _ -> None
    match p1 with
    | Parameter_clause (Some (v1, p2)) -> tail v1 (ws p2)
    | Identifier_list (Some (v1, p2)) -> 
        let ps = v1 |> List.map (fun x -> ("_", x, None))
        tail ps (ws p2)
    | _ -> None

and (|Closure_expression|) p1 =
    match ((br "{") &&& (opt (|Closure_signature|)) &&& (|Statements|) &&& (br "}")) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (Closure (v2, v3), p5)
    | _ -> None

and (|Primary_expression|) p1 =
    match p1 with
    | Identifier (Some (id, j)) -> Some (Variable id, j)
    | Literal (Some r) -> Some r
    | Closure_expression (Some r) -> Some r
    | Parenthesized_expression (Some (v1, p2)) ->
        match v1 with
        | [(None, x)] ->  Some  (x, p2)
        | _ -> Some (TupleExpr v1, p2)
    | _ -> None

and (|Literal|) i =
    match i with
    | Number (Some (v, j)) -> Some (Number v, j)
    | String (Some (v, j)) -> Some (Str v, j)
    | _ -> None

and (|Trailing_closure|) i : (Statement list * Position) option = None

and (|Expression_element|) p1 =
    match p1 with
    | Identifier (Some (v1, p2)) ->
        match ws p2 with
        | Token ":" (Some p3) ->
            match ws p3 with
            | Expression (Some (v3, p4)) -> Some ((Some v1, v3), p4)
            | _ -> None
        | _ ->
            match p1 with
            | Expression (Some (v1, p2)) -> Some ((None, v1), p2)
            | _ -> None
    | Expression (Some (v1, p2)) -> Some ((None, v1), p2)
    | _ -> None

and (|Expression_element_list|) = oneOrMoreSep (|Expression_element|) ","

and (|Parenthesized_expression|) p1 : ((((string option) * Expression) list) * Position) option =
    match p1 with
    | Token "(" (Some p2) ->
        match ws p2 with
        | Token ")" (Some p3) -> Some ([], p3)
        | Expression_element_list (Some (v2, p3)) ->
            match ws p3 with
            | Token ")" (Some p4) -> Some (v2, p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Function_call_expression|) pe i =
    match i with
    | Parenthesized_expression (Some (pae, j)) ->
        match j with
        | Trailing_closure (Some (tc, k)) -> Some (Funcall (!pe, pae, Some tc), k)
        | _ -> Some (Funcall (!pe, pae, None), j)
    | _ ->
        match i with
        | Trailing_closure (Some (tc, j)) -> Some (Funcall (!pe, [], Some tc), j)
        | _ -> None


and (|Explicit_member_expression|) e p1 =
    match p1 with
    | Token "." (Some p2) ->
        match (ws p2) with
        | Identifier (Some (v2, p3)) -> Some (ExplicitMember (!e, v2), p3)
        | _ -> None
    | _ -> None

and (|Optional_chaining_expression|) e p1 =
    match p1 with
    | Token "?" (Some p2) -> Some (OptionalChaining !e, p2)
    | _ -> None

and (|Postfix_expression|) p1 =
    match p1 with
    | Primary_expression (Some (v1, p2)) ->
        let e = ref v1
        let p = ref p2
        let rec loop = function
            | Function_call_expression e (Some (v2, p3))
            | Optional_chaining_expression e (Some (v2, p3))
            | Explicit_member_expression e (Some (v2, p3)) ->
                e := v2
                p := p3
                loop (ws p3)
            | _ -> ()
        loop (ws p2)
        Some (!e, !p)
    | _ -> None

and (|In_out_expression|) p1 =
    match ((br "&") &&& (|Identifier|)) p1 with
    | Some ((v1, v2), p3) -> Some (InOut v2, p3)
    | _ -> None

and (|Prefix_expression|) i =
    match i with
    | In_out_expression (Some r) -> Some r
    | Postfix_expression (Some r) -> Some r
    | _ -> None

and (|Type_casting_operator|) p1 =
    match p1 with
    | Token "as" (Some p2) ->
        match ws p2 with
        | Type (Some (v2, p3)) -> Some (AsBinary v2, p3)
        | _ -> None
    | _ -> None

and (|Binary_expression|) p1 : (Binary * Position) option =
    match p1 with
    | Binary_operator (Some (v1, p2)) ->
        match ws p2 with
        | Prefix_expression (Some (v2, p3)) -> Some (OpBinary (v1, v2), p3)
        | _ -> None
    | Type_casting_operator (Some (v1, p2)) -> Some (v1, p2)
    | _ -> None

and (|Binary_expressions|) i =
    match i with
    | Binary_expression (Some (be, j)) ->
        match ws j with
        | Binary_expressions (Some (bes, k)) -> Some (be :: bes, k)
        | _ -> Some ([be], j)
    | _ -> None

and (|Pattern|) p1 =
    match p1 with
    | Identifier_pattern (Some (v1, p2)) ->
        match ws p2 with
        | Type_annotation (Some (v2, p3)) -> Some (IdentifierPattern (v1, Some v2), p3)
        | _ -> Some (IdentifierPattern (v1, None), p2)
    | Tuple_pattern (Some (v1, p2)) ->
        match ws p2 with
        | Type_annotation (Some (v2, p3)) -> Some (TuplePattern (v1, Some v2), p3)
        | _ -> Some (TuplePattern (v1, None), p2)
    | Expression (Some (v1, p2)) -> Some (ExpressionPattern v1, p2)
    | _ -> None

and (|Tuple_pattern_element_list|) = oneOrMoreSep (|Pattern|) ","

and (|Tuple_pattern|) p1 =
    match p1 with
    | Token "(" (Some p2) ->
        match ws p2 with
        | Token ")" (Some p3) -> Some ([], p3)
        | Tuple_pattern_element_list (Some (v2, p3)) ->
            match ws p3 with
            | Token ")" (Some p4) -> Some (v2, p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Identifier_pattern|) i =
    match i with
    | Identifier (Some r) -> Some r
    | _ -> None

and (|Statement|) p1 =
    match p1 with
    | Declaration (Some (v1, p2)) -> Some (DeclarationStatement v1, p2)
    | Branch_statement (Some (v1, p2)) -> Some (v1, p2)
    | Loop_statement (Some (v1, p2)) -> Some (v1, p2)
    | Expression (Some (v1, p2)) -> Some (ExpressionStatement v1, p2)
    | _ -> None

and (|Statements|) = oneOrMore (|Statement|)

and (|Case_item|) = (|Pattern|)

and (|Case_label|) p1 =
    match ((kwd "case") &&& (oneOrMoreSep (|Case_item|) ",") &&& (br ":")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

and (|Default_label|) p1 =
    match ((kwd "default") &&& (br ":")) p1 with
    | Some (_, p3) -> Some ([], p3)
    | _ -> None

and (|Switch_case|) = ((|Case_label|) &&& (|Statements|)) ||| ((|Default_label|) &&& (|Statements|))

and (|Switch_cases_opt|) = zeroOrMore (|Switch_case|)

and (|Switch_statement|) p1 =
    match ((kwd "switch") &&& (|Expression|) &&& (br "{") &&& (|Switch_cases_opt|) &&& (br "}")) p1 with
    | Some (((((v1, v2), v3), v4), v5), p6) -> Some (SwitchStatement (v2, v4), p6)
    | _ -> None
        
and (|If_condition|) p1 =
    match p1 with
    | Expression (Some (v1, p2)) -> Some (v1, p2)
    | _ -> None

and (|Else_clause|) p1 =
    match p1 with
    | Token "else" (Some p2) ->
        match ws p2 with
        | Code_block (Some (v2, p3)) -> Some (v2, p3)
        | If_statement (Some (v2, p3)) -> Some ([v2], p3)
        | _ -> None
    | _ -> None

and (|If_statement|) p1 =
    match ((kwd "if") &&& (|If_condition|) &&& (|Code_block|) &&& (opt (|Else_clause|))) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (IfStatement (v2, v3, v4), p5)
    | _ -> None

and (|Branch_statement|) = (|Switch_statement|) ||| (|If_statement|)

and (|For_in_statement|) p1 =
    match ((kwd "for") &&& (|Pattern|) &&& (kwd "in") &&& (|Expression|) &&& (|Code_block|)) p1 with
    | Some (((((v1, v2), v3), v4), v5), p6) -> Some (ForInStatement (v2, v4, v5), p6)
    | _ -> None

and (|Loop_statement|) = (|For_in_statement|)

and (|Declaration|) p1 =
    match p1 with
    | Import_declaration (Some r) -> Some r
    | Constant_declaration (Some r) -> Some r
    | Variable_declaration (Some r) -> Some r
    | Typealias_declaration (Some r) -> Some r
    | Struct_declaration (Some r) -> Some r
    | Class_declaration (Some r) -> Some r
    | Protocol_declaration (Some r) -> Some r
    | Initializer_declaration (Some r) -> Some r
    | Function_declaration (Some r) -> Some r
    | Extension_declaration (Some r) -> Some r
    | Enum_declaration (Some r) -> Some r
    | _ -> None

and (|Declarations|) = oneOrMore (|Declaration|)

and (|Declarations_opt|) = zeroOrMore (|Declaration|)

and (|Enum_name|) = (|Identifier|)

and (|Enum_case_name|) = (|Identifier|)

and (|Raw_value_style_enum_case|) = (|Enum_case_name|)

and (|Raw_value_style_enum_case_list|) = oneOrMoreSep (|Raw_value_style_enum_case|) ","

and (|Raw_value_style_enum_case_clause|) p1 =
    match ((kwd "case") &&& (|Raw_value_style_enum_case_list|)) p1 with
    | Some ((v1, v2), p3) -> Some (v2, p3)
    | _ -> None

and (|Raw_value_style_enum_member|) = (|Raw_value_style_enum_case_clause|)

and (|Raw_value_style_enum_members|) = oneOrMore (|Raw_value_style_enum_member|)

and (|Raw_value_style_enum|) p1 =
    match ((kwd "enum") &&& (|Enum_name|) &&& (|Type_inheritance_clause|) &&& (br "{") &&& (|Raw_value_style_enum_members|) &&& (br "}")) p1 with
    | Some ((((((v1, v2), v3), v4), v5), v6), p6) -> Some (RawValueEnumDeclaration (v2, v3, v5), p6)
    | _ -> None

and (|Enum_declaration|) = (|Raw_value_style_enum|)

and (|Initializer_head|) = (opt ((|TokenValue|) "convenience")) &&& ((|TokenValue|) "init")

//initializer-declaration → initializer-head generic-parameter-clause_opt parameter-clause initializer-body
//initializer-head → attributes_opt "convenience"_opt "init"
//initializer-body → code-block
and (|Initializer_declaration|) p1 =
    match p1 with
    | Initializer_head (Some (v1, p2)) ->
        match ws p2 with
        | Parameter_clause (Some (v2, p3)) ->
            match p3 with // Don't call ws before code block
            | Code_block (Some (v3, p4)) -> Some (InitializerDeclaration (v2, v3), p4)
            | _ -> None
        | _ -> None
    | _ -> None
        
and (|External_parameter_name|) = (|Identifier|) ||| (br "_")

and (|Local_parameter_name|) = (|Identifier|) ||| (br "_")

and (|Parameter|) p1 : (Parameter * Position) option =

    let np1, hash =
        match p1 with
        | Token "#" (Some p2) -> ws p2, true
        | _ -> p1, false

    match np1 with
    | External_parameter_name (Some (v1, p2)) ->
        match ws p2 with
        | Local_parameter_name (Some (v2, p3)) ->
            match ws p3 with
            | Type_annotation (Some (v3, p4)) -> Some ((v1, v2, Some v3), p4)
            | _ -> None
        | Type_annotation (Some (v2, p3)) -> Some ((v1, (if hash then v1 else "_"), Some v2), p3)
        | _ -> None
    | _ -> None

and (|Parameter_list|) = oneOrMoreSep (|Parameter|) ","

and (|Parameter_clause|) p1 =
    match p1 with
    | Token "(" (Some p2) ->
        match ws p2 with
        | Token ")" (Some p3) -> Some ([], p3)
        | Parameter_list (Some (v2, p3)) ->
            match ws p3 with
            | Token "..." (Some p4) ->
                match ws p4 with
                | Token ")" (Some p5) -> Some (v2, p5)
                | _ -> None
            | Token ")" (Some p4) -> Some (v2, p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Parameter_clauses|) = oneOrMore (|Parameter_clause|)

and (|Code_block|) p1 =
    match ws p1 with
    | Token "{" (Some p2) ->
        match ws p2 with
        | Token "}" (Some p3) -> Some ([], p3)
        | Statements (Some (v2, p3)) ->
            match ws p3 with
            | Token "}" (Some p4) -> Some (v2, p4)
            | _ -> None
        | _ -> None
    | _ ->
        match p1 with
        | Newline (Some p2) -> Some ([], p2)
        | _ -> None


and (|Struct_declaration|) i =
    match i with
    | Token "struct" (Some p2) ->
        match ws p2 with
        | Struct_name (Some (v2, p3)) ->
            match ws p3 with
            | Type_inheritance_clause (Some (v3, p4)) ->
                match ws p4 with
                | Class_or_struct_body (Some (v4, p5)) -> Some (StructDeclaration (v2, v3, v4), p5)
                | _ -> None
            | Class_or_struct_body (Some (v3, p4)) -> Some (StructDeclaration (v2, [], v3), p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Struct_name|) = (|Identifier|)

and (|Class_declaration|) i =
    match i with
    | Token "class" (Some p2) ->
        match ws p2 with
        | Class_name (Some (v2, p3)) ->
            match ws p3 with
            | Type_inheritance_clause (Some (v3, p4)) ->
                match ws p4 with
                | Class_or_struct_body (Some (v4, p5)) -> Some (ClassDeclaration (v2, v3, v4), p5)
                | _ -> None
            | Class_or_struct_body (Some (v3, p4)) -> Some (ClassDeclaration (v2, [], v3), p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Class_name|) = (|Identifier|)

and (|Type_inheritance_clause|) p1 =
    match p1 with
    | Token ":" (Some p2) ->
        match ws p2 with
        | Type_inheritance_list (Some n2) -> Some n2
        | _ -> None
    | _ -> None

and (|Type_inheritance_list|) = oneOrMoreSep (|Type_identifier|) ","

and (|Class_or_struct_body|) p1 =
    match p1 with
    | Token "{" (Some p2) ->
        match ws p2 with
        | Token "}" (Some p3) -> Some ([], p3)
        | Declarations (Some (v2, p3)) ->
            match ws p3 with
            | Token "}" (Some p4) -> Some (v2, p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Extension_body|) p1 =
    match (((|TokenValue|) "{") &&& (|Declarations_opt|) &&& ((|TokenValue|) "}")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

and (|Extension_declaration|) p1 =
    match (((|TokenValue|) "extension") &&& (|Type_identifier|) &&& (opt (|Type_inheritance_clause|)) &&& (|Extension_body|)) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (ExtensionDeclaration (v1, v2, v3, v4), p5)
    | _ -> None

and (|Protocol_body|) p1 =
    match ((br "{") &&& (|Declarations_opt|) &&& (br "}")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

and (|Protocol_name|) = (|Identifier|)

and (|Protocol_declaration|) p1 =
    match ((kwd "protocol") &&& (|Protocol_name|) &&& (opt (|Type_inheritance_clause|)) &&& (|Protocol_body|)) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (ProtocolDeclaration (v1, v2, v3, v4), p5)
    | _ -> None

and (|Import_declaration|) = function
    | Token "import" (Some j) ->
        match ws j with
        | Import_path (Some (z, k)) -> Some (ImportDeclaration z, k)
        | _ -> None
    | _ -> None

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

and (|Declaration_specifier|) p1 =
    match p1 with
    | Token "static" (Some p2) -> Some (Static, p2)
    | Token "override" (Some p2) -> Some (Override, p2)
    | Token "class" (Some p2) -> Some (Class, p2)
    | _ -> None

and (|Declaration_specifiers|) = oneOrMore (|Declaration_specifier|)

and (|Declaration_specifiers_opt|) = zeroOrMore (|Declaration_specifier|)

and (|Variable_declaration_head|) = (|Declaration_specifiers_opt|) &&& ((|TokenValue|) "var")

and (|Single_variable_declaration|) specs p1 =
    match p1 with
    | Identifier (Some (v1, p2)) ->
        match ws p2 with
        | Type_annotation (Some (v2, p3)) ->
            match ws p3 with
            | Getter_setter_block (Some (v3, p4)) -> Some (GetterSetterVariableDeclaration (specs, (v1, v2), v3), p4)
            | _ -> None
        | _ -> None
    | _ -> None
        
and (|Variable_declaration|) p1 =
    match p1 with
    | Variable_declaration_head (Some ((v1, _), p2)) ->
        match ws p2 with
        | Single_variable_declaration v1 (Some n2) -> Some n2
        | Pattern_initializer_list (Some (v2, p3)) -> Some (PatternVariableDeclaration (v1, v2), p3)
        | _ -> None
    | _ -> None

and (|Getter_clause|) p1 =
    match p1 with
    | Token "get" (Some p2) ->
        match p2 with
        | Code_block (Some (v2, p3)) -> Some (v2, p3)
        | _ -> Some ([], p2)
    | _ -> None

and (|Setter_clause|) p1 =
    match p1 with
    | Token "set" (Some p2) ->
        match p2 with
        | Code_block (Some (v2, p3)) -> Some (v2, p3)
        | _ -> Some ([], p2)
    | _ -> None

and (|Getter_setter_block|) p1 =
    match p1 with
    | Token "{" (Some p2) ->
        match ws p2 with
        | Getter_clause (Some (v2, p3)) ->
            match ws p3 with
            | Setter_clause (Some (v3, p4)) -> Some ((v2, Some v3), p4)
            | Token "}" (Some p4) -> Some ((v2, None), p4)
            | _ -> None
        | _ -> None
    | _ -> None

and (|Attribute|) p1 : (Attribute * Position) option = None

and (|Attributes_opt|) = zeroOrMore (|Attribute|)

and (|Function_head|) p1 =
    match ((|Declaration_specifiers_opt|) &&& ((|TokenValue|) "func")) p1 with
    | Some ((v1, _), p3) -> Some (v1, p3)
    | _ -> None

and (|Function_name|) = (|Identifier|) ||| (|Operator|)

and (|Function_result|) p1 =
    match (((|TokenValue|) "->") &&& (|Attributes_opt|) &&& (|Type|)) p1 with
    | Some (((_, v2), v3), p4) -> Some ((v2, v3), p4)
    | _ -> None

and (|Function_signature|) p1 = 
    match p1 with
    | Parameter_clauses (Some (v1, p2)) ->
        match ws p2 with
        | Function_result (Some (v2, p3)) -> Some ((v1, Some v2), p3)
        | _ -> Some ((v1, None), p2)
    | _ -> None

and (|Function_body|) = (|Code_block|)

and (|Function_declaration|) p1 = 
    match ((|Function_head|) &&& (|Function_name|) &&& (|Function_signature|)) p1 with
    | Some (((v1, v2), (v3, v4)), p5) ->
        match p5 with
        | Function_body (Some (v5, p6)) ->
            Some (FunctionDeclaration (v1, v2, v3, v4, v5), p6)
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

let parseDocument document =
    match Position.Beginning document |> ws with
    | Statements (Some r) -> Some r
    | _ -> None
let parseFile path = parseDocument { Name = path; Body = System.IO.File.ReadAllText (path) }
let parseText text = parseDocument { Name = "<text>"; Body = text }
let toPosition text = Position.Beginning { Name = "<text>"; Body = text }

