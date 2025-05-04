// Torben Ægidius Mogensen
// Introduction to Compiler Design

module TorbenAST

type AST = //
    Literal

and Op = //
    | Add
    | Sub
    | Mul
    | Div

and Literal = //
    | Int of int
    | Float of float
    | Bool of bool
