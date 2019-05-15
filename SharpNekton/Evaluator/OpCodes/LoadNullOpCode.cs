/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes 
{

  class LoadNullOpCode : AOpCode {
    public LoadNullOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_LOADNULL;
    }


    public override string ToString() 
    {
      return "loadnull";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new NullValue();
    }
    
  } // end of class
} // end of namespace
