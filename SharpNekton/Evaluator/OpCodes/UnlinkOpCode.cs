using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{
  class UnlinkOpCode : AOpCode {
    public UnlinkOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_UNLNK;
    }


    public override string ToString() 
    {
      return "unlink";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // returnning from a function = going one run level up
      ev.DecreaseRunLevel();

      // free all localy created data (==GBC==)
      // -- not needed --

      // restore stack pointer
      ev.Stack.StackTop = ev.RegFP;

      // get old FP value
      IValue oldFPValue = ev.Stack.ReadTop();
      ev.Stack.Pop();

      if (oldFPValue.TypeOf() == ValueTypeID.TYPE_STFP) {
        ev.RegFP = oldFPValue.GetIntValue();
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPESTFP) );
      }
    }

  } // end of class
} // end of namespace
