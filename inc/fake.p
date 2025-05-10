#pragma once

#ifdef __cplusplus
extern "C" {
#else
extern
#endif
void initialise_monitor_handles(void);

#include <stdio.h>
#include <fcntl.h>
#include <assert.h>
#include <unistd.h>
#include <string.h>

extern void setup(int argc, char *argv[]);
extern void arg(int argc, char *argv);
extern void loop(void);

extern int yyin;    ///< parser input file
extern char *yyfile;  ///< current file name

#ifdef __cplusplus
}  // extern "C"
#endif
