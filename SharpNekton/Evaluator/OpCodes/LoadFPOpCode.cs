/* SharpNekton - (C) 2019 Premysl Fara 
 
SharpNekton is available under the zlib license:

This software is provided 'as-is', without any express or implied
warranty.  In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.
 
 */

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
