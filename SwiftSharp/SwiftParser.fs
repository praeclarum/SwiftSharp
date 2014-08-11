
module SwiftSharp.SwiftParser

open System.Text

open SwiftLexer

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

and (|Generic_argument_clause|) p1 =
    match ((br "<") &&& (|Generic_argument_list|) &&& (br ">")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

and (|Generic_parameter|) = (|Type_name|)

and (|Generic_parameter_list|) = oneOrMoreSep (|Generic_parameter|) ","

and (|Generic_parameter_clause|) p1 =
    match ((br "<") &&& (|Generic_parameter_list|) &&& (br ">")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (v2, p4)
    | _ -> None

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

and (|Expression|) withTrailingClosure p1 =
    match p1 with
    | Prefix_expression withTrailingClosure (Some (v1, p2)) ->
        match ws p2 with
        | Binary_expressions withTrailingClosure (Some (v2, p3)) -> Some (Compound (v1, v2), p3)
        | _ -> Some (v1, p2)
    | _ -> None

and (|Expression_list|) = oneOrMoreSep ((|Expression|) true) ","

and (|Identifier_list|) = oneOrMoreSep (|Identifier|) ","

and (|Closure_signature|) p1 =
    let tail e p1 = 
        match ((opt (|Function_result|)) &&& (kwd "in")) p1 with
        | Some ((v1, v2), p4) -> Some ((e, v1), p4)
        | _ -> None
    match p1 with
    | Parameter_clause (Some (v1, p2)) -> tail v1 (ws p2)
    | Identifier_list (Some (v1, p2)) -> 
        let ps = v1 |> List.map (fun x -> ([], None, x, None, None))
        tail ps (ws p2)
    | _ -> None

and (|Closure_expression|) p1 =
    match ((br "{") &&& (opt (|Closure_signature|)) &&& (|Statements_opt|) &&& (br "}")) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (Closure (v2, v3), p5)
    | _ -> None

and (|Implicit_member_expression|) p1 =
    match ((br ".") &&& (|Identifier|)) p1 with
    | Some ((v1, v2), p3) -> Some (Member (None, v2), p3)
    | _ -> None



and (|Primary_expression|) p1 =
    match p1 with
    | Implicit_member_expression (Some r) -> Some r
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

and (|Trailing_closure|) = (|Closure_expression|)

and (|Expression_element|) p1 =
    match p1 with
    | Identifier (Some (v1, p2)) ->
        match ws p2 with
        | Token ":" (Some p3) ->
            match ws p3 with
            | Expression true (Some (v3, p4)) -> Some ((Some v1, v3), p4)
            | _ -> None
        | _ ->
            match p1 with
            | Expression true (Some (v1, p2)) -> Some ((None, v1), p2)
            | _ -> None
    | Expression true (Some (v1, p2)) -> Some ((None, v1), p2)
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

and (|Function_call_expression|) e withTrailingClosure p1 =
    match p1 with
    | Parenthesized_expression (Some (v1, p2)) ->
        if not withTrailingClosure then
            Some (Funcall (!e, v1), p2)
        else
            match ws p2 with
            | Trailing_closure (Some (v2, p3)) -> Some (Funcall (!e, List.append v1 [(None, v2)]), p3)
            | _ -> Some (Funcall (!e, v1), p2)
    | _ ->
        if not withTrailingClosure then
            None
        else
            match p1 with
            | Trailing_closure (Some (v1, p2)) -> Some (Funcall (!e, [(None, v1)]), p2)
            | _ -> None


and (|Explicit_member_expression|) e p1 =
    match p1 with
    | Token "." (Some p2) ->
        match (ws p2) with
        | Identifier (Some (v2, p3)) -> Some (Member (Some !e, v2), p3)
        | _ -> None
    | _ -> None

and (|Optional_chaining_expression|) e p1 =
    match p1 with
    | Token "?" (Some p2) -> Some (OptionalChaining !e, p2)
    | _ -> None

and (|Subscript_expression|) e p1 =
    match ((br "[") &&& (|Expression_list|) &&& (br "]")) p1 with
    | Some (((v1, v2), v3), p4) -> Some (Subscript (!e, v2), p4)
    | _ -> None

and (|Postfix_expression|) withTrailingClosure p1 =
    match p1 with
    | Primary_expression (Some (v1, p2)) ->
        let e = ref v1
        let p = ref p2
        let rec loop = function
            | Function_call_expression e withTrailingClosure (Some (v2, p3))
            | Optional_chaining_expression e (Some (v2, p3))
            | Subscript_expression e (Some (v2, p3))
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

and (|Prefix_expression|) withTrailingClosure p1 =
    match p1 with
    | In_out_expression (Some r) -> Some r
    | Postfix_expression withTrailingClosure (Some r) -> Some r
    | _ -> None

and (|Type_casting_operator|) p1 =
    match p1 with
    | Token "as" (Some p2) ->
        match ws p2 with
        | Type (Some (v2, p3)) -> Some (AsBinary v2, p3)
        | _ -> None
    | _ -> None

and (|Binary_expression|) withTrailingClosure p1 =
    match p1 with
    | Binary_operator (Some (v1, p2)) ->
        match ws p2 with
        | Prefix_expression withTrailingClosure (Some (v2, p3)) -> Some (OpBinary (v1, v2), p3)
        | _ -> None
    | Type_casting_operator (Some (v1, p2)) -> Some (v1, p2)
    | _ -> None

and (|Binary_expressions|) withTrailingClosure i =
    match i with
    | Binary_expression withTrailingClosure (Some (be, j)) ->
        match ws j with
        | Binary_expressions withTrailingClosure (Some (bes, k)) -> Some (be :: bes, k)
        | _ -> Some ([be], j)
    | _ -> None

and (|Enum_case_pattern|) p1 =
    match ((opt (|Type_identifier|)) &&& (br ".") &&& (|Enum_case_name|) &&& (opt (|Tuple_pattern|))) p1 with
    | Some ((((v1, v2), v3), v4), p5) -> Some (EnumCasePattern (v1, v3, v4), p5)
    | _ -> None

and (|Value_binding_pattern|) p1 =
    match (((kwd "let") ||| (kwd "var")) &&& (|Pattern|)) p1 with
    | Some ((v1, v2), p3) -> Some (ValueBindingPattern v2, p3)
    | _ -> None

and (|Pattern|) p1 =
    match p1 with
    | Value_binding_pattern (Some r) -> Some r
    | Enum_case_pattern (Some r) -> Some r
    | Identifier_pattern (Some (v1, p2)) ->
        match ws p2 with
        | Type_annotation (Some (v2, p3)) -> Some (IdentifierPattern (v1, Some v2), p3)
        | _ -> Some (IdentifierPattern (v1, None), p2)
    | Tuple_pattern (Some (v1, p2)) ->
        match ws p2 with
        | Type_annotation (Some (v2, p3)) -> Some (TuplePattern (v1, Some v2), p3)
        | _ -> Some (TuplePattern (v1, None), p2)
    | Expression true (Some (v1, p2)) -> Some (ExpressionPattern v1, p2)
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

and (|Identifier_pattern|) = (|Identifier|)

and (|Statement|) p1 =
    match p1 with
    | Declaration (Some (v1, p2)) -> Some (DeclarationStatement v1, p2)
    | Branch_statement (Some (v1, p2)) -> Some (v1, p2)
    | Loop_statement (Some (v1, p2)) -> Some (v1, p2)
    | Expression true (Some (v1, p2)) -> Some (ExpressionStatement v1, p2)
    | _ -> None

and (|Statements|) = oneOrMore (|Statement|)

and (|Statements_opt|) = zeroOrMore (|Statement|)

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
    match ((kwd "switch") &&& ((|Expression|) false) &&& (br "{") &&& (|Switch_cases_opt|) &&& (br "}")) p1 with
    | Some (((((v1, v2), v3), v4), v5), p6) -> Some (SwitchStatement (v2, v4), p6)
    | _ -> None
        
and (|If_condition|) p1 =
    match p1 with
    | Expression false (Some (v1, p2)) -> Some (v1, p2)
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
    match ((kwd "for") &&& (|Pattern|) &&& (kwd "in") &&& ((|Expression|) false) &&& (|Code_block|)) p1 with
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

and (|Union_style_enum_case|) = (|Enum_case_name|) &&& (opt (|Tuple_type|))

and (|Union_style_enum_case_list|) = oneOrMoreSep (|Union_style_enum_case|) ","

and (|Union_style_enum_case_clause|) p1 =
    match ((kwd "case") &&& (|Union_style_enum_case_list|)) p1 with
    | Some ((v1, v2), p3) -> Some (v2, p3)
    | _ -> None

and (|Union_style_enum_member|) = (|Union_style_enum_case_clause|)

and (|Union_style_enum_members|) = zeroOrMore (|Union_style_enum_member|)

and (|Union_style_enum|) p1 =
    match ((kwd "enum") &&& (|Enum_name|) &&& (opt (|Generic_parameter_clause|)) &&& (opt (|Type_inheritance_clause|)) &&& (br "{") &&& (|Union_style_enum_members|) &&& (br "}")) p1 with
    | Some (((((((v1, v2), v3), v4), v5), v6), v7), p8) -> Some (UnionEnumDeclaration (v2, v3, v4, v6), p8)
    | _ -> None


and (|Enum_declaration|) = (|Raw_value_style_enum|) ||| (|Union_style_enum|)

and (|Initializer_head|) = (opt ((|TokenValue|) "convenience")) &&& ((|TokenValue|) "init")

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

and (|Default_argument_clause|) p1 =
    match ((br "=") &&& ((|Expression|) true)) p1 with
    | Some ((v1, v2), p3) -> Some (v2, p3)
    | _ -> None
            
and (|External_parameter_name|) = (|Identifier|) ||| (br "_")
and (|Local_parameter_name|) = (|Identifier|) ||| (br "_")

and (|Parameter_names|) p1 =
    match p1 with
    | External_parameter_name (Some (v1, p2)) ->
        match ws p2 with
        | Local_parameter_name (Some (v2, p3)) -> Some ((Some v1, v2), p3)
        | _ -> Some ((None, v1), p2)
    | _ -> None

and (|Let_or_var_parameter|) p1 : (Parameter * Position) option =
    match ((okwd "inout") &&& ((okwd "let") ||| (nokwd "var")) &&& (okwd "#") &&& (|Parameter_names|) &&& (|Type_annotation|) &&& (opt (|Default_argument_clause|))) p1 with
    | Some ((((((v1, v2), v3), (v4a, v4b)), v5), v6), p7) ->
        let ex, loc =
            match v3 with
            | Some _ -> (Some v4b, v4b)
            | _ -> (v4a, v4b)
        Some (([], ex, loc, Some v5, v6), p7)
    | _ -> None
        
and (|Type_parameter|) p1 : (Parameter * Position) option =
    match ((|Attributes_opt|) &&& (|Type|)) p1 with
    | Some ((v1, v2), p3) -> Some ((v1, None, "_", Some v2, None), p3)
    | _ -> None

and (|Parameter|) = (|Let_or_var_parameter|) ||| (|Type_parameter|)

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

and (|Attribute|) p1 : (Attr * Position) option = None

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
        | Expression true (Some (z, k)) -> Some (z, k)
        | _ -> None
    | _ -> None

let parseDocument document =
    match Position.Beginning document |> ws with
    | Statements (Some r) -> Some r
    | _ -> None
let parseFile path = parseDocument { Name = path; Body = System.IO.File.ReadAllText (path) }
let parseText text = parseDocument { Name = "<text>"; Body = text }
let toPosition text = Position.Beginning { Name = "<text>"; Body = text }

