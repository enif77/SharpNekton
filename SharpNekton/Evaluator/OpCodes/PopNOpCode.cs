/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  class PopNOpCode : ACountOpCode {
    public PopNOpCode(int line, int linePosition, int parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_POPN;
    }

    public override string ToString() 
    {
      return "popn " + Parameter;
    }

    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.Stack.PopN(parameter);
    }

  } // end of class
} // end of namespace
