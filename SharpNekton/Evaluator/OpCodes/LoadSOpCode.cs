/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadSOpCode : AStringOpCode {
    public LoadSOpCode(int line, int linePosition, string parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_LOADS;
    }


    public override string ToString() 
    {
      return "loads \"" + parameter + "\"";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());
      
      ev.RegR = new StringValue(parameter);
    }

  } // end of class
} // end of namespace
