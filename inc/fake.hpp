#pragma once

#ifdef __cplusplus
extern "C" {
#else
extern
#endif
void initialise_monitor_handles(void);

#include <stdio.h>
#include <unistd.h>
#include <string.h>

extern void setup(int argc, char *argv[]);
extern void arg(int argc, char *argv);
extern void loop(void);

#ifdef __cplusplus
}  // extern "C"
#endif
