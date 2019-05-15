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

using SharpNekton.Evaluator.OpCodes;

namespace SharpNekton.Evaluator
{
    public class OpCodeListItem
    {
        private OpCodeList list;
        private OpCodeListItem prev;
        private OpCodeListItem next;
        private AOpCode data;

        public OpCodeListItem()
        {
            this.list = null;
            this.prev = null;
            this.next = null;
            this.data = null;
        }


        public OpCodeListItem(OpCodeList list)
        {
            this.list = list;
            this.prev = null;
            this.next = null;
            this.data = null;
        }


        public OpCodeListItem(OpCodeList list, AOpCode data)
        {
            this.list = list;
            this.prev = null;
            this.next = null;
            this.data = data;
        }


        public OpCodeListItem(OpCodeList list, OpCodeListItem prev, OpCodeListItem next, AOpCode data)
        {
            this.list = list;
            this.prev = prev;
            this.next = next;
            this.data = data;
        }


        public OpCodeList List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }


        public OpCodeListItem Prev
        {
            get
            {
                return prev;
            }

            set
            {
                prev = value;
            }
        }


        public OpCodeListItem Next
        {
            get
            {
                return next;
            }

            set
            {
                next = value;
            }
        }


        public AOpCode Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

    } // end of class


    /// <summary>
    /// Description of OpCodeList.
    /// </summary>
    public class OpCodeList
    {
        private OpCodeListItem head;
        private OpCodeListItem tail;
        private OpCodeListItem current;
        private int numItems;


        public OpCodeList()
        {
            this.head = null;
            this.tail = null;
            this.current = null;
            this.numItems = 0;
        }

        /*--------------------------------------------------------------------*/

        public int NumItems
        {
            get
            {
                return numItems;
            }
        }

        /*--------------------------------------------------------------------*/

        public void EmptyList()
        {
            // TODO: Add some list-items traversal and cleanup
            this.head = null;
            this.tail = null;
            this.current = null;
            this.numItems = 0;
        }

        /*--------------------------------------------------------------------*/

        public void Rewind()
        {
            if (numItems == 0)
            {
                current = null;
            }
            else
            {
                current = head;
            }
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem Next()
        {
            // is the list empty?
            if (numItems == 0)
            {
                Rewind();

                return null;
            }

            // return the current item
            OpCodeListItem item = current;

            // read the next one, if we have one
            if (current != null)
            {
                current = current.Next;
            }

            // return what we found...
            return item;
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem Prev()
        {
            // is the list empty?
            if (numItems == 0)
            {
                Rewind();

                return null;
            }

            // return the current item
            OpCodeListItem item = current;

            // read the next one, if we have one
            if (current != null)
            {
                current = current.Prev;
            }

            // return what we found...
            return item;
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem First()
        {
            // go to the first item
            Rewind();

            // return the first item in the list
            return current;
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem Last()
        {
            // go to the last item in the list
            current = tail;

            // return the current item
            return current;
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem Append()
        {
            return Append(new OpCodeListItem(this));
        }

        /*--------------------------------------------------------------------*/

        public OpCodeListItem Append(AOpCode o)
        {
            return Append(new OpCodeListItem(this, o));
        }

        /*--------------------------------------------------------------------*/
        // TODO: add some tests to ensure, that the given item is from this list
        public OpCodeListItem Append(OpCodeListItem item)
        {
            if (item == null) return null;

            // emty list
            if (numItems == 0)
            {
                head = tail = item;
                item.Prev = null;
                item.Next = null;
            }
            else
            {
                OpCodeListItem tmp = tail;
                tail = item;
                item.Prev = tmp;
                tmp.Next = item;
                item.Next = null;
            }

            item.List = this;

            // new item appended
            numItems++;
            current = item;

            return item;
        }

        /*--------------------------------------------------------------------*/
        // TODO: add some tests to ensure, that the given item is from this list
        public void Disconnect(OpCodeListItem item)
        {
            OpCodeListItem prev, next;

            prev = item.Prev;
            next = item.Next;

            if (prev != null)
            {
                if (next != null)
                {
                    prev.Next = next;  // item between two other items removed
                    next.Prev = prev;
                }
                else
                {
                    this.tail = prev;   // item at the end of the list removed
                    prev.Next = null;
                }
            }
            else
            {
                if (next != null)
                {
                    this.head = next;  // first item removed
                    next.Prev = null;
                }
                else
                {
                    this.head = null;  // last item in the list removed
                    this.tail = null;
                }
            }

            // item disconnected from the list
            item.Prev = null;
            item.Next = null;
            item.List = null;

            // item removed
            this.numItems--;

            // reset the current list item pointer
            if (item == this.current)
            {
                Rewind();
            }
        }

        /*--------------------------------------------------------------------*/

        public void Delete(OpCodeListItem item)
        {
            Disconnect(item);
        }

    } // end of class
} // end of namespace
