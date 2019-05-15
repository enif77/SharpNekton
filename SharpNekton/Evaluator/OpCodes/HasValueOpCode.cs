/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class HasValueOpCode : AOpCode {
    public HasValueOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_HAS_VALUE;
    }


    public override string ToString() 
    {
      return "has_value";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand is in R
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      if (a.TypeOf() == ValueTypeID.TYPE_UNDEFINED) {
        // 'a' is undefined (has no value), use r (the 'b'), delete 'a'
        ;  // do nothing here...
      }
      else {
        // 'a' has a value (is not 'undefined'), move it to 'b' (RegR)
        // (so the 'a' will be used)
        ev.RegR = a;
      }
    }

  } // end of class

}
