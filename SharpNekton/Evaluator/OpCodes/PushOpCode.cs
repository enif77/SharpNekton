/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class PushOpCode : AOpCode {
    public PushOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PUSH;
    }


    public override string ToString() 
    {
      return "push";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.Stack.Push( ev.RegR );
      //ev.RegR = new UndefinedValue();
    }

  } // end of class
} // end of namespace
