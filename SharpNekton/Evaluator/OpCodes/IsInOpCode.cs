/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class IsInOpCode:AOpCode {
    public IsInOpCode(int line, int linePosition) : base(line, linePosition)  
    {
      opCodeID = OpCodeID.O_IS_IN;
    }


    public override string ToString() 
    {
      return "is_in";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand (table) is in R
      IValue tableValue = ev.GetVal();
      if (tableValue.TypeOf() != ValueTypeID.TYPE_TABLEREF) {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }

      // first operand (key)
      IValue key = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop the key

      ValueTable table = (ValueTable) tableValue.GetObjectValue();
      if (table.Search( key.GetStringValue() ) != null) {
        ev.RegR = new BooleanValue(true);
      }
      else {
        ev.RegR = new BooleanValue(false);
      }
    }
  } // end of class

} // end of namespace
