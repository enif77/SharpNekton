/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadBOpCode : ABooleanOpCode {
    public LoadBOpCode(int line, int linePosition, bool parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_LOADB;
    }


    public override string ToString() 
    {
      return "loadb " + Parameter;
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new BooleanValue(parameter);
    }
    
  } // end of class
} // end of namespace
