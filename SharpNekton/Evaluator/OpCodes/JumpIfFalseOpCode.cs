/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class JumpIfFalseOpCode:APointerOpCode {

    public JumpIfFalseOpCode(int line, int linePosition, OpCodeListItem destination) : base(line, linePosition, destination) 
    {
      opCodeID = OpCodeID.O_JUMP_IF_FALSE;
    }


    public override string ToString() 
    {
      return "jump_if_false -> " + Parameter.Data.ToString();
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue a = ev.GetVal();

      if (a.GetBooleanValue() == false) {
        ev.RegPC = parameter;
      }
    }

  } // end of class
} // end of namespace
