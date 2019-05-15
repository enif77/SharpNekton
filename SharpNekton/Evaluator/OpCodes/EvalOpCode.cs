
namespace SharpNekton.Evaluator.OpCodes
{
  public class EvalOpCode : AOpCode
  {
    public EvalOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_EVAL;
    }


    public override string ToString() 
    {
      return "eval";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // r = "a source to be executed"
      ev.GetVal();
      string stringSource = ev.RegR.GetStringValue();
      
      // store the return value
      OpCodeListItem rtsa = ev.RegPC;

      // load and compile the new source
      ev.State.LoadString(stringSource);

      // execute the subprogram
      ev.SubEval(ev.State);

      // only the END state should remain
      if (ev.ProgramState != ProgramStateID.END) {
        ev.ProgramState = ProgramStateID.RUNNING;
      }

      // return from the subprogram to the calling program
      ev.RegPC = rtsa;
    }

  } // end of class
} // end of namespace
