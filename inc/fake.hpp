#pragma once
/// @file
/// @brief @ref vm headers

/// @defgroup libc libc
/// @brief standard headers
/// @{
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <stdint.h>
/// @}

/// @defgroup main main
/// @brief POSIX entry point
/// @{
extern int main(int argc, char *argv[]);
extern void arg(int argc, char *argv);
/// @}

/// @defgroup vm vm
/// @brief Virtual FORTH Machine
/// @details bytecode interpreter
/// @{

/// @defgroup config config
/// @{

/// main memory size, bytes
#define Msz 0x10000
/// return stack size, cells
#define Rsz 0x100
/// data stack size, ints
#define Dsz 0x10

/// @}

/// @defgroup memory memory
/// @{

typedef uint8_t byte;   ///< `u8`
typedef uint16_t addr;  ///< `u16` VM memory address
typedef int32_t cell;   ///< `i32` VM integers

extern byte M[Msz];  ///< main VM memory
extern addr Cp;      ///< compiler pointer
extern addr Ip;      ///< instruction pointer

/// @}

/// @defgroup command command
/// @{

/// @brief opcode
enum class Op {
    nop = 0x00,
    halt = 0xFF,
    repl = 0xEE,
};

extern void nop();   ///< `( -- )` do nothing
extern void halt();  ///< `( -- )` stop system

/// @}

/// @defgroup compiler compiler
/// @{
extern bool compile;  ///< compile/interpret state
extern bool trace;    ///< execution trace
/// @}

/// @defgroup interpreter interpreter
/// @{
extern void excmd(Op cmd);  ///< run single command
extern void repl();         ///< `( -- )` run CLI interface
/// @}

/// @}

/// @defgroup skelex skelex
/// @brief `lex`/`yacc` lexical skeleton
/// @{
extern int yylex();
extern int yylineno;
extern char *yyfile;
extern char *yytext;
extern FILE *yyin;
extern int yyparse();
extern void yyerror(const char *msg);
#include "fake.yacc.hpp"
#include <readline/readline.h>
#include <readline/history.h>
/// @}
