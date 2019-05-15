/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadIOpCode : AOpCode {
    public LoadIOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_LOADI;
    }


    public override string ToString() 
    {
      return "loadi";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // index is in R
      IValue keyValue = ev.GetVal();

      // get table (it is on the stack[top])
      IValue tableValue = ev.GetStackTopVal();
      ev.Stack.Pop();

      // for array returns ptr to array[index] as TYPE_TABLEDATAREF
      // else raises an error
      if (tableValue.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        string key = keyValue.GetStringValue();
        ValueTable table = (ValueTable) tableValue.GetObjectValue();
        ValueTableItem vti = table.Search( key );
        if (vti == null) {
          vti = table.Insert( key, new UndefinedValue() );
        }

        ev.RegR = new TableDataRefValue( vti );
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }
    }
    
  } // end of class
} // end of namespace
