/*=============================================================
(c) 2007 enif, all rights reserved
================================================================*/

using System;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator.OpCodes
{

  class JSROpCode : AOpCode {
    public JSROpCode(int line, int linePosition) : base(line, linePosition) 
    {
      opCodeID = OpCodeID.O_JSR;
    }


    public override string ToString() 
    {
      return "jsr";
    }


    public override void Eval(EvaluatorState ev) 
    {
      //Console.WriteLine(this.ToString());

      // pointer to _argc
      IValue argcValue = ev.Stack.ReadTop();
      if (argcValue.TypeOf() != ValueTypeID.TYPE_NUMBER) {
        Console.WriteLine(">> exp. argc");
        // TODO: add more specific error code here
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADTYPE) );
      }

      // get number of passed params
      int numberOfPassedParams = (int) argcValue.GetNumericValue();

      // functionref is stored on the stack[numparams] by pushp instuction,
      // so GetVal() is not neccessary
      IValue functionRefValue = ev.Stack.ReadN( ev.Stack.StackTop - (numberOfPassedParams + 1) );  // n + 1 = count with _argc

      switch (functionRefValue.TypeOf()) {
      case ValueTypeID.TYPE_FUNCTIONREF :
        // store the return address on the stack
        ev.Stack.Push( new RTSAValue( ev.RegPC ) );

        // check parameter counts
        FunctionRef functionRef = (FunctionRef) functionRefValue.GetObjectValue();
        if (CheckParams(functionRef.NumberOfDefinedParameters, numberOfPassedParams) == false) {
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADPARAMCOUNT) );
        }

        // set PC to new address and test it
        OpCodeListItem functionStartAddress = functionRef.CodePart.First();
        if (functionStartAddress != null) {
          ev.RegPC = functionStartAddress;
        }
        else {
          // TODO: add more specific error code here
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADJMPTARGET) );
        }
        break;

      case ValueTypeID.TYPE_CFUNCTIONREF :
        // check parameter counts
        ExternalFunctionRef extFunctionRef = (ExternalFunctionRef) functionRefValue.GetObjectValue();
        if (CheckParams(extFunctionRef.NumberOfDefinedParameters, numberOfPassedParams) == false) {
          throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_BADPARAMCOUNT) );
        }

        // call the function
        extFunctionRef.FunctionRef(numberOfPassedParams);
        break;

      default :
        //Console.WriteLine(">> type is {0}", functionRefValue.TypeOf().ToString());
        throw new SharpNektonException( new SharpNektonError(SharpNektonErrorID.E_EXPTYPEFUNCREF) );
      }
    }


    private bool CheckParams(int numdefp, int numrelp)
    {
      if (numdefp < 0) {
        return numrelp >= (-numdefp - 1);  // -1 => 0 or n args
      }
      else {
        return numrelp == numdefp;   // real args == defined args
      }
    }
  } // end of class

} // end of namespace
