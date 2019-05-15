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
using System.Collections.Generic;

using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Evaluator
{

    public class ValueTableItem
    {
        private ObjectListItem<ValueTableItem> listItem;
        private ValueTable parent;
        private string key;
        private IValue value;


        public ValueTableItem(ValueTable parent, ObjectListItem<ValueTableItem> listItem, string key, IValue value)
        {
            this.parent = parent;
            this.listItem = listItem;
            this.key = key;
            this.value = value;
        }


        public ValueTable Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }


        public ObjectListItem<ValueTableItem> ListItem
        {
            get
            {
                return listItem;
            }

            set
            {
                listItem = value;
            }
        }


        public string Key
        {
            get
            {
                return key;
            }
        }


        public IValue Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    } // end of class


    public class ValueTable
    {
        private ObjectList<ValueTableItem> list;
        private IDictionary<String, ObjectListItem<ValueTableItem>> htab;
        private int autokey;

        public ValueTable()
        {
            list = new ObjectList<ValueTableItem>();
            htab = new Dictionary<String, ObjectListItem<ValueTableItem>>();
            autokey = 0;
        }


        public ValueTable(int minsize)
        {
            list = new ObjectList<ValueTableItem>();
            htab = new Dictionary<String, ObjectListItem<ValueTableItem>>(minsize);
            autokey = 0;
        }


        public ObjectList<ValueTableItem> List
        {
            get
            {
                return list;
            }
        }


        public int NumItems()
        {
            return list.NumItems;
        }


        public int AutoKey
        {
            get
            {
                return autokey;
            }
        }


        public int AssignAutoKey()
        {
            return autokey++;  // return current autokey's value and incrementi it
        }


        public ValueTable Copy()
        {
            ValueTable tableCopy = new ValueTable();

            this.Rewind();
            ValueTableItem item = Next();
            while (item != null)
            {
                tableCopy.Insert(item.Key, item.Value);
                item = Next();
            }

            return tableCopy;
        }


        public ValueTableItem Rewind()
        {
            list.Rewind();

            ObjectListItem<ValueTableItem> item = list.First();
            if (item != null)
                return item.Data;
            else
                return null;
        }


        public ValueTableItem Next()
        {
            ObjectListItem<ValueTableItem> item = list.Next();
            if (item != null)
                return item.Data;
            else
                return null;
        }


        public ValueTableItem Prev()
        {
            ObjectListItem<ValueTableItem> item = list.Prev();
            if (item != null)
                return item.Data;
            else
                return null;
        }


        public ValueTableItem Search(string key)
        {
            if (htab.ContainsKey(key) == true)
            {
                return htab[key].Data;
            }
            else
            {
                return null;
            }
        }


        public ValueTableItem Search(double key)
        {
            return Search(key.ToString());
        }


        public ValueTableItem Insert(string key, IValue value)
        {
            ValueTableItem newVTItem;
            ObjectListItem<ValueTableItem> newItem;

            if (htab.ContainsKey(key) == true)
            {
                newItem = htab[key];
                newVTItem = newItem.Data;
                newVTItem.Value = value;
            }
            else
            {
                newVTItem = new ValueTableItem(this, null, key, value);
                newItem = list.Append(newVTItem);
                htab.Add(key, newItem);
                newVTItem.ListItem = newItem;
            }

            return newVTItem;
        }


        public ValueTableItem Insert(double key, IValue value)
        {
            return Insert(key.ToString(), value);
        }


        public void Delete(string key)
        {
            if (htab.ContainsKey(key) == true)
            {
                ObjectListItem<ValueTableItem> item = htab[key];
                list.Delete(item);
                htab.Remove(key);
            }
        }


        public void Delete(double key)
        {
            Delete(key.ToString());
        }

    } // end of class
} // end of namespace
