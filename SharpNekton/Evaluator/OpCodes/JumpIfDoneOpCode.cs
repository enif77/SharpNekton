using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{
  class JumpIfDoneOpCode : APointerOpCode {
    public JumpIfDoneOpCode(int line, int linePosition, OpCodeListItem destination) : base(line, linePosition, destination) 
    {
      opCodeID = OpCodeID.O_JUMP_IF_DONE;
    }


    public override string ToString() 
    {
      return "jump_if_done -> " + Parameter.Data.ToString();
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue a = ev.GetVal();

      if (a.TypeOf() == ValueTypeID.TYPE_DONE) {
        ev.RegPC = parameter;
      }
    }

  } // end of class
} // end of namespace
