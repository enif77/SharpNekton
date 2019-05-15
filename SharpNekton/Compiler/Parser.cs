/* SharpNekton - (C) 2019 Premysl Fara 
 
SharpNekton is available under the zlib license:

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
 
 */

/*

new-operatorn :: '@' "new" type-specifier [ ':' parameter-list ] ';' .
type-specifier :: expression
parameter-list :: expression { ',' expression }

Ex.: obj = @new "System.Object";
Ex.: obj = @new "System.String" : "A new String object...";

object-call-operator :: 
  '@' '[' object-specifier message [ ':' parameter-list ] ']'
object-specifier :: expression
message :: expression

Ex.: @[obj "ToString"];
Ex.: @[obj "Sin" : alpha];


Reflection.NewObject( objectType, ... )
  Reflection.NewObject( "object-type", param1, param2, .... )
 
object = Reflection.Invoke( object, message, ... )
  s = (string) Reflection.Invoke(obj, "ToString")
 
 */

using System;
using System.Collections;
using SharpNekton.Compiler.Sources;
using SharpNekton.Evaluator;
using SharpNekton.Evaluator.Values;
using SharpNekton.Evaluator.OpCodes;
using SharpNekton.Shared;

namespace SharpNekton.Compiler
{
    public enum SourceLevelID
    {
        GLOBAL,   // global source level
        LOCAL,    // in functions, ...
    }


    public enum ObjectLevelID
    {
        GLOBAL,
        FORMAL_PAREMETER,
        LOCAL_VARIABLE,
    }


    public class ParserState
    {
        private Tokenizer tokenizer;
        private LocalsStack locals;
        private SourceLevelID sourceLevel;  // Is GLOBAL outside of functions, LOCAL inside

        /*------------------------------------------------------------------------*/

        private void Init(ASource source)
        {
            tokenizer = new Tokenizer(source);
            locals = new LocalsStack();
            sourceLevel = SourceLevelID.GLOBAL;
        }

        /*------------------------------------------------------------------------*/

        public ParserState()
        {
            Init(new StringSource(""));
        }

        /*------------------------------------------------------------------------*/

        public ParserState(ASource source)
        {
            Init(source);
        }

        /*------------------------------------------------------------------------*/

        public ASource Source
        {
            get
            {
                return tokenizer.Source;
            }

            set
            {
                tokenizer.Source = value;
            }
        }

        /*------------------------------------------------------------------------*/

        public void Compile(ScriptState st)
        {
            Program(st);
        }

        /*------------------------------------------------------------------------*/


        /*--------------------------------------------------------------*/
        /**
         * \brief Tests, if the current token is equal to the expected token.
         *
         * Tests, if the current token is equal to the expected token (exptok).
         * If these two are identical, a next token is read. If they are not
         * identical, an error is raised.
         *
        * */

        private void Match(TokenID expTokID)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            if (tokenizer.GetLastToken().TokID == expTokID)
            {
                tokenizer.NextToken();
            }
            else
            {
                SharpNektonErrorID errorID;

                switch (expTokID)
                {
                    case TokenID.T_COLON: errorID = SharpNektonErrorID.E_EXPTERMOPSEP; break;         // ':'
                    case TokenID.T_COMMA: errorID = SharpNektonErrorID.E_EXPCOMMA; break;             // ','
                    case TokenID.T_STAT_END: errorID = SharpNektonErrorID.E_EXPSTATEND; break;        // ';'
                    case TokenID.T_LPAREN: errorID = SharpNektonErrorID.E_EXPLPAREN; break;           // '('
                    case TokenID.T_RPAREN: errorID = SharpNektonErrorID.E_EXPRPAREN; break;           // ')'
                    case TokenID.T_BLOCK_START: errorID = SharpNektonErrorID.E_EXPBLOCKSTART; break;  // '{'
                    case TokenID.T_BLOCK_END: errorID = SharpNektonErrorID.E_EXPBLOCKEND; break;      // '}'
                    case TokenID.T_RBRAC: errorID = SharpNektonErrorID.E_EXPRBRAC; break;             // ']'
                    case TokenID.T_ASSIGN_OP: errorID = SharpNektonErrorID.E_EXPASSIGNOP; break;      // '='
                    case TokenID.T_DATAASGN_OP: errorID = SharpNektonErrorID.E_EXPDATAASGNOP; break;  // '=>'
                    case TokenID.T_KEY_WHILE: errorID = SharpNektonErrorID.E_EXPKEYWHILE; break;      // "while"
                    case TokenID.T_KEY_AS: errorID = SharpNektonErrorID.E_EXPKEYAS; break;            // "as"
                    case TokenID.T_EOF: errorID = SharpNektonErrorID.E_EXPEOF; break;                 // EOF

                    default:
                        errorID = SharpNektonErrorID.UNSPECIFIED_ERROR;
                        break;
                }

                throw new SharpNektonException(new SharpNektonError(errorID, line, linePosition));
            }
        }

        /*--------------------------------------------------------------*/
        // parse the parameter
        // parameter :: expression

        private void Parameter(ScriptState st, OpCodeList codePart)
        {
            // get value
            Expression(st, codePart);

            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // push it as subroutine parameter
            codePart.Append(new PushPOpCode(line, linePosition));
        }

        /*--------------------------------------------------------------*/
        // parse the parameter-list
        // parameter-list :: '(' [ parameter { ',' parameter } ] ')'

        private int ParameterList(ScriptState st, OpCodeList codePart, int numfparams)
        {
            int paramsCount = 0;

            // check for '('
            Match(TokenID.T_LPAREN);

            if (tokenizer.GetLastToken().TokID != TokenID.T_RPAREN)
            {
                Parameter(st, codePart);
                paramsCount++;

                while (tokenizer.GetLastToken().TokID == TokenID.T_COMMA)
                {
                    tokenizer.NextToken();            // eat ','
                    Parameter(st, codePart);
                    paramsCount++;
                }
            }

            // get the source position afther the parameters
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // _argc
            // _argc = number of realy passed params
            codePart.Append(new LoadNOpCode(line, linePosition, (double)paramsCount));
            codePart.Append(new PushPOpCode(line, linePosition));
            paramsCount++;

            // check number of parameters; -1 means check-at-runtime only
            if (numfparams != -1 && paramsCount != numfparams)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADPCOUNT));
            }

            // check for ')'
            Match(TokenID.T_RPAREN);

            return paramsCount;
        }

        /*--------------------------------------------------------------*/
        // handles an identifier
        // identifier :: variable-identifier

        private void Identifier(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> identifier");

            // get the source position of the identifier
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            string objectName = tokenizer.GetLastToken().SVal;

            if (sourceLevel == SourceLevelID.LOCAL)
            {
                // try to find a local object first
                LocalObject localObject = locals.FindLocalObject(objectName);
                if (localObject == null)
                {
                    // object not found at the local level, try it at the global level
                    ValueStore newObject = st.Evaluator.FindGlobalObject(objectName);
                    if (newObject == null)
                    {
                        // object does not exist yet, create it at the local level
                        localObject = locals.AddLocalObject(objectName, ObjectLevelID.LOCAL_VARIABLE, locals.LocalOffset);
                        locals.LocalOffset++;

                        // append load-local-object-ref opcode
                        codePart.Append(new LoadFPOpCode(line, linePosition, localObject.Offset));
                    }
                    else
                    {
                        // object exists at the global level - use it
                        codePart.Append(new LoadStorOpCode(line, linePosition, newObject));
                    }
                }
                else
                {
                    // a local symbol found
                    codePart.Append(new LoadFPOpCode(line, linePosition, localObject.Offset));
                }
            }
            else
            {
                // we are at the global level - work with globals only
                ValueStore newObject = st.Evaluator.FindGlobalObject(objectName);
                if (newObject == null)
                {
                    newObject = st.Evaluator.AddGlobalObject(objectName);
                }

                codePart.Append(new LoadStorOpCode(line, linePosition, newObject));
            }

            tokenizer.NextToken();  // eat the ident
        }

        /*--------------------------------------------------------------*/
        // primary-expr ::
        //   "undefined" |
        //   "null" |
        //   "false" |
        //   "true" |
        //   numeric-constant |
        //   string-literal |
        //   identifier |
        //   '(' expression ')' |
        //   function-declaration-operator

        private bool PrimaryExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> primary-expression");

            // get the source position of the primary expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            bool indexable = false;

            switch (tokenizer.GetLastToken().TokID)
            {
                case TokenID.T_KEY_UNDEFINED:
                    codePart.Append(new LoadUndefinedOpCode(line, linePosition));
                    tokenizer.NextToken();   // eat "undefined"
                    indexable = false;
                    break;

                case TokenID.T_KEY_NULL:
                    codePart.Append(new LoadNullOpCode(line, linePosition));
                    tokenizer.NextToken();   // eat "null"
                    indexable = false;
                    break;

                case TokenID.T_KEY_FALSE:
                    codePart.Append(new LoadBOpCode(line, linePosition, false));
                    tokenizer.NextToken();   // eat "false"
                    indexable = false;
                    break;

                case TokenID.T_KEY_TRUE:
                    codePart.Append(new LoadBOpCode(line, linePosition, true));
                    tokenizer.NextToken();   // eat "true"
                    indexable = false;
                    break;

                case TokenID.T_NUMBER:
                    codePart.Append(new LoadNOpCode(line, linePosition, tokenizer.GetLastToken().DVal));
                    tokenizer.NextToken();  // eat <numeric-constant>
                    indexable = false;
                    break;

                case TokenID.T_STRING_LIT:
                    codePart.Append(new LoadSOpCode(line, linePosition, tokenizer.GetLastToken().SVal));
                    tokenizer.NextToken();  // eat <string-literal>
                    indexable = false;
                    break;

                // variable, function call, ...
                case TokenID.T_IDENTIFIER:
                    Identifier(st, codePart);
                    indexable = true;
                    break;

                case TokenID.T_LPAREN:
                    tokenizer.NextToken();   // eat '('
                    Expression(st, codePart);
                    Match(TokenID.T_RPAREN);  // match ')'
                    indexable = true;
                    break;

                // f = function(a, b) { return a + b; }
                // f = function(a, b) expression ;
                case TokenID.T_KEY_FUNCTION:
                    Function_Declaration(st, codePart);
                    break;

                default:
                    Console.WriteLine(">> tok = {0}", tokenizer.GetLastToken().TokID);
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPVAL));
            }

            return indexable;
        }

        /*--------------------------------------------------------------*/
        // postfix-operator ::
        //   '[' expression ']' { postfix-operator } |
        //   '.' identifier { postfix-operator } |
        //   '(' parameters-list ')' { postfix-operator } |
        //   '++' |
        //   '--'

        private void PostfixOperator(ScriptState st, OpCodeList codePart, bool indexable)
        {
            //Console.WriteLine(">> postfix-operator");

            // get the source position of the postfix operator
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            switch (tokenizer.GetLastToken().TokID)
            {
                case TokenID.T_INCR_OP:        // '++'
                    tokenizer.NextToken();
                    codePart.Append(new PostIncrOpCode(line, linePosition));
                    break;

                case TokenID.T_DECR_OP:       // '--'
                    tokenizer.NextToken();
                    codePart.Append(new PostDecrOpCode(line, linePosition));
                    break;

                case TokenID.T_DOT_OP:        // '.'
                    if (indexable)
                    {
                        // push array reference
                        codePart.Append(new PushOpCode(line, linePosition));

                        tokenizer.NextToken();   // eat '.'

                        // get the position of the new token (should be an identifier)
                        line = tokenizer.GetLastToken().Line;
                        linePosition = tokenizer.GetLastToken().LinePosition;

                        if (tokenizer.GetLastToken().TokID == TokenID.T_IDENTIFIER)
                        {
                            codePart.Append(new LoadSOpCode(line, linePosition, tokenizer.GetLastToken().SVal));
                            codePart.Append(new LoadIOpCode(line, linePosition));
                            tokenizer.NextToken();                // eat the identifier
                            PostfixOperator(st, codePart, true);  // v.x.y or v.x() or v.x++ ...
                        }
                        else
                        {
                            throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPIDENT));
                        }
                    }
                    else
                    {
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_DONTINDEX));
                    }
                    break;

                case TokenID.T_LBRAC:           // string/array index
                    if (indexable)
                    {
                        // push array/string reference
                        codePart.Append(new PushOpCode(line, linePosition));

                        tokenizer.NextToken();   // eat '['

                        Expression(st, codePart);

                        // get the position of the first token behind the expression (should be the ']')
                        line = tokenizer.GetLastToken().Line;
                        linePosition = tokenizer.GetLastToken().LinePosition;

                        Match(TokenID.T_RBRAC);  // match ']'

                        codePart.Append(new LoadIOpCode(line, linePosition));
                    }
                    else
                    {
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_DONTINDEX));
                    }

                    PostfixOperator(st, codePart, true);  // v[i][j] or v[i]() or v[i]++ ...
                    break;


                case TokenID.T_LPAREN:
                    {  // params
                        int paramsCount;

                        // store jsr address to stack
                        codePart.Append(new PushPOpCode(line, linePosition));

                        // proccess parameters first
                        paramsCount = ParameterList(st, codePart, -1);

                        // get the position of the first token behind the closing ')'
                        line = tokenizer.GetLastToken().Line;
                        linePosition = tokenizer.GetLastToken().LinePosition;

                        // jump to subroutine
                        codePart.Append(new JSROpCode(line, linePosition));

                        // remove parameters and/or the function ref  from the stack
                        if (paramsCount > 0)
                        {
                            codePart.Append(new PopNOpCode(line, linePosition, paramsCount + 1));
                        }
                        else
                        {
                            codePart.Append(new PopNOpCode(line, linePosition, 1));
                        }
                    }

                    PostfixOperator(st, codePart, true);  // f()[i]
                    break;
            }
        }

        /*--------------------------------------------------------------*/
        // postfix-expression :: primary-expression [ postfix-operator ]

        private void PostfixExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> postfix-expression");

            bool indexable = false;

            indexable = PrimaryExpression(st, codePart);
            PostfixOperator(st, codePart, indexable);
        }

        /*--------------------------------------------------------------*/
        // table-value :: [ key '=>' ] value
        // key :: expression
        // value :: expression

        private void TableValue(ScriptState st, OpCodeList codePart)
        {
            // key (or data)
            Expression(st, codePart);

            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            if (tokenizer.GetLastToken().TokID == TokenID.T_DATAASGN_OP)
            {
                // push the key in R to stack
                codePart.Append(new PushOpCode(line, linePosition));

                // match '=>'
                Match(TokenID.T_DATAASGN_OP);

                // a value
                Expression(st, codePart);

                codePart.Append(new AppendTableDataOpCode(line, linePosition));
            }
            else
            {
                //printf(">>  AK append \n");
                codePart.Append(new AppendTableDataAutoKeyOpCode(line, linePosition));
            }
        }

        /*--------------------------------------------------------------*/
        // table-value-list :: table-value { ',' table-value }

        private void TableValueList(ScriptState st, OpCodeList codePart)
        {
            // get the first value
            TableValue(st, codePart);

            while (tokenizer.GetLastToken().TokID == TokenID.T_COMMA)
            {
                tokenizer.NextToken();       // eat ','

                // allow ',' at the end of the decl. list
                if (tokenizer.GetLastToken().TokID == TokenID.T_RBRAC) break;

                // get a value
                TableValue(st, codePart);
            }
        }

        /*--------------------------------------------------------------*/
        // data-expression ::
        //   '[' [ value-list ] ']'
        //
        // value-list ::
        //  table-value { ',' array-value }
        //
        // table-value ::
        //   [ key '=>' ] value
        //
        // key :: expression
        // value :: expression

        private void DataExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> data-expression");

            // get the position of the [
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // '['
            Match(TokenID.T_LBRAC);
            // []
            if (tokenizer.GetLastToken().TokID == TokenID.T_RBRAC)
            {
                tokenizer.NextToken();  // eat ']'
                                        // new empty table
                codePart.Append(new NewTableOpCode(line, linePosition));
            }
            else
            {
                codePart.Append(new BeginTableDataOpCode(line, linePosition));
                TableValueList(st, codePart);
                codePart.Append(new EndTableDataOpCode(line, linePosition));
                Match(TokenID.T_RBRAC);  // ']' expected
            }
        }

        /*--------------------------------------------------------------*/
        // unary-expr ::
        //   postfix-expression |
        //   '++' unary-expression |
        //   '--' unary-expression |
        //   unary-operator cast-expression |
        //   data-expression |
        //   arg() |
        //   typeof(unary-expression)
        //   sizeof(unary-expression)
        //   "function" [ identifier ] formal-parameters-list block [postfix-operator]
        //
        // unary-op :: '&' | '+' | '-' | '!' | '@'

        private void UnaryExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> unary-expression");

            // get the position of the unary-expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            switch (tokenizer.GetLastToken().TokID)
            {
                case TokenID.T_INCR_OP:    // '++'
                    tokenizer.NextToken();    // eat '++'
                    UnaryExpression(st, codePart);
                    codePart.Append(new PreIncrOpCode(line, linePosition));
                    break;

                case TokenID.T_DECR_OP:    // '--'
                    tokenizer.NextToken();    // eat '--'
                    UnaryExpression(st, codePart);
                    codePart.Append(new PreDecrOpCode(line, linePosition));
                    break;

                // unary-operators
                case TokenID.T_ADD_OP:    // '+'
                    tokenizer.NextToken();   // eat '+'
                    CastExpression(st, codePart);
                    break;

                case TokenID.T_SUB_OP:    // '-'
                    tokenizer.NextToken();   // eat '-'
                    CastExpression(st, codePart);
                    codePart.Append(new NegOpCode(line, linePosition));
                    break;

                case TokenID.T_NOT_OP:    // '!'
                    tokenizer.NextToken();   // eat '!'
                    CastExpression(st, codePart);
                    codePart.Append(new NotOpCode(line, linePosition));
                    break;

                case TokenID.T_COPYDATA_OP:    // '@'
                    tokenizer.NextToken();   // eat '@'
                    CastExpression(st, codePart);
                    codePart.Append(new CopyDataOpCode(line, linePosition));
                    break;

                case TokenID.T_LBRAC:
                    DataExpression(st, codePart);
                    break;

                case TokenID.T_KEY_ARG:
                    tokenizer.NextToken();   // eat "arg"
                    Match(TokenID.T_LPAREN);  // match '('
                    Expression(st, codePart);
                    Match(TokenID.T_RPAREN);  // match ')'
                    codePart.Append(new GetArgOpCode(line, linePosition));
                    break;

                case TokenID.T_KEY_TYPEOF:
                    tokenizer.NextToken();   // eat "typeof"
                    Match(TokenID.T_LPAREN);  // match '('
                    UnaryExpression(st, codePart);
                    Match(TokenID.T_RPAREN);  // match ')'
                    codePart.Append(new TypeOfOpCode(line, linePosition));
                    break;

                case TokenID.T_KEY_SIZEOF:
                    tokenizer.NextToken();   // eat "sizeof"
                    Match(TokenID.T_LPAREN);  // match '('
                    UnaryExpression(st, codePart);
                    Match(TokenID.T_RPAREN);  // match ')'
                    codePart.Append(new SizeOfOpCode(line, linePosition));
                    break;

                default:    // postfix-expr
                    PostfixExpression(st, codePart);
                    break;
            }
        }

        /*--------------------------------------------------------------*/
        // cast-expression ::
        //   unary-expression |
        //   '(' type-name ')' cast-expression |
        //   '(' assignment-expression ')'

        private void CastExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> cast-expression");

            if (tokenizer.GetLastToken().TokID == TokenID.T_LPAREN)
            {
                tokenizer.NextToken();   // eat '('

                // get the position of the inner token
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                switch (tokenizer.GetLastToken().TokID)
                {
                    case TokenID.T_KEY_BOOLEAN:
                        tokenizer.NextToken();   // eat "boolean"
                        Match(TokenID.T_RPAREN);  // match ')'
                        CastExpression(st, codePart);
                        codePart.Append(new ToBooleanOpCode(line, linePosition));
                        break;

                    case TokenID.T_KEY_NUMBER:
                        tokenizer.NextToken();   // eat "number"
                        Match(TokenID.T_RPAREN);  // match ')'
                        CastExpression(st, codePart);
                        codePart.Append(new ToNumberOpCode(line, linePosition));
                        break;

                    case TokenID.T_KEY_STRING:
                        tokenizer.NextToken();   // eat "string"
                        Match(TokenID.T_RPAREN);  // match ')'
                        CastExpression(st, codePart);
                        codePart.Append(new ToStringOpCode(line, linePosition));
                        break;

                    case TokenID.T_KEY_OBJECT:
                        tokenizer.NextToken();   // eat "object"
                        Match(TokenID.T_RPAREN);  // match ')'
                        CastExpression(st, codePart);
                        codePart.Append(new ToObjectOpCode(line, linePosition));
                        break;

                    default:
                        Expression(st, codePart);
                        Match(TokenID.T_RPAREN);  // match ')'
                                                  // TODO: (a)["index"] never compiles the [... part
                        break;
                }
            }
            else
            {
                UnaryExpression(st, codePart);
            }
        }

        /*--------------------------------------------------------------*/
        // pow-expression :: has-value-expression { pow-operator has-value-expression }
        // pow-operator :: '**'

        private void PowExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> pow-expression");
            CastExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            while (tokenizer.GetLastToken().TokID == TokenID.T_POW_OP)
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();         // eat '**'
                CastExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new PowOpCode(line, linePosition));
            }
        }

        /*--------------------------------------------------------------*/
        // multiplicative-expression :: pow-expression { multiplicative-operator pow-expression }
        // multiplicative-operator :: '*' | '/' | '%' | "div"

        // * / % div
        private static bool IsMulOp(TokenID tokID)
        {
            if (tokID == TokenID.T_MUL_OP ||
                tokID == TokenID.T_DIV_OP ||
                tokID == TokenID.T_MOD_OP ||
                tokID == TokenID.T_KEY_DIV)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        private void MultiplicativeExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> multiplicative-expression");

            PowExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            TokenID tokID = tokenizer.GetLastToken().TokID;
            while (IsMulOp(tokID))
            {
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                PowExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                switch (tokID)
                {
                    case TokenID.T_MUL_OP:
                        codePart.Append(new MulOpCode(line, linePosition));
                        break;
                    case TokenID.T_DIV_OP:
                        codePart.Append(new DivOpCode(line, linePosition));
                        break;
                    case TokenID.T_KEY_DIV:
                        codePart.Append(new DivIOpCode(line, linePosition));
                        break;
                    case TokenID.T_MOD_OP:
                        codePart.Append(new ModOpCode(line, linePosition));
                        break;

                    default:
                        // TODO: add more specific error code here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPOP));
                }

                tokID = tokenizer.GetLastToken().TokID;
            }
        }

        /*--------------------------------------------------------------*/
        // additive-expression :: multiplicative-expression { additional-operator multiplicative-expression }
        // additional-operator :: '+' | '-' | '..'

        // + - ..
        private static bool IsAddOp(TokenID tokID)
        {
            if (tokID == TokenID.T_ADD_OP ||
                tokID == TokenID.T_SUB_OP ||
                tokID == TokenID.T_STRCAT_OP)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        private void AdditiveExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> additive--expression");

            MultiplicativeExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            TokenID tokID = tokenizer.GetLastToken().TokID;
            while (IsAddOp(tokID))
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                MultiplicativeExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                switch (tokID)
                {
                    case TokenID.T_ADD_OP:
                        codePart.Append(new AddOpCode(line, linePosition));
                        break;
                    case TokenID.T_SUB_OP:
                        codePart.Append(new SubOpCode(line, linePosition));
                        break;
                    case TokenID.T_STRCAT_OP:
                        codePart.Append(new StrCatOpCode(line, linePosition));
                        break;

                    default:
                        // TODO: add more specific error code here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPOP));
                }

                tokID = tokenizer.GetLastToken().TokID;
            }
        }

        /*--------------------------------------------------------------*/
        // relational-expression :: multiplicative-expression { relational-operator multiplicative-expression }
        // relational-operator :: '<' | '<=' | '>' | '>=' | "in"

        // < <= > >= in
        private static bool IsRelOp(TokenID tokID)
        {
            if (tokID == TokenID.T_IS_SMALLER_OP || tokID == TokenID.T_IS_SMALLER_OR_EQUAL_OP ||
                tokID == TokenID.T_IS_GREATHER_OP || tokID == TokenID.T_IS_GREATHER_OR_EQUAL_OP ||
                tokID == TokenID.T_KEY_IN)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        private void RelationalExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> relational-expression");

            AdditiveExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            TokenID tokID = tokenizer.GetLastToken().TokID;
            while (IsRelOp(tokID) == true)
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                AdditiveExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                switch (tokID)
                {
                    case TokenID.T_IS_SMALLER_OP:
                        codePart.Append(new IsSmallerOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_SMALLER_OR_EQUAL_OP:
                        codePart.Append(new IsSmallerOrEqualOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_GREATHER_OP:
                        codePart.Append(new IsGreatherOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_GREATHER_OR_EQUAL_OP:
                        codePart.Append(new IsGreatherOrEqualOpCode(line, linePosition));
                        break;
                    case TokenID.T_KEY_IN:
                        codePart.Append(new IsInOpCode(line, linePosition));
                        break;

                    default:
                        // TODO: add more specific error code here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPOP));
                }

                tokID = tokenizer.GetLastToken().TokID;
            }
        }

        /*--------------------------------------------------------------*/
        // equality-expression :: relational-expression { equality-operator relational-expression }
        // equality-operator '==' | '!=' | '===' | '!=='

        // == != === !==
        private static bool IsCompOp(TokenID tokID)
        {
            if (tokID == TokenID.T_IS_EQUAL_OP || tokID == TokenID.T_IS_NOT_EQUAL_OP ||
                tokID == TokenID.T_IS_SAME_OP || tokID == TokenID.T_IS_NOT_SAME_OP)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        private void EqualityExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> equality-expression");

            RelationalExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            TokenID tokID = tokenizer.GetLastToken().TokID;
            while (IsCompOp(tokID) == true)
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                RelationalExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                switch (tokID)
                {
                    case TokenID.T_IS_EQUAL_OP:
                        codePart.Append(new IsEqualOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_NOT_EQUAL_OP:
                        codePart.Append(new IsNotEqualOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_SAME_OP:
                        codePart.Append(new IsSameOpCode(line, linePosition));
                        break;
                    case TokenID.T_IS_NOT_SAME_OP:
                        codePart.Append(new IsNotSameOpCode(line, linePosition));
                        break;

                    default:
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPCMPOP));
                }

                tokID = tokenizer.GetLastToken().TokID;
            }
        }

        /*--------------------------------------------------------------*/
        // logical-fand-expression :: equality-expression { logical-fand-operator equality-expression }
        // logical-fand-operator :: 'and'

        private void LogicalFAndExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> logical-fand-expression");

            EqualityExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            while (tokenizer.GetLastToken().TokID == TokenID.T_KEY_AND)
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                EqualityExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new AndOpCode(line, linePosition));
            }
        }

        /*--------------------------------------------------------------*/
        // logical-and-expression :: logical-fand-expression { logical-and-operator logical-fand-expression }
        // logical-and-operator :: '&&'

        private void LogicalAndExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> logical-and-expression");

            LogicalFAndExpression(st, codePart);

            // generate skip jump label
            if (tokenizer.GetLastToken().TokID == TokenID.T_AND_OP)
            {
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                // creates a NOP opcode
                OpCodeListItem skip_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                while (tokenizer.GetLastToken().TokID == TokenID.T_AND_OP)
                {
                    // get the position of the operator
                    line = tokenizer.GetLastToken().Line;
                    linePosition = tokenizer.GetLastToken().LinePosition;

                    tokenizer.NextToken();  // eat op.

                    // generate a if-false-then-jump-to-skip-label instruction
                    codePart.Append(new JumpIfFalseOpCode(line, linePosition, skip_label));

                    // evaluate the second expr
                    LogicalFAndExpression(st, codePart);
                }

                // append the skip label
                codePart.Append(skip_label);
            }
        }

        /*--------------------------------------------------------------*/
        // logical-for-expression :: logical-and-expression { logical-for-operator logical-and-expression }
        // logical-for-operator 'or'

        private void LogicalFOrExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> logical-for-expression");

            LogicalAndExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            while (tokenizer.GetLastToken().TokID == TokenID.T_KEY_OR)
            {
                // push a value in R to stack
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();
                LogicalAndExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new OrOpCode(line, linePosition));
            }
        }

        /*--------------------------------------------------------------*/
        // logical-or-expression :: logical-for-expression { logical-or-operator logical-for-expression }
        // logical-or-operator '||'

        private void LogicalOrExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> logical-or-expression");

            LogicalFOrExpression(st, codePart);

            // generate skip jump label
            if (tokenizer.GetLastToken().TokID == TokenID.T_OR_OP)
            {
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                OpCodeListItem skip_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                while (tokenizer.GetLastToken().TokID == TokenID.T_OR_OP)
                {
                    // get the position of the operator
                    line = tokenizer.GetLastToken().Line;
                    linePosition = tokenizer.GetLastToken().LinePosition;

                    // eat ||
                    tokenizer.NextToken();

                    // generate a if-true-then-jump-to-skip-label instruction
                    codePart.Append(new JumpIfTrueOpCode(line, linePosition, skip_label));

                    // evaluate the second expr
                    LogicalFOrExpression(st, codePart);
                }

                // append the skip label
                codePart.Append(skip_label);
            }
        }

        /*--------------------------------------------------------------*/
        // ask-expression :: logical-or-expression [ ask-operator ask-expression ]
        // ask-operator :: '??' | '?!'

        private void AskExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> ask-expression");

            LogicalOrExpression(st, codePart);

            // get the position after the expression
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            TokenID tokID = tokenizer.GetLastToken().TokID;
            if (tokID == TokenID.T_HAS_VALUE_OP)
            {
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();              // eat "??"
                AskExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new HasValueOpCode(line, linePosition));
            }
            else if (tokID == TokenID.T_IS_FALSE_OP)
            {
                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();              // eat "?!"
                AskExpression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new IsFalseOpCode(line, linePosition));
            }
        }

        /*------------------------------------------------------------------------*/
        // conditional-expression ::
        //   ask-expression [ '?' expression ':' expression ] |

        private void ConditionalExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> conditional-expression");

            AskExpression(st, codePart);

            TokenID tokID = tokenizer.GetLastToken().TokID;
            if (tokID == TokenID.T_TER_ASK_OP)
            {
                // get the position of the '?'
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                tokenizer.NextToken();              // eat "?"

                // generate a if-false-then-jump-to-false-label instruction
                OpCodeListItem jumptofalseop = codePart.Append(
                  new JumpIfFalseOpCode(line, linePosition, null)
                );

                // a true-expression follows
                Expression(st, codePart);

                // get the position of the ':'
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // match ':'
                Match(TokenID.T_COLON); // eat ':'

                // "false" part
                // jump to end/skip false expression
                OpCodeListItem skipfalseexprop = codePart.Append(
                  new JumpOpCode(line, linePosition, null)
                );

                // the false-label
                APointerOpCode opcode = (APointerOpCode)jumptofalseop.Data;
                opcode.Parameter = codePart.Append(
                  new NopOpCode(line, linePosition)
                );

                // a false-expression follows
                Expression(st, codePart);

                // get the position of the first token behind the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // the true-label
                opcode = (APointerOpCode)skipfalseexprop.Data;
                opcode.Parameter = codePart.Append(
                  new NopOpCode(line, linePosition)
                );
            }
        }

        /*--------------------------------------------------------------*/
        // assignment-expression :: conditional-expression [ assignment-operator expression ]
        // assignment-operator :: '=' | '+=' | '-=' | '*=' | '/=' | '%=' | '**='

        // = += -= *= /= %= **=
        private static bool IsAOp(TokenID tokID)
        {
            if (tokID > TokenID.T_AOP && tokID < TokenID.T_AOP_END) return true; else return false;
        }


        private void AssignmentExpression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> assignment-expression");

            ConditionalExpression(st, codePart);

            TokenID tokID = tokenizer.GetLastToken().TokID;
            if (IsAOp(tokID) == true)
            {
                // get the position of the operator
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new PushOpCode(line, linePosition));
                tokenizer.NextToken();               // eat the assignment-operator

                Expression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                codePart.Append(new StoreOpCode(line, linePosition, tokID));
            }
        }

        /*------------------------------------------------------------------------*/
        // but-expression :: assignment-expression [ but-op expression ]
        // but-op :: "but"

        private void ButExpression(ScriptState st, OpCodeList codePart)
        {
            // Console.WriteLine(">> but-expression");

            AssignmentExpression(st, codePart);

            if (tokenizer.GetLastToken().TokID == TokenID.T_KEY_BUT)
            {
                // get the position of the operator
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                tokenizer.NextToken();      // eat "but" op.

                // create a "simple value" (ex.: a[1] -> &a[1])
                codePart.Append(new GetValOpCode(line, linePosition));
                Expression(st, codePart);
            }
        }

        /*--------------------------------------------------------------*/
        // expression :: but-expression

        private void Expression(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> expression");

            ButExpression(st, codePart);

            //Console.WriteLine(">> expression end");
        }


        /*--------------------------------------------------------------*/
        // local-variable-declaration :: "local" local-variable-declaration-list ';'
        // local-variable-declaration-list ::
        // identifier [ '=' expression ] { ',' identifier [ '=' expression ] }
        //
        // Ex.: local i, j = 10, k;

        //private int LocalVariableDeclaration(SharpNektonState st, OpCodeList codePart, int offset)
        private void LocalVariableDeclaration(ScriptState st, OpCodeList codePart)
        {
            int offset = locals.LocalOffset;

            // eat "local"
            tokenizer.NextToken();

            // first-round flag
            bool firstsym = true;  // TODO: get rid of the firstsym flag

            // { ',' ident }
            do
            {
                LocalObject newLocal;

                if (firstsym == false)
                {
                    tokenizer.NextToken();  // eat ',' before next symbol
                }

                if (tokenizer.GetLastToken().TokID == TokenID.T_IDENTIFIER)
                {
                    newLocal = locals.AddLocalObject(tokenizer.GetLastToken().SVal, ObjectLevelID.LOCAL_VARIABLE, offset);
                    offset++; // advance the offset for the next local variable
                }
                else
                {
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPIDENT));
                }

                // get the source position of the identifier
                int line = tokenizer.GetLastToken().Line;
                int linePosition = tokenizer.GetLastToken().LinePosition;

                // eat ident
                tokenizer.NextToken();

                // '=' expression
                if (tokenizer.GetLastToken().TokID == TokenID.T_ASSIGN_OP)
                {
                    tokenizer.NextToken();  // eat '='

                    // variable's store
                    codePart.Append(new LoadFPOpCode(line, linePosition, newLocal.Offset));

                    // initialisation expression + O_STORE
                    codePart.Append(new PushOpCode(line, linePosition));

                    Expression(st, codePart);

                    // get the source position after the expression
                    line = tokenizer.GetLastToken().Line;
                    linePosition = tokenizer.GetLastToken().LinePosition;

                    codePart.Append(new StoreOpCode(line, linePosition, TokenID.T_ASSIGN_OP));
                }

                // initialize firstsym
                firstsym = false;

            } while (tokenizer.GetLastToken().TokID == TokenID.T_COMMA);

            // match ';'
            Match(TokenID.T_STAT_END);

            locals.LocalOffset = offset;
        }


        /*--------------------------------------------------------------*/
        // block ::
        //   '{' { statement } '}' |
        //   assignment-statement

        private int Block(ScriptState st, OpCodeList codePart, OpCodeListItem return_label)
        {
            // match '{'
            if (tokenizer.GetLastToken().TokID == TokenID.T_BLOCK_START)
            {   // it is the full-block function body
                // eat '{'
                tokenizer.NextToken();

                // statement(s)
                while (tokenizer.GetLastToken().TokID != TokenID.T_EOF)
                {
                    if (tokenizer.GetLastToken().TokID == TokenID.T_BLOCK_END) break;  // '}' found

                    Statement(st, codePart, null, null, return_label);
                }

                // match '}'
                Match(TokenID.T_BLOCK_END);
            }
            else
            {  // it is the "assignment-statement" function body
               // compile "return expression"
                Expression(st, codePart);
                codePart.Append(new GetValOpCode(0, 0));

                if (return_label != null)
                {
                    codePart.Append(new JumpOpCode(0, 0, return_label));
                }
                else
                {
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NORET));
                }

                // match ';'
                Match(TokenID.T_STAT_END);
            }

            // calculate and return number of local variables
            return locals.LocalOffset - 1;
        }

        /*--------------------------------------------------------------*/
        // formal-parameter :: identifier

        private void FormalParameter(ScriptState st, int offset)
        {
            // match ident
            if (tokenizer.GetLastToken().TokID == TokenID.T_IDENTIFIER)
            {
                locals.AddLocalObject(tokenizer.GetLastToken().SVal, ObjectLevelID.FORMAL_PAREMETER, offset);

                // eat ident
                tokenizer.NextToken();
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPIDENT));
            }
        }

        /*--------------------------------------------------------------*/
        /**
         * \brief Parses the formal-parameter-list in function declaration.
         *
         * <pre>
         * formal-parameter-list :: '(' [ formal-parameter { ',' formal-parameter } ] ')'
         * </pre>
         *
         * Offset calculation (first half = in-function, second half = before-function).
         *
         * <pre>
         * Stack situation:
         *
         * local_2       2
         * local_1       1   -> first local variable
         * stfp          0   -> stored frame-pointer
         * ---------------
         * rtsa         -1   -> jsr call
         * _argc        -2   -> argument count (invisible for users)
         * param_2      -3   -> last parameter
         * param_1      -4   -> first parameter
         * funref       -5   -> a function reference
         * </pre>
         *
         * */

        private int FormalParameterList(ScriptState st)
        {
            int numparams = 0; // 0 = first param, 1 = second, ...
            int real_number_of_parameters;

            // check for '('
            Match(TokenID.T_LPAREN);

            if (tokenizer.GetLastToken().TokID != TokenID.T_RPAREN)
            {
                if (tokenizer.GetLastToken().TokID == TokenID.T_ELLIPSIS)
                {
                    // eat '...'
                    tokenizer.NextToken();
                    numparams = -1;         // 0 or more params
                }
                else
                {
                    numparams++;
                    FormalParameter(st, numparams);

                    while (tokenizer.GetLastToken().TokID == TokenID.T_COMMA)
                    {
                        // eat ','
                        tokenizer.NextToken();

                        if (tokenizer.GetLastToken().TokID == TokenID.T_ELLIPSIS)
                        {
                            // eat '...'
                            tokenizer.NextToken();
                            numparams = -(numparams + 1);    // 3 -> -4 = 3 or more params
                            break;
                        }
                        else
                        {
                            numparams++;
                            FormalParameter(st, numparams);
                        }
                    }
                }
            }

            // check for ')'
            Match(TokenID.T_RPAREN);

            // get the real number of parameters
            if (numparams == 0 || numparams == -1)
            {
                real_number_of_parameters = 1;
            }
            else if (numparams > 0)
            {
                real_number_of_parameters = numparams + 1;
            }
            else
            {
                real_number_of_parameters = -numparams;
            }

            // add the "_argc" param
            locals.AddLocalObject("_argc", ObjectLevelID.FORMAL_PAREMETER, real_number_of_parameters);

            // fix offsets (_argc contains number of params - 1 => c(a, b) == 2)
            if (real_number_of_parameters > 0)
            {
                foreach (DictionaryEntry formalDictEnt in locals.FormalParameters)
                {
                    LocalObject formal = (LocalObject)formalDictEnt.Value;
                    formal.Offset = -(real_number_of_parameters - formal.Offset + 2); // 2 = skip fp and rtsa
                }
            }

            return numparams;
        }

        /*--------------------------------------------------------------*/
        /**
         * \brief Compiles internal function structure (statements).
         *
         * Helps scp_Function_Declaration() to do it's job. Compiles internal
         * function structure. Generates opcodes and labels suroundidng the
         * function body and invokes the function-body (block) compiler.
         *
         * */

        private void Function_Body(ScriptState st, OpCodeList codePart)
        {
            //sc_opcode_t *opcode;
            //list_item_t *return_jump, *link_code_ptr, *sub_start_ptr, *sub_return_ptr;
            //int numlocals;

            // return-from-subroutine label
            OpCodeListItem returnFromSubroutineLabel = new OpCodeListItem(codePart, new NopOpCode(0, 0));

            // create stack frame
            OpCodeListItem linkOpCode = codePart.Append(); // will be replaced with the real LINK opcode

            // locals declaration and function's statements
            int numlocals = Block(st, codePart, returnFromSubroutineLabel);

            // create room for locals (emit the real LINK opcode)
            linkOpCode.Data = new LinkOpCode(0, 0, numlocals);

            // emit return label
            codePart.Append(returnFromSubroutineLabel);

            // emit UNLINK opcode
            codePart.Append(new UnlinkOpCode(0, 0));

            // return-from-subroutine
            codePart.Append(new ReturnFromSubroutineOpCode(0, 0));
        }

        /*--------------------------------------------------------------*/
        // function-declaration :: "function" [ identifier ] formal-parameter-list block

        private void Function_Declaration(ScriptState st, OpCodeList codePart)
        {
            OpCodeList funcCodePart;
            int numparams;
            bool anonfunc = false;
            ValueStore subObject = null;

            // remember the current sourceLevel
            SourceLevelID previousSourceLevel = sourceLevel;

            // we are stepping into the local level (a function)
            sourceLevel = SourceLevelID.LOCAL;

            // get the source position of the "function"
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "function"
            tokenizer.NextToken();

            // function identifier found?
            if (tokenizer.GetLastToken().TokID == TokenID.T_IDENTIFIER)
            {   // function ident (params) ...
                // get function's object or create a new one
                subObject = st.Evaluator.AddGlobalObject(tokenizer.GetLastToken().SVal);

                // eat ident
                tokenizer.NextToken();

                // it is not an anonymous function
                anonfunc = false;
            }
            else
            {                // function (params) ...
                subObject = null;   // anonymous fonctions have no global object 
                anonfunc = true;    // it is an anonymous function
            }

            // create a new locals stack (save the old)
            LocalsStack locals_tmp = locals;
            locals = new LocalsStack();

            // parse params
            try
            {
                // compile the parameters
                numparams = FormalParameterList(st);

                // create a new code part for the new function
                funcCodePart = st.Evaluator.NewCodePart();

                // compile function's body
                Function_Body(st, funcCodePart);
            }
            catch (SharpNektonException e)
            {
                //Console.WriteLine(">> {0}", e);
                locals = locals_tmp;
                throw e;
            }

            // return old local symblol stack
            locals = locals_tmp;

            // restore the original source level
            sourceLevel = previousSourceLevel;

            // we are creating a function...
            FunctionRef functionRef = new FunctionRef(funcCodePart, numparams);

            if (anonfunc)
            {
                // load anonymous function pointer into the R
                codePart.Append(new LoadFunctionOpCode(line, linePosition, functionRef));
            }
            else
            {
                // load nonanonymous function refference to the R
                codePart.Append(new LoadStorOpCode(line, linePosition, subObject));
                codePart.Append(new PushOpCode(line, linePosition));
                codePart.Append(new LoadFunctionOpCode(line, linePosition, functionRef));
                codePart.Append(new StoreOpCode(line, linePosition, TokenID.T_ASSIGN_OP));

                // Set nonanonymous function's object to pointer to the function
                subObject.Value = new FunctionRefValue(functionRef);
            }
        }


        /*--------------------------------------------------------------*/
        // assignment-statement :: expression ';'

        private void Assignment_Statement(ScriptState st, OpCodeList codePart)
        {
            //Console.WriteLine(">> assignment-statement");

            Expression(st, codePart);

            // match ';'
            Match(TokenID.T_STAT_END);

            //Console.WriteLine(">> assignment-statement end");
        }

        /*--------------------------------------------------------------*/
        // condition :: '(' expression ')'

        private void Condition(ScriptState st, OpCodeList codePart)
        {
            // check for '('
            Match(TokenID.T_LPAREN);

            Expression(st, codePart);

            // check for ')'
            Match(TokenID.T_RPAREN);
        }

        /*--------------------------------------------------------------*/
        // if-statement :: "if" condition true-statement [ "else" false-statement ]

        /* * <pre>
         * if-statement:
         *
         *   condition
         *   jump_if_false L0
         *   true-statement
         *   jmp L1
         * L0:
         *   false-statement
         * L1:
         * </pre>
         *
         * */

        private void If_Statement(ScriptState st, OpCodeList codePart,
          OpCodeListItem break_label, OpCodeListItem continue_label, OpCodeListItem return_label)
        {
            // eat "if"
            tokenizer.NextToken();

            // condition
            Condition(st, codePart);

            // get the position of the first token after the condition
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // false label
            OpCodeListItem falseLabel = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // generate a if-false-then-jump-to-false-label instruction
            codePart.Append(new JumpIfFalseOpCode(line, linePosition, falseLabel));

            // a true-statement follows
            Statement(st, codePart, break_label, continue_label, return_label);

            // *********** ELSEIF ***********

            // "else" part
            if (tokenizer.GetLastToken().TokID == TokenID.T_KEY_ELSE)
            {
                // get the position of the "else"
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // eat "else"
                tokenizer.NextToken();

                // skip-false-statement-label
                OpCodeListItem postIfLabel = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                // emit the skip-false-statement
                codePart.Append(new JumpOpCode(line, linePosition, postIfLabel));

                // emit the false-label
                codePart.Append(falseLabel);

                // a false-statement follows
                Statement(st, codePart, break_label, continue_label, return_label);

                // emit the skip-false-label
                codePart.Append(postIfLabel);
            }
            else
            {
                // emit the false-label
                codePart.Append(falseLabel);
            }
        }

        /*--------------------------------------------------------------*/
        // while-statement :: "while" condition statement
        // condition :: '(' expression ')'

        /* * <pre>
         * while-statement:
         *
         * L0:         -> continue
         *   condition
         *   jump_if_false L1
         *   statement
         *   jmp L0
         * L1:         -> break
         * </pre>
         *
         * */

        private void While_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem return_label)
        {
            // get the position of the "while"
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "while"
            tokenizer.NextToken();

            // generate break jump label
            OpCodeListItem break_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // continue-label
            OpCodeListItem continue_label = codePart.Append(
              new NopOpCode(line, linePosition)
            );

            // condition
            Condition(st, codePart);

            // get the position of the first token after the condition
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // generate a if-false-then-jump-to-false-label instruction
            codePart.Append(new JumpIfFalseOpCode(line, linePosition, break_label));

            // a true-statement follows
            Statement(st, codePart, break_label, continue_label, return_label);

            // get the position of the first token after the statement
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // loop
            codePart.Append(new JumpOpCode(line, linePosition, continue_label));

            // append the break label
            codePart.Append(break_label);
        }

        /*--------------------------------------------------------------*/
        // do-statement :: "do" statement "while" condition ';'
        // condition :: '(' expression ')'

        /** <pre>
         * do-statement:
         *
         * L0:                     -> loop
         *    statement(L1, L2)
         *  L2:                    -> continue
         *   condition
         *   jump_if_true L0:
         * L1:                    -> break
         * </pre>
         *
         * */

        private void Do_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem return_label)
        {
            // get the position of the "do"
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "do"
            tokenizer.NextToken();

            // generate break jump label
            OpCodeListItem break_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // continue label
            OpCodeListItem continue_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // loop-label
            OpCodeListItem loop_label = codePart.Append(
              new NopOpCode(line, linePosition)
            );

            // statement
            Statement(st, codePart, break_label, continue_label, return_label);

            // append the continue label
            codePart.Append(continue_label);

            // match "while"
            Match(TokenID.T_KEY_WHILE);

            // condition
            Condition(st, codePart);

            // get the position of the first token after the condition
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // generate a if-true-then-jump-to-loop-start-label instruction
            codePart.Append(new JumpIfTrueOpCode(line, linePosition, loop_label));

            // append the break label
            codePart.Append(break_label);

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // for-statement :: "for" '(' [expr-init] ';' [expr-cond] ';' [expr-incr] ')' statement
        // expr-init :: expression
        // expr-cond :: expression
        // expr-incr :: expression
        //
        // expr-init ;
        // while (expr-cond) {
        //   statement
        //   expr-incr ;
        // }

        /** </pre>
         *
         * <pre>
         * for-statement:
         *
         *   expr-init
         * L0:            -> cond-label
         *   expr-cond
         *   jeq L1       -> goto break label
         *   jmp L2       -> goto statement label
         * L3:            -> continue label
         *   expr-incr
         *   jmp L0       -> goto cond-label
         * L2:            -> statement label
         *   statement
         *   jmp L3
         * L1:           -> break label
         * </pre>
         *
         * */

        private void For_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem return_label)
        {
            OpCodeListItem break_label, continue_label = null, cond_label = null, statement_label = null;
            bool have_cond = false;

            // get the position of the "for"
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "for"
            tokenizer.NextToken();

            // generate break jump label
            break_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // check for '('
            Match(TokenID.T_LPAREN);

            /*................  INIT .................*/

            if (tokenizer.GetLastToken().TokID != TokenID.T_STAT_END)
            {
                // compile expr-init
                Expression(st, codePart);
            }

            // match ';'
            Match(TokenID.T_STAT_END);

            /*................  CONTINUE .................*/

            if (tokenizer.GetLastToken().TokID != TokenID.T_STAT_END)
            {
                // get the position before the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                have_cond = true;

                // generate cond-label
                cond_label = codePart.Append(
                  new NopOpCode(line, linePosition)
                );

                // compile expr-cond
                Expression(st, codePart);

                // get the position after the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // generate a if-false-then-jump-to-false-label instruction
                codePart.Append(new JumpIfFalseOpCode(line, linePosition, break_label));

                //  cond == continue
                continue_label = cond_label;
            }

            // match ';'
            Match(TokenID.T_STAT_END);

            /*................  INCREMENT .................*/

            if (tokenizer.GetLastToken().TokID != TokenID.T_RPAREN)
            {
                // get the position before the expression
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                if (have_cond)
                {
                    // generate to statement jump label
                    statement_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                    // goto statement
                    codePart.Append(new JumpOpCode(line, linePosition, statement_label));
                }

                // generate continue-label
                continue_label = codePart.Append(
                  new NopOpCode(line, linePosition)
                );

                // compile expr-incr
                Expression(st, codePart);

                if (have_cond)
                {
                    // get the position after the expression
                    line = tokenizer.GetLastToken().Line;
                    linePosition = tokenizer.GetLastToken().LinePosition;

                    // jum to the cond
                    codePart.Append(new JumpOpCode(line, linePosition, cond_label));

                    // append the statement label
                    codePart.Append(statement_label);
                }
            }
            else
            { // no increment
                if (!have_cond)
                { // and no cond-expr
                  // get the actual position
                    line = tokenizer.GetLastToken().Line;
                    linePosition = tokenizer.GetLastToken().LinePosition;

                    // generate continue-label
                    continue_label = codePart.Append(
                    new NopOpCode(line, linePosition)
                  );
                }
            }

            // match ')'
            Match(TokenID.T_RPAREN);

            /*................  STATEMENT .................*/

            // a statement
            Statement(st, codePart, break_label, continue_label, return_label);

            // get the position after the statement
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // goto continue label
            codePart.Append(new JumpOpCode(line, linePosition, continue_label));

            // append the break label
            codePart.Append(break_label);
        }

        /*--------------------------------------------------------------*/
        // foreach-statement ::
        //   "foreach" '(' expression "as" expression [ ',' expression ] ')' statement

        /**

        <pre>
        O_REWIND
          in:   R = arrayref
          out:  R = arrayref
          op:   arrayref->cindex = 0;

        O_NEXT
          sp:   unchanged
          in:   S[top-1] = arrayref
          out:  R = arrayref[arrayref->cindex]
          op:   if (arrayref->cindex >= arrayref->size) {
                  R = END-OF-READ;
                }
                else {
                  R = arrayref[arrayref->cindex]; arrayref->cindex++;
                }

        O_NEXT_KEY
          sp:   unchanged
          in:   S[top-1] = arrayref
          out:  R = arrayref->cindex
          op:   if (arrayref->cindex >= arrayref->size) {
                  R = END-OF-READ;
                }
                else {
                  R = arrayref->cindex; arrayref->cindex++;
                }

        O_FSTORE
          sin:  S[top] = varref; S[top-1] = arrayref
          sout: S[top] = arrayref
          in:   R = index; S[top] = varref; S[top-1] = arrayref
          out:  R = arrayref[index]; S[top] = arrayref;
          op:   R = arrayref[index]; store;


        foreach (aref as var) stat

        next/store version:
        -------------------

          aref
          rewind     -> cindex = 0; lefts aref in R
          push       -> stack[top] = aref
        cont-lab:
          var        -> R = varref
          push       -> stack[top] = varref; stack[top-1] = arrayref
          next       -> R = aref[cindex] or read end;
          jump-if-end-of-read endread-lab
          store      -> stack[top] = aref
          stat
          jump cont-lab
        endread-lab:
          pop        -> remove varref from stack[top]
        break-lab:
          pop        -> remove aref from stack[top]

        next-key/fstore version:
        ------------------------

          aref
          rewind     -> cindex = 0; lefts aref in R
          push       -> stack[top] = aref
        cont-lab:
          var        -> R = varref
          push       -> stack[top] = varref; stack[top-1] = arrayref
          next-key   -> R = aref[cindex] or read end;
          jump-if-end-of-read endread-lab
          fstore     -> stack[top] = aref
          stat
          jump cont-lab
        endread-lab:
          pop        -> remove varref from stack[top]
        break-lab:
          pop        -> remove aref from stack[top]



        foreach (aref as var, key) stat

          aref
          rewind     -> cindex = 0; lefts aref in R
          push       -> stack[top] = aref
        cont-lab:
          var        -> R = varref
          push       -> stack[top] = varref; stack[top-1] = arrayref

          key
          push
          next-key   -> R = aref->cindex or read end;
          jump-if-end-of-read endread-lab
          store      -> keyref = key; R = key; stack[top] = keyref; stack[top-1] = arrayref
          fstore     -> R = aref[R]; stack[top] = aref
          stat
          jump cont-lab
        endread-lab:
          pop        -> remove keyref from stack[top]
          pop        -> remove varref from stack[top]
        break-lab:
          pop        -> remove aref from stack[top]
        </pre>

        */

        private void Foreach_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem return_label)
        {
            OpCodeListItem endread_label;

            // get the source position of the foreach keyword
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "foreach"
            tokenizer.NextToken();

            // generate break jump label
            OpCodeListItem break_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

            // check for '('
            Match(TokenID.T_LPAREN);

            /*................  ARRAY-EXP .................*/

            // compile array-expr
            Expression(st, codePart);

            // get the source position after the array-expr
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // "rewind" the array
            codePart.Append(new RewindOpCode(line, linePosition));

            // push the array ref. to the stack
            codePart.Append(new PushOpCode(line, linePosition));

            // generate continue-label
            OpCodeListItem continue_label = codePart.Append(new NopOpCode(line, linePosition));

            // check for "as"
            Match(TokenID.T_KEY_AS);

            /*................  VAR-EXP .................*/

            // compile var-expr
            Expression(st, codePart);

            // get the source position after the var-expr
            line = tokenizer.GetLastToken().Line;
            linePosition = tokenizer.GetLastToken().LinePosition;

            // push the var ref. to the stack
            codePart.Append(new PushOpCode(line, linePosition));

            // foreach (arr as val) stat
            if (tokenizer.GetLastToken().TokID == TokenID.T_RPAREN)
            {
                // eat ')'
                tokenizer.NextToken();

                /*................  LOOP .................*/

                // get the source position inside the loop
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // "next"
                codePart.Append(new NextOpCode(line, linePosition));

                // generate end-of-read label
                endread_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                // jump to the end-of-read label
                codePart.Append(new JumpIfDoneOpCode(line, linePosition, endread_label));

                // "store"
                codePart.Append(new StoreOpCode(line, linePosition, TokenID.T_ASSIGN_OP));

                /*................  STATEMENT .................*/

                // a statement
                Statement(st, codePart, break_label, continue_label, return_label);

                // get the source position after the statement
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // goto continue label
                codePart.Append(new JumpOpCode(line, linePosition, continue_label));

                // append the end-of-read label
                codePart.Append(endread_label);

                // pop the var-ref
                codePart.Append(new PopNOpCode(line, linePosition, 1));
            }
            else
            {   // foreach (arr as val, key) stat
                // match ','
                Match(TokenID.T_COMMA);

                // compile key-expr
                Expression(st, codePart);

                // get the source position after the key-expr
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // push the key ref. to the stack
                codePart.Append(new PushOpCode(line, linePosition));

                // match ')'
                Match(TokenID.T_RPAREN);

                /*................  LOOP .................*/

                // get the source position inside the loop
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // "next"
                codePart.Append(new NextKeyOpCode(line, linePosition));

                // generate end-of-read label
                endread_label = new OpCodeListItem(codePart, new NopOpCode(line, linePosition));

                // jump to the end-of-read label
                codePart.Append(new JumpIfDoneOpCode(line, linePosition, endread_label));

                // "store"
                codePart.Append(new StoreOpCode(line, linePosition, TokenID.T_ASSIGN_OP));

                // "fstore"
                codePart.Append(new ForeachStoreOpCode(line, linePosition));

                /*................  STATEMENT .................*/

                // a statement
                Statement(st, codePart, break_label, continue_label, return_label);

                // get the source position after the statement
                line = tokenizer.GetLastToken().Line;
                linePosition = tokenizer.GetLastToken().LinePosition;

                // goto continue label
                codePart.Append(new JumpOpCode(line, linePosition, continue_label));

                // append the end-of-read label
                codePart.Append(endread_label);

                // pop the var-ref and the key-ref
                codePart.Append(new PopNOpCode(line, linePosition, 2));
            }

            /*................  BREAK-LABEL .................*/

            // append the break label
            codePart.Append(break_label);

            // pop the array-ref
            codePart.Append(new PopNOpCode(line, linePosition, 1));
        }

        /*--------------------------------------------------------------*/
        // break-statement :: "break" ';'

        private void Break_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem label)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "break"
            tokenizer.NextToken();

            if (label != null)
            {
                codePart.Append(new JumpOpCode(line, linePosition, label));
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NOBREAK));
            }

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // continue-statement :: "continue" ';'

        private void Continue_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem label)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "continue"
            tokenizer.NextToken();

            if (label != null)
            {
                codePart.Append(new JumpOpCode(line, linePosition, label));
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NOCONT));
            }

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // return-statement :: "return" [ expression ] ';'

        private void Return_Statement(ScriptState st, OpCodeList codePart, OpCodeListItem label)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "return"
            tokenizer.NextToken();

            // compile return value, if present
            if (tokenizer.GetLastToken().TokID != TokenID.T_STAT_END)
            {
                Expression(st, codePart);
                codePart.Append(new GetValOpCode(line, linePosition));
            }

            if (label != null)
            {
                codePart.Append(new JumpOpCode(line, linePosition, label));
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NORET));
            }

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // end-statement :: "end" ';'

        private void End_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "end"
            tokenizer.NextToken();

            // return value = 0
            codePart.Append(new LoadNOpCode(line, linePosition, 0.0));
            codePart.Append(new EndOpCode(line, linePosition));

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // exit-statement :: "exit" expression ';'

        private void Exit_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "exit"
            tokenizer.NextToken();

            // expecting a return value
            if (tokenizer.GetLastToken().TokID == TokenID.T_STAT_END)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPEXITVAL));
            }

            // compile the return value
            Expression(st, codePart);
            codePart.Append(new GetValOpCode(line, linePosition));
            codePart.Append(new EndOpCode(line, linePosition));

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // print-statement :: "print" [ expression ] ';'

        private void Print_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "print"
            tokenizer.NextToken();

            // expecting a return value
            if (tokenizer.GetLastToken().TokID != TokenID.T_STAT_END)
            {
                Expression(st, codePart);
                codePart.Append(new GetValOpCode(line, linePosition));
            }
            else
            {
                // print; = print null; -> prints a line feed
                codePart.Append(new LoadNullOpCode(line, linePosition));
            }

            codePart.Append(new PrintOpCode(line, linePosition));

            // match ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // include-statement :: "include" expression ';'

        private void Include_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "include"
            tokenizer.NextToken();

            Expression(st, codePart);
            codePart.Append(new GetValOpCode(line, linePosition));
            codePart.Append(new IncludeOpCode(line, linePosition));

            // check for ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // import-statement :: "import" expression ';'

        private void Import_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "import"
            tokenizer.NextToken();

            Expression(st, codePart);
            codePart.Append(new GetValOpCode(line, linePosition));
            codePart.Append(new ImportOpCode(line, linePosition));

            // check for ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // eval-statement :: "eval" expression ';'

        private void Eval_Statement(ScriptState st, OpCodeList codePart)
        {
            int line = tokenizer.GetLastToken().Line;
            int linePosition = tokenizer.GetLastToken().LinePosition;

            // eat "eval"
            tokenizer.NextToken();

            Expression(st, codePart);
            codePart.Append(new GetValOpCode(line, linePosition));
            codePart.Append(new EvalOpCode(line, linePosition));

            // check for ';'
            Match(TokenID.T_STAT_END);
        }

        /*--------------------------------------------------------------*/
        // compound-statement :: '{' { statement } '}'

        private void Compound_Statement(ScriptState st, OpCodeList codePart,
          OpCodeListItem break_label, OpCodeListItem continue_label, OpCodeListItem return_label)
        {
            // match '{'
            Match(TokenID.T_BLOCK_START);

            // statement(s)
            while (tokenizer.GetLastToken().TokID != TokenID.T_EOF)
            {
                if (tokenizer.GetLastToken().TokID == TokenID.T_BLOCK_END) break; // '}' found

                Statement(st, codePart, break_label, continue_label, return_label);
            }

            // match '}'
            Match(TokenID.T_BLOCK_END);
        }

        /*--------------------------------------------------------------*/
        /**
         * \brief Parses a statement.
         *
         * <pre>
         * statement ::
         *   assignment-statement |
         *   compound-statement |
         *   if-statement |
         *   while-statement |
         *   do-statement |
         *   for-statement |
         *   foreach-statement |
         *   break-statement |
         *   continue-statement |
         *   return-statement |
         *   end-statement |
         *   exit-statement |
         *   include-statement |
         *   import-statement |
         *   eval-statement |
         *   function-statement |
         *   local-variable-declaration |
         *   print-statement |
         *   empty-statement
         * </pre>
         * */

        private void Statement(ScriptState st, OpCodeList codePart,
          OpCodeListItem break_label, OpCodeListItem continue_label, OpCodeListItem return_label)
        {
            switch (tokenizer.GetLastToken().TokID)
            {
                case TokenID.T_BLOCK_START:
                    Compound_Statement(st, codePart, break_label, continue_label, return_label);
                    break;

                case TokenID.T_KEY_IF:
                    If_Statement(st, codePart, break_label, continue_label, return_label);
                    break;

                case TokenID.T_KEY_WHILE:
                    While_Statement(st, codePart, return_label);
                    break;

                case TokenID.T_KEY_DO:
                    Do_Statement(st, codePart, return_label);
                    break;

                case TokenID.T_KEY_FOR:
                    For_Statement(st, codePart, return_label);
                    break;

                case TokenID.T_KEY_FOREACH:
                    Foreach_Statement(st, codePart, return_label);
                    break;

                case TokenID.T_KEY_BREAK:
                    Break_Statement(st, codePart, break_label);
                    break;

                case TokenID.T_KEY_CONTINUE:
                    Continue_Statement(st, codePart, continue_label);
                    break;

                case TokenID.T_KEY_RETURN:
                    if (sourceLevel == SourceLevelID.LOCAL)
                    {  // valid in a function body only
                        Return_Statement(st, codePart, return_label);
                    }
                    else
                    {
                        // TODO: add more specific error code here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_UNSUPCON));
                    }
                    break;

                case TokenID.T_KEY_END:
                    End_Statement(st, codePart);
                    break;

                case TokenID.T_KEY_EXIT:
                    Exit_Statement(st, codePart);
                    break;

                case TokenID.T_KEY_FUNCTION:
                    Function_Declaration(st, codePart);
                    break;

                case TokenID.T_KEY_PRINT:
                    Print_Statement(st, codePart);
                    break;

                case TokenID.T_KEY_INCLUDE:
                    Include_Statement(st, codePart);
                    break;

                case TokenID.T_KEY_IMPORT:
                    Import_Statement(st, codePart);
                    break;

                case TokenID.T_KEY_EVAL:
                    Eval_Statement(st, codePart);
                    break;

                case TokenID.T_STAT_END:  // empty-statement
                    tokenizer.NextToken();
                    break;

                case TokenID.T_KEY_ELSE:  // else without if
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_ELSEIF));

                case TokenID.T_KEY_LOCAL: // local variable declaration
                    if (sourceLevel == SourceLevelID.LOCAL)
                    {  // valid in a function body only
                        LocalVariableDeclaration(st, codePart);
                    }
                    else
                    {
                        // TODO: add more specific error code here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_UNSUPCON));
                    }
                    break;

                default:
                    Assignment_Statement(st, codePart);  // assignment, function call, ...
                    break;
            }
        }

        /*--------------------------------------------------------------*/
        // program-block :: { statement }

        private void Program_Block(ScriptState st, OpCodeList codePart)
        {
            // store previous sourceLevel
            SourceLevelID previousSourceLevel = sourceLevel;

            // we start at the global sourceLevel
            sourceLevel = SourceLevelID.GLOBAL;

            // statements
            while (tokenizer.GetLastToken().TokID != TokenID.T_EOF)
            {
                Statement(st, codePart, null, null, null);
            }

            // restore the old sourceLevel
            sourceLevel = previousSourceLevel;
        }

        /*--------------------------------------------------------------*/
        // program :: program-block

        private void Program(ScriptState st)
        {
            // create a new code part
            OpCodeList programCodePart = st.Evaluator.NewCodePart();

            // compile the program
            Program_Block(st, programCodePart);

            // emit end-of-program
            programCodePart.Append(new SubEndOpCode(tokenizer.GetLastToken().Line, tokenizer.GetLastToken().LinePosition));

            // match 'EOF'
            Match(TokenID.T_EOF);

            // set reg_R to a pointer to the new compiled program
            st.Evaluator.RegR = new ProgramRefValue(programCodePart);
        }

        /*------------------------------------------------------------------------*/

    }  // end of class

}
