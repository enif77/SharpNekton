using SharpNekton.Evaluator.Values;


namespace SharpNekton.Evaluator.OpCodes
{
  class PrintOpCode : AOpCode {
    public PrintOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PRINT;
    }


    public override string ToString() 
    {
      return "print";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_NULL) {
        ValueStore lfs = ev.FindGlobalObject( ScriptState.BASE_LF_VAR_NAME );
        if (lfs != null) {
          ev.State.PrintFCallBack(lfs.Value.GetStringValue());
        }
        else {
          ev.State.PrintFCallBack(System.Environment.NewLine);
        }  
      }
      else {
        ev.State.PrintFCallBack(ev.RegR.GetStringValue());
      }
    }

  } // end of class
} // end of namespace
