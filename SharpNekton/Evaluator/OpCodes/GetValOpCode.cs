/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class GetValOpCode : AOpCode {
    public GetValOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_GETVAL;
    }


    public override string ToString() 
    {
      return "getval";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.GetVal();
    }

  } // end of class
} // end fo namespace
