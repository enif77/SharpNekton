/*
 * Created by SharpDevelop.
 * User: locadmin
 * Date: 8.1.2008
 * Time: 14:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using SharpNekton.Evaluator.Values;

namespace SharpNekton.Evaluator.OpCodes
{
	public class ToObjectOpCode : AOpCode {
    public ToObjectOpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_TO_OBJECT;
    }


    public override string ToString() 
    {
      return "to_object";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // operand
      IValue a = ev.GetVal();

      if (a.TypeOf() != ValueTypeID.TYPE_OBJECTREF) {
        ev.RegR = new ObjectValue( a.GetObjectValue() );
      }
    }
    
  } // end of class
} // end of namespace

