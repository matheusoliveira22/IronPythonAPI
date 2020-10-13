using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;

namespace IronPythonAPI
{
    public class PythonExec
    {
        private static PythonExec _pythonExec = null;
        private static readonly object locker = new object();

        private ScriptRuntimeSetup setup;
        private ScriptRuntime runtime;
        private ScriptEngine engine;
        private Dictionary<String, PythonProgram> programs;
        
        public class PythonProgram
        {
            public String programName {get; private set;}
            public ScriptSource source {get; private set;}
            public CompiledCode compiled {get; private set;}
            public ScriptScope scope {get; private set;}

            public PythonProgram(String programName, ScriptScope scope, ScriptSource source, CompiledCode compiled=null)
            {
                this.programName = programName;
                this.source = source;
                this.scope = scope;

                if(compiled is null)
                    this.compiled = source.Compile();
                else
                    this.compiled = compiled;
            }

            public void Execute()
            {
                compiled.Execute(scope);
            }

            public void SetVariable(String varName, object value)
            {
                scope.SetVariable(varName, value);
            }

            public object GetVariable(String varName)
            {
                try
                {
                    return scope.GetVariable(varName);
                }
                catch
                {
                    return null;
                }
            }
        }

        private PythonExec()
        {
            setup = Python.CreateRuntimeSetup(null);
            runtime = new ScriptRuntime(setup);
            engine = Python.GetEngine(runtime);
            programs = new Dictionary<string, PythonProgram>();
        }

        public static PythonExec GetInstance()
        {
            lock(locker)
            {
                if(_pythonExec == null) _pythonExec = new PythonExec();
                return _pythonExec;
            }
        }

        public void AddProgram(String programName, String sourceCode)
        {
            ScriptScope scope = engine.CreateScope();
            ScriptSource source = engine.CreateScriptSourceFromString(sourceCode);
            PythonProgram pgm = new PythonProgram(programName, scope, source);

            programs.Add(programName, pgm);
        }

        public void Execute(String programName)
        {
            programs[programName].Execute();
        }

        public PythonProgram GetProgram(String programName)
        {
            PythonProgram atual = programs[programName];
            ScriptScope scope = engine.CreateScope();
            ScriptSource source = atual.source;
            CompiledCode compiled = atual.compiled;

            PythonProgram pgm = new PythonProgram(programName,scope,source,compiled);
            return pgm;
        }

        public List<String> GetAllProgramsNames()
        {
            List<String> pgms = new List<String>();

            foreach(KeyValuePair<String, PythonProgram> p in programs)
            {
                pgms.Add(p.Key);
            }

            return pgms;
        }
    }
}
