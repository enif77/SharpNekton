# Sharp Nekton

Sharp Nekton is a small embedable language for .NET.

A very old piece of code I just (2019-05-15) retrieved from my archives.
It is incomplete and without any documentation. I'll revive it over time...

The most interesting part here is the evaluator. It is a stack based virtual machine,
that uses as-a-class implemented opcodes, without using any kind of a bytecode. 
It makes the evaluator very simple and effective.

## Syntax

<pre>
#  - A comment.
{} - Repeat 0 or more.
() - Groups things together.
[] - An optional part.
|  - Or.
&  - And.
!  - Not.
:  - A definition name and its definition separator.
-  - No white spaces allowed.
"" - A specific string (keyword etc.).
'' - A specific character.
.. - A range.
.  - The end of a definition.

program : program-block .

program-block : { statement } .

statement ::
    assignment-statement |
    compound-statement |
    if-statement |
    while-statement |
    do-statement |
    for-statement |
    foreach-statement |
    break-statement |
    continue-statement |
    return-statement |
    end-statement |
    exit-statement |
    include-statement |
    import-statement |
    eval-statement |
    function-statement |
    local-variable-declaration |
    print-statement |
    empty-statement .

compound-statement : '{' { statement } '}' .

eval-statement : "eval" expression ';' .

import-statement : "import" expression ';' .

include-statement : "include" expression ';' .

print-statement : "print" [ expression ] ';' .

exit-statement : "exit" expression ';' .

end-statement : "end" ';' .

return-statement : "return" [ expression ] ';' .

continue-statement : "continue" ';' .

break-statement : "break" ';' .

foreach-statement :
    "foreach" '(' expression "as" expression [ ',' expression ] ')' statement .

for-statement : 
    "for" '(' [expr-init] ';' [expr-cond] ';' [expr-incr] ')' statement .

expr-init : expression .

expr-cond : expression .

expr-incr : expression .

do-statement : "do" statement "while" condition ';' .

condition :: '(' expression ')' .

while-statement : "while" condition statement .

if-statement : "if" condition true-statement [ "else" false-statement ] .

assignment-statement : expression ';' .

function-declaration : "function" [ identifier ] formal-parameter-list block .

formal-parameter-list : '(' [ formal-parameter { ',' formal-parameter } ] ')' .

formal-parameter : identifier .

block : '{' { statement } '}' | assignment-statement .

local-variable-declaration : "local" local-variable-declaration-list ';' .

local-variable-declaration-list :
    identifier [ '=' expression ] { ',' identifier [ '=' expression ] } .

expression : but-expression .

but-expression : assignment-expression [ but-op expression ] .

but-op : "but" .

assignment-expression : conditional-expression [ assignment-operator expression ] .

assignment-operator : '=' | '+=' | '-=' | '*=' | '/=' | '%=' | '**=' .

conditional-expression : ask-expression [ '?' expression ':' expression ] .

ask-expression : logical-or-expression [ ask-operator ask-expression ] .

ask-operator : '??' | '?!' .

logical-or-expression : 
    logical-for-expression { logical-or-operator logical-for-expression } .

logical-or-operator : '||' .

logical-for-expression : 
    logical-and-expression { logical-for-operator logical-and-expression } .

logical-for-operator : 'or' .

logical-and-expression : 
    logical-fand-expression { logical-and-operator logical-fand-expression } .

logical-and-operator : '&&' .

logical-fand-expression : 
    equality-expression { logical-fand-operator equality-expression } .

logical-fand-operator : 'and' .

equality-expression : 
    relational-expression { equality-operator relational-expression } .

equality-operator : '==' | '!=' | '===' | '!==' .

relational-expression :
    multiplicative-expression { relational-operator multiplicative-expression } .

relational-operator : '<' | '<=' | '>' | '>=' | "in" .

additive-expression : 
    multiplicative-expression { additional-operator multiplicative-expression } .

additional-operator : '+' | '-' | '..' .

multiplicative-expression :
    pow-expression { multiplicative-operator pow-expression } .

multiplicative-operator : '*' | '/' | '%' | "div" .

pow-expression : has-value-expression { pow-operator has-value-expression } .

pow-operator : '**' .

cast-expression :
    unary-expression |
    '(' type-name ')' cast-expression |
    '(' assignment-expression ')' .

unary-expr :
    postfix-expression |
    '++' unary-expression |
    '--' unary-expression |
    unary-operator cast-expression |
    data-expression |
    arg '(' expression ')' |
    typeof '(' unary-expression ')' |
    sizeof '(' unary-expression ')' |
    "function" [ identifier ] formal-parameters-list block [postfix-operator] .
    
unary-operator : '&' | '+' | '-' | '!' | '@' .

data-expression : '[' [ table-value-list ] ']' .
    
table-value-list : table-value { ',' table-value } .

table-value : [ key '=>' ] value .

key : expression .

value : expression .

postfix-expression : primary-expression [ postfix-operator ] .

postfix-operator :
    '[' expression ']' { postfix-operator } |
    '.' identifier { postfix-operator } |
    '(' parameters-list ')' { postfix-operator } |
    '++' |
    '--' .

primary-expr :
    "undefined" |
    "null" |
    "false" |
    "true" |
    numeric-constant |
    string-literal |
    identifier |
    '(' expression ')' |
    function-declaration-operator .

identifier : variable-identifier .

parameter-list : '(' [ parameter { ',' parameter } ] ')' .

parameter : expression .

</pre>

## License

### BasicBasic - (C) 2019 Premysl Fara 
 
SharpNekton is available under the **zlib license**:

This software is provided 'as-is', without any express or implied
warranty.  In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.
