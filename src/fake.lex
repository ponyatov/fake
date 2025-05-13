    /// @file
    /// @brief `F` lexer
%{
    #include "fake.hpp"
    #include "fake.yacc.hpp"
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

"nop"       { yylval.o = Op::nop;      return CMD; }
"halt"      { yylval.o = Op::halt;     return CMD; }

"repl"      { yylval.o = Op::repl;     return CMD; }
"?"         { yylval.o = Op::quest;    return CMD; }
"dump"      { yylval.o = Op::dump;     return CMD; }

"dup"       { yylval.o = Op::dup;      return CMD; }
"drop"      { yylval.o = Op::drop;     return CMD; }
"swap"      { yylval.o = Op::swap;     return CMD; }
"over"      { yylval.o = Op::over;     return CMD; }
"press"     { yylval.o = Op::press;    return CMD; }

"rrot"      { yylval.o = Op::rrot;     return CMD; }
"lrot"      { yylval.o = Op::lrot;     return CMD; }
"pick"      { yylval.o = Op::pick;     return CMD; }
"depth"     { yylval.o = Op::depth;    return CMD; }
"dot"       { yylval.o = Op::dot;      return CMD; }
"."         { yylval.o = Op::dot;      return CMD; }

"add"       { yylval.o = Op::add;      return CMD; }
"sub"       { yylval.o = Op::sub;      return CMD; }
"mul"       { yylval.o = Op::mul;      return CMD; }
"div"       { yylval.o = Op::div;      return CMD; }
"mod"       { yylval.o = Op::mod;      return CMD; }
"pow"       { yylval.o = Op::pow;      return CMD; }
"neg"       { yylval.o = Op::neg;      return CMD; }

{s}?"0x"[0-9a-fA-F]+ { yylval.n = hex(yytext); return INT; }
{s}?"0o"[0-9a-fA-F]+ { yylval.n = oct(yytext); return INT; }
{s}?"0b"[0-9a-fA-F]+ { yylval.n = bin(yytext); return INT; }
{s}?{n}+             { yylval.n = dec(yytext); return INT; }

{a}{an}*    { yylval.s =      yytext ; return ID ; }

.           {yyerror("");}  // any undetected char
