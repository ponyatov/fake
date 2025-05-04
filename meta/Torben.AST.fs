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
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr
    | Plus of Expr
    | Minus of Expr
