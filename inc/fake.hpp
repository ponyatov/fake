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

extern cell D[Dsz];  ///< data stack
extern byte Dp;      ///< @ref D pointer

/// @}

/// @defgroup command command
/// @{

/// @brief opcode
enum class Op {
    nop = 0x00,
    dup = 0x10,
    drop = 0x11,
    swap = 0x12,
    over = 0x13,
    press = 0x14,
    rrot = 0x15,
    lrot = 0x16,
    pick = 0x17,
    depth = 0x18,
    dot = 0xD0,
    repl = 0xEE,
    halt = 0xFF,
};

/// @defgroup flow flow control
/// @{
extern void nop();   ///< `( -- )` do nothing
extern void halt();  ///< `( -- )` stop system
/// @}

/// @defgroup stack stack
/// @{
extern void push(cell n);  ///< `( -- n)` push cell
extern cell top();         ///< `( n -- n )` copy top element
extern cell pop();         ///< `( n -- )` get top element
extern void dup();         ///< `( n -- n n )`
extern void drop();        ///< `( n1 n2 -- n1 )`
extern void swap();        ///< `( n1 n2 -- n2 n1 )`
extern void over();        ///< `( n1 n2 -- n1 n2 n1 )`
extern void press();       ///< `( n1 n2 -- n2 )`
extern void rrot();        ///< `( n1 n2 n3 -- n2 n3 n1 )`
extern void lrot();        ///< `( n1 n2 n3 -- n3 n1 n2 )`
extern void pick();        ///< `( ... idx -- ... ni )`
extern void depth();       ///< `( ... -- ... Dp )`
/// @}

/// @defgroup debug debug
/// @{
extern void dot();  ///< `( -- )` print @ref D
/// @}

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
#include <readline/readline.h>
#include <readline/history.h>
extern int yylex();                    ///< lexer
extern int yylineno;                   ///< code line number
extern char *yyfile;                   ///< file name
extern char *yytext;                   ///< lexeme value
extern FILE *yyin;                     ///< current file
extern int yyparse();                  ///< parser
extern void yyerror(const char *msg);  ///< syntax error callback
// #include "fake.lex.hpp"
// #include "fake.yacc.hpp"
/// @}
