/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class NopOpCode : AOpCode {
    public NopOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NOP;
    }


    public override string ToString() 
    {
      return "nop";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // nothing to be done here
    }

  } // end of class
} // end of class
