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

namespace SharpNekton.Compiler.Sources
{
    public abstract class ASource
    {
        protected int linePosition;
        protected int line;


        public ASource()
        {
            linePosition = 0;  // first char is at 1:1
            line = 1;
        }


        public int LineNumber
        {
            get
            {
                return line;
            }
        }


        public int LinePosition
        {
            get
            {
                return linePosition;
            }
        }

        public abstract override string ToString();
        public abstract int Read();
        public abstract void Close();
        public abstract string Name();
    }
}