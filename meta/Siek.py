# Essentials of Compilation
# An Incremental Approach in Python
# Jeremy G. Siek
# https://github.com/IUCompilerCourse/Essentials-of-Compilation/releases/tag/python-MIT-press

class AST:
    def __init__(self, value): self.value = value; self.nest = []
    def tag(self): return self.__class__.__name__.lower()
    def val(self): return f'{self.value}'
    def head(self,prefix=''):
        return f'{prefix}{self.tag()}:{self.val()}'
    def dump(self,depth=0):
        def tab(depth): return '\t'*depth
        ret = self.head(prefix=tab(depth))
        return ret
    def __repr__(self): return self.dump()

class Constant(AST): pass

eight = Constant(8)
eight

class UnaryOp(AST):
    def __init__(self, op, operand):
        self.op = op
        self.operand = operand

## unary subtraction
class USub(AST):
    def __init__(self): super().__init__('-')
