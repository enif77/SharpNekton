/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class ToBooleanOpCode : AOpCode {
    public ToBooleanOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TO_BOOLEAN;
    }


    public override string ToString() 
    {
      return "to_boolean";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      if (a.TypeOf() != ValueTypeID.TYPE_BOOLEAN) {
        ev.RegR = new BooleanValue( a.GetBooleanValue() );
      }
    }
    
  } // end of class
} // end of namespace
