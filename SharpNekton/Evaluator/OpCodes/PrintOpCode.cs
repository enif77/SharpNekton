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
  class PrintOpCode : AOpCode {
    public PrintOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PRINT;
    }


    public override string ToString() 
    {
      return "print";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_NULL) {
        ValueStore lfs = ev.FindGlobalObject( ScriptState.BASE_LF_VAR_NAME );
        if (lfs != null) {
          ev.State.PrintFCallBack(lfs.Value.GetStringValue());
        }
        else {
          ev.State.PrintFCallBack(System.Environment.NewLine);
        }  
      }
      else {
        ev.State.PrintFCallBack(ev.RegR.GetStringValue());
      }
    }

  } // end of class
} // end of namespace
