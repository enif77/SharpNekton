/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes 
{

  class LoadFPOpCode : AOffsetOpCode {
    public LoadFPOpCode(int line, int linePosition, int parameter) : base(line, linePosition, parameter) 
    {
      opCodeID = OpCodeID.O_LOADFP;
    }


    public override string ToString() 
    {
      return "loadfp " + Parameter;
    }


    public override void Eval(EvaluatorState ev) 
    {
      int numberOfDefinedParams = 0, parameterIndex = -1;

      //Console.WriteLine(this.ToString());

      // recalculate offsets only for function parameters and not for 
      // _argc and local vars
      int offset = this.parameter;
      if (offset < -2) {
        // pointer to _argc
        IValue argcValue = ev.Stack.ReadN( ev.RegFP - 2 );
        if (argcValue.TypeOf() != ValueTypeID.TYPE_NUMBER) {
          Console.WriteLine(">> exp. argc\n");
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADTYPE) );
        }

        // get number of passed params
        int numberOfPassedParams = (int) argcValue.GetNumericValue() + 1; // n + 1 = count with _argc

        // get defined number of params
        IValue functinRefValue = ev.Stack.ReadN( ev.RegFP + -(numberOfPassedParams + 2) );
        
        switch (functinRefValue.TypeOf()) {
        case ValueTypeID.TYPE_FUNCTIONREF :
          // get the defined parameter count
          numberOfDefinedParams = ((FunctionRef) functinRefValue.GetObjectValue()).NumberOfDefinedParameters;
          break;

        // TODO: implement cfunction LOADFP
        //case N_TYPE_CFUNCTIONREF :
        //  // get the defined parameter count
        //  numdefparams = t->value.cfunction.nparams;
        //  break;

        default :
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPEFUNCREF) );
        }

        // calculate number of fixed params
        if (numberOfDefinedParams < 0) numberOfDefinedParams = -numberOfDefinedParams - 1;

        // calculate position of the parameter in the parameters list
        parameterIndex = (numberOfDefinedParams + (offset + 2)) + 1;

        // calculate the new offset
        offset = -(numberOfPassedParams - parameterIndex + 2);
      }

      // return a pointer to a parameter or local var
      ev.RegR = new StoreRefValue( ev.Stack.NRef( ev.RegFP + offset ) );
    }
    
  } // end of class
} // end of namespace
