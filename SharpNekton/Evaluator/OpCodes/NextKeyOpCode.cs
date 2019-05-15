
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;


namespace SharpNekton.Evaluator.OpCodes
{
  class NextKeyOpCode : AOpCode {
    public NextKeyOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_NEXT_KEY;
    }


    public override string ToString() 
    {
      return "next_key";
    }


    /*
    O_NEXT_KEY
      sp:   unchanged
      in:   S[top-2] = arrayref
      out:  R = arrayref->cindex
      op:   if (arrayref->cindex >= arrayref->size) {
              R = END-OF-READ;
            }
            else {
              R = arrayref->cindex; arrayref->cindex++;
            }
    */

    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // get the array reference
      IValue tableValue = ev.Stack.ReadN(ev.Stack.StackTop - 2);

      // work with arrays only
      if (tableValue.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        ValueTable table = (ValueTable) tableValue.GetObjectValue();

        ValueTableItem vti = table.Next();
        if (vti == null) {
          ev.RegR = new DoneValue();
        }
        else {
          ev.RegR = new StringValue( vti.Key );
        }
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }
    }

  } // end of class
} // end of namespace
