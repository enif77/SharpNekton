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

namespace SharpNekton
{
    using System;
    using System.Collections.Generic;

    using SharpNekton.Compiler;
    using SharpNekton.Compiler.Sources;
    using SharpNekton.Evaluator;
    using SharpNekton.Evaluator.Values;
    using SharpNekton.Libraries;
    using SharpNekton.Evaluator.OpCodes;
    using SharpNekton.Shared;


    // the print opcode support function:
    // through this function goes all the output of the O_PRINT
    // opcode
    public delegate void DPrintFCallBack(string output);

    public class ScriptState
    {
        public const string BASE_LF_VAR_NAME = "EOL";

        private readonly ParserState _parser;
        private readonly EvaluatorState _evaluator;
        private readonly Dictionary<string, string> _fileSources;
        private bool _sourceLoaded;
        private readonly Dictionary<string, ALibrary> _libraries;

        private DPrintFCallBack _printFCallBack;


        public ScriptState()
        {
            _parser = new ParserState();
            _evaluator = new EvaluatorState(this);
            _fileSources = new Dictionary<string, string>();
            _sourceLoaded = false;
            _libraries = new Dictionary<string, ALibrary>();

            _printFCallBack = DefaultPrintF;

            // register all basic _libraries
            RegisterLibrary(new BaseLib(this));
            RegisterLibrary(new MathLib(this));
        }

        /*--------------------------------------------------------------*/

        public ParserState Parser
        {
            get
            {
                return _parser;
            }
        }


        public EvaluatorState Evaluator
        {
            get
            {
                return _evaluator;
            }
        }

        /*--------------------------------------------------------------*/

        public void RegisterLibrary(ALibrary library)
        {
            // TODO: add something interesting here
            if (library == null) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));

            // do not allow to library reregistration
            if (_libraries.ContainsKey(library.Name))
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.LIBRARY_REREGISTRATION));
            }

            // add the new library to the library stack
            _libraries.Add(library.Name, library);

            // add a refference to the library to a global object "library.Name"
            var libraryObject = RegisterGlobalObject(library.Name);
            libraryObject.Value = new TableRefValue(library.LibraryData);

            // let the library register it's members
            library.Register();
        }


        public void UnregisterLibrary(string name)
        {
            // TODO: add more specific code here
            if (String.IsNullOrEmpty(name)) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));

            if (_libraries.ContainsKey(name))
            {
                _libraries[name].UnRegister();
                _libraries.Remove(name);

                // TODO: remove the library refference from the global object
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.LIBRARY_NOT_FOUND));
            }
        }

        /*--------------------------------------------------------------*/

        public ValueStore RegisterGlobalObject(string name)
        {
            return _evaluator.AddGlobalObject(name);
        }

        /*--------------------------------------------------------------*/

        public void LoadString(string source)
        {
            // TODO: add more specific code here
            if (source == null) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));

            _parser.Source = new StringSource(source);
            _parser.Compile(this);
            _sourceLoaded = true;
        }


        /// <summary>
        /// Loads and prepares to run a source file.
        /// </summary>
        /// <param name="fileName">A file to be loaded.</param>
        public void LoadFile(string fileName)
        {
            // TODO: add more specific code here
            if (fileName == null) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));

            if (_fileSources.ContainsKey(fileName) == false)
            {
                _fileSources.Add(fileName, fileName);  // TODO: add something interesting here
            }

            _parser.Source = new FileSource(fileName);
            _parser.Compile(this);
            _sourceLoaded = true;
        }


        public void ImportFile(string fileName)
        {
            // TODO: add more specific code here
            if (fileName == null) throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.UNSPECIFIED_ERROR));

            if (_fileSources.ContainsKey(fileName) == false)
            {
                _fileSources.Add(fileName, fileName);  // TODO: add something interesting here

                _parser.Source = new FileSource(fileName);
                _parser.Compile(this);
                _sourceLoaded = true;
            }
            else {
                // create a dummy code with the SUBEND opcode only
                var dummyCodePart = _evaluator.NewCodePart();
                dummyCodePart.Append(new SubEndOpCode(0, 0));

                // set reg_R to a pointer to the new compiled program
                _evaluator.RegR = new ProgramRefValue(dummyCodePart);
                _sourceLoaded = true;
            }
        }

        /*--------------------------------------------------------------*/

        /// <summary>
        /// Avaluates currently loaded and compiled source.
        /// </summary>
        public void Evaluate()
        {
            if (_sourceLoaded)
            {
                _evaluator.Evaluate(this);
            }
            else
            {
                throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_NOCODE));
            }
        }

        /*--------------------------------------------------------------*/

        public DPrintFCallBack PrintFCallBack
        {
            get
            {
                return _printFCallBack;
            }

            set
            {
                _printFCallBack = value ?? DefaultPrintF;
            }
        }


        private void DefaultPrintF(string output)
        {
            if (output == null) return;
            
            Console.Write(output);
        }

        /*--------------------------------------------------------------*/

    }  // end of class
} // end of namespace
