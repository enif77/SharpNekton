/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

namespace SharpNekton.Evaluator.OpCodes
{

  public abstract class APointerOpCode : AOpCode{
    protected OpCodeListItem parameter;


    public APointerOpCode(int line, int linePosition, OpCodeListItem parameter) : base(line, linePosition) {
      this.parameter = parameter;
    }


    public OpCodeListItem Parameter {
      get {
        return parameter;
      }

      set {
        parameter = value;
      }
    }

  } // end of class
} // end of namespace
