using SharpNekton.Compiler;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{
  class ForeachStoreOpCode : AOpCode
  {
    public ForeachStoreOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_FSTORE;
    }


    public override string ToString() 
    {
      return "fstore";
    }


    /*
    O_FSTORE
      sin:  S[top] = varref; S[top-1] = arrayref
      sout: S[top] = arrayref
      in:   R = index; S[top] = varref; S[top-1] = arrayref
      out:  R = arrayref[index]; S[top] = arrayref;
      op:   R = arrayref[index]; store;
    */

    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // get the array reference
      IValue tableValue = ev.Stack.ReadN(ev.Stack.StackTop - 1);

      // work with arrays only
      if (tableValue.TypeOf() == ValueTypeID.TYPE_TABLEREF) {
        ValueTable table = (ValueTable) tableValue.GetObjectValue();

        // if this creates a new array item, foreach fall into a endless loop!
        ValueTableItem vti = table.Search( ev.RegR.GetStringValue() );
        if (vti != null) {
          ev.RegR = new TableDataRefValue(vti);
        }
        else {
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADKEYINFOREACH) );
        }
      }
      else {
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPETABLEREF) );
      }

      // store the R to the var
      // TODO: remove the new opcode creation here
      AOpCode storeOpCode = new StoreOpCode(this.line, this.linePosition, TokenID.T_ASSIGN_OP );
      storeOpCode.Eval(ev);
    }

  } // end of class
} // end of namespace
