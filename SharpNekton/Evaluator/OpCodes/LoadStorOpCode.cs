/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class LoadStorOpCode : AStorePointerOpCode {
    public LoadStorOpCode(int line, int linePosition, ValueStore parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_LOADSTOR;
    }


    public override string ToString() 
    {
      return "loadstor";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      ev.RegR = new StoreRefValue(parameter);
    }
  } // end of class

} // end of namespace
