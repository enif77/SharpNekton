/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class JumpOpCode : APointerOpCode {

    public JumpOpCode(int line, int linePosition, OpCodeListItem destination) : base(line, linePosition, destination) 
    {
      opCodeID = OpCodeID.O_JUMP;
    }


    public override string ToString() 
    {
      return "jump -> " + Parameter.Data.ToString();
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegPC = parameter;
    }

  } // end of class
} // end of namespace
