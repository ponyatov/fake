// Torben Ægidius Mogensen
// Introduction to Compiler Design

module Torben.AST

type AST = //
    Expr list

and Literal = //
    | Int of int
    | Float of float
    | Bool of bool

and Expr = //
    | Literal of Literal
    | Infix of Expr * Op * Expr
    | Pfx of Op * Expr

and Op = //
    | Add
    | Sub
    | Mul
    | Div
