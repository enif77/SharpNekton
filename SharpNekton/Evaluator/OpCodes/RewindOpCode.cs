using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;


namespace SharpNekton.Evaluator.OpCodes
{
  class RewindOpCode : AOpCode {
    public RewindOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_REWIND;
    }


    public override string ToString() 
    {
      return "rewind";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue a = ev.GetVal();

      // work with arrays only
      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        ValueTable table = (ValueTable) ev.RegR.GetObjectValue();
        table.Rewind();
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }
    }

  } // end of class
} // end of namespace
