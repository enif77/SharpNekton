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
using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsNotEqualOpCode : AOpCode {
    public IsNotEqualOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_IS_NOT_EQUAL;
    }


    public override string ToString() 
    {
      return "is_not_equal";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      bool result = false;

      // second operand
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      if (a.TypeOf() == ValueTypeID.TYPE_STRING || b.TypeOf() == ValueTypeID.TYPE_STRING) {
        if ( String.Compare(a.GetStringValue(), b.GetStringValue()) != 0) {
          result = true;
        }
        else {
          result = false;
        }
      }
      else if (a.TypeOf() == ValueTypeID.TYPE_TABLEREF && b.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        result = a.SizeOf() != b.SizeOf();
      }
      else {
        if (a.GetNumericValue() != b.GetNumericValue()) {
          result = true;
        }
        else {
          result = false;
        }
      }

      ev.RegR = new BooleanValue( result );
    }
    
  } // end of class
} // end of namespace
