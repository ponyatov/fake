/// @file
/// @brief @ref vm core code

#include "fake.hpp"

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

const char *line = nullptr;
void highlight(char *l) {
    line = l;
    ("line: %s\n", line);
}

void repl() {
    if (trace) fprintf(stderr, NOPARAM "repl\n");
    rl_callback_handler_install("> ", highlight);
    while (true) {
        rl_callback_read_char();
        rl_redisplay();
        // line = readline("> ");
        // if (!line) break;  // Ctrl+C
        // fprintf(stderr, "\n[%s]\n", line);
    }
}
