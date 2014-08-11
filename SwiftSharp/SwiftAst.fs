namespace SwiftSharp


type Type =
    | ArrayType of Type
    | DictionaryType of Type * Type
    | FunctionType of Type * Type
    | IdentifierType of string * (Type list)
    | ImplicitlyUnwrappedOptionalType of Type
    | NestedType of Type list
    | OptionalType of Type
    | TupleType of (Type list) * bool

type Attr = string * string

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

and GenericParameter = string

and Parameter = (Attr list) * (string option) * string * (Type option) * (Expression option)

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
    | UnionEnumDeclaration of string * ((GenericParameter list) option) * ((Type list) option) * (((string * (Type option)) list) list)

and Argument = (string option) * Expression

and Expression =
    | Number of float
    | Str of string
    | Variable of string
    | Compound of Expression * (Binary list)
    | Funcall of Expression * (Argument list)
    | Member of (Expression option) * string
    | Closure of (((Parameter list) * (FunctionResult option)) option) * (Statement list)
    | OptionalChaining of Expression
    | InOut of string
    | TupleExpr of ((string option) * Expression) list
    | Subscript of Expression * (Expression list)

and FunctionResult = ((Attr list) * Type)

and Binary =
    | AsBinary of Type
    | OpBinary of string * Expression

and Pattern =
    | EnumCasePattern of (Type option) * string * ((Pattern list) option)
    | ExpressionPattern of Expression
    | IdentifierPattern of string * (Type option)
    | TuplePattern of (Pattern list) * (Type option)
    | ValueBindingPattern of Pattern


