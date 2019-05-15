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

namespace SharpNekton
{
    public class FunctionRef
    {
        private OpCodeList codePart;
        private int numParams;

        public FunctionRef(OpCodeList codePart, int numberOfDefinedParameters)
        {
            this.codePart = codePart;
            this.numParams = numberOfDefinedParameters;
        }


        public OpCodeList CodePart
        {
            get
            {
                return codePart;
            }
        }


        public int NumberOfDefinedParameters
        {
            get
            {
                return numParams;
            }
        }

    } // end of class
} // end of namespace
