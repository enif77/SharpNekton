/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class AStringOpCode : AOpCode {
    protected string parameter;


    public AStringOpCode(int line, int linePosition, string parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public string Parameter {
      get {
        return parameter;
      }
    }

  } // end of class
} // end of namespace
