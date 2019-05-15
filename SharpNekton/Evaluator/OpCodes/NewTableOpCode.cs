/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;


namespace SharpNekton.Evaluator.OpCodes
{

  class NewTableOpCode : AOpCode {
    public NewTableOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NEWTABLE;
    }


    public override string ToString() 
    {
      return "newtable";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new TableRefValue();
    }
    
  } // end of class
} // end of namespace
