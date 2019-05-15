/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class ModOpCode : AOpCode {
    public ModOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_MOD;
    }


    public override string ToString() 
    {
      return "mod";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue b = ev.GetVal();
      int bi = b.GetIntValue();

      // do not divide by zero!
      if (bi == 0) throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_DIVZERO) );

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a
      int ai = a.GetIntValue();

      ev.RegR = new NumericValue( (double) (ai % bi) );
    }
    
  } // end of class
} // end of namespace
