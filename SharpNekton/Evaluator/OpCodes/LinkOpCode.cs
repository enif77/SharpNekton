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
  class LinkOpCode : ACountOpCode {
    public LinkOpCode(int line, int linePosition, int countParameter) : base(line, linePosition, countParameter) 
    {
      opCodeID = OpCodeID.O_LINK;
    }


    public override string ToString() 
    {
      return "link " + Parameter;
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // calling a function = entering a deeper run level
      ev.IncreaseRunLevel();

      // get the number of local variables
      int numlocals = parameter;

      // push(reg_FP)
      ev.Stack.Push( new STFPValue(ev.RegFP) );

      // set FP to current stack position
      ev.RegFP = ev.Stack.StackTop;

      // create room for local variables
      ev.Stack.StackTop += numlocals;

      // set locals to undefined
      IValue undefinedValue = new UndefinedValue();
      int stackTop = ev.Stack.StackTop;
      for (int i = 0; i < numlocals; i++) {
        ev.Stack.WriteN(stackTop - i, undefinedValue);
      }
    }

  } // end of class
} // end of namespace
