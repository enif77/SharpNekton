/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator;
using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes 
{

  class SizeOfOpCode : AOpCode {
    public SizeOfOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_SIZEOF;
    }


    public override string ToString() 
    {
      return "sizeof";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      ev.RegR = new NumericValue( a.SizeOf() );
    }
    
  } // end of class
} // end of namespace
