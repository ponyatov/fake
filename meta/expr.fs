module expr

type expr =
    | CstI of int
    | Prim of string * expr * expr
    | Var of string
    | Let of string * expr * expr

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

let rec eval (e: expr) (env: env) : int =
    match e with
    | CstI i -> i

    | Var v -> lookup env v // existing variable
    | Let(v, rhs, body) ->
        let value = eval rhs env // eval binding value
        let envx = (v, value) :: env in // extend environment
        eval body envx // evaluate in nested env

    | Prim("+", e1, e2) -> eval e1 env + eval e2 env
    | Prim("*", e1, e2) -> eval e1 env * eval e2 env
    | Prim("-", e1, e2) -> eval e1 env - eval e2 env
    | Prim _ -> failwith "unknown primitive"

// eval c17 // 17
// eval m34 // -1
// eval pm7910 // 73

let baf = Var "baf"
// eval baf empty // not found
// eval baf glob  // 666

let e3a = Prim("+", CstI 3, Var "a") // 3+a
let b9a = Prim("+", Prim("*", Var "b", CstI 9), Var "a") // b*9+a
// eval e3a glob // 6
// eval b9a glob // 1002

// expression closed in variables set
let rec closedin (e: expr) (vars: string list) : bool =
    match e with
    | CstI i -> true // primitives always closed
    | Var v -> List.contains v vars // var name must be in list
    | Let(v, rhs, body) -> // nested scope check:
        let varx = v :: vars in // extend scope env

        closedin rhs varx //     rhc is closed
        && closedin body varx // and body is closed
    | Prim(_, e1, e2) -> // any binop expression
        closedin e1 vars // \ both subtrees
        && closedin e2 vars // / are closed

// expression closed if closed in empty var set []
let closed (e: expr) = closedin e []

type texpr = (* target expressions *)
    | TCstI of int (* constant index in runtime *)
    | TVar of int (* index into runtime environment *)
    | TLet of texpr * texpr (* erhs and ebody *)
    | TPrim of string * texpr * texpr


/// sestoft#2.4 deBruijn indexing
let rec tcomp (e: expr) (vars: string list) : texpr = //
    TCstI 0

let teval (te: texpr) (addrs: int list) : int = //
    0
