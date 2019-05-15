
namespace SharpNekton.Evaluator.OpCodes
{
  public class SubEndOpCode : AOpCode
  {
    public SubEndOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_SUBEND;
    }


    public override string ToString() 
    {
      return "subend";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.ProgramState = ProgramStateID.SUBEND;
    }

  } // end of class
} // end of namespace