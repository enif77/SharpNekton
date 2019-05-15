/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class ACountOpCode : AOpCode {
    protected int parameter;


    public ACountOpCode(int line, int linePosition, int parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public int Parameter {
      get {
        return parameter;
      }
    }

  } // end of class
} // end of namespace
