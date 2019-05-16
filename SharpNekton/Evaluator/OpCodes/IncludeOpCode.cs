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

namespace SharpNekton.Evaluator.OpCodes
{
  class IncludeOpCode : AOpCode
  {
    public IncludeOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_INCLUDE;
    }


    public override string ToString() 
    {
      return "include";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // r = "filename"
      ev.GetVal();
      string fileName = ev.RegR.GetStringValue();
      
      // store the return value
      OpCodeListItem rtsa = ev.RegPC;

      // load and compile the new source
      ev.State.LoadFile(fileName);

      // execute the subprogram
      ev.SubEval(ev.State);

      // only the END state should remain
      if (ev.ProgramState != ProgramStateID.END) {
        ev.ProgramState = ProgramStateID.RUNNING;
      }

      // return from the subprogram to the calling program
      ev.RegPC = rtsa;
    }

  } // end of class
} // end of namespace
