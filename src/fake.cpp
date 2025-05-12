/// @file
/// @brief @ref VM core code

#include "fake.hpp"
#include "fake.lex.hpp"

int main(int argc, char *argv[]) {
    arg(0, argv[0]);
    for (int i = 1; i < argc; i++) {  //
        arg(i, argv[i]);
        yyfile = argv[i];
        assert(yyin = fopen(yyfile, "r"));
        yyparse();
        fclose(yyin);
        yyfile = nullptr;
    }
    return 0;
}

void arg(int argc, char *argv) {  //
    fprintf(stderr, "arg[%i] = <%s>\n", argc, argv);
}

bool batch = true;

void recovery() {
    assert(!batch);
    fprintf(stderr, "\n");
    yy_scan_string("");
}

void yyerror(const char *msg) {
    fprintf(stderr, "\n\n%s:%i %s [%s]\n\n", yyfile, yylineno, msg, yytext);
    if (batch)
        exit(-1);
    else
        recovery();
}

byte M[Msz];
addr Cp = 0;
addr Ip = 0;

cell D[Dsz];
byte Dp = 0;

void quest() {
    if (trace) fprintf(stderr, "\tquest\n");
    fprintf(stderr, "\n[ ");
    for (int i = 0; i < Dp; i++) fprintf(stderr, "%i ", D[i]);
    fprintf(stderr, "]\n");
    push(0x0);
    push(0x10);
    dump();
}

void dump() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "dump\t%.4X %i\n", D[Dp - 2], D[Dp - 1]);
    addr s = D[--Dp];
    assert(s < 0x100);
    addr a = D[--Dp];
    assert(a < 0x100);
    for (addr i = a; i < a + s; i++) {
        if (i % 0x10 == 0) fprintf(stderr, "\n%.4X:\t", i);
        fprintf(stderr, "%.2X ", M[i]);
    }
    fprintf(stderr, "\n");
}

void push(cell n) {
    assert(Dp < Dsz);
    if (trace) fprintf(stderr, "\tpush\t%i\n", n);
    D[Dp++] = n;
}

cell top() {
    assert(Dp > 0);
    cell n = D[Dp - 1];
    if (trace) fprintf(stderr, "\ttop\t%i\n", n);
    return n;
}

cell pop() {
    assert(Dp > 0);
    cell n = D[--Dp];
    if (trace) fprintf(stderr, "\tpop\t%i\n", n);
    return n;
}

void dup() {
    assert(Dp > 0);
    if (trace) fprintf(stderr, "dup\t%i\n", D[Dp - 1]);
    push(top());
}

void drop() {
    assert(Dp > 0);
    if (trace) fprintf(stderr, "drop\t%i\n", D[Dp - 1]);
    pop();
}

void swap() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "swap\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    cell n = D[Dp - 1];
    D[Dp - 1] = D[Dp - 2];
    D[Dp - 2] = n;
}

void over() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "over\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp++] = D[Dp - 2];
}

void press() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "press\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 1];
    Dp--;
}

void rrot() {
    assert(Dp >= 3);
    if (trace)
        fprintf(stderr, "rrot\t%i %i %i\n", D[Dp - 3], D[Dp - 2], D[Dp - 1]);
    cell n = D[Dp - 3];
    D[Dp - 3] = D[Dp - 2];
    D[Dp - 2] = D[Dp - 1];
    D[Dp - 1] = n;
}

void lrot() {
    assert(Dp >= 3);
    if (trace)
        fprintf(stderr, "lrot\t%i %i %i\n", D[Dp - 3], D[Dp - 2], D[Dp - 1]);
    cell n = D[Dp - 1];
    D[Dp - 1] = D[Dp - 2];
    D[Dp - 2] = D[Dp - 3];
    D[Dp - 3] = n;
}

void pick() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "pick\t%i\n", D[Dp - 1]);
    int idx = pop();
    assert(Dp > idx);
    push(D[Dp - idx - 1]);
}

void depth() {
    if (trace) fprintf(stderr, "depth\t%i\n", Dp);
    push(Dp);
}

void dot() {
    if (trace) fprintf(stderr, "dot\n");
    Dp = 0;
}

bool compile = false;

bool trace = true;

void cmd(Op op) {
    if (trace) fprintf(stderr, "\n%.4X: %.2X ", Ip, op);
    switch (op) {
        case Op::nop:
            nop();
            break;
        case Op::halt:
            halt();
            break;
        case Op::quest:
            quest();
            break;
        case Op::dump:
            dump();
            break;
        case Op::repl:
            repl();
            break;
        case Op::dup:
            dup();
            break;
        case Op::drop:
            drop();
            break;
        case Op::swap:
            swap();
            break;
        case Op::over:
            over();
            break;
        case Op::press:
            press();
            break;
        case Op::rrot:
            rrot();
            break;
        case Op::lrot:
            lrot();
            break;
        case Op::pick:
            pick();
            break;
        case Op::depth:
            depth();
            break;
        case Op::dot:
            dot();
            break;
        case Op::add:
            add();
            break;
        case Op::sub:
            sub();
            break;
        case Op::mul:
            mul();
            break;
        case Op::div:
            div();
            break;
        case Op::mod:
            mod();
            break;
        case Op::pow:
            pow();
            break;
        case Op::neg:
            neg();
            break;
        default:
            abort();
    }
}

#define NOPARAM "     "

void nop() {
    if (trace) fprintf(stderr, NOPARAM "nop\n");
}

void halt() {
    if (trace) fprintf(stderr, NOPARAM "halt\n");
    exit(0);
}

void add() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "add\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 2] + D[Dp - 1];
    Dp--;
}

void sub() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "sub\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 2] - D[Dp - 1];
    Dp--;
}

void mul() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "mul\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 2] * D[Dp - 1];
    Dp--;
}

void div() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "div\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 2] / D[Dp - 1];
    Dp--;
}

void mod() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "mod\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp - 2] = D[Dp - 2] % D[Dp - 1];
    Dp--;
}

void pow() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "pow\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    int exp = D[--Dp];
    assert(exp >= 0);
    int n = D[--Dp];
    int r = 1;
    for (int i = 0; i < exp; i++) r *= n;
    D[Dp++] = r;
}

void neg() {
    assert(Dp >= 1);
    if (trace) fprintf(stderr, "neg\t%i\n", D[Dp - 1]);
    D[Dp - 1] = -D[Dp - 1];
}

void highlight(char *line) {
    if (!line) {  // on Ctrl+C/D
        trace = false;
        halt();
    }

    if (strlen(line)) add_history(line);
    yyfile = (char *)"repl";
    yylineno = 0;
    if (trace) fprintf(stderr, "input: %s\n", line);
    yy_scan_string(line);
    yyparse();
    yyfile = nullptr;
    dump();
    free(line);
}

void repl() {
    if (trace) fprintf(stderr, NOPARAM "repl\n");
    batch = false;
    rl_callback_handler_install("\n> ", highlight);
    while (true) {
        rl_callback_read_char();
        rl_redisplay();
    }
    batch = true;
}
