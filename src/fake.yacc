    /// @file
    /// @brief `F` syntax parser
%{
    #include "fake.hpp"
%}

%defines %union { char c; int n; float f; char *s; Op o; }

%token<n> INT
%token<s> ID
%token<o> CMD

%%
REPL: | REPL ex
ex    : INT         { if (compile) abort();
                      else         push($1); }
      | ID          { if (trace) fprintf(stderr, "\tid:%s",$1); }
      | CMD         { if (compile) M[Cp++] = (byte)$1;
                      else         excmd($1);        }
