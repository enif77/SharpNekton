/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadNOpCode : ANumericOpCode {
    public LoadNOpCode(int line, int linePosition, double numericParameter) : base(line, linePosition, numericParameter) 
    {
      opCodeID = OpCodeID.O_LOADN;
    }


    public override string ToString() 
    {
      return "loadn " + Parameter;
    }


    public override void Eval(EvaluatorState ev)
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new NumericValue(parameter);
    }
    
  } // end of class
} // end of namespace
