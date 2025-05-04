// Torben Ægidius Mogensen
// Introduction to Compiler Design

module Torben.AST

type AST = //
    Expr list

and Expr = //
    | Int of int
    | Add of Expr * Expr
    | Sub of Expr * Expr
    | Mul of Expr * Expr
    | Div of Expr * Expr
    | Plus of Expr
    | Minus of Expr
    | Var of string
    | Set of string * Expr
