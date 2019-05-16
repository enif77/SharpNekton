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

  class TypeOfOpCode : AOpCode {
    public TypeOfOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TYPEOF;
    }


    public override string ToString() 
    {
      return "typeof";
    }


    public override void Eval(EvaluatorState ev)
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      ev.RegR = new StringValue( Tools.GetDescription( a.TypeOf() ) );
    }
    
  } // end of class
} // end of namespace
