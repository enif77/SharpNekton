/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class AStorePointerOpCode : AOpCode {
    protected ValueStore parameter;


    public AStorePointerOpCode(int line, int linePosition, ValueStore parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public ValueStore Parameter {
      get {
        return parameter;
      }

      set {
        parameter = value;
      }
    }

  } // end of class
} // end of namespace
