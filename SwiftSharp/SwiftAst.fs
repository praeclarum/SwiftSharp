namespace SwiftSharp


type SwiftType =
    | FunctionType of SwiftType * SwiftType
    | IdentifierType of string * (SwiftType list)
    | ImplicitlyUnwrappedOptionalType of SwiftType
    | NestedType of SwiftType list
    | OptionalType of SwiftType
    | TupleType of (SwiftType list) * bool

type Attr = string * string

type DeclarationSpecifier =
    | Static
    | Override
    | Class
    | Private

type Statement =
    | DeclarationStatement of Declaration
    | ExpressionStatement of Expression
    | ForInStatement of Pattern * Expression * (Statement list)
    | IfStatement of IfCondition * (Statement list) * ((Statement list) option)
    | ReturnStatement of Expression option
    | SwitchStatement of Expression * (SwitchCase list)

and IfCondition =
    | DeclarationIfCondition of Declaration
    | ExpressionIfCondition of Expression

and SwitchCase = (Pattern list) * (Statement list)

and GenericParameter = string

and Parameter = (Attr list) * (string option) * string * (SwiftType option) * (Expression option)

and Declaration =
    | ImportDeclaration of string list
    | GetterSetterVariableDeclaration of (DeclarationSpecifier list) * (string * SwiftType) * ((Statement list) * ((Statement list) option))
    | PatternVariableDeclaration of (DeclarationSpecifier list) * ((Pattern * (Expression option)) list)
    | ConstantDeclaration of (Pattern * (Expression option)) list
    | TypealiasDeclaration of string * SwiftType
    | StructDeclaration of string * (SwiftType list) * (Declaration list)
    | ClassDeclaration of string * (SwiftType list) * (Declaration list)
    | InitializerDeclaration of (Parameter list) * (Statement list)
    | FunctionDeclaration of (DeclarationSpecifier list) * string * ((Parameter list) list) * (FunctionResult option) * (Statement list)
    | ExtensionDeclaration of string * SwiftType * ((SwiftType list) option) * (Declaration list)
    | ProtocolDeclaration of string * string * ((SwiftType list) option) * (Declaration list)
    | RawValueEnumDeclaration of string * (SwiftType list) * ((string list) list)
    | UnionEnumDeclaration of string * ((GenericParameter list) option) * ((SwiftType list) option) * (((string * (SwiftType option)) list) list)

and Argument = (string option) * Expression

and Expression =
    | Number of float
    | Str of string
    | DictionaryLiteral of (Expression * Expression) list
    | ArrayLiteral of Expression list
    | Variable of string
    | Compound of Expression * (Binary list)
    | Funcall of Expression * (Argument list)
    | Member of (Expression option) * string
    | Closure of (((Parameter list) * (FunctionResult option)) option) * (Statement list)
    | OptionalChaining of Expression
    | InOut of string
    | TupleExpr of ((string option) * Expression) list
    | Subscript of Expression * (Expression list)

and FunctionResult = ((Attr list) * SwiftType)

and Binary =
    | AsCastBinary of SwiftType
    | AsOptionalCastBinary of SwiftType
    | TernaryConditionalBinary of Expression * Expression
    | OpBinary of string * Expression

and Pattern =
    | EnumCasePattern of (SwiftType option) * string * ((Pattern list) option)
    | ExpressionPattern of Expression
    | IdentifierPattern of string * (SwiftType option)
    | TuplePattern of (Pattern list) * (SwiftType option)
    | ValueBindingPattern of Pattern


