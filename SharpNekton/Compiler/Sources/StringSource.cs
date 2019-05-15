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
    /// <summary>
    /// Represents a source readed from a string.
    /// </summary>
    public class StringSource : ASource
    {
        private string source;
        private int sourcePosition;
        private int sourceLength;
        private bool closed;

        public StringSource(string source) : base()
        {
            this.source = source;
            sourcePosition = 0;
            sourceLength = source.Length;
            closed = false;
        }


        public override string ToString()
        {
            return source;
        }


        public override int Read()
        {
            if (closed == false)
            {
                if (sourcePosition < sourceLength)
                {
                    int look = (int)source[sourcePosition++];
                    linePosition++;
                    if (look == '\n')
                    {
                        linePosition = 1;
                        line++;
                    }

                    return look;
                }
                else
                {
                    return Tokenizer.EOF;  // the end of the source was reached
                }
            }
            else
            {
                return Tokenizer.EOF;  // source is closed, return EOF (-1)
            }
        }


        public override void Close()
        {
            closed = true;
        }


        public override string Name()
        {
            return "?";  // string sources have no name
        }

    } // end of class
} // end of namespace
