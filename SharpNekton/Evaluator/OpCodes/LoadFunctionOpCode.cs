using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{
  class LoadFunctionOpCode : AFunctionRefOpCode {
    public LoadFunctionOpCode(int line, int linePosition, FunctionRef parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_LOADF;
    }


    public override string ToString() 
    {
      return "loadf " + parameter.NumberOfDefinedParameters;
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new FunctionRefValue(parameter);
    }

  } // end of class
} // end of namespace
