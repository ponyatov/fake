    /// @file
    /// @brief `F` syntax parser
%{
    #include "fake.hpp"
%}

%defines %union { char c; int n; float f; char *s; }

%token<n> INT
%token<s> ID

%%
REPL: | REPL ex
ex    : INT
      | ID
