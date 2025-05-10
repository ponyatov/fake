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
ex    : INT         { fprintf(stderr,"int:%i\t",$1); }
      | ID          { fprintf(stderr, "id:%s\t",$1); }
      | CMD         { fprintf(stderr,"cmd:%.2X\t",$1); }
