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

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator
{

    public class Stack
    {
        private ValueStore[] items;
        private int top;
        private int numItems;
        private int size;


        private void Init()
        {
            items = new ValueStore[size];

            for (int i = 0; i < size; i++)
            {
                items[i] = new ValueStore();
            }

            numItems = 0;
            top = -1;
        }


        public Stack()
        {
            size = 256;
            Init();
        }


        public Stack(int stackSize)
        {
            if (stackSize > 0)
            {
                size = stackSize;
            }
            else
            {
                size = 256;
            }

            Init();
        }


        public int StackTop
        {
            get
            {
                return top;
            }

            set
            {
                if (value >= size)
                {
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
                }

                if (value < -1)
                {
                    throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
                }

                top = value;
            }
        }


        public int NumItems
        {
            get
            {
                return numItems;
            }
        }


        public int Size
        {
            get
            {
                return size;
            }
        }


        public void Push(IValue v)
        {
            int stacktop = top;

            stacktop++;
            if (stacktop >= size)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }
            else
            {
                items[stacktop].Value = v;
                top = stacktop;
            }
        }


        public void Pop()
        {
            int stacktop = top;

            stacktop--;
            if (stacktop < -1)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            top = stacktop;
        }


        public void PopN(int n)
        {
            int stacktop = top;

            stacktop -= n;
            if (stacktop < -1)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            top = stacktop;
        }


        public void WriteTop(IValue v)
        {
            if (top <= -1)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (top >= size)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            items[top].Value = v;
        }


        public IValue ReadTop()
        {
            if (top <= -1)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (top >= size)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            return items[top].Value;
        }


        public ValueStore TopRef()
        {
            if (top <= -1)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (top >= size)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            return items[top];
        }


        public IValue ReadN(int n)
        {
            if (n < 0)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (n > top)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            return items[n].Value;
        }


        public ValueStore NRef(int n)
        {
            if (n < 0)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (n > top)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            return items[n];
        }


        public void WriteN(int n, IValue v)
        {
            if (n < 0)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STUNDER));
            }

            if (n > top)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_STOVER));
            }

            items[n].Value = v;
        }

    } // end of class

} // end of namespace
