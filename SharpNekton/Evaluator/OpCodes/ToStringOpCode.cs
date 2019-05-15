/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class ToStringOpCode : AOpCode {
    public ToStringOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TO_STRING;
    }


    public override string ToString() 
    {
      return "to_string";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      if (a.TypeOf() != ValueTypeID.TYPE_STRING) {
        ev.RegR = new StringValue( a.GetStringValue() );
      }
    }
    
  } // end of class
} // end of namespace
