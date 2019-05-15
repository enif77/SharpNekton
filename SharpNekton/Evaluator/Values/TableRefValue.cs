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

    class TableRefValue : IValue
    {
        private ValueTypeID valueType;
        private ValueTable val;

        public TableRefValue()
        {
            valueType = ValueTypeID.TYPE_TABLEREF;
            val = new ValueTable();
        }


        public TableRefValue(ValueTable table)
        {
            valueType = ValueTypeID.TYPE_TABLEREF;
            val = table;
        }


        /// convert the scalar to an int
        public int GetIntValue()
        {
            return 0;
        }


        /// convert the scalar to a boolean
        public bool GetBooleanValue()
        {
            if (val.NumItems() == 0)
            {   // [] == false
                return false;
            }
            else
            {
                return true;
            }
        }


        /// convert the scalar to a double
        public double GetNumericValue()
        {
            return 0.0;
        }


        /// convert the scalar to a string
        public string GetStringValue()
        {
            return "array";
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
            return val.NumItems();
        }


        public override string ToString()
        {
            return "String value: \"" + GetStringValue() + "\"";
        }

    } // end of class

}

