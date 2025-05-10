    /// @file
    /// @brief `F` lexer
%{
    #include "fake.hpp"
    char *yyfile = nullptr;
%}

%option noyywrap yylineno

                    // number sign
s  [+/-]
                    // decimal 
n  [0-9]
                    // alpha
a  [a-zA-Z_]
                    // alphanumeric
an [a-zA-Z_0-9]

%%
#[^\n]*     {}              // line comment
[ \t\r\n]+  {}              // drop spaces

"nop"       { yylval.o = Op::nop;  return CMD; }
"halt"      { yylval.o = Op::halt; return CMD; }
"repl"      { yylval.o = Op::repl; return CMD; }

{s}?{n}+    { yylval.n = atoi(yytext); return INT; }
{a}{an}*    { yylval.s = yytext; return ID; }

.           {yyerror("");}  // any undetected char
