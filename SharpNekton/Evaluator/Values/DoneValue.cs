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
    /// <summary>
    /// Represents an internaly used value, which meaning is, 
    /// that the foreach loop has reached the last item in a table.
    /// </summary>
    class DoneValue : IValue
    {
        private ValueTypeID valueType;

        /// <summary>
        /// The default constructor. Creates a new DONE value. 
        /// </summary>
        public DoneValue()
        {
            valueType = ValueTypeID.TYPE_DONE;
        }


        /// <summary>
        /// Returns the integer representation of the stored value (allways 1).
        /// </summary>
        /// <returns>Integer representation of the stored value.</returns>
        public int GetIntValue()
        {
            return 1;
        }


        /// <summary>
        /// Returns the boolean representation of the stored value (allways true).
        /// </summary>
        /// <returns>Boolean representation of the stored value.</returns>
        public bool GetBooleanValue()
        {
            return true;
        }


        /// <summary>
        /// Returns the numeric (double) representation of the stored value (allways 1.0).
        /// </summary>
        /// <returns>Numeric representation of the stored value.</returns>
        public double GetNumericValue()
        {
            return 1.0;
        }


        /// <summary>
        /// Returns the string representation of the stored value (allways "done").
        /// </summary>
        /// <returns>String representation of the stored value.</returns>
        public string GetStringValue()
        {
            return "done";
        }


        /// <summary>
        /// Returns an object, representing the stored value.
        /// </summary>
        /// <returns>Object, representing the stored value.</returns>
        /// </summary>
        /// <returns></returns>
        public object GetObjectValue()
        {
            //return (Object) null;
            return new object();   // TODO: What to return?
        }


        /// <summary>
        /// Returns the exact type of the stored value.
        /// </summary>
        /// <returns>The type of the stored value.</returns>
        public ValueTypeID TypeOf()
        {
            return valueType;
        }


        /// <summary>
        /// Returns the size of the stored value in number of used ValueStore items.
        /// </summary>
        /// <returns>The size of the stored value.</returns>
        public int SizeOf()
        {
            return 1;
        }


        /// <summary>
        /// Returns the string representation of the stored value ready for debug/printing. 
        /// </summary>
        /// <returns>The string representation of the stored value with a commentary.</returns>
        public override string ToString()
        {
            return "Done value";
        }

    } // end of class
} // end of namespace
