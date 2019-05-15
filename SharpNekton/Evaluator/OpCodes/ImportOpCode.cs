
namespace SharpNekton.Evaluator.OpCodes
{
  public class ImportOpCode : AOpCode
  {
    public ImportOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_IMPORT;
    }


    public override string ToString() 
    {
      return "import";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // r = "filename"
      ev.GetVal();
      string fileName = ev.RegR.GetStringValue();
      
      // store the return value
      OpCodeListItem rtsa = ev.RegPC;

      // load and compile the new source
      ev.State.ImportFile(fileName);

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
