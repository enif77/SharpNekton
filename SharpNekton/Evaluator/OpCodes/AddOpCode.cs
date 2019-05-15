/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class AddOpCode:AOpCode {
    public AddOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_ADD;
    }


    public override string ToString() 
    {
      return "add";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand is in R
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      // is it a numeric operation or a string concatenation?
      if (a.TypeOf() != ValueTypeID.TYPE_STRING && b.TypeOf() != ValueTypeID.TYPE_STRING) {
        ev.RegR = new NumericValue( a.GetNumericValue() + b.GetNumericValue() );
      }
      else {
        ev.RegR = new StringValue( a.GetStringValue() + b.GetStringValue() );
      }
    }

  } // end of class
} // end of namespace
