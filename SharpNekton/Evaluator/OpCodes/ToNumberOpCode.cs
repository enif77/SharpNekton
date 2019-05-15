/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes 
{

  class ToNumberOpCode : AOpCode {
    public ToNumberOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TO_NUMBER;
    }


    public override string ToString() 
    {
      return "to_number";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      if (a.TypeOf() != ValueTypeID.TYPE_NUMBER) {
        ev.RegR = new NumericValue( a.GetNumericValue() );
      }
    }
    
  } // end of class
} // end of namespace
