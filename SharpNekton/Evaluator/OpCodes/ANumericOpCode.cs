/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class ANumericOpCode : AOpCode {
    protected double parameter;


    public ANumericOpCode(int line, int linePosition, double parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public double Parameter {
      get {
        return parameter;
      }
    }

  } // end of class
} // end of namespace
