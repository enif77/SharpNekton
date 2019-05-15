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
using System.Collections;
using SharpNekton.Compiler.Sources;
using SharpNekton.Shared;

namespace SharpNekton.Compiler
{

    /*
   * Tokenizer.java
   *
   * Created on 19. listopad 2007, 14:01
   *
   * To change this template, choose Tools | Template Manager
   * and open the template in the editor.
   */
    public class Tokenizer
    {
        public const int EOF = -1;

        private ASource source;
        private int look;
        private Hashtable keywords;
        private Token token;

        /*------------------------------------------------------------------------*/

        /** Creates a new instance of Tokenizer */
        public Tokenizer()
        {
            InitKeywords();
            Source = new StringSource("");
        }

        /*------------------------------------------------------------------------*/

        public Tokenizer(ASource source)
        {
            InitKeywords();
            Source = source;
        }

        /*------------------------------------------------------------------------*/

        /** Initializes the keywords table.
         *
         * Inserts all keywords defined by Nekton language into the internal
         * hashtable.
         *
         * */
        private void InitKeywords()
        {
            keywords = new Hashtable(40); // 40 = number of items in the new hashtable

            keywords.Add("and", TokenID.T_KEY_AND);
            keywords.Add("arg", TokenID.T_KEY_ARG);
            keywords.Add("as", TokenID.T_KEY_AS);
            keywords.Add("boolean", TokenID.T_KEY_BOOLEAN);
            keywords.Add("break", TokenID.T_KEY_BREAK);
            keywords.Add("but", TokenID.T_KEY_BUT);
            keywords.Add("continue", TokenID.T_KEY_CONTINUE);
            keywords.Add("div", TokenID.T_KEY_DIV);
            keywords.Add("do", TokenID.T_KEY_DO);
            keywords.Add("else", TokenID.T_KEY_ELSE);
            keywords.Add("end", TokenID.T_KEY_END);
            keywords.Add("eval", TokenID.T_KEY_EVAL);
            keywords.Add("exit", TokenID.T_KEY_EXIT);
            keywords.Add("false", TokenID.T_KEY_FALSE);
            keywords.Add("for", TokenID.T_KEY_FOR);
            keywords.Add("foreach", TokenID.T_KEY_FOREACH);
            keywords.Add("function", TokenID.T_KEY_FUNCTION);
            keywords.Add("if", TokenID.T_KEY_IF);
            keywords.Add("import", TokenID.T_KEY_IMPORT);
            keywords.Add("in", TokenID.T_KEY_IN);
            keywords.Add("include", TokenID.T_KEY_INCLUDE);
            keywords.Add("local", TokenID.T_KEY_LOCAL);
            keywords.Add("null", TokenID.T_KEY_NULL);
            keywords.Add("number", TokenID.T_KEY_NUMBER);
            keywords.Add("object", TokenID.T_KEY_OBJECT);
            keywords.Add("or", TokenID.T_KEY_OR);
            keywords.Add("print", TokenID.T_KEY_PRINT);
            keywords.Add("return", TokenID.T_KEY_RETURN);
            keywords.Add("sizeof", TokenID.T_KEY_SIZEOF);
            keywords.Add("string", TokenID.T_KEY_STRING);
            keywords.Add("true", TokenID.T_KEY_TRUE);
            keywords.Add("typeof", TokenID.T_KEY_TYPEOF);
            keywords.Add("undefined", TokenID.T_KEY_UNDEFINED);
            keywords.Add("while", TokenID.T_KEY_WHILE);
        }

        /*------------------------------------------------------------------------*/

        public ASource Source
        {
            get
            {
                return source;
            }

            set
            {
                if (value != null)
                {
                    source = value;
                }
                else
                {
                    source = new StringSource("");
                }

                token = new Token(0, 0);
                NextChar();
                NextToken();
            }
        }

        /*------------------------------------------------------------------------*/

        /** Defines white-space characters recognized by the language.
         *
         * <pre>
         * white-space :: ' ' | '\t' | '\x0B' | '\f' | '\xA0' | '\r' | '\\n' .
         * </pre>
         *
         * <p>Note: 0xA0 = no-break space (NBSP, see ECMAScript 3.0)<br />
         * Note: 0x0B = vercical tab (VT, '\v')</p>
         *
         * @param c An ASCII representation of the character to be tested.
         *
         * @return Returns true, if the given character is a white-space. If it
         * is not a white-space, returns false.
         *
         * */
        public static bool IsWhite(int c)
        {
            //if (c == ' ' || c == '\t' || c == '\v' || c == '\f' || c == 0xA0 || c == '\r' || c == '\n')
            if (c == ' ' || c == '\t' || c == 0x0B || c == '\f' || c == 0xA0 || c == '\r' || c == '\n')
                return true;
            else
                return false;
        }

        /*------------------------------------------------------------------------*/

        /** Defines digit characters recognized by the language.
         *
         * <pre>
         * digit ::
         *   '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .
         * </pre>
         *
         * @param c An ASCII representation of the character to be tested.
         *
         * @return Returns true, if the given character is a digit. If it
         * is not a digit, returns false.
         *
         * */
        public static bool IsDigit(int c)
        {
            if (c >= '0' && c <= '9')
                return true;
            else
                return false;
        }

        /*------------------------------------------------------------------------*/

        /** Defines hexadecimal-digit characters recognized by the language.
         *
         * <pre>
         * hexadecimal-digit ::
         *   digit |
         *   'a' | 'b' | 'c' | 'd' | 'e' | 'f' |
         *   'A' | 'B' | 'C' | 'D' | 'E' | 'F' .
         * </pre>
         *
         * @param c An ASCII representation of the character to be tested.
         *
         * @return Returns true, if the given character is a hexadecimal-digit. If it
         * is not a hexadecimal-digit, returns false.
         *
         * */
        public static bool IsHexDigit(int c)
        {
            if ((c >= '0' && c <= '9') ||
                 (c >= 'a' && c <= 'f') ||
                 (c >= 'A' && c <= 'F')
               )
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------*/

        /** Defines non-digit characters recognized by the language.
        *
        * <pre>
        * non-digit ::
        *   '_' | '$' |
        *   'a' .. 'z' | 'A' .. 'Z' .
        * </pre>
        *
        * @param c An ASCII representation of the character to be tested.
        *
        * @return Returns true, if the given character is a non-digit. If it
        * is not a non-digit, returns false. For example, identifiers
        * begins and can contain non-digits (and digits also).
        *
        * */
        public static bool IsNonDigit(int c)
        {
            if ((c >= 'a' && c <= 'z') ||
                 (c >= 'A' && c <= 'Z') ||
                 (c == '_' || c == '$'))
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        /*------------------------------------------------------------------------*/

        /** Tests, if the given identifier is or is not a keyword.
        *
        * If the identifier represents a keyword, new token with correct T_KEY_* token
        * ID is generated. Nothing happens if it is an ordinary identifier.
        *
        * */
        private void TestForKeyword()
        {
            if (token.TokID == TokenID.T_IDENTIFIER)
            {
                if (keywords.ContainsKey(token.SVal))
                {
                    token = new Token(token.Line, token.LinePosition, (TokenID)keywords[token.SVal]);
                }
            }
        }

        /*------------------------------------------------------------------------*/

        /** Parses a numeric constant.
         *
         * A numeric constant is defined as:
         *
         * <pre>
         * number :: [ sign ] digit-sequence [ fractional-part ] [ scale-factor ] .
         * fractional-part :: '.' digit-sequence .
         * scale-factor :: ( 'e' | 'E' ) [ scale-factor-sign ]  digit-sequence .
         * digit-sequence :: digit { digit } .
         * scale-factor-sign :: sign
         * sign :: '+' | '-' .
         * digit :: '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .
         * </pre>
         *
         * @throws Throws an JNektonException with one of the errorcodes included:
         * E_EXPDIGSEQ, E_EXPFRACP, E_EXPSCALEF, if a malformated numeric constant is found.
         *
         * */
        private void ParseNumericLiteral()
        {
            double n, sign;
            int line = source.LineNumber;
            int linePosition = source.LinePosition;

            // skip starting whites
            while (IsWhite(look) == true) NextChar();

            // [ sign ]
            sign = 1.0;
            if (look == '-')
            {
                sign = -1.0;             // set sign
                NextChar();               // eat '-'
            }
            else if (look == '+')
            {
                NextChar();               // eat '+'
            }

            // digit-sequence expected
            if (IsDigit(look) == false)
            {
                token = new Token(line, linePosition, 0.0);

                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPDIGSEQ));
            }

            // integer-part
            n = 0.0;
            while (IsDigit(look) == true)
            {
                n *= 10.0;
                n += (double)(look - '0');
                NextChar();
            }

            // [ fractional-part ]
            if (look == '.')
            {
                double frac, frac_scale;

                NextChar();      // eat '.'

                // digit-sequence expected
                if (IsDigit(look) == false)
                {
                    token = new Token(line, linePosition, n);

                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPFRACP));
                }

                // read the fractional-part
                frac = 0.0;
                frac_scale = 0.1;
                while (IsDigit(look) == true)
                {
                    frac += frac_scale * ((double)(look - '0'));
                    frac_scale *= 0.1;
                    NextChar();
                }

                // add fractional-part to the real part
                n += frac;
            }

            // [ scale-factor ]
            if (look == 'e' || look == 'E')
            {
                double sf, sfsign;

                NextChar();      // eat the 'e' or the 'E'

                // [ scale-factor-sign ]
                sfsign = 1.0;
                if (look == '-')
                {
                    sfsign = -1.0;             // set sign
                    NextChar();                 // eat '-'
                }
                else if (look == '+')
                {
                    NextChar();                 // eat '+'
                }

                // digit-sequence expected
                if (IsDigit(look) == false)
                {
                    token = new Token(line, linePosition, n);

                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPSCALEF));
                }

                // read the scale-factor
                sf = 0.0;
                while (IsDigit(look) == true)
                {
                    sf *= 10.0;
                    sf += (double)(look - '0');
                    NextChar();
                }

                // aplly a scale-factor-sign to the scale-factor
                sf *= sfsign;

                // apply the scale-factor to the n
                n *= Math.Pow(10.0, sf);
            }

            // aply the sign to the number
            n *= sign;

            // return the parsed number
            token = new Token(line, linePosition, n);
        }

        /*------------------------------------------------------------------------*/

        /** Converts an ASCII character to it's upper-case representation.
         *
         * @param c The ASCII representation of the character to be converted.
         *
         * @return Returns upper-case version of the character.
         *
         *  */
        private int UpCase(int c)
        {
            return (c >= 'a' && c <= 'z') ? (c - 32) : c;
        }

        /*------------------------------------------------------------------------*/

        /** Parses a special (or escape) characters defined for double-quoted string literals.
        *
        * Special characters defined for double-quoted string literals:
        *
        * <pre>
        * \0, \b, \t, \n, \v, \f, \r, \", \', \\, \x00, \X00.
        * </pre>
        *
        * Special characters undefined by te language specification are reported by the E_BADSPECCHR
        * error code. Parsing starts with the '\' character in the look field.
        *
        * */
        private string ParseStringSpecialChar()
        {
            int num = 0, d = 0;
            string chars = "";

            NextChar();  // eat '\'

            switch (look)
            {
                case '0': chars += "\0"; break;
                case 'b': chars += (char)0x08; break;
                case 't': chars += (char)0x09; break;
                case 'n': chars += (char)0x0A; break;
                case 'v': chars += (char)0x0B; break;
                case 'f': chars += (char)0x0C; break;
                case 'r': chars += (char)0x0D; break;
                case '\\': chars += '\\'; break;
                case '\"': chars += '\"'; break;
                case '\'': chars += '\''; break;
                case 'x':
                case 'X':
                    NextChar();
                    if (IsDigit(look))
                        d = look - '0';
                    else if (IsHexDigit(look))
                        d = UpCase(look) - 'A' + 10;
                    else
                    {
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPHEXSTR));
                    }

                    num = 16 * d;

                    NextChar();
                    if (IsDigit(look))
                        d = look - '0';
                    else if (IsHexDigit(look))
                        d = UpCase(look) - 'A' + 10;
                    else
                    {
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPHEXSTR));
                    }

                    num += d;

                    chars += (char)num;
                    break;

                default:
                    if (look == EOF)
                    {
                        // unfinished string
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPSTREND));
                    }
                    else
                    {
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADSPECCHR));
                    }
            }

            return chars;
        }

        /*------------------------------------------------------------------------*/

        /** Parses a double-quoted string literal.
        *
        * A double-quoted string literal is defined as:
        *
        * <pre>
        * string-literal :: '"' { character | escape-sequence } '"' .
        * escape-sequence :: '\' escape-sequence-command .
        * escape-sequence-command ::
        *   '0' | 'b' | 'f' | 'n' | 'r' | 't' | '\' | '"' | ''' | hexadecimal-character-spec .
        * hexadecimal-character-spec :: ( 'x' | 'X' ) hexadecimal-digit hexadecimal-digit .
        * hexadecimal-digit :: digit | 'a' .. 'f' | 'A' .. 'F' .
        * digit :: '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .
        * </pre>
        *
        * */
        private void ParseStringLiteral()
        {
            string str = "";
            int line = source.LineNumber;
            int linePosition = source.LinePosition;

            NextChar();
            while (look != EOF)
            {
                if (look == '\"') break;

                if (look == '\\')
                {
                    str += ParseStringSpecialChar();
                }
                else
                {
                    str += (char)look;
                }

                NextChar();
            }

            if (look == '\"')
            {
                NextChar();   // eat "
                token = new Token(line, linePosition, str);
            }
            else
            {
                token = new Token(line, linePosition);

                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPSTREND));
            }
        }

        /*------------------------------------------------------------------------*/

        /** Parses a single-quoted (raw) string literal.
        *
        * A single-quoted raw string literal is defined as:
        *
        * <pre>
        * raw-string-literal :: ''' { character | raw-string-escape-sequence } ''' .
        * raw-string-escape-sequence :: '\' raw-string-escape-sequence-command .
        * raw-string-escape-sequence-command :: '\' | ''' .
        * </pre>
        *
        * */
        private void ParseRawStringLiteral()
        {
            string str = "";
            int line = source.LineNumber;
            int linePosition = source.LinePosition;

            NextChar();  // eat starting '''
            while (look != EOF)
            {
                // end of string
                if (look == '\'') break;

                if (look == '\\')
                {
                    NextChar();            // read the next char
                    if (look == '\\')
                    {    // "\\"
                        str += (char)'\\';
                        NextChar();
                    }
                    else if (look == '\'')
                    {  // "\'"
                        str += (char)'\'';
                        NextChar();
                    }
                    else
                    {
                        str += (char)'\\';
                    }
                }
                else
                {
                    str += (char)look;
                    NextChar();             // read a next char
                }
            }

            if (look == '\'')
            {
                NextChar();
                token = new Token(line, linePosition, str);
            }
            else if (look == EOF)
            {
                token = new Token(line, linePosition);
                // unfinished string
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPSTREND));
            }
            else
            {
                token = new Token(line, linePosition);
                // something is wrong here...
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));
            }
        }

        /*------------------------------------------------------------------------*/

        /** Parses an identifier.
        *
        * An identifier is defined as:
        *
        * <pre>
        * identifier :: nondigit { ( nondigit | digit ) } .
        * nondigit :: 'a' .. 'z' | 'A' .. 'Z' | '_' | '$' .
        * digit :: '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .
        * </pre>
        *
        * */
        private void ParseIdentifier()
        {
            string ident = "";
            int line = source.LineNumber;
            int linePosition = source.LinePosition;

            while (IsNonDigit(look) == true || IsDigit(look) == true)
            {
                ident += (char)look;
                NextChar();
            }

            token = new Token(line, linePosition, TokenID.T_IDENTIFIER, ident);
        }

        /*------------------------------------------------------------------------*/

        /** Reads and skips a simple-comment.
        *
        * Skips all simple-comment's characters between '//'
        * and the nearest end of line character ('\n') or EOF:
        *
        * <pre>
        * simple-comment :: '//' { character } ( EOLN | EOF )
        * EOLN :: end-of-line-character
        * EOF :: end-of-file-character
        * end-of-line-character :: '\n'
        * </pre>
        *
        * */
        private void SkipSimpleComment()
        {
            while (look != EOF)
            {      // skip a comment
                if (look == '\n') break;
                NextChar();
            }

            NextChar();    // eat '\n'
        }

        /*------------------------------------------------------------------------*/

        /** Reads and skips multiline-comment.
        *
        * Skips all comment's characters between the '/' '*'
        * and the '*' '/' characters:
        *
        * <pre>
        * comment :: '/' '*' { ( character | comment ) } '*' '/'
        * </pre>
        *
        * Note: Comments can be nested.
        *
        * */
        private void SkipComment()
        {
            int clevel = 0;
            int line = source.LineNumber;
            int linePosition = source.LinePosition;

            while (look != EOF)
            {      // skip a comment
                if (look == '*')
                {
                    NextChar();            // eat '*'
                    if (look == '/')
                    {     // '*/'
                        if (clevel == 0)
                        {
                            break;
                        }
                        else
                        {
                            clevel--;
                            if (clevel < 0)
                            {
                                token = new Token(line, linePosition);
                                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_UNBLCOM));
                            }
                        }
                    }
                }
                else if (look == '/')
                {
                    NextChar();            // eat '/'
                    if (look == '*')
                    {     // '/' '*'
                        NextChar();          // eat '*'
                        clevel++;
                    }
                }
                else
                {
                    NextChar();
                }
            }

            if (look == EOF)
            {
                token = new Token(line, linePosition);
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_UNBLCOM));
            }

            NextChar(); // eat closing '/'
        }

        /*------------------------------------------------------------------------*/

        /** Reads one character from the source.
         *
         * The character is stored in the field look. If there is no source, or the
         * end of input was reached, look is set to EOF.
         *
         * */
        private void NextChar()
        {
            look = source.Read();
        }

        /*------------------------------------------------------------------------*/

        /** Returns the last red token.
         *
         * @return Returns the last red token.
         *
         *  */
        public Token GetLastToken()
        {
            return token;
        }

        /*------------------------------------------------------------------------*/

        /** Reads an input and separates individual tokens from it.
        *
        * Main tokenizer routine...
        *
        * */
        public void NextToken()
        {
        // a go-to statement replacement (see the continue statements below)
        newscan:

            while (IsWhite(look) == true) NextChar();

            if (IsDigit(look) == true)
            {
                ParseNumericLiteral();
            }
            else if (IsNonDigit(look) == true)
            {
                ParseIdentifier();
                TestForKeyword();
            }
            else
            {
                int line = source.LineNumber;
                int linePosition = source.LinePosition;

                switch (look)
                {
                    case '\"':
                        ParseStringLiteral();
                        break;

                    case '\'':
                        ParseRawStringLiteral();
                        break;

                    case '+':
                        NextChar();
                        if (look == '+')
                        {
                            token = new Token(line, linePosition, TokenID.T_INCR_OP);  // '++'
                            NextChar();
                        }
                        else if (look == '=')
                        { // +=
                            token = new Token(line, linePosition, TokenID.T_ADD_ASSIGN_OP);  // '+='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_ADD_OP);  // '+'
                        }
                        break;

                    case '-':
                        NextChar();
                        if (look == '-')
                        {
                            token = new Token(line, linePosition, TokenID.T_DECR_OP);  // '--'
                            NextChar();
                        }
                        else if (look == '=')
                        { // -=
                            token = new Token(line, linePosition, TokenID.T_SUB_ASSIGN_OP);  // '-='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_SUB_OP);  // '-'
                        }
                        break;

                    case '*':
                        NextChar();
                        if (look == '/')
                        {    // */
                            token = new Token(line, linePosition);
                            throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_UNBLCOM));
                        }
                        else if (look == '*')
                        { // **
                            NextChar();
                            if (look == '=')
                            {    // **=
                                token = new Token(line, linePosition, TokenID.T_POW_ASSIGN_OP);  // '**='
                                NextChar();
                            }
                            else
                            {
                                token = new Token(line, linePosition, TokenID.T_POW_OP);  // '**'
                            }
                        }
                        else if (look == '=')
                        { // *=
                            token = new Token(line, linePosition, TokenID.T_MUL_ASSIGN_OP);  // '*='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_MUL_OP);  // '*'
                        }
                        break;

                    case '/':
                        NextChar();
                        if (look == '/')
                        {           // '//'
                            NextChar();                // eat second '/'
                            SkipSimpleComment();
                            goto newscan;              // comment skipped, continue with scanning
                        }
                        else if (look == '*')
                        {      // '/' '*'
                            NextChar();                // eat '*'
                            SkipComment();             // skip comment
                            goto newscan;              // comment skipped, continue with scanning
                        }
                        else if (look == '=')
                        { // /=
                            token = new Token(line, linePosition, TokenID.T_DIV_ASSIGN_OP);  // '/='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_DIV_OP);  // '/'
                        }
                        break;

                    case '%':
                        NextChar();
                        if (look == '=')
                        {       // %=
                            token = new Token(line, linePosition, TokenID.T_MOD_ASSIGN_OP);  // '%='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_MOD_OP);  // '%'
                        }
                        break;

                    case '(': token = new Token(line, linePosition, TokenID.T_LPAREN); NextChar(); break;
                    case ')': token = new Token(line, linePosition, TokenID.T_RPAREN); NextChar(); break;

                    case '[': token = new Token(line, linePosition, TokenID.T_LBRAC); NextChar(); break;
                    case ']': token = new Token(line, linePosition, TokenID.T_RBRAC); NextChar(); break;

                    case '{': token = new Token(line, linePosition, TokenID.T_BLOCK_START); NextChar(); break;
                    case '}': token = new Token(line, linePosition, TokenID.T_BLOCK_END); NextChar(); break;

                    case '=':
                        NextChar();
                        if (look == '=')
                        {
                            NextChar();
                            if (look == '=')
                            {     // '==='
                                token = new Token(line, linePosition, TokenID.T_IS_SAME_OP);  // '==='
                                NextChar();
                            }
                            else
                            {
                                token = new Token(line, linePosition, TokenID.T_IS_EQUAL_OP);  // '=='
                            }
                        }
                        else if (look == '>')
                        {   // '=>'
                            token = new Token(line, linePosition, TokenID.T_DATAASGN_OP);  // '=>'
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_ASSIGN_OP);  // '='
                        }
                        break;

                    case '!':
                        NextChar();
                        if (look == '=')
                        {
                            NextChar();
                            if (look == '=')
                            {      // '!=='
                                token = new Token(line, linePosition, TokenID.T_IS_NOT_SAME_OP);  // '!=='
                                NextChar();
                            }
                            else
                            {
                                token = new Token(line, linePosition, TokenID.T_IS_NOT_EQUAL_OP);  // '!='
                            }
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_NOT_OP);  // '!'
                        }
                        break;

                    case '|':
                        NextChar();
                        if (look == '|')
                        {
                            token = new Token(line, linePosition, TokenID.T_OR_OP);  // '||'
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition);
                            throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNKNOWN_TOKEN));
                        }
                        break;

                    case '&':
                        NextChar();
                        if (look == '&')
                        {
                            token = new Token(line, linePosition, TokenID.T_AND_OP);  // '&&'
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_COPYDATA_OP);  // '&'
                        }
                        break;

                    case '<':
                        NextChar();
                        if (look == '=')
                        {
                            token = new Token(line, linePosition, TokenID.T_IS_SMALLER_OR_EQUAL_OP);  // '<='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_IS_SMALLER_OP);  // '<'
                        }
                        break;

                    case '>':
                        NextChar();
                        if (look == '=')
                        {
                            token = new Token(line, linePosition, TokenID.T_IS_GREATHER_OR_EQUAL_OP);  // '>='
                            NextChar();
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_IS_GREATHER_OP);  // '>'
                        }
                        break;

                    case '.':
                        NextChar();
                        if (look == '.')
                        {
                            NextChar();

                            if (look == '.')
                            {
                                token = new Token(line, linePosition, TokenID.T_ELLIPSIS);  // '...'
                                NextChar();
                            }
                            else if (look == '=')
                            {
                                token = new Token(line, linePosition, TokenID.T_STRCAT_ASSIGN_OP);  // '..='
                                NextChar();
                            }
                            else
                            {
                                token = new Token(line, linePosition, TokenID.T_STRCAT_OP);  // '..'
                            }
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_DOT_OP);  // '.'
                        }
                        break;

                    case '?':
                        NextChar();
                        if (look == '?')
                        {
                            NextChar();
                            token = new Token(line, linePosition, TokenID.T_HAS_VALUE_OP);  // '??'
                        }
                        else if (look == '!')
                        {
                            NextChar();
                            token = new Token(line, linePosition, TokenID.T_IS_FALSE_OP);  // '?!'
                        }
                        else
                        {
                            token = new Token(line, linePosition, TokenID.T_TER_ASK_OP);  // '?'
                        }
                        break;   // '?'

                    case ':': token = new Token(line, linePosition, TokenID.T_COLON); NextChar(); break;
                    case ',': token = new Token(line, linePosition, TokenID.T_COMMA); NextChar(); break;
                    case ';': token = new Token(line, linePosition, TokenID.T_STAT_END); NextChar(); break;

                    case EOF:
                        token = new Token(line, linePosition, TokenID.T_EOF);
                        break;

                    default:
                        token = new Token(line, linePosition);  // report error here
                        throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNKNOWN_TOKEN));
                }
            }

            //Console.WriteLine(">> tok: {0}", token.ToString());
        }

    } // end of class
} // end of namespace
