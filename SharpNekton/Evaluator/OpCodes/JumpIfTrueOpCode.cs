/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class JumpIfTrueOpCode : APointerOpCode {
    public JumpIfTrueOpCode(int line, int linePosition, OpCodeListItem destination) : base(line, linePosition, destination) 
    {
      opCodeID = OpCodeID.O_JUMP_IF_TRUE;
    }


    public override string ToString() 
    {
      return "jump_if_true -> " + Parameter.Data.ToString();
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue a = ev.GetVal();

      if (a.GetBooleanValue() == true) {
        ev.RegPC = parameter;
        //Console.WriteLine(">> jtrue: {0}", parameter.GetType().ToString());
        //Console.WriteLine(">> jtrue.Data: {0}", parameter.Data.GetType().ToString());
      }
    }

  } // end of class
} // end of namespace
