# Essentials of Compilation
# An Incremental Approach in Python
# Jeremy G. Siek
# https://github.com/IUCompilerCourse/Essentials-of-Compilation/releases/tag/python-MIT-press

class AST:
    def __init__(self, value): self.value = value
    def tag(self): return self.__class__.__name__.lower()
    def val(self): return f'{self.value}'
    def __repr__(self): return f'{self.tag()}:{self.val()}'

class Constant(AST): pass

eight = Constant(8)
eight

