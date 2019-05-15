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
using System.Text;
using System.IO;

namespace SharpNekton.Compiler.Sources
{
    /// <summary>
    /// Represents a source readed from a file.
    /// </summary>
    public class FileSource : ASource
    {
        private StreamReader stream;
        private string fileName;
        private bool closed;

        public FileSource(string fileName) : base()
        {
            this.fileName = fileName;
            closed = false;

            try
            {
                stream = new StreamReader(fileName, Encoding.Default);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(">> {0}", e);
                stream = null;
                closed = true;
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine(">> {0}", e);
                stream = null;
                closed = true;
            }
        }


        public override string ToString()
        {
            return fileName;
        }


        public override int Read()
        {
            if (closed == false && stream != null)
            {
                if (stream.EndOfStream == false)
                {
                    int look = (int)stream.Read();
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
                    return Tokenizer.EOF;  // end of the source reached
                }
            }
            else
            {
                return Tokenizer.EOF;  // source is closed, return EOF (-1)
            }
        }


        public override void Close()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            closed = true;
        }


        public override string Name()
        {
            return fileName;
        }

    } // end of class
} // end of namespace
