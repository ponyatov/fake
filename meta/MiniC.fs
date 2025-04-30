// http://timjones.io/blog/archive/2014/04/13/writing-a-minic-to-msil-compiler-in-fsharp-part-0-introduction

// http://timjones.io/blog/archive/2014/04/20/writing-a-minic-to-msil-compiler-in-fsharp-part-1-defining-the-abstract-syntax-tree

type Program = //
    Decl list

and Decl =
    | VarDecl of VarDecl
    | FunDecl of FunDecl

and TypeSpec =
    | Void
    | Bool
    | Int
    | Float

and VarDecl =
    | ScalarDecl of TypeSpec * Id
    | ArrayDecl of TypeSpec * Id

and FunDecl = //
    TypeSpec * Id * Params * Compound

and Id = //
    string

and IdRef = //
    { Id: string }

and Params = //
    VarDecl list

and Statement =
    | Expression of Expression
    | Compound of Compound
    | If of If
    | While of While
    | Return of Expression option
    | Break

