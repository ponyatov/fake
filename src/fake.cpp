#include "fake.hpp"

void setup(int argc, char *argv[]) {
    initialise_monitor_handles();
    arg(0, argv[0]);
    for (int i = 1; i < argc; i++) {  //
        arg(i, argv[i]);
        yyfile = argv[i];
        assert(yyin = open(yyfile, O_RDONLY, 0));
        close(yyin);
        yyfile = nullptr;
    }
}

void arg(int argc, char *argv) {
    write(0, argv, strlen(argv));
    write(0, "\n", 1);
}

void loop(void) {  //
    arg(0, (char *)"hello world!");
}

int yyin;
char *yyfile = nullptr;
