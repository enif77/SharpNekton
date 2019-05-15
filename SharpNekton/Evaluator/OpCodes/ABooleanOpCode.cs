/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class ABooleanOpCode : AOpCode {
    protected bool parameter;


    public ABooleanOpCode(int line, int linePosition, bool parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public bool Parameter {
      get {
        return parameter;
      }
    }

  } // end of class
} // end of namespace
