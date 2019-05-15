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

namespace SharpNekton.Shared
{

    public enum SharpNektonErrorID
    {
        UNSPECIFIED_ERROR,
        OK,

        LIBRARY_REREGISTRATION,
        LIBRARY_NOT_FOUND,

        STACK_OVERFLOW,
        STACK_UNDERFLOW,
        DIVISION_BY_ZERO,

        UNKNOWN_TOKEN,
        E_UNBLCOM,
        E_EXPDIGSEQ,
        E_EXPFRACP,
        E_EXPSCALEF,
        E_EXPSTREND,
        E_EXPHEXSTR,
        E_BADSPECCHR,

        E_BADSIMPLEOP,
        E_BADVAROP,
        E_BADPOINTEROP,
        E_BADSTRINGOP,
        E_BADCOUNTOP,
        E_BADBOOLEANOP,
        E_BADNUMERICOP,
        E_BADOFFSETOP,
        E_BADSTOREPOINTEROP,

        E_EXPTERMOPSEP,
        E_EXPCOMMA,
        E_EXPSTATEND,
        E_EXPLPAREN,
        E_EXPRPAREN,
        E_EXPBLOCKSTART,
        E_EXPBLOCKEND,
        E_EXPRBRAC,
        E_EXPASSIGNOP,
        E_EXPDATAASGNOP,
        E_EXPKEYWHILE,
        E_EXPKEYAS,
        E_EXPEOF,
        E_EXPCMPOP,
        E_EXPOP,
        E_EXPIDENT,
        E_EXPVAL,
        E_EXPEXITVAL,
        E_DONTINDEX,
        E_BADPCOUNT,
        E_ELSEIF,
        E_LOCALSYMREDEF,
        E_BADLOCALSYMLEVEL,
        E_UNSUPCON,
        E_NORET,
        E_NOCONT,
        E_NOBREAK,

        E_NOCODE,
        E_STOVER,
        E_STUNDER,
        E_DIVZERO,
        E_NOGLOBARGS,
        E_BADTYPE,
        E_BADSTOREOPERATIONOP,
        E_BADPARAMCOUNT,
        E_BADPARAMINDEX,
        E_BADJMPTARGET,
        E_BADRUNLEVEL,
        E_BADKEYINFOREACH,
        E_EXPTYPETABLEREF,
        E_EXPTYPESTOREREF,
        E_EXPTYPEFUNCREF,
        E_EXPTYPESTFP,
        E_EXPTYPERTSA,
    }

    /*------------------------------------------------------------------------*/

    public class SharpNektonError
    {
        private SharpNektonErrorID errorID;
        private int line;
        private int linePosition;


        public SharpNektonError()
        {
            errorID = SharpNektonErrorID.UNSPECIFIED_ERROR;
            line = 0;
            linePosition = 0;
        }


        public SharpNektonError(SharpNektonErrorID errorID)
        {
            this.errorID = errorID;
            line = 0;
            linePosition = 0;
        }


        public SharpNektonError(SharpNektonErrorID errorID, int line, int linePosition)
        {
            this.errorID = errorID;
            this.line = line;
            this.linePosition = linePosition;
        }


        public SharpNektonErrorID ErrorID
        {
            get
            {
                return errorID;
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


        public override string ToString()
        {
            string msg;

            switch (this.errorID)
            {
                case SharpNektonErrorID.UNSPECIFIED_ERROR: msg = "Unspecified error."; break;
                case SharpNektonErrorID.OK: msg = "OK"; break;

                case SharpNektonErrorID.LIBRARY_REREGISTRATION: msg = "Library cna not be registered twice."; break;
                case SharpNektonErrorID.LIBRARY_NOT_FOUND: msg = "Library not found."; break;

                // Runtime errors
                case SharpNektonErrorID.STACK_OVERFLOW: msg = "Stack overflow"; break;
                case SharpNektonErrorID.STACK_UNDERFLOW: msg = "Stack underflow"; break;
                case SharpNektonErrorID.DIVISION_BY_ZERO: msg = "Division by zero"; break;

                // Tokenizer's errors
                case SharpNektonErrorID.UNKNOWN_TOKEN: msg = "Unknown token found"; break;
                case SharpNektonErrorID.E_UNBLCOM: msg = "Unballanced comment braces - /* or */ is missing"; break;
                case SharpNektonErrorID.E_EXPDIGSEQ: msg = "Digit sequence expected"; break;
                case SharpNektonErrorID.E_EXPFRACP: msg = "Fractional part expected"; break;
                case SharpNektonErrorID.E_EXPSCALEF: msg = "Scale factor expected"; break;
                case SharpNektonErrorID.E_EXPSTREND: msg = "The end of a string literal expected"; break;
                case SharpNektonErrorID.E_EXPHEXSTR: msg = "A hexadecimal character expected"; break;
                case SharpNektonErrorID.E_BADSPECCHR: msg = "Unsupported special string character"; break;

                // Emiter's errors
                case SharpNektonErrorID.E_BADSIMPLEOP: msg = "Bad simple opcode ID"; break;
                case SharpNektonErrorID.E_BADVAROP: msg = "Bad variation opcode ID"; break;
                case SharpNektonErrorID.E_BADPOINTEROP: msg = "Bad pointer opcode ID"; break;
                case SharpNektonErrorID.E_BADSTRINGOP: msg = "Bad string opcode ID"; break;
                case SharpNektonErrorID.E_BADCOUNTOP: msg = "Bad count opcode ID"; break;
                case SharpNektonErrorID.E_BADBOOLEANOP: msg = "Bad boolean opcode ID"; break;
                case SharpNektonErrorID.E_BADNUMERICOP: msg = "Bad numeric opcode ID"; break;
                case SharpNektonErrorID.E_BADOFFSETOP: msg = "Bad offset opcode ID"; break;
                case SharpNektonErrorID.E_BADSTOREPOINTEROP: msg = "Bad value pointer opcode ID"; break;

                // Compiler's errors
                case SharpNektonErrorID.E_EXPTERMOPSEP: msg = "\':\' expected in conditional expression"; break;
                case SharpNektonErrorID.E_EXPCOMMA: msg = "\',\' expected"; break;
                case SharpNektonErrorID.E_EXPSTATEND: msg = "\';\' expected"; break;
                case SharpNektonErrorID.E_EXPLPAREN: msg = "\'(\' expected"; break;
                case SharpNektonErrorID.E_EXPRPAREN: msg = "\')\' expected"; break;
                case SharpNektonErrorID.E_EXPBLOCKSTART: msg = "\'{\' expected"; break;
                case SharpNektonErrorID.E_EXPBLOCKEND: msg = "\'}\' expected"; break;
                case SharpNektonErrorID.E_EXPRBRAC: msg = "\'[\' expected"; break;
                case SharpNektonErrorID.E_EXPASSIGNOP: msg = "\']\' expected"; break;
                case SharpNektonErrorID.E_EXPDATAASGNOP: msg = "\'=>\' expected"; break;
                case SharpNektonErrorID.E_EXPKEYWHILE: msg = "\"while\" expected"; break;
                case SharpNektonErrorID.E_EXPKEYAS: msg = "\"as\" expected"; break;
                case SharpNektonErrorID.E_EXPEOF: msg = "EOF expected"; break;
                case SharpNektonErrorID.E_EXPCMPOP: msg = "Comparison operator expected."; break;
                case SharpNektonErrorID.E_EXPOP: msg = "Operator expected."; break;
                case SharpNektonErrorID.E_EXPIDENT: msg = "Identifier expected."; break;
                case SharpNektonErrorID.E_EXPVAL: msg = "Value expected."; break;
                case SharpNektonErrorID.E_EXPEXITVAL: msg = "Exit value expected."; break;
                case SharpNektonErrorID.E_DONTINDEX: msg = "Index can not be used here."; break;
                case SharpNektonErrorID.E_BADPCOUNT: msg = "Parameters count does not match previous declaration."; break;
                case SharpNektonErrorID.E_ELSEIF: msg = "Else without if."; break;
                case SharpNektonErrorID.E_LOCALSYMREDEF: msg = "Local symbol redefinition."; break;
                case SharpNektonErrorID.E_BADLOCALSYMLEVEL: msg = "Bad Local symbol level ID."; break;
                case SharpNektonErrorID.E_UNSUPCON: msg = "This construction is unsupported here."; break;
                case SharpNektonErrorID.E_NORET: msg = "No return point."; break;
                case SharpNektonErrorID.E_NOCONT: msg = "No continue point."; break;
                case SharpNektonErrorID.E_NOBREAK: msg = "No break point."; break;

                // Runtime errors
                case SharpNektonErrorID.E_NOCODE: msg = "No code to execute."; break;
                case SharpNektonErrorID.E_STOVER: msg = "Stack overflow."; break;
                case SharpNektonErrorID.E_STUNDER: msg = "Stack underflow."; break;
                case SharpNektonErrorID.E_DIVZERO: msg = "Division by zerro."; break;
                case SharpNektonErrorID.E_NOGLOBARGS: msg = "The arg() operator can not be used on global level."; break;
                case SharpNektonErrorID.E_BADTYPE: msg = "Bad type for the operation."; break;
                case SharpNektonErrorID.E_BADSTOREOPERATIONOP: msg = "Bad operation type for the store opcode."; break;
                case SharpNektonErrorID.E_BADPARAMCOUNT: msg = "The passed number of parameters does not match to the defined number of parameters."; break;
                case SharpNektonErrorID.E_BADPARAMINDEX: msg = "Bad parameter index."; break;
                case SharpNektonErrorID.E_BADJMPTARGET: msg = "Bad jump instruction target."; break;
                case SharpNektonErrorID.E_BADRUNLEVEL: msg = "Bad run level."; break;
                case SharpNektonErrorID.E_BADKEYINFOREACH: msg = "Bad key in foreach loop."; break;
                case SharpNektonErrorID.E_EXPTYPETABLEREF: msg = "A table refference expected."; break;
                case SharpNektonErrorID.E_EXPTYPESTOREREF: msg = "A store refference expected."; break;
                case SharpNektonErrorID.E_EXPTYPEFUNCREF: msg = "A function refference expected."; break;
                case SharpNektonErrorID.E_EXPTYPESTFP: msg = "A STFP expected."; break;
                case SharpNektonErrorID.E_EXPTYPERTSA: msg = "A RTSA expected."; break;

                default: msg = "Unknown error " + this.errorID; break;
            }

            return "[Li:" + this.Line + " Po:" + this.linePosition + "] " + msg;
        }

    } // end of class
} // end of namespace
