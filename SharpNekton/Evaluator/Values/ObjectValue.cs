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

namespace SharpNekton.Evaluator.Values
{
    public class ObjectValue : IValue
    {
        private ValueTypeID valueType;
        private object val;

        public ObjectValue(object val)
        {
            valueType = ValueTypeID.TYPE_OBJECTREF;
            this.val = val;
        }


        public int GetIntValue()
        {
            return Convert.ToInt32(val);
        }


        public bool GetBooleanValue()
        {
            return Convert.ToBoolean(val);
        }


        public double GetNumericValue()
        {
            return Convert.ToDouble(val);
        }


        public string GetStringValue()
        {
            return val.ToString();
        }


        public object GetObjectValue()
        {
            return val;
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
            return "Refference value: \"" + GetStringValue() + "\"";
        }

    } // end of class
} // end of namespace
