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
