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
  class NextOpCode : AOpCode {
    public NextOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NEXT;
    }


    public override string ToString() 
    {
      return "next";
    }

    /*
    O_NEXT
      sp:   unchanged
      in:   S[top-1] = arrayref
      out:  R = arrayref[arrayref->cindex];
      op:   if (arrayref->cindex >= arrayref->size) {
              R = END-OF-READ;
            }
            else {
              R = arrayref[arrayref->cindex]; arrayref->cindex++;
            }
    */

    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // get the array reference
      IValue tableValue = ev.Stack.ReadN( ev.Stack.StackTop - 1);

      // work with arrays only
      if (tableValue.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        ValueTable table = (ValueTable) tableValue.GetObjectValue();

        ValueTableItem vti = table.Next();
        if (vti == null) {
          ev.RegR = new DoneValue();
        }
        else {
          ev.RegR = new TableDataRefValue(vti);  // should be TYPE_STOREREF?
        }
      }
      else {
        //Console.WriteLine(">> nextop tableval.type = {0}", tableValue.TypeOf().ToString());
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }
    }

  } // end of class
} // end of namespace
