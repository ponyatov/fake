/// @file
/// @brief @ref vm core code

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

void yyerror(const char *msg) {
    fprintf(stderr, "\n\n%s:%i %s [%s]\n\n", yyfile, yylineno, msg, yytext);
    exit(-1);
}

byte M[Msz];
addr Cp = 0;
addr Ip = 0;

cell D[Dsz];
byte Dp = 0;

void dot() {
    if (trace) fprintf(stderr, "dot\n");
    fprintf(stderr, "\n[ ");
    for (int i = 0; i < Dp; i++) fprintf(stderr, "%i ", D[i]);
    fprintf(stderr, "]\n");
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
    cell n2 = pop(), n1 = pop();
    push(n2);
    push(n1);
}

void over() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "over\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    push(Dp - 2);
}

void press() {
    assert(Dp >= 2);
    if (trace) fprintf(stderr, "press\t%i %i\n", D[Dp - 2], D[Dp - 1]);
    D[Dp-2] = D[Dp-1]; Dp--;
}

void rrot() {
    if (trace)
        fprintf(stderr, "rrot\t%i %i %i\n", D[Dp - 3], D[Dp - 2], D[Dp - 1]);
}

void lrot() {
    if (trace)
        fprintf(stderr, "lrot\t%i %i %i\n", D[Dp - 3], D[Dp - 2], D[Dp - 1]);
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

bool compile = false;

bool trace = true;

void excmd(Op op) {
    if (trace) fprintf(stderr, "\n%.4X: %.2X ", Ip, op);
    switch (op) {
        case Op::nop:
            nop();
            break;
        case Op::halt:
            halt();
            break;
        case Op::repl:
            repl();
            break;
        case Op::dot:
            dot();
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
        default:
            abort();
    }
}

#define NOPARAM "     "

void nop() {
    if (trace) fprintf(stderr, NOPARAM "nop");
}

void halt() {
    if (trace) fprintf(stderr, NOPARAM "halt\n");
    exit(0);
}

void highlight(char *line) {
    if (!line) {  // on Ctrl+C/D
        trace = false;
        halt();
    }

    yyfile = (char *)"repl";
    if (trace) fprintf(stderr, "input: %s\n", line);
    yy_scan_string(line);
    yyparse();
    yyfile = nullptr;
    dot();
}

void repl() {
    if (trace) fprintf(stderr, NOPARAM "repl\n");
    rl_callback_handler_install("\n> ", highlight);
    while (true) {
        rl_callback_read_char();
        rl_redisplay();
    }
}
