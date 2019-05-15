/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

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
