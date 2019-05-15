/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsSameOpCode : AOpCode {
    public IsSameOpCode(int line, int linePosition) : base(line, linePosition)  
    {
      opCodeID = OpCodeID.O_IS_SAME;
    }


    public override string ToString() 
    {
      return "is_same";
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
        switch ( a.TypeOf() ) {
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

      ev.RegR = new BooleanValue( result );
    }
  }

}
