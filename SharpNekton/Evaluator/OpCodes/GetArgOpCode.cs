/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class GetArgOpCode : AOpCode {
    public GetArgOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_GETARG;
    }


    public override string ToString() 
    {
      return "getarg";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // can be used at local runlevel only!
      if (ev.RunLevel <= 0) {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_NOGLOBARGS) );
      }

      // pointer to _argc
      IValue argcValue = ev.Stack.ReadN( ev.RegFP - 2 );
      if (argcValue.TypeOf() != ValueTypeID.TYPE_NUMBER) {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADTYPE) );
      }

      // get the number of passed params
      int numberOfPassedParams = (int) argcValue.GetNumericValue() + 1; // n + 1 = count with _argc

      // get parameter index
      IValue parameterIndexValue = ev.GetVal();
      int parameterIndex = (int) argcValue.GetNumericValue() + 1;  // n + 1 = start at offset 1

      // test argno validity
      if (parameterIndex < 1 || parameterIndex >= numberOfPassedParams) {  // >= numparams => hide _argc to arg()
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADPARAMINDEX) );
      }

      // arg[n] -> R
      parameterIndex = -(numberOfPassedParams - parameterIndex + 2);
      ev.RegR = ev.Stack.ReadN( ev.RegFP + parameterIndex );
    }
    
  } // end of class
} // end of namespace
