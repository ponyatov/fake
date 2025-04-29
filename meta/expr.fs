module expr

type expr =
    | CstI of int
    | Prim of string * expr * expr
    | Var of string

let c17 = CstI 17
let m34 = Prim("-", CstI 3, CstI 4)
let pm7910 = Prim("+", Prim("*", CstI 7, CstI 9), CstI 10)

type env = (string * int) list
let glob: env = [ ("a", 3); ("c", 78); ("baf", 666); ("b", 111) ]
let empty: env = []

let rec lookup (env: env) (name: string) =
    match env with
    | [] -> failwith $"{name} not found"
    | (k, v) :: r -> if name = k then v else lookup r name

// lookup empty "x"
// lookup glob "x"
// lookup glob "c"

let rec eval (e: expr) : int =
    match e with
    | CstI i -> i
    | Var v -> lookup glob v
    | Prim("+", e1, e2) -> eval e1 + eval e2
    | Prim("*", e1, e2) -> eval e1 * eval e2
    | Prim("-", e1, e2) -> eval e1 - eval e2
    | Prim _ -> failwith "unknown primitive"

// eval c17 // 17
// eval m34 // -1
// eval pm7910 // 73

let x = Var "x"
let baf = Var "baf"
// eval x // not found
// eval baf /// 666

let e3a = Prim("+", CstI 3, Var "a") // 3+a
let b9a = Prim("+", Prim("*", Var "b", CstI 9), Var "a") // b*9+a
// eval e3a // 6
// eval b9a // 1002
