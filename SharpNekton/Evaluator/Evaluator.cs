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

using System;
using System.Collections;
using System.Collections.Generic;
using SharpNekton.Evaluator.Values;
using SharpNekton.Evaluator.OpCodes;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator
{

    public enum ProgramStateID
    {
        RUNNING,  // program is still running
        END,      // end of program (O_END)
        SUBEND,   // end of subprogram (O_SUBEND)
    }

    public class EvaluatorState
    {
        private ScriptState state; // the global state

        private Stack stack;            // stack
        private IValue reg_R;           // the R register
        private OpCodeListItem reg_PC;  // the PC register
        private int reg_FP;             // the FP (frame pointer) register
        private Hashtable globals;      // global objects (ValueStore)
        private List<OpCodeList> codeParts;
        private ProgramStateID programState;
        private int runLevel;


        public EvaluatorState(ScriptState state)
        {
            this.state = state;
            stack = new Stack();
            reg_R = new UndefinedValue();
            reg_PC = null;
            reg_FP = -1;
            globals = new Hashtable(128);
            codeParts = new List<OpCodeList>();
            programState = ProgramStateID.RUNNING;
            runLevel = 0;
        }

        /*--------------------------------------------------------------*/

        public ScriptState State
        {
            get
            {
                return state;
            }
        }

        /*--------------------------------------------------------------*/

        public IValue RegR
        {
            get
            {
                return reg_R;
            }

            set
            {
                reg_R = value;
            }
        }

        /*--------------------------------------------------------------*/

        public OpCodeListItem RegPC
        {
            get
            {
                return reg_PC;
            }

            set
            {
                if (value == null)
                {
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADJMPTARGET));
                }
                else
                {
                    reg_PC = value;
                }
            }
        }

        /*--------------------------------------------------------------*/

        public int RegFP
        {
            get
            {
                return reg_FP;
            }

            set
            {
                reg_FP = value;
            }
        }

        /*--------------------------------------------------------------*/

        public Stack Stack
        {
            get
            {
                return stack;
            }
        }

        /*--------------------------------------------------------------*/

        public Hashtable Globals
        {
            get
            {
                return globals;
            }
        }

        /*--------------------------------------------------------------*/

        public ValueStore FindGlobalObject(string name)
        {
            if (globals.ContainsKey(name))
            {
                return (ValueStore)globals[name];
            }
            else
            {
                return null;
            }
        }

        /*--------------------------------------------------------------*/

        public ValueStore AddGlobalObject(string name)
        {
            ValueStore newGlobalObject = FindGlobalObject(name);
            if (newGlobalObject == null)
            {
                globals.Add(name, new ValueStore());
                newGlobalObject = (ValueStore)globals[name];
            }

            return newGlobalObject;
        }

        /*--------------------------------------------------------------*/

        public List<OpCodeList> CodeParts
        {
            get
            {
                return codeParts;
            }
        }

        /*--------------------------------------------------------------*/

        public OpCodeList NewCodePart()
        {
            OpCodeList newCodePart = new OpCodeList();
            codeParts.Add(newCodePart);

            return newCodePart;
        }


        public void DumpCodePart(OpCodeList codePart, int id)
        {
            if (codePart == null) return;

            Console.WriteLine("  Dumping code part {0}:", id);

            codePart.Rewind();
            OpCodeListItem item = codePart.Next();
            int opCodeID = 0;
            while (item != null)
            {
                //Console.WriteLine(">>   item = {0}", item.GetType().ToString());
                //Console.WriteLine(">>   item.Data = {0}", item.Data.GetType().ToString());

                AOpCode opcode = item.Data;
                Console.WriteLine("    [Li:{0} Po:{1} ID:{2}] {3}", opcode.Line, opcode.LinePosition, opCodeID++, opcode.ToString());
                item = codePart.Next();
            }

            Console.WriteLine("  End of code part {0}", id);
        }


        public void DumpCode()
        {
            Console.WriteLine("Dumping program code:");

            int codePartID = 0;
            foreach (OpCodeList codePart in codeParts)
            {
                DumpCodePart(codePart, codePartID++);
                Console.WriteLine();
            }

            Console.WriteLine("End of program code");
        }

        /*--------------------------------------------------------------*/

        public bool IsDone()
        {
            if (programState == ProgramStateID.RUNNING)
                return false;
            else
                return true;
        }

        /*--------------------------------------------------------------*/

        public ProgramStateID ProgramState
        {
            get
            {
                return programState;
            }

            set
            {
                programState = value;
            }
        }

        /*--------------------------------------------------------------*/

        public int RunLevel
        {
            get
            {
                return runLevel;
            }

            set
            {
                runLevel = value;
                if (runLevel < 0) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADRUNLEVEL));
            }
        }


        public int IncreaseRunLevel()
        {
            return ++runLevel;
        }


        public int DecreaseRunLevel()
        {
            runLevel--;
            if (runLevel < 0)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADRUNLEVEL));
            }

            return runLevel;
        }

        /*--------------------------------------------------------------*/

        private AOpCode DispatchOpCode()
        {
            AOpCode opCode;

            if (reg_PC != null)
            {
                opCode = reg_PC.Data;
                reg_PC = reg_PC.Next;

                return opCode;
            }
            else
            {
                return null;
            }
        }

        /*--------------------------------------------------------------*/

        public void EvalStep(ScriptState st)
        {
            AOpCode opcode;

            opcode = DispatchOpCode();
            if (opcode == null)
            {
                programState = ProgramStateID.END;
            }
            else
            {
                programState = ProgramStateID.RUNNING;     // clear the done flag
                                                           //Console.WriteLine(">> {0}", opcode);
                opcode.Eval(st.Evaluator);  // run the instruction
            }
        }

        /*--------------------------------------------------------------*/

        // wasEnd == 1 -> END or EXIT
        // wasEnd == 2 -> SUBEND
        public void SubEval(ScriptState st)
        {
            if (reg_R.TypeOf() != ValueTypeID.TYPE_PROGRAMREF)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NOCODE));
            }

            // set up the subprogram code pointers
            //sti->code = (list_head_t *) r->value.program.code;
            reg_PC = ((OpCodeList)reg_R.GetObjectValue()).First();

            // evaluate code
            programState = ProgramStateID.RUNNING;
            for (; ; )
            {
                EvalStep(st);
                if (programState != ProgramStateID.RUNNING) break;
            }

            //*wasEnd = st->done;

            GetVal();
        }

        /*--------------------------------------------------------------*/

        public void Evaluate(ScriptState st)
        {
            // run the subprogram
            SubEval(st);

            // get and return exit status (if desired)
            //r = &st->reg_R;
            //if (retval != NULL) {
            //  IS_ERR( sctypes_ToNumber(st, r) );
            //*retval = (int) r->value.number;
            //}
        }

        /*--------------------------------------------------------------*/

        public IValue GetVal(IValue v)
        {
            // deref. value
            // get a valule from the table item 
            // TYPE_TABLEDATAREF -> value
            if (v.TypeOf() == ValueTypeID.TYPE_TABLEDATAREF)
            {
                ValueTableItem dti = (ValueTableItem)v.GetObjectValue();

                return dti.Value;
            }
            // TYPE_STOREREF -> value
            else if (v.TypeOf() == ValueTypeID.TYPE_STOREREF)
            {
                ValueStore vstore = (ValueStore)v.GetObjectValue();
                return (IValue)vstore.Value;
            }

            // all other types are passed throught unchanged
            return v;
        }

        /*--------------------------------------------------------------*/

        public IValue GetVal()
        {
            return RegR = GetVal(RegR);
        }

        /*--------------------------------------------------------------*/

        public IValue GetStackTopVal()
        {
            stack.WriteTop(GetVal(stack.ReadTop()));
            return stack.ReadTop();
        }

        /*--------------------------------------------------------------*/

        public IValue GetParameter(int parameter, int numberOfPassedParameters)
        {
            // n + 1 = count with the _argc
            return stack.ReadN(stack.StackTop - ((numberOfPassedParameters + 1) - parameter));
        }


        public double GetNumericParameter(int parameter, int numberOfPassedParameters)
        {
            // n + 1 = count with the _argc
            IValue val = stack.ReadN(stack.StackTop - ((numberOfPassedParameters + 1) - parameter));

            return val.GetNumericValue();
        }

        /*--------------------------------------------------------------*/

        public void Return(IValue value)
        {
            if (value != null)
            {
                RegR = value;
            }
            else
            {
                RegR = new NullValue();
            }
        }


        public void Return(bool b)
        {
            RegR = new BooleanValue(b);
        }


        public void ReturnNull()
        {
            RegR = new NullValue();
        }


        public void ReturnUndefined()
        {
            RegR = new UndefinedValue();
        }


        public void Return(double n)
        {
            RegR = new NumericValue(n);
        }


        public void Return(string s)
        {
            if (s != null)
            {
                RegR = new StringValue(s);
            }
            else
            {
                RegR = new NullValue();
            }
        }

        /*--------------------------------------------------------------*/

    } // end of class
} // end of namespace
