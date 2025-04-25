type expr =
    | CstI of int
    | Prim of string * expr * expr
    | Var of string

let c17 = CstI 17
let m34 = Prim("-", CstI 3, CstI 4)
let pm7910 = Prim("+", Prim("*", CstI 7, CstI 9), CstI 10)

let rec eval (e: expr) : int =
    match e with
    | CstI i -> i
    | Prim("+", e1, e2) -> eval e1 + eval e2
    | Prim("*", e1, e2) -> eval e1 * eval e2
    | Prim("-", e1, e2) -> eval e1 - eval e2
    | Prim _ -> failwith "unknown primitive"

eval c17
eval m34
eval pm7910
