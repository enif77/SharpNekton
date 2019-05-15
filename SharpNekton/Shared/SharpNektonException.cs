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

namespace SharpNekton.Shared
{
    public class SharpNektonException : Exception
    {
        private SharpNektonError error;


        public SharpNektonException() : base("A SharpNekton exception fired!")
        {
            error = new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR);
        }


        public SharpNektonException(String msg) : base("SharpNekton exception: " + msg)
        {
            error = new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR);
        }

        public SharpNektonException(SharpNektonError error) : base("SharpNekton exception: " + error.ToString())
        {
            this.error = error;
        }


        public SharpNektonError Error
        {
            get
            {
                return error;
            }
        }

    } // end of class
} // end of namespace
