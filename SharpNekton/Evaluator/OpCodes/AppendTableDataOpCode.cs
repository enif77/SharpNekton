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

/*
load s      -> r = key
push
load        -> r = value
tstore      -> array[key] = value; S[top] = arayref;
*/

using System;
using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{

  class AppendTableDataOpCode:AOpCode {
    public AppendTableDataOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_APPEND_TABLEDATA;
    }


    public override string ToString() 
    {
      return "append_tabledata";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // value is in R
      IValue val = ev.GetVal();

      // pop key
      IValue key = ev.GetStackTopVal();
      ev.Stack.Pop();     // pop key

      // get tableptr
      // TODO: add a test, if is the value at the stack top a table
      ValueTable table = (ValueTable) ev.Stack.ReadTop().GetObjectValue();

      // table[key] = value
      table.Insert( key.GetStringValue(), val );
    }
  } // end of class

} // end of namespace
