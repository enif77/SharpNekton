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

namespace SharpNekton.Evaluator.OpCodes
{

  public enum OpCodeID {
    O_NOP,           // no operation; jump target
    O_END,           // end of program
    O_SUBEND,        // end of subprogram
    O_NEG,           // R = -R;
    O_ADD,           // R = T + R;
    O_STRCAT,        // R = (string) T + (string) R
    O_SUB,           // R = T - R;
    O_MUL,           // R = T * R;
    O_DIV,           // R = T / R;
    O_DIVI,          // R = T div R;
    O_MOD,           // R = T % R;
    O_POW,           // R = T ** R;
    O_NOT,           // boolean not, and, or
    O_AND,
    O_OR,
    O_IS_EQUAL,             // ==
    O_IS_NOT_EQUAL,         // !=
    O_IS_SAME,              // ===
    O_IS_NOT_SAME,          // !==
    O_IS_SMALLER,           // <
    O_IS_SMALLER_OR_EQUAL,  // <=
    O_IS_GREATHER,          // >
    O_IS_GREATHER_OR_EQUAL, // >=
    O_IS_IN,                // key in table
    O_HAS_VALUE,     // ??
    O_IS_FALSE,      // ?!
    O_PREDECR,       // --a
    O_PREINCR,       // ++a
    O_POSTDECR,      // a--
    O_POSTINCR,      // a++
    O_GETVAL,        // R = e_GetVal(R);

    // loaders
    O_LOADB,         // load boolean
    O_LOADN,         // R = param.dval;  -> double number
    O_LOADS,         // R = param.str;   -> string
    O_LOADI,         // T = pop(); R = T[R]; -> mark R as an index to indexable value
    O_LOADUNDEFINED,
    O_LOADNULL,
    O_LOADSTOR,      // load store reference (param.ptr)
    O_LOADFP,        // load offset to FP
    O_LOADF,         // load function ptr

    // function ops
    O_PUSH,          // push(R);
    O_PUSHP,         // push(R); -> push subroutine parameter, unrefereced
    O_POPN,          // remove n items from the stack (used for removing sub params)
    O_STORE,         // *SP[top] = R; pop();
    O_JUMP,          // goto param.lval;   -> index to code[]
    O_JUMP_IF_FALSE, // if (R == 0) goto param.lval;
    O_JUMP_IF_TRUE,  // if (R != 0) goto param.lval;
    O_JSR,
    O_RTS,
    O_LINK,
    O_UNLNK,
    O_GETARG,

    O_NEWTABLE,           // {}
    O_BEGIN_TABLEDATA,    // push data; data = newarray(size); index = 0
    O_APPEND_TABLEDATA,   // data[index] = R; index++; R = NULL
    O_APPEND_TABLEDATA_AUTOKEY, // generates a integer key automaticaly
    O_END_TABLEDATA,      // R = data; data = pop();

    // foreach loop
    O_FSTORE,
    O_REWIND,
    O_NEXT,
    O_NEXT_KEY,
    O_JUMP_IF_DONE,  // all done = TYPE_DONE

    // special commands
    O_PRINT,         // print ... ;
    O_COPYDATA,      // @table
    O_TYPEOF,        // typeof()
    O_SIZEOF,        // sizeof()
    O_TO_BOOLEAN,    // (boolean)
    O_TO_NUMBER,     // (number)
    O_TO_STRING,     // (string)
    O_TO_OBJECT,     // (object)

    // include and import commands
    O_INCLUDE,
    O_IMPORT,
    O_EVAL,
  };


  public abstract class AOpCode {
    protected OpCodeID opCodeID;
    protected int line;
    protected int linePosition;

    public AOpCode(int line, int linePosition) {
      opCodeID = OpCodeID.O_NOP;
      this.line = line;
      this.linePosition = linePosition;
    }


    public int Line {
      get {
        return line;
      }
    }


    public int LinePosition {
      get {
        return linePosition;
      }
    }


    public abstract override string ToString();
    public abstract void Eval(EvaluatorState ev);

  } // end of class
} // end of namespace
