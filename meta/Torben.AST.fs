// Torben Ægidius Mogensen
// Introduction to Compiler Design

module Torben.AST

type AST = //
    | Literal of Literal

and Op = //
    | Add
    | Sub
    | Mul
    | Div

and Literal = //
    | Int of int
    | Float of float
    | Bool of bool
