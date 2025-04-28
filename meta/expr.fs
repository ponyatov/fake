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
c17 // 17
let x = Var "X" // X
let e3a = Prim("+", CstI 3, Var "a") // 3+a
let b9a = Prim("+", Prim("*", Var "b", CstI 9), Var "a") // b*9+a
let env = [ ("a", 3); ("c", 78); ("baf", 666); ("b", 111) ]
