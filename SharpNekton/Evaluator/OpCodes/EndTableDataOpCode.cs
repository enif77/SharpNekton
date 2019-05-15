/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class EndTableDataOpCode : AOpCode {
    public EndTableDataOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_END_TABLEDATA;
    }


    public override string ToString() 
    {
      return "end_tabledata";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // move the tableref to R and pop it from the stack
      // TODO: add some test, that stack top contains a table ref.
      ev.RegR = ev.Stack.ReadTop();
      ev.Stack.Pop();     // pop the table
    }
  } // end of class

} // end of namespace
