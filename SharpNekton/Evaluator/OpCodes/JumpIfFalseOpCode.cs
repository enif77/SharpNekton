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

namespace SharpNekton.Evaluator.OpCodes
{

  class JumpIfFalseOpCode:APointerOpCode {

    public JumpIfFalseOpCode(int line, int linePosition, OpCodeListItem destination) : base(line, linePosition, destination) 
    {
      opCodeID = OpCodeID.O_JUMP_IF_FALSE;
    }


    public override string ToString() 
    {
      return "jump_if_false -> " + Parameter.Data.ToString();
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue a = ev.GetVal();

      if (a.GetBooleanValue() == false) {
        ev.RegPC = parameter;
      }
    }

  } // end of class
} // end of namespace
