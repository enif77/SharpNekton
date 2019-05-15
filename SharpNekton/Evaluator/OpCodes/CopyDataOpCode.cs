/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class CopyDataOpCode : AOpCode {
    public CopyDataOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_COPYDATA;
    }


    public override string ToString() 
    {
      return "copydata";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // R -> value
      ev.GetVal();

      // if it is an table, make a copy of it...
      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        ValueTable table = (ValueTable) ev.RegR.GetObjectValue();
        ev.RegR = new TableRefValue( table.Copy() );
      }
    }

  } // end of class
} // end of namespace
