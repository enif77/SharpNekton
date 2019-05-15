/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator.Values;


namespace SharpNekton.Evaluator.OpCodes
{

  class PowOpCode : AOpCode {
    public PowOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_POW;
    }


    public override string ToString() 
    {
      return "pow";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue b = ev.GetVal();

      // first operand
      IValue a = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop a

      ev.RegR = new NumericValue( Math.Pow( a.GetNumericValue(), b.GetNumericValue()) );
    }
    
  } // end of class
} // end of namespace
