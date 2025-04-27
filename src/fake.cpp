#include "fake.hpp"

void setup(int argc, char *argv[]) {
    initialise_monitor_handles();
    arg(0, argv[0]);
    for (int i = 1; i < argc; i++) {  //
        arg(i, argv[i]);
    }
}

void arg(int argc, char *argv) {
    write(0, argv, strlen(argv));
    write(0, "\n", 1);
}

void loop(void) {  //
    printf("hello world!\n");
}
