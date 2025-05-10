    /// @file
    /// @brief `F` lexer
%{
    #include "fake.hpp"
    char *yyfile = nullptr;
%}

%option noyywrap yylineno

%%
#[^\n]*     {}              // line comment
[ \t\r\n]+  {}              // drop spaces
.           {yyerror("");}  // any undetected char
