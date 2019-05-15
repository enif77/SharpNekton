/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class DivOpCode:AOpCode {
    public DivOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_DIV;
    }


    public override string ToString() 
    {
      return "div";
    }


    public override void Eval(EvaluatorState ev)
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue b = ev.GetVal();
      double bn = b.GetNumericValue();

      // do not divide by zero!
      if (bn == 0.0) throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_DIVZERO) );

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      ev.RegR = new NumericValue( a.GetNumericValue() / bn );
    }
  } // end of class

} // end of namespace
