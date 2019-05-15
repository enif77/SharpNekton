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

using System;

namespace SharpNekton.Compiler
{

    public enum TokenID
    {
        T_UNKNOWN,    // unknown token
        T_EOF,          // end of file
        T_NUMBER,       // double (real) number
        T_IDENTIFIER,   // identifier
        T_STRING_LIT,   // "string literal"

        T_OP,           // begin operators
        T_ADD_OP,       // '+'
        T_SUB_OP,       // '-'
        T_MUL_OP,       // '*'
        T_DIV_OP,       // '/'
        T_MOD_OP,       // '%'
        T_POW_OP,       // '**'
        T_IS_EQUAL_OP,  // '=='
        T_IS_NOT_EQUAL_OP,  // '!='
        T_IS_SAME_OP,       // '==='
        T_IS_NOT_SAME_OP,   // '!=='
        T_IS_SMALLER_OP,           // <
        T_IS_SMALLER_OR_EQUAL_OP,  // <=
        T_IS_GREATHER_OP,          // >
        T_IS_GREATHER_OR_EQUAL_OP, // >=
        T_NOT_OP,       // '!'
        T_OR_OP,        // '||'
        T_AND_OP,       // '&&'
                        //    T_B_OR_OP,      // '|'  /* UNUSED */
        T_DOT_OP,       // '.'
        T_STRCAT_OP,    // '..'
        T_DECR_OP,      // '--'
        T_INCR_OP,      // '++'
        T_DATAASGN_OP,  // '=>'
        T_TER_ASK_OP,   // '?'
        T_COPYDATA_OP,  // '&'
        T_HAS_VALUE_OP, // '??'
        T_IS_FALSE_OP,  // '?!'
        T_OP_END,       // end operators

        T_AOP,          // begin assign operators
        T_ASSIGN_OP,      // '='
        T_ADD_ASSIGN_OP,  // '+='
        T_SUB_ASSIGN_OP,  // '-='
        T_MUL_ASSIGN_OP,  // '*='
        T_DIV_ASSIGN_OP,  // '/='
        T_MOD_ASSIGN_OP,  // '%='
        T_POW_ASSIGN_OP,  // '**='
        T_STRCAT_ASSIGN_OP,  // '..='
        T_AOP_END,      // end assign operators

        T_LPAREN,       // '('
        T_RPAREN,       // ')'
        T_LBRAC,        // '['
        T_RBRAC,        // ']'
        T_COLON,        // ':'
        T_COMMA,        // ','
        T_STAT_END,     // ';'
        T_BLOCK_START,  // '{'
        T_BLOCK_END,    // '}'
        T_ELLIPSIS,     // '...'

        T_KEY_AND,       // "and"
        T_KEY_ARG,       // "arg"
        T_KEY_AS,        // "as"
        T_KEY_BOOLEAN,   // "boolean"
        T_KEY_BREAK,     // "break"
        T_KEY_BUT,       // "but"
        T_KEY_CONTINUE,  // "continue"
        T_KEY_DIV,       // "div"
        T_KEY_DO,        // "do"
        T_KEY_ELSE,      // "else"
        T_KEY_END,       // "end"
        T_KEY_EVAL,      // "eval"
        T_KEY_EXIT,      // "exit"
        T_KEY_FALSE,     // "false"
        T_KEY_FOR,       // "for"
        T_KEY_FOREACH,   // "foreach"
        T_KEY_FUNCTION,  // "function"
        T_KEY_IF,        // "if"
        T_KEY_IMPORT,    // "import"
        T_KEY_IN,        // "in"
        T_KEY_INCLUDE,   // "include"
        T_KEY_LOCAL,     // "local"
        T_KEY_NULL,      // "null"
        T_KEY_NUMBER,    // "number"
        T_KEY_OBJECT,    // "object"
        T_KEY_OR,        // "or"
        T_KEY_PRINT,     // "print"
        T_KEY_RETURN,    // "return"
        T_KEY_SIZEOF,    // "sizeof"
        T_KEY_STRING,    // "string"
        T_KEY_TRUE,      // "true"
        T_KEY_TYPEOF,    // "typeof"
        T_KEY_UNDEFINED, // "undefined"
        T_KEY_WHILE      // "while"
    }

    public class Token
    {
        private TokenID tokenID;
        private double dval;
        private String sval;
        private int line;
        private int linePosition;

        /*------------------------------------------------------------------------*/
        /** Creates a new UNKNOWN token */
        public Token(int line, int linePosition)
        {
            this.line = line;
            this.linePosition = linePosition;
            tokenID = TokenID.T_UNKNOWN;
            dval = 0.0;
            sval = "";
        }

        /*------------------------------------------------------------------------*/
        /** Creates a new token */
        public Token(int line, int linePosition, TokenID tokenID)
        {
            this.line = line;
            this.linePosition = linePosition;
            this.tokenID = tokenID;
            this.dval = 0.0;
            sval = "";
        }

        /*------------------------------------------------------------------------*/
        /** Creates a new token with a string parameter */
        public Token(int line, int linePosition, TokenID tokenID, String name)
        {
            this.line = line;
            this.linePosition = linePosition;
            this.tokenID = tokenID;
            this.dval = 0.0;
            sval = name;
        }

        /*------------------------------------------------------------------------*/
        /** Creates a new NUMBER token */
        public Token(int line, int linePosition, double dval)
        {
            this.line = line;
            this.linePosition = linePosition;
            tokenID = TokenID.T_NUMBER;
            this.dval = dval;
            sval = "";
        }

        /*------------------------------------------------------------------------*/
        /** Creates a new STRING token */
        public Token(int line, int linePosition, String sval)
        {
            this.line = line;
            this.linePosition = linePosition;
            tokenID = TokenID.T_STRING_LIT;
            dval = 0.0;
            this.sval = sval;
        }

        /*------------------------------------------------------------------------*/

        public TokenID TokID
        {
            get
            {
                return this.tokenID;
            }
        }


        public int Line
        {
            get
            {
                return line;
            }
        }


        public int LinePosition
        {
            get
            {
                return linePosition;
            }
        }

        /*------------------------------------------------------------------------*/

        public double DVal
        {
            get
            {
                return dval;
            }
        }

        /*------------------------------------------------------------------------*/

        public String SVal
        {
            get
            {
                return sval;
            }
        }

        /*------------------------------------------------------------------------*/

        public override String ToString()
        {
            string msg = "[l:" + line + " p:" + linePosition + "] ";

            switch (tokenID)
            {
                default:
                case TokenID.T_UNKNOWN: msg += "unknown"; break;

                case TokenID.T_EOF: msg += "EOF"; break;
                case TokenID.T_NUMBER: msg += "number [" + dval + "]"; break;
                case TokenID.T_STRING_LIT: msg += "string [\"" + sval + "\"]"; break;
                case TokenID.T_IDENTIFIER: msg += "identifier [\"" + sval + "\"]"; break;

                case TokenID.T_OP: msg += "operators begin"; break;
                case TokenID.T_ADD_OP: msg += "+"; break;
                case TokenID.T_SUB_OP: msg += "-"; break;
                case TokenID.T_MUL_OP: msg += "*"; break;
                case TokenID.T_DIV_OP: msg += "/"; break;
                case TokenID.T_MOD_OP: msg += "%"; break;
                case TokenID.T_POW_OP: msg += "**"; break;
                case TokenID.T_IS_EQUAL_OP: msg += "=="; break;
                case TokenID.T_IS_NOT_EQUAL_OP: msg += "!="; break;
                case TokenID.T_IS_SAME_OP: msg += "==="; break;
                case TokenID.T_IS_NOT_SAME_OP: msg += "!=="; break;
                case TokenID.T_IS_SMALLER_OP: msg += "<"; break;
                case TokenID.T_IS_SMALLER_OR_EQUAL_OP: msg += "<="; break;
                case TokenID.T_IS_GREATHER_OP: msg += ">"; break;
                case TokenID.T_IS_GREATHER_OR_EQUAL_OP: msg += ">="; break;
                case TokenID.T_NOT_OP: msg += "!"; break;
                case TokenID.T_OR_OP: msg += "||"; break;
                case TokenID.T_AND_OP: msg += "&&"; break;
                //    case TokenID.T_B_OR_OP : msg += "|"; break;  /* UNUSED */
                case TokenID.T_DOT_OP: msg += "."; break;
                case TokenID.T_STRCAT_OP: msg += ".."; break;
                case TokenID.T_DECR_OP: msg += "--"; break;
                case TokenID.T_INCR_OP: msg += "++"; break;
                case TokenID.T_DATAASGN_OP: msg += "=>"; break;
                case TokenID.T_TER_ASK_OP: msg += "?"; break;
                case TokenID.T_COPYDATA_OP: msg += "&"; break;
                case TokenID.T_HAS_VALUE_OP: msg += "??"; break;
                case TokenID.T_IS_FALSE_OP: msg += "?!"; break;
                case TokenID.T_OP_END: msg += "operators end"; break;

                case TokenID.T_AOP: msg += "assign operators begin"; break;
                case TokenID.T_ASSIGN_OP: msg += "="; break;
                case TokenID.T_ADD_ASSIGN_OP: msg += "+="; break;
                case TokenID.T_SUB_ASSIGN_OP: msg += "-="; break;
                case TokenID.T_MUL_ASSIGN_OP: msg += "*="; break;
                case TokenID.T_DIV_ASSIGN_OP: msg += "/="; break;
                case TokenID.T_MOD_ASSIGN_OP: msg += "%="; break;
                case TokenID.T_POW_ASSIGN_OP: msg += "**="; break;
                case TokenID.T_STRCAT_ASSIGN_OP: msg += "..="; break;
                case TokenID.T_AOP_END: msg += "assign operators end"; break;

                case TokenID.T_LPAREN: msg += "("; break;
                case TokenID.T_RPAREN: msg += ")"; break;
                case TokenID.T_LBRAC: msg += "["; break;
                case TokenID.T_RBRAC: msg += "]"; break;
                case TokenID.T_COLON: msg += ":"; break;
                case TokenID.T_COMMA: msg += ","; break;
                case TokenID.T_STAT_END: msg += ";"; break;
                case TokenID.T_BLOCK_START: msg += "{"; break;
                case TokenID.T_BLOCK_END: msg += "}"; break;
                case TokenID.T_ELLIPSIS: msg += "..."; break;

                case TokenID.T_KEY_AND: msg += "and"; break;
                case TokenID.T_KEY_ARG: msg += "arg"; break;
                case TokenID.T_KEY_AS: msg += "as"; break;
                case TokenID.T_KEY_BOOLEAN: msg += "boolean"; break;
                case TokenID.T_KEY_BREAK: msg += "break"; break;
                case TokenID.T_KEY_BUT: msg += "but"; break;
                case TokenID.T_KEY_CONTINUE: msg += "continue"; break;
                case TokenID.T_KEY_DIV: msg += "div"; break;
                case TokenID.T_KEY_DO: msg += "do"; break;
                case TokenID.T_KEY_ELSE: msg += "else"; break;
                case TokenID.T_KEY_END: msg += "end"; break;
                case TokenID.T_KEY_EVAL: msg += "eval"; break;
                case TokenID.T_KEY_EXIT: msg += "exit"; break;
                case TokenID.T_KEY_FALSE: msg += "false"; break;
                case TokenID.T_KEY_FOR: msg += "for"; break;
                case TokenID.T_KEY_FOREACH: msg += "foreach"; break;
                case TokenID.T_KEY_FUNCTION: msg += "function"; break;
                case TokenID.T_KEY_IF: msg += "if"; break;
                case TokenID.T_KEY_IMPORT: msg += "import"; break;
                case TokenID.T_KEY_IN: msg += "in"; break;
                case TokenID.T_KEY_INCLUDE: msg += "include"; break;
                case TokenID.T_KEY_LOCAL: msg += "local"; break;
                case TokenID.T_KEY_NULL: msg += "null"; break;
                case TokenID.T_KEY_NUMBER: msg += "number"; break;
                case TokenID.T_KEY_OBJECT: msg += "object"; break;
                case TokenID.T_KEY_OR: msg += "or"; break;
                case TokenID.T_KEY_PRINT: msg += "print"; break;
                case TokenID.T_KEY_RETURN: msg += "return"; break;
                case TokenID.T_KEY_SIZEOF: msg += "sizeof"; break;
                case TokenID.T_KEY_STRING: msg += "string"; break;
                case TokenID.T_KEY_TRUE: msg += "true"; break;
                case TokenID.T_KEY_TYPEOF: msg += "typeof"; break;
                case TokenID.T_KEY_UNDEFINED: msg += "undefined"; break;
                case TokenID.T_KEY_WHILE: msg += "while"; break;
            }

            return msg;
        }

    } // end of class
} // end of namespace
