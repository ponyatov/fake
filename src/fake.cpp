#include "fake.hpp"

void setup(int argc, char *argv[]) {
    initialise_monitor_handles();
    for (int i = 0; i < argc; i++) {
        write(0, argv[i], strlen(argv[i]));
        write(0, "\n", 1);
    }
}

void loop(void) {  //
    printf("hello world!\n");
}
