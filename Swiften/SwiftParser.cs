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
//t    "top_level_declaration :",
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
//t  };
//t public static string getRule (int index) {
//t    return yyRule [index];
//t }
//t}
  protected static readonly string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    "'('","')'",null,null,"','",null,"'.'",null,null,null,null,null,null,
    null,null,null,null,null,"':'","';'","'<'","'='","'>'",null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,"'{'",null,"'}'",null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,"NEWLINE","FOR",
    "IN","DO","WHILE","IF","ELSE","SWITCH","CASE","DEFAULT","WHERE",
    "IDENTIFIER","BREAK","CONTINUE","FALLTHROUGH","RETURN","EQEQ_OP",
    "CLASS","MUTATING","NONMUTATING","OVERRIDE","STATIC","UNOWNED",
    "UNOWNED_SAFE","UNOWNED_UNSAFE","WEAK","IMPORT","TYEPALIAS","STRUCT",
    "ENUM","PROTOCOL","VAR","FUNC","LET","GET","SET","WILLSET","DIDSET",
    "TYPEALIAS","expression","expression_list","pattern","type_name",
    "type_identifier","protocol_composition_type","type",
    "function_declaration","enum_declaration","struct_declaration",
    "class_declaration","protocol_declaration","initializer_declaration",
    "deinitializer_declaration","extension_declaration",
    "subscript_declaration","operator_declaration","attributes",
    "operator_","type_annotation",
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
    2,    2,   47,   47,   48,   48,   49,   49,   49,   49,
   49,   49,   49,   49,   49,   50,   50,   12,   12,   44,
   44,   44,   44,   51,   51,   51,   51,   51,   51,   51,
   52,   52,   53,   53,   45,   45,   45,   45,   54,   54,
   55,   55,   56,   13,   13,   13,   13,   13,   13,   57,
   57,   57,   57,   58,   59,   59,   59,   62,   62,   63,
   63,   63,   63,   64,   60,   60,   60,   65,   65,   66,
   66,   61,   61,   61,   67,   67,   67,   67,   68,   68,
   68,   68,   46,   69,   71,   70,
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
    1,    1,    1,    1,    1,    1,    0,    3,    2,    4,
    3,    3,    2,    1,    1,    1,    1,    1,    1,    1,
    1,    3,    1,    1,    4,    3,    3,    2,    1,    3,
    2,    1,    1,    2,    4,    4,    4,    5,    4,    3,
    2,    2,    1,    1,    4,    3,    4,    3,    2,    4,
    3,    3,    2,    3,    4,    3,    4,    2,    1,    2,
    1,    4,    3,    4,    4,    3,    3,    2,    4,    3,
    3,    2,    2,    2,    1,    2,
  };
   static readonly short [] yyDefRed = {            0,
    0,    0,    0,    0,    0,   68,    0,    0,   77,    0,
  117,  118,  119,  120,  121,  122,  123,  124,  125,    0,
  163,    0,    0,    0,  103,  104,  105,  106,  107,  108,
  109,  110,  111,  112,    0,    0,    0,    0,    0,    0,
    0,    0,   14,   15,   16,   17,  101,   41,   42,    0,
    0,   69,   70,   71,   72,   99,  100,  102,    0,    0,
    0,    0,    0,    0,   35,    0,    0,    0,   34,    0,
    0,    0,   38,   39,    0,   45,   46,    0,    0,   73,
   75,   78,  143,  136,  134,  135,  137,  138,  139,  140,
  144,    0,  133,    0,    0,  148,    0,  195,  194,    1,
    2,    0,  161,    0,    0,   13,    3,    4,    5,    6,
    7,    8,    9,   10,   11,   65,   66,   67,  162,    0,
  116,  164,  154,    0,    0,  193,    0,    0,    0,    0,
    0,    0,    0,  129,    0,    0,   37,    0,    0,  132,
    0,  153,  151,    0,    0,  131,  146,  160,    0,  147,
    0,  196,    0,    0,   25,    0,    0,    0,    0,    0,
    0,  128,   40,    0,   43,    0,    0,   50,    0,    0,
    0,    0,  142,  150,  130,  145,    0,  155,    0,  156,
  157,  159,    0,   23,   24,    0,    0,    0,    0,    0,
   36,    0,    0,   21,   47,   48,    0,    0,    0,   62,
   49,   52,   55,   53,   56,   54,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,  158,   22,
    0,    0,    0,   33,    0,    0,    0,    0,   19,   20,
    0,   58,   57,   61,  169,    0,  173,    0,  188,    0,
  192,    0,    0,    0,    0,    0,    0,  166,    0,    0,
    0,    0,    0,  181,  176,    0,    0,  179,    0,    0,
  183,    0,    0,    0,    0,    0,    0,   31,   32,    0,
    0,    0,   29,   18,   64,   63,    0,  172,  187,  191,
  168,  171,    0,  186,    0,  190,    0,    0,  165,    0,
  167,  180,  175,  178,  177,  182,  184,   30,    0,   27,
   28,  174,  170,  185,  189,   26,
  };
  protected static readonly short [] yyDgoto  = {           135,
   37,   38,   39,   40,   41,   42,   43,   44,   45,   46,
   68,  235,   47,   75,   48,   49,   78,  165,  169,  170,
  171,  172,  198,  199,  232,  276,   50,   51,   52,   53,
   54,   55,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   56,   57,   58,    0,   59,   60,    0,
   92,   93,   94,   96,   97,  143,   61,  124,  180,  181,
  182,  212,  213,  238,  214,  215,  216,  217,   62,  126,
   99,
  };
  protected static readonly short [] yySindex = {          302,
  -26,  -78,  342,  382, -235,    0, -219, -219,    0, -217,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -177,
    0, -243, -198,  -47,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  422,    0,  302,  -44,  -42,  -41,
 -173,  -40,    0,    0,    0,    0,    0,    0,    0, -160,
   45,    0,    0,    0,    0,    0,    0,    0, -234, -161,
 -220,   64,  -50,  128,    0, -170,  439,   75,    0, -153,
  246, -125,    0,    0,  -78,    0,    0,  -78,   15,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0, -221,    0,   93, -156,    0,   97,    0,    0,    0,
    0, -177,    0, -243, -226,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -243,
    0,    0,    0, -172, -158,    0,   83, -102,  -39,   86,
 -149, -140,  -33,    0,   25,  342,    0, -110,  -93,    0,
 -221,    0,    0, -243, -221,    0,    0,    0, -243,    0,
 -100,    0,  -77,  -78,    0,   95,  -38,  -32,  -78,   96,
  -66,    0,    0,  -88,    0, -142,  100,    0,   35, -171,
   31,   87,    0,    0,    0,    0,  190,    0,   40,    0,
    0,    0,  -78,    0,    0,  -37,  120,  -78,  105,  -36,
    0,  -65,  -78,    0,    0,    0,  -99,  107, -142,    0,
    0,    0,    0,    0,    0,    0,  -78,  -18,  -18,  -18,
  152, -115, -250,  -91, -247, -101, -241, -225,    0,    0,
  126,  -78,  -78,    0,  -35,  130,  -78,  -78,    0,    0,
 -130,    0,    0,    0,    0,  -92,    0,  -78,    0,  -78,
    0,  -78,  -78,  -18,  -18,  -18,  -18,    0, -123,   50,
  -78, -113,   54,    0,    0, -112,   57,    0, -108,   59,
    0, -109,   63, -104,   67, -163,  -78,    0,    0,  154,
  -78,  -78,    0,    0,    0,    0,  156,    0,    0,    0,
    0,    0,  -78,    0,  -78,    0,  -78,  -18,    0,  -78,
    0,    0,    0,    0,    0,    0,    0,    0,  -78,    0,
    0,    0,    0,    0,    0,    0,
  };
  protected static readonly short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,  -34,  -30,    0,  -29,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    2,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0, -166,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  -48,  -43,    0,  -46,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,  -28,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,   77,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,  -51,    0,  146,    0,
    0,    0,    0,    0,    0,    0,  -89, -232,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,  -19, -231,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,
  };
  protected static readonly short [] yyGindex = {           28,
    0,   34,  155,    0,    0,    0,    0,    0,    0,    0,
  168,   -2,   38,   98,   69,  185,    0,    0,  104,    0,
    0,    0,   76,    0,    0,    0,    0,  125,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    7,    0,    0,
  135,  -49,    0,  -21,    0,   88,    0,    0,    0,    0,
   99,   53,   65, -159,   61,   66,   62,   68,    0,    0,
    0,
  };
  protected static readonly short [] yyTable = {            72,
  152,   12,  188,  222,  227,  271,   59,   70,  128,  248,
  141,  101,  149,   64,  108,  152,  110,  112,  115,  157,
   71,  236,  177,  261,   74,  161,  190,   36,   76,   79,
   44,  168,   63,  255,   71,  179,   74,   77,   69,  123,
  251,  105,  140,  258,   71,   71,   83,  122,    6,  240,
  242,  209,  146,  119,   95,  120,   71,   71,  181,  180,
   79,  148,  252,  149,  106,  259,  121,  209,  210,   98,
   70,  264,  137,  132,  141,  138,  149,   95,   82,  152,
  181,  180,  147,  113,  283,  285,  287,  266,  131,  203,
   83,  173,   91,  166,  167,  175,   84,    1,  150,    2,
    3,   69,  118,    5,   71,  178,   85,   86,   87,   88,
   89,   90,   11,   12,   13,   14,   15,   16,   17,   18,
   19,  115,  174,  115,  125,  155,   12,  176,  283,  245,
  246,   80,   81,  133,  119,  136,   91,  139,  141,  142,
  144,  153,  151,  152,  158,  205,  159,  148,  178,  162,
  184,  185,  164,  186,  192,  197,  191,  200,  194,  201,
  223,  195,  218,  225,  233,  275,  267,  231,  288,   74,
  272,  166,  167,    4,  289,  277,  247,  290,  291,  292,
  220,  293,  294,  295,  246,  224,  129,  296,  245,  229,
  230,  297,  210,  154,  299,  142,  302,  249,  204,  206,
  254,   51,  179,   60,  116,  237,  239,  241,  141,  100,
  149,  262,  107,  152,  109,  111,  114,  105,  183,  268,
  269,  256,   74,  179,  273,  274,   76,   79,   44,  193,
  228,  130,  196,  163,  117,  278,  145,  279,  179,  280,
  281,  282,  284,  286,  237,  127,   59,   11,   12,   13,
   14,   15,   16,   17,   18,   19,  156,  187,  221,  226,
  270,   21,  160,  189,  298,  253,   12,   12,  300,  301,
   65,   66,  178,  202,  234,  260,  250,  219,  265,  257,
  303,    0,  304,  263,  305,  282,   67,  281,    1,    0,
    2,    3,    4,  178,    5,    0,  306,    0,    6,    7,
    8,    9,   10,    0,   11,   12,   13,   14,   15,   16,
   17,   18,   19,   20,  134,    0,    0,    0,   21,    0,
   22,    0,    0,    0,    0,   23,   24,    0,    0,    0,
    0,    0,    0,   25,   26,   27,   28,   29,   30,   31,
   32,   33,   34,   35,    1,    0,    2,    3,    4,    0,
    5,    0,    0,    0,    6,    7,    8,    9,   10,    0,
   11,   12,   13,   14,   15,   16,   17,   18,   19,   20,
  134,    0,    0,    0,   21,    0,   22,    0,    0,    0,
    0,   23,   24,    0,    0,    0,    0,    0,    0,   25,
   26,   27,   28,   29,   30,   31,   32,   33,   34,   35,
    0,   11,   12,   13,   14,   15,   16,   17,   18,   19,
    0,    0,    0,    0,    0,   21,    0,    0,    0,    0,
    0,    0,    0,    0,   65,   11,   12,   13,   14,   15,
   16,   17,   18,   19,  102,    0,    0,    0,    0,  103,
   67,  104,  243,  244,  245,  246,    0,    1,    0,    2,
    3,    4,    0,    5,    0,    0,    0,    6,    7,    8,
    9,   10,    0,   11,   12,   13,   14,   15,   16,   17,
   18,   19,   20,    0,    0,    0,    0,   21,    0,   22,
  207,  208,  209,  210,   23,   24,    0,    0,    0,    0,
    0,    0,   25,   26,   27,   28,   29,   30,   31,   32,
   33,   34,  211,    1,    0,    2,    3,    4,    0,    5,
    0,    0,    0,    6,    7,    8,    9,   10,    0,   11,
   12,   13,   14,   15,   16,   17,   18,   19,   20,    0,
    0,    0,    0,   21,    0,   22,    0,    0,    0,    0,
   23,   24,    0,    0,    0,    0,    0,    0,   25,   26,
   27,   28,   29,   30,   31,   32,   33,   34,   35,    1,
    0,    2,    3,    4,    0,    5,    0,    0,    0,    6,
    7,    8,    9,   10,    0,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,    0,    0,    0,   21,
    0,   22,    0,    0,    0,    0,   23,   24,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,   30,
   31,   32,   33,   34,   35,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,    0,    0,    0,   21,
    0,   22,    0,    0,    0,    0,   23,   73,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,   30,
   31,   32,   33,   34,   35,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,    0,    0,    0,    0,   21,
    0,   22,    0,    0,    0,    0,   23,   76,    0,    0,
    0,    0,    0,    0,   25,   26,   27,   28,   29,   30,
   31,   32,   33,   34,   35,   11,   12,   13,   14,   15,
   16,   17,   18,   19,  102,    0,    0,    0,    0,  103,
    0,  104,   11,   12,   13,   14,   15,   16,   17,   18,
   19,    0,    0,    0,    0,    0,  103,
  };
  protected static readonly short [] yyCheck = {             2,
   44,    0,   41,   41,   41,   41,   58,    1,   59,  125,
   59,   59,   59,   40,   59,   59,   59,   59,   59,   59,
  123,   40,  123,  125,   59,   59,   59,    0,   59,   59,
   59,  125,   59,  125,  123,  125,    3,    4,    1,   61,
  291,   35,   92,  291,  123,  123,  268,  268,  268,  209,
  210,  293,  102,  288,  298,  290,  123,  123,  291,  291,
  296,  288,  313,  290,   37,  313,   60,  293,  294,  268,
   64,  313,   75,   67,  123,   78,  123,  298,  296,  123,
  313,  313,  104,  257,  244,  245,  246,  313,  259,   59,
  268,  141,  314,  265,  266,  145,  274,  258,  120,  260,
  261,   64,   58,  264,  123,  125,  284,  285,  286,  287,
  288,  289,  274,  275,  276,  277,  278,  279,  280,  281,
  282,  288,  144,  290,   61,  128,  125,  149,  288,  293,
  294,    7,    8,   59,  288,  261,  314,  123,   46,  296,
   44,   59,  315,  302,   59,   59,  296,  288,  151,  125,
  153,  154,  263,   59,   59,  298,  159,   58,  161,  125,
   41,  164,  123,   59,   58,  296,   41,  267,  292,  136,
   41,  265,  266,  262,  125,  268,  292,  291,  125,  292,
  183,  125,  291,  125,  294,  188,   59,  125,  293,  192,
  193,  125,  294,  296,   41,  296,   41,  313,  171,  172,
  292,  125,  292,   58,   50,  208,  209,  210,  257,  257,
  257,  313,  257,  257,  257,  257,  257,  211,  296,  222,
  223,  313,  257,  313,  227,  228,  257,  257,  257,  296,
  296,   64,  164,  136,   50,  238,  102,  240,  151,  242,
  243,  244,  245,  246,  247,  296,  298,  274,  275,  276,
  277,  278,  279,  280,  281,  282,  296,  296,  296,  296,
  296,  288,  296,  296,  267,  213,  265,  266,  271,  272,
  297,  298,  292,  170,  199,  215,  212,  179,  217,  214,
  283,   -1,  285,  216,  287,  288,  313,  290,  258,   -1,
  260,  261,  262,  313,  264,   -1,  299,   -1,  268,  269,
  270,  271,  272,   -1,  274,  275,  276,  277,  278,  279,
  280,  281,  282,  283,  125,   -1,   -1,   -1,  288,   -1,
  290,   -1,   -1,   -1,   -1,  295,  296,   -1,   -1,   -1,
   -1,   -1,   -1,  303,  304,  305,  306,  307,  308,  309,
  310,  311,  312,  313,  258,   -1,  260,  261,  262,   -1,
  264,   -1,   -1,   -1,  268,  269,  270,  271,  272,   -1,
  274,  275,  276,  277,  278,  279,  280,  281,  282,  283,
  125,   -1,   -1,   -1,  288,   -1,  290,   -1,   -1,   -1,
   -1,  295,  296,   -1,   -1,   -1,   -1,   -1,   -1,  303,
  304,  305,  306,  307,  308,  309,  310,  311,  312,  313,
   -1,  274,  275,  276,  277,  278,  279,  280,  281,  282,
   -1,   -1,   -1,   -1,   -1,  288,   -1,   -1,   -1,   -1,
   -1,   -1,   -1,   -1,  297,  274,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,  288,
  313,  290,  291,  292,  293,  294,   -1,  258,   -1,  260,
  261,  262,   -1,  264,   -1,   -1,   -1,  268,  269,  270,
  271,  272,   -1,  274,  275,  276,  277,  278,  279,  280,
  281,  282,  283,   -1,   -1,   -1,   -1,  288,   -1,  290,
  291,  292,  293,  294,  295,  296,   -1,   -1,   -1,   -1,
   -1,   -1,  303,  304,  305,  306,  307,  308,  309,  310,
  311,  312,  313,  258,   -1,  260,  261,  262,   -1,  264,
   -1,   -1,   -1,  268,  269,  270,  271,  272,   -1,  274,
  275,  276,  277,  278,  279,  280,  281,  282,  283,   -1,
   -1,   -1,   -1,  288,   -1,  290,   -1,   -1,   -1,   -1,
  295,  296,   -1,   -1,   -1,   -1,   -1,   -1,  303,  304,
  305,  306,  307,  308,  309,  310,  311,  312,  313,  258,
   -1,  260,  261,  262,   -1,  264,   -1,   -1,   -1,  268,
  269,  270,  271,  272,   -1,  274,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,  288,
   -1,  290,   -1,   -1,   -1,   -1,  295,  296,   -1,   -1,
   -1,   -1,   -1,   -1,  303,  304,  305,  306,  307,  308,
  309,  310,  311,  312,  313,  274,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,  288,
   -1,  290,   -1,   -1,   -1,   -1,  295,  296,   -1,   -1,
   -1,   -1,   -1,   -1,  303,  304,  305,  306,  307,  308,
  309,  310,  311,  312,  313,  274,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,  288,
   -1,  290,   -1,   -1,   -1,   -1,  295,  296,   -1,   -1,
   -1,   -1,   -1,   -1,  303,  304,  305,  306,  307,  308,
  309,  310,  311,  312,  313,  274,  275,  276,  277,  278,
  279,  280,  281,  282,  283,   -1,   -1,   -1,   -1,  288,
   -1,  290,  274,  275,  276,  277,  278,  279,  280,  281,
  282,   -1,   -1,   -1,   -1,   -1,  288,
  };

#line 463 "Swiften/SwiftParser.jay"

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
  public const int NEWLINE = 257;
  public const int FOR = 258;
  public const int IN = 259;
  public const int DO = 260;
  public const int WHILE = 261;
  public const int IF = 262;
  public const int ELSE = 263;
  public const int SWITCH = 264;
  public const int CASE = 265;
  public const int DEFAULT = 266;
  public const int WHERE = 267;
  public const int IDENTIFIER = 268;
  public const int BREAK = 269;
  public const int CONTINUE = 270;
  public const int FALLTHROUGH = 271;
  public const int RETURN = 272;
  public const int EQEQ_OP = 273;
  public const int CLASS = 274;
  public const int MUTATING = 275;
  public const int NONMUTATING = 276;
  public const int OVERRIDE = 277;
  public const int STATIC = 278;
  public const int UNOWNED = 279;
  public const int UNOWNED_SAFE = 280;
  public const int UNOWNED_UNSAFE = 281;
  public const int WEAK = 282;
  public const int IMPORT = 283;
  public const int TYEPALIAS = 284;
  public const int STRUCT = 285;
  public const int ENUM = 286;
  public const int PROTOCOL = 287;
  public const int VAR = 288;
  public const int FUNC = 289;
  public const int LET = 290;
  public const int GET = 291;
  public const int SET = 292;
  public const int WILLSET = 293;
  public const int DIDSET = 294;
  public const int TYPEALIAS = 295;
  public const int expression = 296;
  public const int expression_list = 297;
  public const int pattern = 298;
  public const int type_name = 299;
  public const int type_identifier = 300;
  public const int protocol_composition_type = 301;
  public const int type = 302;
  public const int function_declaration = 303;
  public const int enum_declaration = 304;
  public const int struct_declaration = 305;
  public const int class_declaration = 306;
  public const int protocol_declaration = 307;
  public const int initializer_declaration = 308;
  public const int deinitializer_declaration = 309;
  public const int extension_declaration = 310;
  public const int subscript_declaration = 311;
  public const int operator_declaration = 312;
  public const int attributes = 313;
  public const int operator_ = 314;
  public const int type_annotation = 315;
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
