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
using SharpNekton.Compiler;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes 
{

  class StoreOpCode : AOpCode {
    private TokenID variation;

    public StoreOpCode(int line, int linePosition, TokenID variation) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_STORE;
      this.variation = variation;
    }


    public override string ToString() 
    {
      return "store " + variation;
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand (source)
      ev.GetVal();

      // first operand (destination)
      IValue storeValue = ev.Stack.ReadTop();
      ev.Stack.Pop();  // pop destination or index

      if (storeValue.TypeOf() == ValueTypeID.TYPE_STOREREF) {
        DoStoreOp((ValueStore) storeValue.GetObjectValue(), ev.RegR, variation);
      }
      else if (storeValue.TypeOf() == ValueTypeID.TYPE_TABLEDATAREF) {
        ValueTableItem vti = (ValueTableItem) storeValue.GetObjectValue();

        if (ev.RegR.TypeOf() == ValueTypeID.TYPE_UNDEFINED) {
          ValueTable table = vti.Parent;
          table.Delete( vti.Key );
        }
        else {
          DoTableOp(vti, ev.RegR, variation);
        }
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPESTOREREF) );
      }
    }


    private void DoStoreOp(ValueStore dest, IValue b, TokenID variation)
    {
      dest.Value = DoOperation(dest.Value, b, variation);
    }


    private void DoTableOp(ValueTableItem dest, IValue b, TokenID variation)
    {
      dest.Value = DoOperation(dest.Value, b, variation);
    }


    private IValue DoOperation(IValue a, IValue b, TokenID variation)
    {
      switch (variation) {
      case TokenID.T_ASSIGN_OP :  // a = b
        return b;
     
      case TokenID.T_ADD_ASSIGN_OP :  // a += b
        // is it a numeric operation or a string concatenation?
        if (a.TypeOf() != ValueTypeID.TYPE_STRING && b.TypeOf() != ValueTypeID.TYPE_STRING) {
          return new NumericValue( a.GetNumericValue() + b.GetNumericValue() );
        }
        else {
          return new StringValue( a.GetStringValue() + b.GetStringValue() );
        }

      case TokenID.T_SUB_ASSIGN_OP :  // a -= b
        return new NumericValue( a.GetNumericValue() - b.GetNumericValue() );

      case TokenID.T_MUL_ASSIGN_OP :  // a *= b
        return new NumericValue( a.GetNumericValue() * b.GetNumericValue() );

      case TokenID.T_DIV_ASSIGN_OP : // a /= b (allways real-number op.)
        double bn = b.GetNumericValue();
        // do not divide by zero!
        if (bn == 0.0) throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_DIVZERO) );
        return new NumericValue( a.GetNumericValue() / bn );

      case TokenID.T_MOD_ASSIGN_OP :  // a %= b
        int bi = b.GetIntValue();
        // do not divide by zero!
        if (bi == 0) throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_DIVZERO) );
        int ai = a.GetIntValue();
        return new NumericValue( (double) (ai % bi) );

      case TokenID.T_POW_ASSIGN_OP :  // a **= b
        return new NumericValue( Math.Pow (a.GetNumericValue(), b.GetNumericValue()) );

      case TokenID.T_STRCAT_ASSIGN_OP :  // a ..= b
        return new StringValue( a.GetStringValue() + b.GetStringValue() );

      default :
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADSTOREOPERATIONOP) );
      }
    }

  } // end of class
} // end of namespace
