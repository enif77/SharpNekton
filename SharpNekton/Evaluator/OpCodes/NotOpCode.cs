/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class NotOpCode : AOpCode {
    public NotOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NOT;
    }


    public override string ToString() 
    {
      return "not";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // second operand
      IValue a = ev.GetVal();

      ev.RegR = new BooleanValue( !a.GetBooleanValue() );
    }

  } // end of class
} // end of namespace
