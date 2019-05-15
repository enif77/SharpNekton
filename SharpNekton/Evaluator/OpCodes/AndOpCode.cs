/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class AndOpCode : AOpCode {
    public AndOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_AND;
    }


    public override string ToString() 
    {
      return "and";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand is in R
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      ev.RegR = new BooleanValue( a.GetBooleanValue() && b.GetBooleanValue() );
    }

  } // end of class
} // end of namespace
