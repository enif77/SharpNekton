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

using SharpNekton.Evaluator;
using SharpNekton.Evaluator.Values;

namespace SharpNekton.Libraries
{
    class BaseLib : ALibrary
    {
        public BaseLib(ScriptState state) : base(state)
        {
            this.name = "base";
        }


        public override void Register()
        {
            this.RegisterFunction("strcat", StrCatF, 2);
            this.RegisterFunction("nump", NumPF, -1);


            //ValueTableItem testObj = this.RegisterObject("testObj");
            //testObj.Value = new StringValue("testObj: The test object.");

            ValueTableItem gObj = this.RegisterObject(ScriptState.BASE_LF_VAR_NAME);
            gObj.Value = new StringValue(System.Environment.NewLine);
        }


        public override void UnRegister()
        {
            ; // nothing to be done here
        }

        /*--------------------------------------------------------------*/
        // s = s1 + s2

        private bool StrCatF(int numParams)
        {
            string a = state.Evaluator.GetParameter(1, numParams).GetStringValue();
            string b = state.Evaluator.GetParameter(2, numParams).GetStringValue();

            state.Evaluator.Return(a + b);

            return true;
        }


        private bool NumPF(int numParams)
        {
            state.Evaluator.Return(numParams);

            return true;
        }

    } // end of class
} // end of namespace
