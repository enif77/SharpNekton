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

namespace SharpNekton.Compiler
{
    using System.Collections;

    using SharpNekton.Shared;


    public class LocalObject
    {
        public string Name { get; }
        public ObjectLevelID Level { get; }
        public int Offset { get; set; }


        public LocalObject(string name, ObjectLevelID level, int offset)
        {
            Name = name;
            Level = level;
            Offset = offset;
        }
    } 


    public class LocalsStack
    {
        private Hashtable formalParameters;  // formal parameters
        private Hashtable localVariables;   // local variables
        private int localOffset;

        public LocalsStack()
        {
            formalParameters = new Hashtable();
            localVariables = new Hashtable();
            localOffset = 1;  // localOffset - 1 = number of local variables
        }


        public Hashtable FormalParameters
        {
            get
            {
                return formalParameters;
            }
        }


        public Hashtable LocalVariables
        {
            get
            {
                return localVariables;
            }
        }


        public int LocalOffset
        {
            get
            {
                return localOffset;
            }

            set
            {
                localOffset = value;
            }
        }


        public LocalObject FindLocalObject(string name)
        {
            //Console.WriteLine(">> findloc: {0}", name);

            // try local variables
            if (localVariables.ContainsKey(name))
            {
                return (LocalObject)localVariables[name];
            }
            // try formal parameters
            else if (formalParameters.ContainsKey(name))
            {
                return (LocalObject)formalParameters[name];
            }
            else
            {
                return null;
            }
        }


        public LocalObject AddLocalObject(string name, ObjectLevelID level, int offset)
        {
            LocalObject localObject = FindLocalObject(name);
            if (localObject != null && localObject.Level == level)
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_LOCALSYMREDEF));
            }

            if (level == ObjectLevelID.FORMAL_PAREMETER)
            {
                formalParameters.Add(name, new LocalObject(name, level, offset));
                localObject = (LocalObject)formalParameters[name];
            }
            else if (level == ObjectLevelID.LOCAL_VARIABLE)
            {
                localVariables.Add(name, new LocalObject(name, level, offset));
                localObject = (LocalObject)localVariables[name];
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_BADLOCALSYMLEVEL));
            }

            return localObject;
        }


        public int GetLocalObjectsOffset(string name)
        {
            LocalObject localObject = FindLocalObject(name);
            if (localObject != null)
            {
                return localObject.Offset;
            }
            else
            {
                return 0;
            }
        }

    } // end of class
} // end of namespace
