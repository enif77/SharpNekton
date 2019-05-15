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

using SharpNekton.Evaluator;
using SharpNekton.Evaluator.Values;
using SharpNekton.Shared;

namespace SharpNekton.Libraries
{
    /// <summary>
    /// Represents a delegate (a refference to a function) for a build-in library function
    /// to be called from a script.
    /// </summary>
    /// <param name="numParams">The number of desired function parameters, that has to be passed to
    /// this function. 0 means no parameters, >0 means 1 to n parameters, -1 represents 0 or more
    /// parameters and -2 and less means n required parameters and m optional parameters 
    /// (Ex.: -3 means two required parameters and unlimited amount of optional parameters = f(a, b, ...).)</param>
    /// <returns>The library function should return a boolean true, if it's execution was successfull,
    /// false otherwise.</returns>
    public delegate bool LibraryFunction(int numParams);


    /// <summary>
    /// The base abstract class for creating build-in function libraries.
    /// </summary>
    public abstract class ALibrary
    {
        protected string name;
        protected ScriptState state;
        protected ValueTable libraryData;


        public ALibrary(ScriptState state)
        {
            this.name = "library";
            this.state = state;
            this.libraryData = new ValueTable();
        }


        public string Name
        {
            get
            {
                return name;
            }
        }


        public ValueTable LibraryData
        {
            get
            {
                return libraryData;
            }
        }


        public override string ToString()
        {
            return "Library: " + name;
        }


        public ValueStore RegisterGlobalObject(string name)
        {
            // TODO: check for object redefinition
            return state.RegisterGlobalObject(name);
        }


        public void RegisterGlobalFunction(string name, LibraryFunction functionRef, int numberOfRequiredParameters)
        {
            ExternalFunctionRef extFuncRef = new ExternalFunctionRef(name, functionRef, numberOfRequiredParameters);

            // TODO: check for object redefinition
            ValueStore functionObject = state.RegisterGlobalObject(name);
            functionObject.Value = new ExternalFunctionRefValue(extFuncRef);
        }


        public ValueTableItem RegisterObject(string name)
        {
            // TODO: check for object redefinition
            return libraryData.Insert(name, new UndefinedValue());
        }


        public void RegisterNumericConstant(string name, double n)
        {
            // TODO: check for object redefinition
            libraryData.Insert(name, new NumericValue(n));
        }


        public void RegisterFunction(string name, LibraryFunction functionRef, int numberOfRequiredParameters)
        {
            ExternalFunctionRef extFuncRef = new ExternalFunctionRef(name, functionRef, numberOfRequiredParameters);

            // TODO: check for object redefinition
            libraryData.Insert(name, new ExternalFunctionRefValue(extFuncRef));
        }


        public abstract void Register();
        public abstract void UnRegister();

    } // end of class
} // end of namespace
