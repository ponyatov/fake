#pragma once

/// @defgroup libc libc
/// @{
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
/// @}

/// @defgroup main main
/// @{
extern int main(int argc, char *argv[]);
extern void arg(int argc, char *argv);
/// @}

/// @defgroup skelex skelex
/// @{
extern int yylex();
extern int yylineno;
extern char *yyfile;
extern FILE *yyin;
extern int yyparse();
#include "fake.yacc.hpp"
/// @}
