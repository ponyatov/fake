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
extern char *yytext;
extern FILE *yyin;
extern int yyparse();
extern void yyerror(const char *msg);
#include "fake.yacc.hpp"
/// @}
