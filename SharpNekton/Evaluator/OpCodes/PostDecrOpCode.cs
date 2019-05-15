/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;


namespace SharpNekton.Evaluator.OpCodes
{

  class PostDecrOpCode : AOpCode {
    public PostDecrOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_POSTDECR;
    }


    public override string ToString() {
      return "postdecr";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_STOREREF) {
        ValueStore store = (ValueStore) ev.RegR.GetObjectValue();
        double number = store.Value.GetNumericValue();
        ev.RegR = store.Value;
        store.Value = new NumericValue(number - 1.0);
      }
      else if (ev.RegR.TypeOf() == ValueTypeID.TYPE_TABLEDATAREF) {
        ValueTableItem item = (ValueTableItem) ev.RegR.GetObjectValue();
        double number = item.Value.GetNumericValue();
        ev.RegR = item.Value;
        item.Value = new NumericValue(number - 1.0);
      }
      else {
       throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPESTOREREF) );
      }
    }
    
  } // end of class
} // end of namespace
