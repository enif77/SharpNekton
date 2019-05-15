/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class PushPOpCode:AOpCode {
    public PushPOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PUSHP;
    }


    public override string ToString() 
    {
      return "pushp";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.Stack.Push( ev.GetVal() );
      //ev.RegR = new UndefinedValue();
    }
    
  } // end of class
} // end of namespace
