/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class BeginTableDataOpCode : AOpCode {
    public BeginTableDataOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_BEGIN_TABLEDATA;
    }


    public override string ToString() 
    {
      return "begin_tabledata";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // create a new table
      ev.RegR = new TableRefValue();
      ev.Stack.Push(ev.RegR);
    }
  } // end of class

} // end of namespace
