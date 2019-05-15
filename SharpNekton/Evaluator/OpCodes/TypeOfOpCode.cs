/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class TypeOfOpCode : AOpCode {
    public TypeOfOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TYPEOF;
    }


    public override string ToString() 
    {
      return "typeof";
    }


    public override void Eval(EvaluatorState ev)
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      ev.RegR = new StringValue( Tools.GetDescription( a.TypeOf() ) );
    }
    
  } // end of class
} // end of namespace
