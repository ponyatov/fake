// http://timjones.io/blog/archive/2014/04/13/writing-a-minic-to-msil-compiler-in-fsharp-part-0-introduction

// http://timjones.io/blog/archive/2014/04/20/writing-a-minic-to-msil-compiler-in-fsharp-part-1-defining-the-abstract-syntax-tree

type Program = //
    Declaration list

and Declaration = //
    | VariableDeclaration of VariableDeclaration
    | FunctionDeclaration of FunctionDeclaration

and TypeSpec = //
    | Void
    | Bool
    | Int
    | Float

and VariableDeclaration = //
    TypeSpec * Identifier

and FunctionDeclaration = //
    TypeSpec * Identifier * Parameters * Parameters

and Identifier = //
    string

and IdentifierRef = //
    { Identifier: string }

and Parameters = //
    VariableDeclaration list

and BinaryOperator = //
    | Equal
    | Add
    | Subtract
    | Multiply
    | Divide
