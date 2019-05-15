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

namespace SharpNekton.Shared
{

  public class ObjectListItem<T> {
    protected ObjectList<T> list;
    protected ObjectListItem<T> prev;
    protected ObjectListItem<T> next;
    protected T data;


    public ObjectListItem() {
      this.list = null;
      this.prev = null;
      this.next = null;
      this.data = default(T);
    }


    public ObjectListItem(ObjectList<T> list) {
      this.list = list;
      this.prev = null;
      this.next = null;
      this.data = default(T);
    }


    public ObjectListItem(ObjectList<T> list, T data) {
      this.list = list;
      this.prev = null;
      this.next = null;
      this.data = data;
    }


    public ObjectListItem(ObjectList<T> list, ObjectListItem<T> prev, ObjectListItem<T> next, T data) {
      this.list = list;
      this.prev = prev;
      this.next = next;
      this.data = data;
    }


    public ObjectList<T> List {
      get {
        return list;
      }

      set {
        list = value;
      }
    }


    public ObjectListItem<T> Prev {
      get {
        return prev;
      }

      set {
        prev = value;
      }
    }


    public ObjectListItem<T> Next {
      get {
        return next;
      }

      set {
        next = value;
      }
    }


    public T Data {
      get {
        return data;
      }

      set {
        data = value;
      }
    }
  }

/*--------------------------------------------------------------------------*/

  public class ObjectList<T> {
    protected ObjectListItem<T> head;
    protected ObjectListItem<T> tail;
    protected ObjectListItem<T> current;
    protected int numItems;


    public ObjectList() {
      this.head = null;
      this.tail = null;
      this.current = null;
      this.numItems = 0;
    }

/*--------------------------------------------------------------------------*/

    public int NumItems {
      get {
        return numItems;
      }
    }

/*--------------------------------------------------------------------------*/

    public void EmptyList() {
      // TODO: Add some list-items traversal and cleanup
      this.head = null;
      this.tail = null;
      this.current = null;
      this.numItems = 0;
    }

/*--------------------------------------------------------------------------*/

    public void Rewind() {
      if (numItems == 0) {
        current = null;
      }
      else {
        current = head;
      }
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Next() {
      ObjectListItem<T> item;

      // is the list empty?
      if (numItems == 0) {
        Rewind();

        return null;
      }

      // return the current item
      item = current;

      // read the next one, if we have one
      if (current != null) {
        current = current.Next;
      }

      // return what we found...
      return item;
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Prev() {
      ObjectListItem<T> item;

      // is the list empty?
      if (numItems == 0) {
        Rewind();

        return null;
      }

      // return the current item
      item = current;

      // read the next one, if we have one
      if (current != null) {
        current = current.Prev;
      }

      // return what we found...
      return item;
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> First() {
      // go to the first item
      Rewind();

      // return the first item in the list
      return current;
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Last() {
      // go to the last item in the list
      current = tail;

      // return the current item
      return current;
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Append() {
      return Append(new ObjectListItem<T>());
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Append(T o) {
      return Append( new ObjectListItem<T>(this, o) );
    }

/*--------------------------------------------------------------------------*/

    public ObjectListItem<T> Append(ObjectListItem<T> item) {
      ObjectListItem<T> tmp;

      if (item == null) return null;

      // emty list
      if (numItems == 0) {
        head = tail = item;
        item.Prev = null;
        item.Next = null;
      }
      else {
        tmp = tail;
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

/*--------------------------------------------------------------------------*/
    // TODO: add some tests to ensure, that the given item is from this list
    public void Disconnect(ObjectListItem<T> item)
    {
      ObjectListItem<T> prev, next;

      prev = item.Prev;
      next = item.Next;

      if (prev != null) {
        if (next != null) {
          prev.Next = next;  // item between two other items removed
          next.Prev = prev;
        }
        else {
          this.tail = prev;   // item at the end of the list removed
          prev.Next = null;
        }
      }
      else {
        if (next != null) {
          this.head = next;  // first item removed
          next.Prev = null;
        }
        else {
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
      if (item == this.current) {
        Rewind();
      }
    }

/*--------------------------------------------------------------------------*/

    public void Delete(ObjectListItem<T> item)
    { 
      Disconnect(item);
    }

  } // end of class

}
