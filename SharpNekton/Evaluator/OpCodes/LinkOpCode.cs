using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{
  class LinkOpCode : ACountOpCode {
    public LinkOpCode(int line, int linePosition, int countParameter) : base(line, linePosition, countParameter) 
    {
      opCodeID = OpCodeID.O_LINK;
    }


    public override string ToString() 
    {
      return "link " + Parameter;
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // calling a function = entering a deeper run level
      ev.IncreaseRunLevel();

      // get the number of local variables
      int numlocals = parameter;

      // push(reg_FP)
      ev.Stack.Push( new STFPValue(ev.RegFP) );

      // set FP to current stack position
      ev.RegFP = ev.Stack.StackTop;

      // create room for local variables
      ev.Stack.StackTop += numlocals;

      // set locals to undefined
      IValue undefinedValue = new UndefinedValue();
      int stackTop = ev.Stack.StackTop;
      for (int i = 0; i < numlocals; i++) {
        ev.Stack.WriteN(stackTop - i, undefinedValue);
      }
    }

  } // end of class
} // end of namespace
