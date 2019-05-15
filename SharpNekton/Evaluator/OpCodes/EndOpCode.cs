
namespace SharpNekton.Evaluator.OpCodes
{
  class EndOpCode : AOpCode
  {
    public EndOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_END;
    }


    public override string ToString() 
    {
      return "end";
    }


    public override void Eval(EvaluatorState ev)
    {
      ev.ProgramState = ProgramStateID.END;
    }

  } // end of class
} // end of namespace
