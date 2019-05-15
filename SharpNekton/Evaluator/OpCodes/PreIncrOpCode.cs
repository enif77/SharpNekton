/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;


namespace SharpNekton.Evaluator.OpCodes
{

  class PreIncrOpCode : AOpCode {
    public PreIncrOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PREINCR;
    }


    public override string ToString() 
    {
      return "preincr";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_STOREREF) {
        ValueStore store = (ValueStore) ev.RegR.GetObjectValue();
        double number = store.Value.GetNumericValue();
        store.Value = new NumericValue(number + 1.0);
        ev.RegR = store.Value;
      }
      else if (ev.RegR.TypeOf() == ValueTypeID.TYPE_TABLEDATAREF) {
        ValueTableItem item = (ValueTableItem) ev.RegR.GetObjectValue();
        double number = item.Value.GetNumericValue();
        item.Value = new NumericValue(number + 1.0);
        ev.RegR = item.Value;
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPESTOREREF) );
      }
    }
    
  } // end of class
} // end of namespace
