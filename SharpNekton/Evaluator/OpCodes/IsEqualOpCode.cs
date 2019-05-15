/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator;
using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsEqualOpCode : AOpCode {
    public IsEqualOpCode(int line, int linePosition) : base(line, linePosition)  
    {
      opCodeID = OpCodeID.O_IS_EQUAL;
    }


    public override string ToString() 
    {
      return "is_equal";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      bool result = false;

      // second operand is in R
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      if (a.TypeOf() == ValueTypeID.TYPE_STRING || b.TypeOf() == ValueTypeID.TYPE_STRING) {
        if ( String.Compare(a.GetStringValue(), b.GetStringValue()) == 0) {
          result = true;
        }
        else {
          result = false;
        }
      }
      else if (a.TypeOf() == ValueTypeID.TYPE_TABLEREF && b.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        result = a.SizeOf() == b.SizeOf();
      }
      else {
        if (a.GetNumericValue() == b.GetNumericValue()) {
          result = true;
        }
        else {
          result = false;
        }
      }

      ev.RegR = new BooleanValue( result );
    }
  } // end of class

}