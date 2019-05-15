/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes 
{

  class NegOpCode : AOpCode {
    public NegOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NEG;
    }


    public override string ToString() 
    {
      return "neg";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue a = ev.GetVal();

      ev.RegR = new NumericValue( -a.GetNumericValue() );
    }

  } // end of class
} // end of namespace
