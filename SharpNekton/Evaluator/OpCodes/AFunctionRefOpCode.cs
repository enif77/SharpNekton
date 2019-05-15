
namespace SharpNekton.Evaluator.OpCodes
{
  public abstract class AFunctionRefOpCode : AOpCode {
    protected FunctionRef parameter;


    public AFunctionRefOpCode(int line, int linePosition, FunctionRef parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public FunctionRef Parameter {
      get {
        return parameter;
      }
    }

  } // end of class
} // end of namespace
