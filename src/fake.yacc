%{
    #include "fake.hpp"
    char *yyfile = nullptr;
%}

%defines %union { char c; int n; float f; char *s; }

%token<n> INT
%token<s> ID

%%
REPL: | REPL ex
ex  : INT
    | ID
