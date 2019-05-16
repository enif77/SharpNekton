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

  class PreIncrOpCode : AOpCode {
    public PreIncrOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_PREINCR;
    }


    public override string ToString() 
    {
      return "preincr";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      if (ev.RegR.TypeOf() == ValueTypeID.TYPE_STOREREF) {
        ValueStore store = (ValueStore) ev.RegR.GetObjectValue();
        double number = store.Value.GetNumericValue();
        store.Value = new NumericValue(number + 1.0);
        ev.RegR = store.Value;
      }
      else if (ev.RegR.TypeOf() == ValueTypeID.TYPE_TABLEDATAREF) {
        ValueTableItem item = (ValueTableItem) ev.RegR.GetObjectValue();
        double number = item.Value.GetNumericValue();
        item.Value = new NumericValue(number + 1.0);
        ev.RegR = item.Value;
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPESTOREREF) );
      }
    }
    
  } // end of class
} // end of namespace
