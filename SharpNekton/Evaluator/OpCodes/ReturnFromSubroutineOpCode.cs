using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{
  class ReturnFromSubroutineOpCode : AOpCode {
    public ReturnFromSubroutineOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_RTS;
    }


    public override string ToString() 
    {
      return "rts";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // get a return address
      IValue rtsValue = ev.Stack.ReadTop();
      ev.Stack.Pop();  // pop return address from the stack

      if (rtsValue.TypeOf() == ValueTypeID.TYPE_RTSA) {
        ev.RegPC = (OpCodeListItem) rtsValue.GetObjectValue();
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPERTSA) );
      }
    }

  } // end of class
} // end of namespace
