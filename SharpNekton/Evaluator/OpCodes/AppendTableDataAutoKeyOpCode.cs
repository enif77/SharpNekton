/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class AppendTableDataAutoKeyOpCode : AOpCode {
    public AppendTableDataAutoKeyOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_APPEND_TABLEDATA_AUTOKEY;
    }


    public override string ToString() 
    {
      return "append_tabledata_autokey";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue val = ev.GetVal();

      // get tableptr
      // TODO: add a test, if is the value at the stack top a table
      ValueTable table = (ValueTable) ev.Stack.ReadTop().GetObjectValue();

      // get the key and assign the value to the array
      // table[key] = value
      table.Insert( table.AssignAutoKey().ToString(), val );
    }
  } // end of class

} // end of namespace
