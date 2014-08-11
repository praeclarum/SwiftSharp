namespace SwiftSharp


type Type =
    | IdentifierType of string * (Type list)
    | NestedType of Type list
    | ImplicitlyUnwrappedOptionalType of Type
    | OptionalType of Type
    | FunctionType of Type * Type
    | TupleType of (Type list) * bool
    | ArrayType of Type
    | DictionaryType of Type * Type

type Attr = string * string
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

and FunctionResult = ((Attr list) * Type)

and Binary =
    | OpBinary of string * Expression
    | AsBinary of Type

and Pattern =
    | IdentifierPattern of string * (Type option)
    | ExpressionPattern of Expression
    | TuplePattern of (Pattern list) * (Type option)


