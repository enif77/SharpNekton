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
