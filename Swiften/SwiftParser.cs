// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "Swiften/SwiftParser.jay"
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;

#pragma warning disable 219,414

namespace Swiften
{
	public partial class SwiftParser
	{
		const int yacc_verbose_flag = 1;
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = new StringWriter ();

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /* An EOF token */
  public int eof_token;

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
//t  internal yydebug.yyDebug debug;

  protected const int yyFinal = 36;
//t // Put this array into a separate class so it is only initialized if debugging is actually used
//t // Use MarshalByRefObject to disable inlining
//t class YYRules : MarshalByRefObject {
//t  public static readonly string [] yyRule = {
//t    "$accept : statements",
//t    "statement : expression NEWLINE",
//t    "statement : expression ';'",
//t    "statement : declaration NEWLINE",
//t    "statement : declaration ';'",
//t    "statement : loop_statement NEWLINE",
//t    "statement : loop_statement ';'",
//t    "statement : branch_statement NEWLINE",
//t    "statement : branch_statement ';'",
//t    "statement : labeled_statement NEWLINE",
//t    "statement : control_transfer_statement NEWLINE",
//t    "statement : control_transfer_statement ';'",
//t    "statements : statement",
//t    "statements : statement statements",
//t    "loop_statement : for_statement",
//t    "loop_statement : for_in_statement",
//t    "loop_statement : while_statement",
//t    "loop_statement : do_while_statement",
//t    "for_statement : FOR for_init ';' expression ';' expression code_block",
//t    "for_statement : FOR for_init ';' expression ';' code_block",
//t    "for_statement : FOR for_init ';' ';' expression code_block",
//t    "for_statement : FOR for_init ';' ';' code_block",
//t    "for_statement : FOR ';' expression ';' expression code_block",
//t    "for_statement : FOR ';' expression ';' code_block",
//t    "for_statement : FOR ';' ';' expression code_block",
//t    "for_statement : FOR ';' ';' code_block",
//t    "for_statement : FOR '(' for_init ';' expression ';' expression ')' code_block",
//t    "for_statement : FOR '(' for_init ';' expression ';' ')' code_block",
//t    "for_statement : FOR '(' for_init ';' ';' expression ')' code_block",
//t    "for_statement : FOR '(' for_init ';' ';' ')' code_block",
//t    "for_statement : FOR '(' ';' expression ';' expression ')' code_block",
//t    "for_statement : FOR '(' ';' expression ';' ')' code_block",
//t    "for_statement : FOR '(' ';' ';' expression ')' code_block",
//t    "for_statement : FOR '(' ';' ';' ')' code_block",
//t    "for_init : variable_declaration",
//t    "for_init : expression_list",
//t    "for_in_statement : FOR pattern IN expression code_block",
//t    "while_statement : WHILE while_condition code_block",
//t    "while_condition : expression",
//t    "while_condition : declaration",
//t    "do_while_statement : DO code_block WHILE while_condition",
//t    "branch_statement : if_statement",
//t    "branch_statement : switch_statement",
//t    "if_statement : IF if_condition code_block else_clause",
//t    "if_statement : IF if_condition code_block",
//t    "if_condition : expression",
//t    "if_condition : declaration",
//t    "else_clause : ELSE code_block",
//t    "else_clause : ELSE if_statement",
//t    "switch_statement : SWITCH expression '{' switch_cases '}'",
//t    "switch_statement : SWITCH expression '{' '}'",
//t    "switch_cases : switch_case",
//t    "switch_cases : switch_case switch_cases",
//t    "switch_case : case_label statements",
//t    "switch_case : default_label statements",
//t    "switch_case : case_label ';'",
//t    "switch_case : default_label ';'",
//t    "case_label : CASE case_item_list ':'",
//t    "case_item : pattern guard_clause",
//t    "case_item : pattern",
//t    "case_item_list : case_item",
//t    "case_item_list : case_item case_item_list",
//t    "default_label : DEFAULT ':'",
//t    "guard_clause : WHERE guard_expression",
//t    "guard_expression : expression",
//t    "labeled_statement : statement_label loop_statement",
//t    "labeled_statement : statement_label switch_statement",
//t    "statement_label : label_name ':'",
//t    "label_name : IDENTIFIER",
//t    "control_transfer_statement : break_statement",
//t    "control_transfer_statement : continue_statement",
//t    "control_transfer_statement : fallthrough_statement",
//t    "control_transfer_statement : return_statement",
//t    "break_statement : BREAK label_name",
//t    "break_statement : BREAK",
//t    "continue_statement : CONTINUE label_name",
//t    "continue_statement : CONTINUE",
//t    "fallthrough_statement : FALLTHROUGH",
//t    "return_statement : RETURN expression",
//t    "return_statement : RETURN",
//t    "generic_parameter_clause : '<' generic_parameter_list requirement_clause '>'",
//t    "generic_parameter_clause : '<' generic_parameter_list '>'",
//t    "generic_parameter_list : generic_parameter",
//t    "generic_parameter_list : generic_parameter ',' generic_parameter_list",
//t    "generic_parameter : type_name",
//t    "generic_parameter : type_name ':' type_identifier",
//t    "generic_parameter : type_name ':' protocol_composition_type",
//t    "requirement_clause : WHERE requirement_list",
//t    "requirement_list : requirement",
//t    "requirement_list : requirement ',' requirement_list",
//t    "requirement : conformance_requirement",
//t    "requirement : same_type_requirement",
//t    "conformance_requirement : type_identifier ':' type_identifier",
//t    "conformance_requirement : type_identifier ':' protocol_composition_type",
//t    "same_type_requirement : type_identifier EQEQ_OP type_identifier",
//t    "generic_argument_clause : '<' generic_argument_list '>'",
//t    "generic_argument_list : generic_argument",
//t    "generic_argument_list : generic_argument ',' generic_argument_list",
//t    "generic_argument : type",
//t    "declaration : import_declaration",
//t    "declaration : constant_declaration",
//t    "declaration : variable_declaration",
//t    "declaration : typealias_declaration",
//t    "declaration : function_declaration",
//t    "declaration : enum_declaration",
//t    "declaration : struct_declaration",
//t    "declaration : class_declaration",
//t    "declaration : protocol_declaration",
//t    "declaration : initializer_declaration",
//t    "declaration : deinitializer_declaration",
//t    "declaration : extension_declaration",
//t    "declaration : subscript_declaration",
//t    "declaration : operator_declaration",
//t    "declarations : declaration",
//t    "declarations : declaration declarations",
//t    "declaration_specifiers : declaration_specifier",
//t    "declaration_specifiers : declaration_specifier declaration_specifiers",
//t    "declaration_specifier : CLASS",
//t    "declaration_specifier : MUTATING",
//t    "declaration_specifier : NONMUTATING",
//t    "declaration_specifier : OVERRIDE",
//t    "declaration_specifier : STATIC",
//t    "declaration_specifier : UNOWNED",
//t    "declaration_specifier : UNOWNED_SAFE",
//t    "declaration_specifier : UNOWNED_UNSAFE",
//t    "declaration_specifier : WEAK",
//t    "top_level_declaration : statements",
//t    "code_block : '{' statements '}'",
//t    "code_block : '{' '}'",
//t    "import_declaration : attributes IMPORT import_kind import_path",
//t    "import_declaration : attributes IMPORT import_path",
//t    "import_declaration : IMPORT import_kind import_path",
//t    "import_declaration : IMPORT import_path",
//t    "import_kind : TYEPALIAS",
//t    "import_kind : STRUCT",
//t    "import_kind : CLASS",
//t    "import_kind : ENUM",
//t    "import_kind : PROTOCOL",
//t    "import_kind : VAR",
//t    "import_kind : FUNC",
//t    "import_path : import_path_identifier",
//t    "import_path : import_path_identifier '.' import_path",
//t    "import_path_identifier : IDENTIFIER",
//t    "import_path_identifier : operator_",
//t    "constant_declaration : attributes declaration_specifiers LET pattern_initializer_list",
//t    "constant_declaration : attributes LET pattern_initializer_list",
//t    "constant_declaration : declaration_specifiers LET pattern_initializer_list",
//t    "constant_declaration : LET pattern_initializer_list",
//t    "pattern_initializer_list : pattern_initializer",
//t    "pattern_initializer_list : pattern_initializer ',' pattern_initializer_list",
//t    "pattern_initializer : pattern initializer",
//t    "pattern_initializer : pattern",
//t    "initializer : expression",
//t    "variable_declaration : variable_declaration_head pattern_initializer_list",
//t    "variable_declaration : variable_declaration_head variable_name type_annotation code_block",
//t    "variable_declaration : variable_declaration_head variable_name type_annotation getter_setter_block",
//t    "variable_declaration : variable_declaration_head variable_name type_annotation getter_setter_keyword_block",
//t    "variable_declaration : variable_declaration_head variable_name type_annotation initializer willSet_didSet_block",
//t    "variable_declaration : variable_declaration_head variable_name type_annotation willSet_didSet_block",
//t    "variable_declaration_head : attributes declaration_specifiers VAR",
//t    "variable_declaration_head : attributes VAR",
//t    "variable_declaration_head : declaration_specifiers VAR",
//t    "variable_declaration_head : VAR",
//t    "variable_name : IDENTIFIER",
//t    "getter_setter_block : '{' getter_clause setter_clause '}'",
//t    "getter_setter_block : '{' getter_clause '}'",
//t    "getter_setter_block : '{' setter_clause getter_clause '}'",
//t    "getter_clause : attributes GET code_block",
//t    "getter_clause : GET code_block",
//t    "setter_clause : attributes SET setter_name code_block",
//t    "setter_clause : attributes SET code_block",
//t    "setter_clause : SET setter_name code_block",
//t    "setter_clause : SET code_block",
//t    "setter_name : '(' IDENTIFIER ')'",
//t    "getter_setter_keyword_block : '{' getter_keyword_clause setter_keyword_clause '}'",
//t    "getter_setter_keyword_block : '{' getter_keyword_clause '}'",
//t    "getter_setter_keyword_block : '{' setter_keyword_clause getter_keyword_clause '}'",
//t    "getter_keyword_clause : attributes GET",
//t    "getter_keyword_clause : GET",
//t    "setter_keyword_clause : attributes SET",
//t    "setter_keyword_clause : SET",
//t    "willSet_didSet_block : '{' willSet_clause didSet_clause '}'",
//t    "willSet_didSet_block : '{' willSet_clause '}'",
//t    "willSet_didSet_block : '{' didSet_clause willSet_clause '}'",
//t    "willSet_clause : attributes WILLSET setter_name code_block",
//t    "willSet_clause : attributes WILLSET code_block",
//t    "willSet_clause : WILLSET setter_name code_block",
//t    "willSet_clause : WILLSET code_block",
//t    "didSet_clause : attributes DIDSET setter_name code_block",
//t    "didSet_clause : attributes DIDSET code_block",
//t    "didSet_clause : DIDSET setter_name code_block",
//t    "didSet_clause : DIDSET code_block",
//t    "typealias_declaration : typealias_head typealias_assignment",
//t    "typealias_head : TYPEALIAS typealias_name",
//t    "typealias_name : IDENTIFIER",
//t    "typealias_assignment : '=' type",
//t    "function_declaration : function_head function_name generic_parameter_clause function_signature function_body",
//t    "function_declaration : function_head function_name function_signature function_body",
//t    "function_head : attributes declaration_specifiers FUNC",
//t    "function_head : attributes FUNC",
//t    "function_head : declaration_specifiers FUNC",
//t    "function_head : FUNC",
//t    "function_name : IDENTIFIER",
//t    "function_name : operator_",
//t    "function_signature : parameter_clauses function_result",
//t    "function_signature : parameter_clauses",
//t    "function_result : ARROW_OP attributes type",
//t    "function_result : ARROW_OP type",
//t    "function_body : code_block",
//t    "parameter_clauses : parameter_clause",
//t    "parameter_clauses : parameter_clause parameter_clauses",
//t    "parameter_clause : '(' ')'",
//t    "parameter_clause : '(' parameter_list DOTDOTDOT_OP ')'",
//t    "parameter_clause : '(' parameter_list ')'",
//t    "parameter_list : parameter",
//t    "parameter_list : parameter ',' parameter_list",
//t    "parameter : parameter_head local_parameter_name type_annotation default_argument_clause",
//t    "parameter : parameter_head local_parameter_name type_annotation",
//t    "parameter : parameter_head type_annotation default_argument_clause",
//t    "parameter : parameter_head type_annotation",
//t    "parameter : attributes type",
//t    "parameter : type",
//t    "parameter_head : INOUT LET '#' parameter_name",
//t    "parameter_head : INOUT LET parameter_name",
//t    "parameter_head : INOUT '#' parameter_name",
//t    "parameter_head : INOUT parameter_name",
//t    "parameter_head : LET '#' parameter_name",
//t    "parameter_head : LET parameter_name",
//t    "parameter_head : '#' parameter_name",
//t    "parameter_head : parameter_name",
//t    "parameter_head : INOUT VAR '#' parameter_name",
//t    "parameter_head : INOUT VAR parameter_name",
//t    "parameter_head : VAR '#' parameter_name",
//t    "parameter_head : VAR parameter_name",
//t    "parameter_name : IDENTIFIER",
//t    "parameter_name : '_'",
//t    "local_parameter_name : IDENTIFIER",
//t    "local_parameter_name : '_'",
//t    "default_argument_clause : expression",
//t    "enum_declaration : attributes ENUM union_style_enum",
//t    "enum_declaration : ENUM union_style_enum",
//t    "enum_declaration : attributes ENUM raw_value_style_enum",
//t    "enum_declaration : ENUM raw_value_style_enum",
//t    "union_style_enum : enum_name generic_parameter_clause '{' union_style_enum_members '}'",
//t    "union_style_enum : enum_name generic_parameter_clause '{' '}'",
//t    "union_style_enum : enum_name '{' union_style_enum_members '}'",
//t    "union_style_enum : enum_name '{' '}'",
//t    "union_style_enum_members : union_style_enum_member",
//t    "union_style_enum_members : union_style_enum_member union_style_enum_members",
//t    "union_style_enum_member : declaration",
//t    "union_style_enum_member : union_style_enum_case_clause",
//t    "union_style_enum_case_clause : attributes CASE union_style_enum_case_list",
//t    "union_style_enum_case_clause : CASE union_style_enum_case_list",
//t    "union_style_enum_case_list : union_style_enum_case",
//t    "union_style_enum_case_list : union_style_enum_case ',' union_style_enum_case_list",
//t    "union_style_enum_case : enum_case_name tuple_type",
//t    "union_style_enum_case : enum_case_name",
//t    "enum_name : IDENTIFIER",
//t    "enum_case_name : IDENTIFIER",
//t    "raw_value_style_enum : enum_name generic_parameter_clause ':' type_identifier '{' raw_value_style_enum_members '}'",
//t    "raw_value_style_enum : enum_name generic_parameter_clause ':' type_identifier '{' '}'",
//t    "raw_value_style_enum : enum_name ':' type_identifier '{' raw_value_style_enum_members '}'",
//t    "raw_value_style_enum : enum_name ':' type_identifier '{' '}'",
//t    "raw_value_style_enum_members : raw_value_style_enum_member",
//t    "raw_value_style_enum_members : raw_value_style_enum_member raw_value_style_enum_members",
//t    "raw_value_style_enum_member : declaration",
//t    "raw_value_style_enum_member : raw_value_style_enum_case_clause",
//t    "raw_value_style_enum_case_clause : attributes CASE raw_value_style_enum_case_list",
//t    "raw_value_style_enum_case_clause : CASE raw_value_style_enum_case_list",
//t    "raw_value_style_enum_case_list : raw_value_style_enum_case",
//t    "raw_value_style_enum_case_list : raw_value_style_enum_case ',' raw_value_style_enum_case_list",
//t    "raw_value_style_enum_case : enum_case_name raw_value_assignment",
//t    "raw_value_style_enum_case : enum_case_name",
//t    "raw_value_assignment : '=' literal",
//t    "struct_declaration : attributes STRUCT struct_name generic_parameter_clause type_inheritance_clause struct_body",
//t    "struct_declaration : attributes STRUCT struct_name generic_parameter_clause struct_body",
//t    "struct_declaration : attributes STRUCT struct_name type_inheritance_clause struct_body",
//t    "struct_declaration : attributes STRUCT struct_name struct_body",
//t    "struct_declaration : STRUCT struct_name generic_parameter_clause type_inheritance_clause struct_body",
//t    "struct_declaration : STRUCT struct_name generic_parameter_clause struct_body",
//t    "struct_declaration : STRUCT struct_name type_inheritance_clause struct_body",
//t    "struct_declaration : STRUCT struct_name struct_body",
//t    "struct_name : IDENTIFIER",
//t    "struct_body : '{' declarations '}'",
//t    "struct_body : '{' '}'",
//t    "class_declaration : attributes CLASS class_name generic_parameter_clause type_inheritance_clause class_body",
//t    "class_declaration : attributes CLASS class_name generic_parameter_clause class_body",
//t    "class_declaration : attributes CLASS class_name type_inheritance_clause class_body",
//t    "class_declaration : attributes CLASS class_name class_body",
//t    "class_declaration : CLASS class_name generic_parameter_clause type_inheritance_clause class_body",
//t    "class_declaration : CLASS class_name generic_parameter_clause class_body",
//t    "class_declaration : CLASS class_name type_inheritance_clause class_body",
//t    "class_declaration : CLASS class_name class_body",
//t    "class_name : IDENTIFIER",
//t    "class_body : '{' declarations '}'",
//t    "class_body : '{' '}'",
//t    "protocol_declaration : attributes PROTOCOL protocol_name type_inheritance_clause protocol_body",
//t    "protocol_declaration : attributes PROTOCOL protocol_name protocol_body",
//t    "protocol_declaration : PROTOCOL protocol_name type_inheritance_clause protocol_body",
//t    "protocol_declaration : PROTOCOL protocol_name protocol_body",
//t    "protocol_name : IDENTIFIER",
//t    "protocol_body : '{' protocol_member_declarations '}'",
//t    "protocol_body : '{' '}'",
//t    "protocol_member_declaration : protocol_property_declaration",
//t    "protocol_member_declaration : protocol_method_declaration",
//t    "protocol_member_declaration : protocol_initializer_declaration",
//t    "protocol_member_declaration : protocol_subscript_declaration",
//t    "protocol_member_declaration : protocol_associated_type_declaration",
//t    "protocol_member_declarations : protocol_member_declaration",
//t    "protocol_member_declarations : protocol_member_declaration protocol_member_declarations",
//t    "protocol_property_declaration : variable_declaration_head variable_name type_annotation getter_setter_keyword_block",
//t    "protocol_method_declaration : function_head function_name generic_parameter_clause function_signature",
//t    "protocol_method_declaration : function_head function_name function_signature",
//t    "protocol_initializer_declaration : initializer_head generic_parameter_clause parameter_clause",
//t    "protocol_initializer_declaration : initializer_head parameter_clause",
//t    "protocol_subscript_declaration : subscript_head subscript_result getter_setter_keyword_block",
//t    "protocol_associated_type_declaration : typealias_head type_inheritance_clause typealias_assignment",
//t    "protocol_associated_type_declaration : typealias_head type_inheritance_clause",
//t    "protocol_associated_type_declaration : typealias_head typealias_assignment",
//t    "protocol_associated_type_declaration : typealias_head",
//t    "initializer_declaration : initializer_head generic_parameter_clause parameter_clause initializer_body",
//t    "initializer_declaration : initializer_head parameter_clause initializer_body",
//t    "initializer_head : attributes CONVENIENCE INIT",
//t    "initializer_head : attributes INIT",
//t    "initializer_head : CONVENIENCE INIT",
//t    "initializer_head : INIT",
//t    "initializer_body : code_block",
//t    "deinitializer_declaration : attributes DEINIT code_block",
//t    "deinitializer_declaration : DEINIT code_block",
//t    "extension_declaration : EXTENSION type_identifier type_inheritance_clause extension_body",
//t    "extension_declaration : EXTENSION type_identifier extension_body",
//t    "extension_body : '{' declarations '}'",
//t    "extension_body : '{' '}'",
//t    "subscript_declaration : subscript_head subscript_result code_block",
//t    "subscript_declaration : subscript_head subscript_result getter_setter_block",
//t    "subscript_declaration : subscript_head subscript_result getter_setter_keyword_block",
//t    "subscript_head : attributes SUBSCRIPT parameter_clause",
//t    "subscript_head : SUBSCRIPT parameter_clause",
//t    "subscript_result : attributes type",
//t    "subscript_result : type",
//t    "operator_declaration : prefix_operator_declaration",
//t    "operator_declaration : postfix_operator_declaration",
//t    "operator_declaration : infix_operator_declaration",
//t    "prefix_operator_declaration : OPERATOR PREFIX operator '{' '}'",
//t    "postfix_operator_declaration : OPERATOR POSTFIX operator '{' '}'",
//t    "infix_operator_declaration : OPERATOR INFIX operator '{' infix_operator_attributes '}'",
//t    "infix_operator_declaration : OPERATOR INFIX operator '{' '}'",
//t    "infix_operator_attributes : precedence_clause associativity_clause",
//t    "infix_operator_attributes : precedence_clause",
//t    "infix_operator_attributes : associativity_clause",
//t    "precedence_clause : PRECEDENCE NUMBER",
//t    "associativity_clause : ASSOCIATIVITY associativity_value",
//t    "associativity_value : LEFT",
//t    "associativity_value : RIGHT",
//t    "associativity_value : NONE",
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,"'#'",null,null,null,
    null,"'('","')'",null,null,"','",null,"'.'",null,null,null,null,null,
    null,null,null,null,null,null,"':'","';'","'<'","'='","'>'",null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,"'_'",null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,"'{'",null,"'}'",null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,
    "IDENTIFIER","NUMBER","NEWLINE","FOR","IN","DO","WHILE","IF","ELSE",
    "SWITCH","CASE","DEFAULT","WHERE","BREAK","CONTINUE","FALLTHROUGH",
    "RETURN","EQEQ_OP","CLASS","MUTATING","NONMUTATING","OVERRIDE",
    "STATIC","UNOWNED","UNOWNED_SAFE","UNOWNED_UNSAFE","WEAK","IMPORT",
    "TYEPALIAS","STRUCT","ENUM","PROTOCOL","VAR","FUNC","LET","GET","SET",
    "WILLSET","DIDSET","TYPEALIAS","ARROW_OP","DOTDOTDOT_OP","INOUT",
    "CONVENIENCE","INIT","DEINIT","EXTENSION","SUBSCRIPT","OPERATOR",
    "PREFIX","POSTFIX","INFIX","PRECEDENCE","ASSOCIATIVITY","LEFT",
    "RIGHT","NONE","expression","expression_list","pattern","type_name",
    "type_identifier","protocol_composition_type","type","attributes",
    "operator_","type_annotation","tuple_type","literal",
    "type_inheritance_clause","operator",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
//t  public static string yyname (int token) {
//t    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
//t    string name;
//t    if ((name = yyNames[token]) != null) return name;
//t    return "[unknown]";
//t  }

  //int yyExpectingState;
  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected int [] yyExpectingTokens (int state){
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];
    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    int [] result = new int [len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = token;
    return result;
  }
  protected string[] yyExpecting (int state) {
    int [] tokens = yyExpectingTokens (state);
    string [] result = new string[tokens.Length];
    for (int n = 0; n < tokens.Length;  n++)
      result[n++] = yyNames[tokens [n]];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
//t    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

	static int[] global_yyStates;
	static object[] global_yyVals;
	protected bool use_global_stacks;
	object[] yyVals;					// value stack
	object yyVal;						// value stack ptr
	int yyToken;						// current input
	int yyTop;

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;		// initial size
    int yyState = 0;                   // state stack ptr
    int [] yyStates;               	// state stack 
    yyVal = null;
    yyToken = -1;
    int yyErrorFlag = 0;				// #tks to shift
	if (use_global_stacks && global_yyStates != null) {
		yyVals = global_yyVals;
		yyStates = global_yyStates;
   } else {
		yyVals = new object [yyMax];
		yyStates = new int [yyMax];
		if (use_global_stacks) {
			global_yyVals = yyVals;
			global_yyStates = yyStates;
		}
	}

    /*yyLoop:*/ for (yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        global::System.Array.Resize (ref yyStates, yyStates.Length+yyMax);
        global::System.Array.Resize (ref yyVals, yyVals.Length+yyMax);
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
//t      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ while (true) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
//t            if (debug != null)
//t              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              //yyExpectingState = yyState;
              // yyerror(String.Format ("syntax error, got token `{0}'", yyname (yyToken)), yyExpecting(yyState));
//t              if (debug != null) debug.error("syntax error");
              if (yyToken == 0 /*eof*/ || yyToken == eof_token) throw new yyParser.yyUnexpectedEof ();
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
//t                  if (debug != null)
//t                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
//t                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
//t              if (debug != null) debug.reject();
              throw new yyParser.yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
//t                if (debug != null) debug.reject();
                throw new yyParser.yyException("irrecoverable syntax error at end-of-file");
              }
//t              if (debug != null)
//t                debug.discard(yyState, yyToken, yyname(yyToken),
//t  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
//t        if (debug != null)
//t          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyV > yyTop ? null : yyVals[yyV]; // yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
//t          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
//t            if (debug != null)
//t               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
//t            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
//t        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: ;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: ;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

/*
 All more than 3 lines long rules are wrapped into a method
*/
#line default
   static readonly short [] yyLhs  = {              -1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    0,    0,    3,    3,    3,    3,    7,    7,    7,
    7,    7,    7,    7,    7,    7,    7,    7,    7,    7,
    7,    7,    7,   11,   11,    8,    9,   14,   14,   10,
    4,    4,   15,   15,   17,   17,   18,   18,   16,   16,
   19,   19,   20,   20,   20,   20,   21,   24,   24,   23,
   23,   22,   25,   26,    5,    5,   27,   28,    6,    6,
    6,    6,   29,   29,   30,   30,   31,   32,   32,   33,
   33,   34,   34,   36,   36,   36,   35,   37,   37,   38,
   38,   39,   39,   40,   41,   42,   42,   43,    2,    2,
    2,    2,    2,    2,    2,    2,    2,    2,    2,    2,
    2,    2,   57,   57,   58,   58,   59,   59,   59,   59,
   59,   59,   59,   59,   59,   60,   12,   12,   44,   44,
   44,   44,   61,   61,   61,   61,   61,   61,   61,   62,
   62,   63,   63,   45,   45,   45,   45,   64,   64,   65,
   65,   66,   13,   13,   13,   13,   13,   13,   67,   67,
   67,   67,   68,   69,   69,   69,   72,   72,   73,   73,
   73,   73,   74,   70,   70,   70,   75,   75,   76,   76,
   71,   71,   71,   77,   77,   77,   77,   78,   78,   78,
   78,   46,   79,   81,   80,   47,   47,   82,   82,   82,
   82,   83,   83,   84,   84,   87,   87,   85,   86,   86,
   88,   88,   88,   89,   89,   90,   90,   90,   90,   90,
   90,   91,   91,   91,   91,   91,   91,   91,   91,   91,
   91,   91,   91,   94,   94,   92,   92,   93,   48,   48,
   48,   48,   95,   95,   95,   95,   98,   98,   99,   99,
  100,  100,  101,  101,  102,  102,   97,  103,   96,   96,
   96,   96,  104,  104,  105,  105,  106,  106,  107,  107,
  108,  108,  109,   49,   49,   49,   49,   49,   49,   49,
   49,  110,  111,  111,   50,   50,   50,   50,   50,   50,
   50,   50,  112,  113,  113,   51,   51,   51,   51,  114,
  115,  115,  117,  117,  117,  117,  117,  116,  116,  118,
  119,  119,  120,  120,  121,  122,  122,  122,  122,   52,
   52,  123,  123,  123,  123,  126,   53,   53,   54,   54,
  127,  127,   55,   55,   55,  124,  124,  125,  125,   56,
   56,   56,  128,  129,  130,  130,  131,  131,  131,  132,
  133,  134,  134,  134,
  };
   static readonly short [] yyLen = {           2,
    2,    2,    2,    2,    2,    2,    2,    2,    2,    2,
    2,    1,    2,    1,    1,    1,    1,    7,    6,    6,
    5,    6,    5,    5,    4,    9,    8,    8,    7,    8,
    7,    7,    6,    1,    1,    5,    3,    1,    1,    4,
    1,    1,    4,    3,    1,    1,    2,    2,    5,    4,
    1,    2,    2,    2,    2,    2,    3,    2,    1,    1,
    2,    2,    2,    1,    2,    2,    2,    1,    1,    1,
    1,    1,    2,    1,    2,    1,    1,    2,    1,    4,
    3,    1,    3,    1,    3,    3,    2,    1,    3,    1,
    1,    3,    3,    3,    3,    1,    3,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    2,    1,    2,    1,    1,    1,    1,
    1,    1,    1,    1,    1,    1,    3,    2,    4,    3,
    3,    2,    1,    1,    1,    1,    1,    1,    1,    1,
    3,    1,    1,    4,    3,    3,    2,    1,    3,    2,
    1,    1,    2,    4,    4,    4,    5,    4,    3,    2,
    2,    1,    1,    4,    3,    4,    3,    2,    4,    3,
    3,    2,    3,    4,    3,    4,    2,    1,    2,    1,
    4,    3,    4,    4,    3,    3,    2,    4,    3,    3,
    2,    2,    2,    1,    2,    5,    4,    3,    2,    2,
    1,    1,    1,    2,    1,    3,    2,    1,    1,    2,
    2,    4,    3,    1,    3,    4,    3,    3,    2,    2,
    1,    4,    3,    3,    2,    3,    2,    2,    1,    4,
    3,    3,    2,    1,    1,    1,    1,    1,    3,    2,
    3,    2,    5,    4,    4,    3,    1,    2,    1,    1,
    3,    2,    1,    3,    2,    1,    1,    1,    7,    6,
    6,    5,    1,    2,    1,    1,    3,    2,    1,    3,
    2,    1,    2,    6,    5,    5,    4,    5,    4,    4,
    3,    1,    3,    2,    6,    5,    5,    4,    5,    4,
    4,    3,    1,    3,    2,    5,    4,    4,    3,    1,
    3,    2,    1,    1,    1,    1,    1,    1,    2,    4,
    4,    3,    3,    2,    3,    3,    2,    2,    1,    4,
    3,    3,    2,    2,    1,    1,    3,    2,    4,    3,
    3,    2,    3,    3,    3,    3,    2,    2,    1,    1,
    1,    1,    5,    5,    6,    5,    2,    1,    1,    2,
    2,    1,    1,    1,
  };
   static readonly short [] yyDefRed = {            0,
   68,    0,    0,    0,    0,    0,    0,    0,   77,    0,
    0,  118,  119,  120,  121,  122,  123,  124,  125,    0,
    0,    0,    0,  162,  201,    0,    0,    0,  325,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   14,   15,   16,   17,  101,   41,   42,    0,
    0,   69,   70,   71,   72,   99,  100,  102,  103,  104,
  105,  106,  107,  108,  109,  110,  111,  112,    0,    0,
    0,    0,    0,    0,    0,  340,  341,  342,  117,    0,
    0,   35,    0,    0,    0,   34,    0,    0,    0,   38,
   39,    0,   45,   46,    0,    0,   73,   75,   78,  293,
    0,  142,  135,  133,  134,  136,  137,  138,  139,  143,
    0,  132,    0,  282,    0,  257,  240,  242,    0,  300,
    0,    0,  147,    0,  194,  193,  324,  328,    0,    0,
  337,    0,    0,    0,    1,    2,    0,    0,    0,    0,
    0,  160,  199,    0,    0,  323,    0,    0,    0,   13,
    3,    4,    5,    6,    7,    8,    9,   10,   11,   65,
   66,   67,  161,  200,    0,  116,  163,  153,    0,    0,
  192,  202,  203,    0,    0,    0,    0,  339,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  128,    0,    0,
   37,    0,    0,    0,    0,    0,  292,  131,    0,    0,
    0,    0,  281,    0,    0,    0,    0,    0,  299,  152,
  150,    0,    0,    0,  330,  234,    0,    0,    0,  211,
  221,    0,    0,  235,    0,    0,    0,  229,    0,    0,
    0,    0,    0,  130,    0,  239,  241,    0,  145,  322,
  327,  336,  159,  198,    0,  146,    0,  195,    0,    0,
    0,    0,    0,    0,    0,    0,  326,  321,  338,    0,
  333,  334,  335,    0,    0,   25,    0,    0,    0,    0,
    0,    0,  127,   40,    0,   43,    0,    0,   50,    0,
    0,    0,    0,  295,    0,    0,  291,    0,  290,  141,
  284,    0,  280,    0,  279,    0,  246,    0,  249,    0,
    0,  250,    0,    0,    0,  302,    0,    0,    0,    0,
    0,    0,    0,  303,  304,  305,  306,  307,    0,    0,
  298,  149,  332,    0,  329,    0,  233,    0,  227,    0,
    0,    0,  225,  220,  228,    0,  213,    0,  236,    0,
  237,    0,    0,    0,    0,    0,    0,  288,  129,    0,
    0,  277,    0,  297,  144,    0,  154,    0,  155,  156,
  158,    0,  208,  197,    0,  204,  210,    0,    0,   81,
    0,    0,  320,    0,    0,    0,    0,    0,    0,    0,
    0,   23,   24,    0,    0,    0,    0,    0,   36,    0,
    0,   21,   47,   48,    0,    0,    0,   62,   49,   52,
   55,   53,   56,   54,  114,  294,  289,  283,  278,  258,
  252,    0,    0,    0,  245,  248,    0,  244,    0,    0,
    0,    0,    0,  318,    0,  301,  309,    0,  314,    0,
  331,  232,  226,    0,  231,    0,  223,  224,  212,  215,
  238,  218,    0,  343,  344,    0,    0,  346,    0,    0,
  349,  287,    0,  286,  276,    0,  275,  296,    0,    0,
    0,    0,    0,    0,  157,  196,  207,    0,   85,   86,
    0,   87,    0,   90,   91,   80,   83,  168,    0,  172,
    0,    0,    0,    0,  165,    0,    0,    0,    0,    0,
  180,  175,    0,    0,  178,    0,    0,   22,    0,    0,
    0,   33,    0,    0,    0,    0,   19,   20,    0,   58,
   57,   61,    0,  255,  251,    0,  262,    0,  265,    0,
    0,  266,  243,    0,    0,  316,    0,  312,  313,    0,
  315,  230,  222,  216,  350,  352,  353,  354,  351,  345,
  347,  285,  274,  187,    0,  191,    0,    0,    0,  182,
    0,    0,    0,    0,    0,  206,    0,    0,    0,    0,
  171,  167,  170,    0,    0,  164,    0,  166,  179,  174,
  177,  176,    0,   31,   32,    0,    0,    0,   29,   18,
   64,   63,  254,    0,  268,    0,    0,  261,  264,  260,
    0,  310,  311,    0,  186,  190,  185,    0,  189,    0,
  181,  183,   94,   92,   93,   89,  173,  169,   30,    0,
   27,   28,    0,  271,    0,  267,  259,  184,  188,   26,
  273,  270,
  };
  protected static readonly short [] yyDgoto  = {           189,
   37,   38,   39,   40,   41,   42,   43,   44,   45,   46,
   85,  257,   47,   92,   48,   49,   95,  276,  280,  281,
  282,  283,  396,  397,  510,  582,   50,   51,   52,   53,
   54,   55,  176,  254,  371,  255,  472,  473,  474,  475,
    0,    0,    0,   56,   57,   58,   59,   60,   61,   62,
   63,   64,   65,   66,   67,   68,  286,   69,   70,    0,
  111,  112,  113,  123,  124,  211,   71,  169,  262,  263,
  361,  377,  378,  481,  379,  380,  462,  463,   72,  171,
  126,   73,  174,  250,  364,  251,  366,  252,  225,  226,
  227,  342,  442,  228,  117,  118,  119,  300,  301,  302,
  411,  412,  413,  520,  521,  522,  585,  586,  614,  115,
  203,  101,  197,  121,  209,  312,  313,  314,  315,  316,
  317,  318,   74,   75,  180,  258,  215,   76,   77,   78,
  449,  450,  451,  539,
  };
  protected static readonly short [] yySindex = {         1456,
    0,   39,   17, 1587, 1634, -220,  -91,  -91,    0, -157,
  -89,    0,    0,    0,    0,    0,    0,    0,    0,  268,
  -87,  -77,  -71,    0,    0, -140,  -60, -110,    0,   17,
 -125,  177,   95,  -18, 1870,    0, 1456,    4,    6,   52,
   -5,   87,    0,    0,    0,    0,    0,    0,    0, -129,
  176,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  121,  646,
 -144,  187, -151,   75, -204,    0,    0,    0,    0,  -16,
   55,    0,   44,  543,  211,    0,   38,  861,  102,    0,
    0,   17,    0,    0,   17,  234,    0,    0,    0,    0,
  -49,    0,    0,    0,    0,    0,    0,    0,    0,    0,
 -147,    0,  327,    0,  -47,    0,    0,    0,   72,    0,
 -108,  135,    0,  338,    0,    0,    0,    0,  -98,  -25,
    0,   81,   89,  108,    0,    0,  -89,  268,  -87,  -77,
  -71,    0,    0, -140,  158,    0,   17,  177,  129,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0, -140,    0,    0,    0,  138,  145,
    0,    0,    0,   75,  149,  177,   17,    0,  147,  345,
  413,  -84,    9,  414,  160,  186,   26,    0,  351, 1587,
    0,  212,  -52, 1232,  359,  -83,    0,    0, -147, 1285,
  361,  -81,    0,  912,  169,   85,  231,  365,    0,    0,
    0, -140, 1341,  366,    0,    0,    1,   67,   56,    0,
    0,  170,  -43,    0,  -27,  448,  -73,    0,  370,  371,
  372,  -23, -147,    0,  -22,    0,    0,  -70,    0,    0,
    0,    0,    0,    0, -140,    0,  -65,    0,  177,   17,
  199,  177,  441,  -31,  457,   17,    0,    0,    0,  805,
    0,    0,    0,  -63,   17,    0,  445,  -33,   28,   17,
  459,  -61,    0,    0,  -59,    0,  184,  458,    0,  390,
  101,  432,  488,    0, 1681,  398,    0,  359,    0,    0,
    0,  399,    0,  361,    0,  269,    0, 1750,    0,  403,
 1504,    0,  406,  965,  215,    0,  302,  -50,  277,  -44,
 -151,  411, 1714,    0,    0,    0,    0,    0,   75, -204,
    0,    0,    0,  412,    0,  -43,    0,  -43,    0,   68,
   86,  -43,    0,    0,    0,  497,    0,  -11,    0,  225,
    0,  217,  416,  423,   57,  359,  -67,    0,    0,  361,
  -66,    0,  365,    0,    0,  752,    0,  426,    0,    0,
    0,   17,    0,    0,   41,    0,    0,   66,  232,    0,
  489,  149,    0,   17,   50, 1840,  -97, -183,  -64, -180,
   17,    0,    0,  -29,  518,   17,  501,  -21,    0,  -46,
   17,    0,    0,    0,  296,  508,  184,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  523,  244,  269,    0,    0, 1021,    0,  444,  449,
  124,  248,  187,    0,   75,    0,    0,  177,    0,  450,
    0,    0,    0,  -43,    0,  -43,    0,    0,    0,    0,
    0,    0,  225,    0,    0,  328,  -86,    0,  451,  278,
    0,    0,  359,    0,    0,  361,    0,    0,   50,   50,
 1810,   53, -165,   69,    0,    0,    0,  267,    0,    0,
  -32,    0,  545,    0,    0,    0,    0,    0,  336,    0,
   17,   17,   50,   50,    0,  301,  471,   17,  306,  474,
    0,    0,  308,  479,    0,  313,  493,    0,  584,   17,
   17,    0,  -20,  585,   17,   17,    0,    0,  314,    0,
    0,    0,  269,    0,    0,  269,    0, 1780,    0,  505,
 1551,    0,    0, 1072,  450,    0,  177,    0,    0, -174,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   17,    0,   17,   50,   50,    0,
  337,  506,  346,  514,  148,    0,  323,  126,  232,  601,
    0,    0,    0,   17,   50,    0,   17,    0,    0,    0,
    0,    0,   17,    0,    0,  602,   17,   17,    0,    0,
    0,    0,    0,  583,    0,  604,  269,    0,    0,    0,
  520,    0,    0,  155,    0,    0,    0,   17,    0,   17,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   17,
    0,    0,  321,    0,  269,    0,    0,    0,    0,    0,
    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,   90,   91,    0,   93,
  116,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,   33,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  132,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  568,    0,    0,    0,    0,    0,    0,    0,
    0,  150,    0,  615,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  116,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,   94,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   -6,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  692,  662,  -14,    0,  -17,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  524,    0,    0,    0,  525,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  526,    0,    0,    0,    0,    0,    0,    0,    0, 1374,
    0,    0,  527,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  -12,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   58, -167,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  -40,    0,  595,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, 1125,  333,    0,    0,    0,    0,    0,    0,    0,
    0,    0, 1404,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   10,    0,    0,    0,    0,    0,    0,  540,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  610,    0,    0,    0,    0,    0,    0,    0,
    0,   65, -166,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  548,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  380,    0, 1181,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,
  };
  protected static readonly short [] yyGindex = {            7,
    0,   -2,  625,    0,    0,    0,    0,    0,    0,    0,
  596,   -3,   64,  496,  404,  628,    0,    0,  407,    0,
    0,    0,  290,    0,    0,    0,    0,  456,    0,    0,
    0,    0,  -96,  325,    0,    0,  131,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0, -118,   -1,    0,    0,
  561,  -30,    0,  318,    0,  453, -152,  397,  470, -241,
  367,  348,  347,  -79,  349,  352,  264,  279, -148, -263,
    0, -114,  419, -240,  377,  490,    0,  -28,  405,    0,
    0,    0,  304,  -95,  609,  616,    0,  -82,    0,    0,
 -339,    0, -428, -317,    0,    0, -488,    0,    0,  605,
 -130,  618, -146,  621, -141,  460,    0,    0,    0,    0,
    0,    0, -103, -100,  437,  530,  566,    0,    0,    0,
    0,    0,  331,    0,
  };
  protected static readonly short [] yyTable = {            89,
   87,   91,   94,  131,  196,  360,   36,  386,  362,  223,
  175,  500,  175,  337,  207,  220,  170,   59,  202,  505,
  577,  341,  206,  223,  213,  558,  128,  485,  219,   84,
  370,  219,   12,  149,  214,  326,  175,  175,   88,  194,
  136,  200,  182,  150,   82,  177,  424,   84,  287,  289,
  217,  224,  207,  217,  309,  194,  200,  356,  310,   88,
  492,   88,  152,   88,  154,   86,  321,  268,  166,  224,
  293,  295,  279,  194,  515,  200,   88,  249,   81,   87,
  198,  292,  186,  224,  272,  348,  388,  584,  191,  479,
  332,  192,  311,   96,  324,  224,  354,   80,  616,  194,
  200,  328,  434,  319,  352,  172,  320,  234,  488,  102,
  156,  495,  167,  183,  130,  178,  179,  495,  491,  242,
  436,  327,  329,  333,  180,  179,  622,  335,  459,  205,
    2,  175,    3,    4,  175,  347,    6,  489,  351,   88,
  496,  407,  305,  241,   86,  159,  594,  256,   74,   76,
  224,   79,   44,  180,  179,  553,   99,   12,  584,  526,
  309,  224,  224,  409,  310,    1,  405,  100,  290,  114,
  173,  122,   88,  583,  110,  122,  261,  550,  266,  116,
  224,  448,  178,  339,  528,  120,  584,   91,  531,  177,
  127,  285,  129,  151,  204,  484,  125,  285,  311,  452,
  454,  299,  349,  589,    5,  308,  591,  304,  151,  319,
  285,  458,  320,  216,  277,  278,  130,  208,  416,  455,
  457,  419,  428,  486,  536,  537,  538,  214,  491,  265,
  432,  216,  433,  162,  435,  437,  438,  369,  163,  164,
  135,  557,  288,  357,  294,  216,  363,  170,  210,  340,
  381,   82,  391,  157,   84,  353,  493,  216,  453,  456,
  382,  383,  151,  217,  153,  218,  389,  506,  392,  187,
  336,  393,  151,  219,  151,   59,  195,  217,  201,  218,
  385,  423,  285,  592,  499,  219,  593,  219,  402,  404,
  429,  214,  504,  576,  221,  222,  149,  181,  299,   12,
   12,  299,  346,  350,  185,  421,  542,  217,  221,  222,
  155,  308,  216,   79,   12,   13,   14,   15,   16,   17,
   18,   19,  267,  216,  216,  543,  163,   24,  527,   79,
   12,   13,   14,   15,   16,   17,   18,   19,  532,  271,
  533,  387,  216,   24,  330,  158,  331,  460,   74,   76,
  178,   79,   44,   82,   83,  306,  193,  177,  363,   84,
  467,  468,  459,  460,  190,  446,  447,  277,  278,   82,
  478,  480,  199,  551,  149,   84,  256,  498,  178,  545,
  547,  212,  502,  469,  470,  177,  507,  508,  168,  555,
  117,  117,  117,  117,  117,  117,  117,  117,  117,  529,
  132,  133,  134,  564,  117,  117,  117,  229,  151,  163,
  164,  165,  243,  244,  519,  230,  151,  243,  244,  245,
  115,  115,  115,  272,  151,  151,  151,  151,  151,  151,
  151,  151,  151,  151,  231,  151,  151,  151,  151,  151,
  151,  548,  549,  604,  605,  151,  571,  569,  210,  151,
  151,  151,  151,  151,  151,  544,  546,  256,  240,  149,
  247,  239,   97,   98,  248,  253,  259,  260,  598,  600,
  151,  264,  269,  270,  243,  273,  275,  561,  562,  563,
  480,  194,  246,  200,  478,  564,  303,  207,  213,  334,
  401,  338,  343,  344,  345,  365,  574,  575,  368,  395,
  372,  579,  580,  384,  272,   79,   12,   13,   14,   15,
   16,   17,   18,   19,  399,  398,  149,  390,  519,   24,
   25,  519,  406,  408,  102,  410,   27,  415,  417,  322,
   28,   29,  420,  167,   32,  426,  431,  439,  441,  443,
  444,  595,  103,  596,  597,  599,  403,  445,  464,  471,
  476,  307,  104,  105,  106,  107,  108,  109,  501,  503,
  608,  563,  355,  562,  509,  511,  513,  514,  523,  609,
  525,  524,  530,  611,  612,  540,   79,   12,   13,   14,
   15,   16,   17,   18,   19,  535,  556,  447,  559,  110,
  142,  143,  560,  565,  618,  566,  619,  567,  568,  256,
  569,  145,  146,  570,  571,  148,  620,  256,  256,  256,
  256,  256,  256,  256,  256,  256,  256,  572,  256,  256,
  256,  256,  256,  256,  573,  578,  140,  581,  256,  588,
  601,  549,  256,  256,  256,  256,  256,  256,  602,  548,
  603,  607,  610,  613,  617,  621,  272,  615,   51,  113,
  247,  308,   60,  256,  272,  272,  272,  272,  272,  272,
  272,  272,  272,  272,  348,  272,  272,  272,  272,  272,
  272,   88,  263,  148,  160,  272,  184,  161,  394,  272,
  272,  272,  272,  272,  272,  274,  512,  400,    1,  606,
  140,    2,  140,    3,    4,    5,  477,    6,  233,  358,
  272,    7,    8,    9,   10,  422,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,  359,   21,   22,   23,
   24,   25,   26,  487,  465,  490,  554,   27,  497,  425,
  494,   28,   29,   30,   31,   32,   33,  148,  466,  148,
  552,  367,  440,  235,    1,   34,  534,    2,  236,    3,
    4,    5,   35,    6,  232,  237,  430,    7,    8,    9,
   10,  238,   11,   12,   13,   14,   15,   16,   17,   18,
   19,   20,  427,   21,   22,   23,   24,   25,   26,  325,
  541,    0,    0,   27,  209,  373,  209,   28,   29,   30,
   31,   32,   33,    0,    0,    0,    0,    0,    0,    0,
    0,   34,    0,    0,    0,    0,    0,    0,   35,    0,
    0,    0,    0,    0,  205,    0,  205,   79,   12,   13,
   14,   15,   16,   17,   18,   19,  140,    0,    0,    0,
    0,  142,    0,    0,  140,    0,    0,    0,    0,    0,
    0,    0,  140,  140,  140,  140,  140,  140,  140,  140,
  140,  140,    0,  140,  140,  140,  140,  140,  140,    0,
    0,    0,    0,  140,    0,    0,    0,  140,  140,  140,
  140,  140,  140,  148,    0,    0,  188,    0,    0,    0,
    0,  148,    0,    0,    0,    0,    0,    0,  140,  148,
  148,  148,  148,  148,  148,  148,  148,  148,  148,    0,
  148,  148,  148,  148,  148,  148,    0,    0,    0,    0,
  148,    0,    0,    0,  148,  148,  148,  148,  148,  148,
   79,   12,   13,   14,   15,   16,   17,   18,   19,  188,
    0,    0,    0,    0,    0,  148,  209,  209,  209,  209,
  209,  209,  209,  209,  209,    0,    0,    0,    0,    0,
  209,  209,    0,    0,    0,    0,    0,  209,  209,    0,
    0,  209,  209,    0,    0,  209,  205,  205,  205,  205,
  205,  205,  205,  205,  205,    0,    0,    0,    0,    0,
  205,  205,  209,    0,    0,  188,    0,  205,    0,    0,
    0,  205,  205,    0,    0,  205,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    1,    0,
    0,    2,  205,    3,    4,    5,    0,    6,    0,    0,
    0,    7,    8,    9,   10,    0,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,  297,   21,   22,   23,
   24,   25,   26,  374,  375,  459,  460,   27,    0,    0,
    0,   28,   29,   30,   31,   32,   33,    0,    0,    0,
    0,    1,    0,    0,    2,   34,    3,    4,    5,    0,
    6,    0,  461,    0,    7,    8,    9,   10,    0,   11,
   12,   13,   14,   15,   16,   17,   18,   19,   20,  418,
   21,   22,   23,   24,   25,   26,  374,  375,    0,    0,
   27,    0,    0,    0,   28,   29,   30,   31,   32,   33,
    0,    0,    0,    0,    0,    0,    0,    1,   34,    0,
    2,    0,    3,    4,    5,  376,    6,    0,    0,    0,
    7,    8,    9,   10,    0,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,  517,   21,   22,   23,   24,
   25,   26,    0,    0,    0,    0,   27,    0,    0,    0,
   28,   29,   30,   31,   32,   33,    0,    0,    0,    0,
    0,    0,    0,    0,   34,    0,    0,    0,  296,    0,
    0,   35,    0,    0,    0,    0,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,  590,   21,   22,   23,
   24,   25,   26,    0,    0,    0,    0,   27,    0,    0,
    0,   28,   29,   30,   31,   32,   33,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  296,  298,    0,    0,    0,    0,    0,    0,   11,
   12,   13,   14,   15,   16,   17,   18,   19,   20,  253,
   21,   22,   23,   24,   25,   26,    0,    0,    0,    0,
   27,    0,    0,    0,   28,   29,   30,   31,   32,   33,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  298,    0,  516,    0,    0,
    0,    0,    0,    0,    0,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,  269,   21,   22,   23,   24,
   25,   26,    0,    0,    0,    0,   27,    0,    0,    0,
   28,   29,   30,   31,   32,   33,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  516,    0,
    0,  518,    0,    0,    0,    0,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,  284,   21,   22,   23,
   24,   25,   26,    0,    0,    0,    0,   27,    0,    0,
    0,   28,   29,   30,   31,   32,   33,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  253,  518,    0,    0,    0,    0,    0,    0,  253,
  253,  253,  253,  253,  253,  253,  253,  253,  253,  291,
  253,  253,  253,  253,  253,  253,    0,    0,    0,    0,
  253,    0,    0,    0,  253,  253,  253,  253,  253,  253,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,  253,    0,  269,    0,    0,
    0,    0,    0,    0,    0,  269,  269,  269,  269,  269,
  269,  269,  269,  269,  269,  323,  269,  269,  269,  269,
  269,  269,    0,    0,    0,    0,  269,    0,    0,    0,
  269,  269,  269,  269,  269,  269,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  319,    0,
    0,  269,    0,    0,    0,    0,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,    0,   21,   22,   23,
   24,   25,   26,    0,    0,    0,    0,   27,  317,    0,
    0,   28,   29,   30,   31,   32,   33,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,   35,    0,    0,    0,    0,    0,    0,   11,
   12,   13,   14,   15,   16,   17,   18,   19,   20,    0,
   21,   22,   23,   24,   25,   26,    0,    0,    0,    0,
   27,    0,    0,    0,   28,   29,   30,   31,   32,   33,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   35,    0,    0,    0,    0,
    0,    0,    0,    0,    0,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,   21,   22,   23,   24,
   25,   26,    0,    0,    0,    0,   27,    0,    0,    0,
   28,   29,   30,   31,   32,   33,    0,    0,  319,  319,
  319,  319,  319,  319,  319,  319,  319,    0,    0,    0,
    0,   35,  319,  319,    0,    0,    0,    0,    0,  319,
    0,    0,    0,  319,  319,    0,    0,  319,  317,  317,
  317,  317,  317,  317,  317,  317,  317,    0,    0,    0,
    0,    0,  317,  317,  319,    0,    0,    0,    0,  317,
    0,    0,    0,  317,  317,    0,    0,  317,    0,    0,
    0,    0,    1,    0,    0,    2,    0,    3,    4,    5,
    0,    6,    0,    0,  317,    7,    8,    9,   10,    0,
   11,   12,   13,   14,   15,   16,   17,   18,   19,   20,
    0,   21,   22,   23,   24,   25,   26,    0,    0,    0,
    0,   27,    0,    0,    0,   28,   29,   30,   31,   32,
   33,    0,    0,    0,    0,    0,    0,    0,    0,   34,
  296,    0,    0,    0,    0,    0,   35,    0,   11,   12,
   13,   14,   15,   16,   17,   18,   19,   20,    0,   21,
   22,   23,   24,   25,   26,    0,    0,    0,    0,   27,
    0,    0,    0,   28,   29,   30,   31,   32,   33,    0,
    0,    0,    0,    0,    0,    0,    0,  516,    0,    0,
    0,    0,    0,    0,  298,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,   21,   22,   23,   24,
   25,   26,    0,    0,    0,    0,   27,    0,    0,    0,
   28,   29,   30,   31,   32,   33,    0,    0,    0,    0,
    0,   11,   12,   13,   14,   15,   16,   17,   18,   19,
   20,  518,   21,   22,   23,   24,   25,   26,    0,    0,
    0,    0,   27,    0,    0,    0,   28,   29,   30,   31,
   32,   33,    0,    0,    0,    0,    0,    0,    0,    0,
   90,    0,    0,    0,    0,    0,    0,   35,   11,   12,
   13,   14,   15,   16,   17,   18,   19,   20,    0,   21,
   22,   23,   24,   25,   26,    0,    0,    0,    0,   27,
    0,    0,    0,   28,   29,   30,   31,   32,   33,    0,
    0,    0,    0,    0,    0,    0,    0,   93,    0,    0,
    0,    0,    0,    0,   35,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,   21,   22,   23,   24,
   25,   26,    0,    0,    0,    0,   27,    0,    0,    0,
   28,   29,   30,   31,   32,   33,    0,    0,   79,   12,
   13,   14,   15,   16,   17,   18,   19,    0,    0,    0,
    0,   35,   24,   25,    0,    0,    0,    0,    0,   27,
    0,    0,    0,   28,   29,    0,  414,   32,    0,    0,
    0,    0,    0,    0,  137,   12,   13,   14,   15,   16,
   17,   18,   19,  138,  307,  139,  140,  141,  142,  143,
  144,    0,    0,    0,    0,    0,  587,    0,    0,  145,
  146,  147,    0,  148,  137,   12,   13,   14,   15,   16,
   17,   18,   19,  138,    0,  139,  140,  141,  142,  143,
  144,    0,    0,    0,    0,    0,    0,    0,    0,  145,
  146,  147,    0,  148,  137,   12,   13,   14,   15,   16,
   17,   18,   19,  138,    0,  139,  140,  141,  142,  143,
  144,  482,  483,  548,  549,    0,    0,    0,    0,  145,
  146,  147,    0,  148,  137,   12,   13,   14,   15,   16,
   17,   18,   19,  138,    0,  139,  140,  141,  142,  143,
  144,  482,  483,    0,    0,    0,    0,    0,    0,  145,
  146,  147,    0,  148,  137,   12,   13,   14,   15,   16,
   17,   18,   19,  138,    0,  139,  140,  141,  142,  143,
  144,    0,    0,    0,    0,    0,    0,    0,    0,  145,
  146,  147,    0,  148,
  };
  protected static readonly short [] yyCheck = {             3,
    2,    4,    5,   32,  101,  247,    0,   41,  249,   35,
   60,   41,   60,   41,  123,   41,   61,   58,  115,   41,
   41,   95,  119,   35,  123,   58,   30,  125,   41,   44,
   62,   44,    0,   35,   41,   35,   60,   60,  123,  123,
   59,  123,   59,   37,   62,   74,  310,   62,  195,  196,
   41,   95,  123,   44,  207,  123,  123,  123,  207,  123,
  125,  123,   59,  123,   59,    2,  208,   59,   70,   95,
  201,  202,  125,  123,  414,  123,  123,  174,   40,   81,
  111,  200,   84,   95,   59,  232,   59,  516,   92,   40,
   35,   95,  207,  314,  213,   95,  238,   59,  587,  123,
  123,   35,   35,  207,  235,  257,  207,  138,  292,  257,
   59,  292,  257,   59,   40,  320,  321,  292,  293,  148,
   35,  217,  218,  219,  292,  292,  615,  223,  294,   58,
  260,   60,  262,  263,   60,  232,  266,  321,  235,  123,
  321,  288,   58,  147,   81,   59,  321,  176,   59,   59,
   95,   59,   59,  321,  321,  321,  314,  125,  587,  423,
  313,   95,   95,  294,  313,  257,  285,  257,  199,  257,
  322,  316,  123,  513,  322,  316,  180,  125,  182,  257,
   95,  125,  125,  257,  425,  257,  615,  190,  430,  125,
  301,  194,  318,   44,  123,  293,  257,  200,  313,  346,
  347,  204,  233,  521,  264,  207,  524,  123,   59,  313,
  213,  353,  313,  257,  267,  268,   40,  326,  301,  350,
  351,  304,  319,  321,  311,  312,  313,  326,  293,  314,
  326,  257,  328,   58,  330,  331,  332,  269,  289,  290,
  259,  274,  326,  247,  326,  257,  250,   61,  314,  323,
  314,  269,  314,  259,  269,  326,  321,  257,  326,  326,
  264,  265,  259,  289,  259,  291,  270,  314,  272,   59,
  298,  275,  123,  299,  125,  316,  326,  289,  326,  291,
  314,  326,  285,  525,  314,  298,  527,  299,  282,  283,
  319,  298,  314,  314,  320,  321,  298,  314,  301,  267,
  268,  304,  326,  326,  261,  307,  453,  298,  320,  321,
  259,  313,  257,  275,  276,  277,  278,  279,  280,  281,
  282,  283,  314,  257,  257,  456,  289,  289,  425,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  434,  314,
  436,  314,  257,  289,  289,  259,  291,  295,  259,  259,
  293,  259,  259,  315,  316,  125,  123,  293,  362,  321,
  320,  321,  294,  295,  263,  309,  310,  267,  268,  315,
  374,  375,   46,  321,  376,  321,   44,  381,  321,  459,
  460,   44,  386,  318,  319,  321,  390,  391,   71,  321,
  275,  276,  277,  278,  279,  280,  281,  282,  283,  428,
  306,  307,  308,  483,  289,  290,  291,  327,  259,  289,
  290,  291,  289,  290,  417,  327,  267,  289,  290,  291,
  289,  290,  291,   44,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,  327,  286,  287,  288,  289,  290,
  291,  294,  295,  318,  319,  296,  292,  293,  314,  300,
  301,  302,  303,  304,  305,  459,  460,  125,  301,  461,
  323,  144,    7,    8,  320,  317,  320,  123,  548,  549,
  321,   59,   59,  314,  289,  125,  265,  481,  482,  483,
  484,  123,  165,  123,  488,  565,  318,  123,  123,  320,
   59,   44,  123,  123,  123,  297,  500,  501,   58,  316,
   44,  505,  506,   59,  125,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  125,   58,  518,   59,  521,  289,
  290,  524,  125,  125,  257,  257,  296,  125,  123,  212,
  300,  301,  318,  257,  304,  125,  125,   41,  314,  323,
  125,  545,  275,  547,  548,  549,   59,  125,  123,  318,
   62,  321,  285,  286,  287,  288,  289,  290,   41,   59,
  564,  565,  245,  567,  269,   58,   44,  324,  125,  573,
  323,  123,  123,  577,  578,  125,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  258,  320,  310,   44,  322,
  289,  290,  257,  293,  598,  125,  600,  292,  125,  267,
  293,  300,  301,  125,  292,  304,  610,  275,  276,  277,
  278,  279,  280,  281,  282,  283,  284,  125,  286,  287,
  288,  289,  290,  291,   41,   41,   59,  314,  296,  125,
  125,  295,  300,  301,  302,  303,  304,  305,  125,  294,
  318,   41,   41,   61,  125,  325,  267,   44,  125,  125,
  125,  125,   58,  321,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,  125,  286,  287,  288,  289,  290,
  291,   62,  125,   59,   50,  296,   81,   50,  275,  300,
  301,  302,  303,  304,  305,  190,  397,  281,  257,  559,
  123,  260,  125,  262,  263,  264,  372,  266,  138,  247,
  321,  270,  271,  272,  273,  309,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  284,  247,  286,  287,  288,
  289,  290,  291,  377,  358,  378,  463,  296,  380,  311,
  379,  300,  301,  302,  303,  304,  305,  123,  362,  125,
  462,  252,  338,  139,  257,  314,  443,  260,  140,  262,
  263,  264,  321,  266,  137,  140,  320,  270,  271,  272,
  273,  141,  275,  276,  277,  278,  279,  280,  281,  282,
  283,  284,  313,  286,  287,  288,  289,  290,  291,  214,
  450,   -1,   -1,  296,  123,  256,  125,  300,  301,  302,
  303,  304,  305,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  314,   -1,   -1,   -1,   -1,   -1,   -1,  321,   -1,
   -1,   -1,   -1,   -1,  123,   -1,  125,  275,  276,  277,
  278,  279,  280,  281,  282,  283,  259,   -1,   -1,   -1,
   -1,  289,   -1,   -1,  267,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  275,  276,  277,  278,  279,  280,  281,  282,
  283,  284,   -1,  286,  287,  288,  289,  290,  291,   -1,
   -1,   -1,   -1,  296,   -1,   -1,   -1,  300,  301,  302,
  303,  304,  305,  259,   -1,   -1,  125,   -1,   -1,   -1,
   -1,  267,   -1,   -1,   -1,   -1,   -1,   -1,  321,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  284,   -1,
  286,  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,
  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,
  275,  276,  277,  278,  279,  280,  281,  282,  283,  125,
   -1,   -1,   -1,   -1,   -1,  321,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,   -1,
  289,  290,   -1,   -1,   -1,   -1,   -1,  296,  297,   -1,
   -1,  300,  301,   -1,   -1,  304,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,   -1,
  289,  290,  321,   -1,   -1,  125,   -1,  296,   -1,   -1,
   -1,  300,  301,   -1,   -1,  304,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,   -1,
   -1,  260,  321,  262,  263,  264,   -1,  266,   -1,   -1,
   -1,  270,  271,  272,  273,   -1,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  284,  125,  286,  287,  288,
  289,  290,  291,  292,  293,  294,  295,  296,   -1,   -1,
   -1,  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,
   -1,  257,   -1,   -1,  260,  314,  262,  263,  264,   -1,
  266,   -1,  321,   -1,  270,  271,  272,  273,   -1,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  284,  125,
  286,  287,  288,  289,  290,  291,  292,  293,   -1,   -1,
  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  257,  314,   -1,
  260,   -1,  262,  263,  264,  321,  266,   -1,   -1,   -1,
  270,  271,  272,  273,   -1,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,  125,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  314,   -1,   -1,   -1,  267,   -1,
   -1,  321,   -1,   -1,   -1,   -1,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  284,  125,  286,  287,  288,
  289,  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,
   -1,  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  267,  321,   -1,   -1,   -1,   -1,   -1,   -1,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  284,  125,
  286,  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,
  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  321,   -1,  267,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,  125,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  267,   -1,
   -1,  321,   -1,   -1,   -1,   -1,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  284,  125,  286,  287,  288,
  289,  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,
   -1,  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,  267,  321,   -1,   -1,   -1,   -1,   -1,   -1,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  284,  125,
  286,  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,
  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  321,   -1,  267,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,  125,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  125,   -1,
   -1,  321,   -1,   -1,   -1,   -1,  275,  276,  277,  278,
  279,  280,  281,  282,  283,  284,   -1,  286,  287,  288,
  289,  290,  291,   -1,   -1,   -1,   -1,  296,  125,   -1,
   -1,  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,  321,   -1,   -1,   -1,   -1,   -1,   -1,  275,
  276,  277,  278,  279,  280,  281,  282,  283,  284,   -1,
  286,  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,
  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  321,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,   -1,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,  275,  276,
  277,  278,  279,  280,  281,  282,  283,   -1,   -1,   -1,
   -1,  321,  289,  290,   -1,   -1,   -1,   -1,   -1,  296,
   -1,   -1,   -1,  300,  301,   -1,   -1,  304,  275,  276,
  277,  278,  279,  280,  281,  282,  283,   -1,   -1,   -1,
   -1,   -1,  289,  290,  321,   -1,   -1,   -1,   -1,  296,
   -1,   -1,   -1,  300,  301,   -1,   -1,  304,   -1,   -1,
   -1,   -1,  257,   -1,   -1,  260,   -1,  262,  263,  264,
   -1,  266,   -1,   -1,  321,  270,  271,  272,  273,   -1,
  275,  276,  277,  278,  279,  280,  281,  282,  283,  284,
   -1,  286,  287,  288,  289,  290,  291,   -1,   -1,   -1,
   -1,  296,   -1,   -1,   -1,  300,  301,  302,  303,  304,
  305,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  314,
  267,   -1,   -1,   -1,   -1,   -1,  321,   -1,  275,  276,
  277,  278,  279,  280,  281,  282,  283,  284,   -1,  286,
  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,  296,
   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  267,   -1,   -1,
   -1,   -1,   -1,   -1,  321,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,   -1,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,   -1,   -1,
   -1,  275,  276,  277,  278,  279,  280,  281,  282,  283,
  284,  321,  286,  287,  288,  289,  290,  291,   -1,   -1,
   -1,   -1,  296,   -1,   -1,   -1,  300,  301,  302,  303,
  304,  305,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
  314,   -1,   -1,   -1,   -1,   -1,   -1,  321,  275,  276,
  277,  278,  279,  280,  281,  282,  283,  284,   -1,  286,
  287,  288,  289,  290,  291,   -1,   -1,   -1,   -1,  296,
   -1,   -1,   -1,  300,  301,  302,  303,  304,  305,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,   -1,  314,   -1,   -1,
   -1,   -1,   -1,   -1,  321,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  284,   -1,  286,  287,  288,  289,
  290,  291,   -1,   -1,   -1,   -1,  296,   -1,   -1,   -1,
  300,  301,  302,  303,  304,  305,   -1,   -1,  275,  276,
  277,  278,  279,  280,  281,  282,  283,   -1,   -1,   -1,
   -1,  321,  289,  290,   -1,   -1,   -1,   -1,   -1,  296,
   -1,   -1,   -1,  300,  301,   -1,  267,  304,   -1,   -1,
   -1,   -1,   -1,   -1,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,  321,  286,  287,  288,  289,  290,
  291,   -1,   -1,   -1,   -1,   -1,  267,   -1,   -1,  300,
  301,  302,   -1,  304,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,   -1,  286,  287,  288,  289,  290,
  291,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  300,
  301,  302,   -1,  304,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,   -1,  286,  287,  288,  289,  290,
  291,  292,  293,  294,  295,   -1,   -1,   -1,   -1,  300,
  301,  302,   -1,  304,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,   -1,  286,  287,  288,  289,  290,
  291,  292,  293,   -1,   -1,   -1,   -1,   -1,   -1,  300,
  301,  302,   -1,  304,  275,  276,  277,  278,  279,  280,
  281,  282,  283,  284,   -1,  286,  287,  288,  289,  290,
  291,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,  300,
  301,  302,   -1,  304,
  };

#line 824 "Swiften/SwiftParser.jay"

}


































#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 System.Diagnostics.Debug.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int IDENTIFIER = 257;
  public const int NUMBER = 258;
  public const int NEWLINE = 259;
  public const int FOR = 260;
  public const int IN = 261;
  public const int DO = 262;
  public const int WHILE = 263;
  public const int IF = 264;
  public const int ELSE = 265;
  public const int SWITCH = 266;
  public const int CASE = 267;
  public const int DEFAULT = 268;
  public const int WHERE = 269;
  public const int BREAK = 270;
  public const int CONTINUE = 271;
  public const int FALLTHROUGH = 272;
  public const int RETURN = 273;
  public const int EQEQ_OP = 274;
  public const int CLASS = 275;
  public const int MUTATING = 276;
  public const int NONMUTATING = 277;
  public const int OVERRIDE = 278;
  public const int STATIC = 279;
  public const int UNOWNED = 280;
  public const int UNOWNED_SAFE = 281;
  public const int UNOWNED_UNSAFE = 282;
  public const int WEAK = 283;
  public const int IMPORT = 284;
  public const int TYEPALIAS = 285;
  public const int STRUCT = 286;
  public const int ENUM = 287;
  public const int PROTOCOL = 288;
  public const int VAR = 289;
  public const int FUNC = 290;
  public const int LET = 291;
  public const int GET = 292;
  public const int SET = 293;
  public const int WILLSET = 294;
  public const int DIDSET = 295;
  public const int TYPEALIAS = 296;
  public const int ARROW_OP = 297;
  public const int DOTDOTDOT_OP = 298;
  public const int INOUT = 299;
  public const int CONVENIENCE = 300;
  public const int INIT = 301;
  public const int DEINIT = 302;
  public const int EXTENSION = 303;
  public const int SUBSCRIPT = 304;
  public const int OPERATOR = 305;
  public const int PREFIX = 306;
  public const int POSTFIX = 307;
  public const int INFIX = 308;
  public const int PRECEDENCE = 309;
  public const int ASSOCIATIVITY = 310;
  public const int LEFT = 311;
  public const int RIGHT = 312;
  public const int NONE = 313;
  public const int expression = 314;
  public const int expression_list = 315;
  public const int pattern = 316;
  public const int type_name = 317;
  public const int type_identifier = 318;
  public const int protocol_composition_type = 319;
  public const int type = 320;
  public const int attributes = 321;
  public const int operator_ = 322;
  public const int type_annotation = 323;
  public const int tuple_type = 324;
  public const int literal = 325;
  public const int type_inheritance_clause = 326;
  public const int operator = 327;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }
  internal class yyUnexpectedEof : yyException {
    public yyUnexpectedEof (string message) : base (message) {
    }
    public yyUnexpectedEof () : base ("") {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
