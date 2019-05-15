/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class MulOpCode : AOpCode {
    public MulOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_MUL;
    }


    public override string ToString() 
    {
      return "mul";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      ev.RegR = new NumericValue( a.GetNumericValue() * b.GetNumericValue() );
    }
    
  } // end of class
} // end of namespace
