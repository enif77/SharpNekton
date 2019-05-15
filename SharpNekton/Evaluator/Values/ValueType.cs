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

using System.ComponentModel;

namespace SharpNekton.Evaluator.Values
{

    public enum ValueTypeID
    {
        // base/public types
        [Description("unknown")] TYPE_UNKNOWN,
        [Description("undefined")] TYPE_UNDEFINED,
        [Description("null")] TYPE_NULL,
        [Description("boolean")] TYPE_BOOLEAN,
        [Description("number")] TYPE_NUMBER,
        [Description("string")] TYPE_STRING,
        [Description("table")] TYPE_TABLEREF,
        [Description("function")] TYPE_FUNCTIONREF,   // pointer to a function (value.function)
        [Description("function")] TYPE_CFUNCTIONREF,  // pointer to external/cfunction (value.cfunction)
                                                      //    [Description("userdata")]   TYPE_USERDATAREF,
        [Description("program")] TYPE_PROGRAMREF,    // compiled program refference
        [Description("object")] TYPE_OBJECTREF,
        [Description("internal")] TYPE_INTERNAL,

        // internal types
        [Description("storeref")] TYPE_STOREREF,      // &store
        [Description("offset")] TYPE_OFFSET,        // value->offset
        [Description("tabledata")] TYPE_TABLEDATAREF,  // pointer to sc_htabitem_t
        [Description("rtsa")] TYPE_RTSA,          // return adres from sub (value.ptr)
        [Description("stfp")] TYPE_STFP,          // STored Frame Pointer (value.number)
        [Description("done")] TYPE_DONE,          // O_NEXT reached the end of the array
    }


    //public class ValueType {
    //  private ValueTypeID typeID;

    //  public ValueType() {
    //    typeID = ValueTypeID.TYPE_UNKNOWN;
    //  }


    //  public ValueType(ValueTypeID typeID) {
    //    this.typeID = typeID;
    //  }


    //  public ValueTypeID TypeID {
    //    get {
    //      return typeID;
    //    }
    //  }


    //  public override string ToString() {
    //    string msg = "";

    //    switch (this.typeID) {
    //    default :
    //    case ValueTypeID.TYPE_UNKNOWN : msg = "unknown"; break;
    //    case ValueTypeID.TYPE_UNDEFINED : msg = "udefined"; break;
    //    case ValueTypeID.TYPE_NULL : msg = "null"; break;
    //    case ValueTypeID.TYPE_BOOLEAN : msg = "boolean"; break;
    //    case ValueTypeID.TYPE_NUMBER : msg = "number"; break;
    //    case ValueTypeID.TYPE_STRING : msg = "string"; break;
    //    case ValueTypeID.TYPE_TABLEREF : msg = "table"; break;
    //    case ValueTypeID.TYPE_FUNCTIONREF : msg = "function"; break;
    //    case ValueTypeID.TYPE_CFUNCTIONREF : msg = "cfunction"; break;
    //    case ValueTypeID.TYPE_USERDATAREF : msg = "user data"; break;
    //    case ValueTypeID.TYPE_PROGRAMREF : msg = "program"; break;
    //    case ValueTypeID.TYPE_INTERNAL : msg = "internal"; break;
    //    case ValueTypeID.TYPE_STOREREF : msg = "store refference"; break;
    //    case ValueTypeID.TYPE_OFFSET : msg = "offet"; break;
    //    case ValueTypeID.TYPE_TABLEDATAREF : msg = "table data refference"; break;
    //    case ValueTypeID.TYPE_STRINGSLICE : msg = "string slice"; break;
    //    case ValueTypeID.TYPE_RTSA : msg = "rtsa"; break;
    //    case ValueTypeID.TYPE_STFP : msg = "stack frame pointer"; break;
    //    case ValueTypeID.TYPE_DONE : msg = "done"; break;
    //    case ValueTypeID.TYPE_OBJECTREF : msg = "object"; break;
    //    }

    //    return msg;
    //  }
    //}
}
