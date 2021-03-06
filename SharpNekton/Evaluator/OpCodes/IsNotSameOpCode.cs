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
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsNotSameOpCode : AOpCode {
    public IsNotSameOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_IS_NOT_SAME;
    }


    public override string ToString() 
    {
      return "is_not_same";
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

      if (a.TypeOf() == b.TypeOf()) {
        switch (a.TypeOf()) {
        case ValueTypeID.TYPE_UNDEFINED :
          result = true;
          break;

        case ValueTypeID.TYPE_NULL :
          result = true;
          break;

        case ValueTypeID.TYPE_BOOLEAN :
          result = a.GetBooleanValue() == b.GetBooleanValue();
          break;

        case ValueTypeID.TYPE_NUMBER :
          result = a.GetNumericValue() == b.GetNumericValue();
          break;

        case ValueTypeID.TYPE_STRING :
          result = String.Compare(a.GetStringValue(), b.GetStringValue()) == 0;
          break;

        case ValueTypeID.TYPE_TABLEREF :
          result = a.GetObjectValue() == b.GetObjectValue();
          break;

        default :
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADTYPE) );
        }
      }
      else {
        result = false;
      }

      ev.RegR = new BooleanValue( !result );
    }
    
  } // end of class
} // end of namespace
