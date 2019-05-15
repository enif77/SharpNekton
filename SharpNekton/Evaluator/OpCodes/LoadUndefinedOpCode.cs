/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadUndefinedOpCode : AOpCode {
    public LoadUndefinedOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_LOADUNDEFINED;
    }


    public override string ToString() 
    {
      return "loadundefined";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new UndefinedValue();
    }
    
  } // end of class
} // end of namespace
