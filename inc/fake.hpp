#pragma once
/// @file
/// @brief @ref VM headers

/// @defgroup libc libc
/// @brief standard headers
/// @{
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <stdint.h>

#include <map>
#include <string>
/// @}

/// @defgroup main main
/// @brief POSIX entry point
/// @{
extern int main(int argc, char *argv[]);
extern void arg(int argc, char *argv);
/// @}

/// @defgroup VM VM
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

/// @name types
typedef uint8_t byte;   ///< `u8`
typedef uint16_t addr;  ///< `u16` @ref VM memory address
typedef int32_t cell;   ///< `i32` integer

/// @name main memory
/// @{
extern byte M[Msz];  ///< main @ref VM memory
extern addr Cp;      ///< compiler pointer
extern addr Ip;      ///< instruction pointer

/// memory header
struct HEADER {
    byte _jmp;   ///< first (reset) entry jump
    cell entry;  ///< bytecode entry point
    cell lfa;    ///< link field area: last defined word in vocabulary
    cell free;   ///< free memory blocks list (last free block)
    cell used;   ///< used memory blocks list (last used block)
} __attribute__((packed));

extern HEADER *header;

extern void init();  ///< initialize @ref VM (fill @ref header)
/// @}

/// @name data stack
/// @{
extern cell D[Dsz];  ///< data stack
extern byte Dp;      ///< @ref D pointer
/// @}

/// @}

/// @defgroup command command
/// @{

/// @brief opcode
enum class Op {
    // control flow
    nop = 0x00,
    halt = 0xFF,
    jmp = 0x01,
    qjmp = 0x02,
    call = 0x03,
    ret = 0x04,
    lit = 0x05,
    // debug
    quest = 0xD0,
    dump = 0xD1,
    repl = 0xEE,
    // stack ops
    dup = 0x10,
    drop = 0x11,
    swap = 0x12,
    over = 0x13,
    press = 0x14,
    rrot = 0x15,
    lrot = 0x16,
    pick = 0x17,
    depth = 0x18,
    dot = 0x19,
    // math
    add = 0x20,
    sub = 0x21,
    mul = 0x22,
    div = 0x23,
    mod = 0x24,
    pow = 0x25,
    neg = 0x26,
};

/// @defgroup flow flow control
/// @{
extern void nop();   ///< `0x00 ( -- )` do nothing
extern void halt();  ///< `0xFF ( -- )` stop system
/// @}

/// @defgroup stack stack
/// @{
extern void push(cell n);  ///< `( -- n)` push cell
extern cell top();         ///< `( n -- n )` copy top element
extern cell pop();         ///< `( n -- )` get top element
extern void dup();         ///< `0x10 ( n -- n n )`
extern void drop();        ///< `0x11 ( n1 n2 -- n1 )`
extern void swap();        ///< `0x12 ( n1 n2 -- n2 n1 )`
extern void over();        ///< `0x13 ( n1 n2 -- n1 n2 n1 )`
extern void press();       ///< `0x14 ( n1 n2 -- n2 )`
extern void rrot();        ///< `0x15 ( n1 n2 n3 -- n2 n3 n1 )`
extern void lrot();        ///< `0x16 ( n1 n2 n3 -- n3 n1 n2 )`
extern void pick();        ///< `0x17 ( ... idx -- ... ni )`
extern void depth();       ///< `0x18 ( ... -- ... Dp )`
extern void dot();         ///< `0x19 ( ... -- )` clean @ref D
/// @}

/// @defgroup math math
/// @{
extern void add();  ///< `0x20 ( n1 n2 -- n1+n2 ) +`
extern void sub();  ///< `0x21 ( n1 n2 -- n1-n2 ) -`
extern void mul();  ///< `0x22 ( n1 n2 -- n1*n2 ) *`
extern void div();  ///< `0x23 ( n1 n2 -- n1/n2 ) /`
extern void mod();  ///< `0x24 ( n1 n2 -- n1%n2 ) %`
extern void pow();  ///< `0x25 ( n1 n2 -- n1^n2 ) ^`
extern void neg();  ///< `0x26 ( n -- -n )`
/// @}

/// @defgroup debug debug
/// @{
extern void quest();  ///< `0xD0 ? ( -- )` print @ref D
extern void dump();   ///< `0xD1 dump ( -- )` print @ref D
/// @}

/// @}

/// @defgroup compiler compiler
/// @{
extern bool compile;                   ///< compile/interpret state
extern bool trace;                     ///< execution trace
extern std::map<std::string, addr> W;  ///< vocabulary: symbol table
/// @}

/// @defgroup interpreter interpreter
/// @{
extern void cmd(Op cmd);  ///< run single command
extern void repl();       ///< 0xEE `( -- )` run CLI interface
/// @}

/// @defgroup error error
/// @details error processing & @ref recovery
/// @{
extern bool batch;       ///< batch/repl mode
extern void recovery();  ///< error recovery in repl mode
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

/// @name string to int conversion
/// @{
extern int hex(char *);
extern int oct(char *);
extern int bin(char *);
extern int dec(char *);
/// @}

// #include "fake.lex.hpp"
// #include "fake.yacc.hpp"

/// @}
