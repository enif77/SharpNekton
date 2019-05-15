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

namespace SharpNekton.Evaluator.Values
{
    class STFPValue : IValue
    {
        private ValueTypeID valueType;
        private int val;


        public STFPValue(int val)
        {
            valueType = ValueTypeID.TYPE_STFP;
            this.val = val;
        }


        /// convert the scalar to an int
        public int GetIntValue()
        {
            return val;
        }


        /// convert the scalar to a boolean
        public bool GetBooleanValue()
        {
            return true;
        }


        /// convert the scalar to a double
        public double GetNumericValue()
        {
            return (double)val;
        }


        /// convert the scalar to a string
        public string GetStringValue()
        {
            return val.ToString();
        }


        /// convert the scalar to an object value *shrug*
        public object GetObjectValue()
        {
            return (object)val;
        }


        public ValueTypeID TypeOf()
        {
            return valueType;
        }


        public int SizeOf()
        {
            return 1;
        }


        public override string ToString()
        {
            return "SFTP value: " + GetStringValue();
        }

    } // end of class
} // end of namespace
