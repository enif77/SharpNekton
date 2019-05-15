/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsFalseOpCode : AOpCode{
    public IsFalseOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_IS_FALSE;
    }


    public override string ToString() 
    {
      return "is_false";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand is in R
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      if (a.GetBooleanValue() == false) {
        // 'a' is false (has a false value), use 'b'
        ;
      }
      else {
        // 'a' has a true value (true == (bool) 'a'), move it to r
        // (so the value of the 'a' will be used)
        ev.RegR = a;
      }
    }
    
  } // end of class
} // end of namespace
