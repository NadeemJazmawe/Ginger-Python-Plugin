﻿using Amdocs.Ginger.Plugin.Core;
using System;
using System.Diagnostics;
using System.IO;


using IronPython.Hosting;
using IronPython.Runtime;
using IronPython;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting;
using System.Collections.Generic;

namespace GingerPythonPlugin
{
    [GingerService("Python", "Run Python")]
    public class GingerPythonService : IGingerService, IStandAloneAction
    {

        [GingerAction("RunPythonFile", "Run Python file" )]
        public ScriptScope RunPython(IGingerAction GA, string PythonFile, List<String> LibList)
        {
            Console.WriteLine("start RunPython");
            ScriptScope scope = null;
            if (PythonFile != null)
            {
                var engine = Python.CreateEngine();
                if (LibList != null && LibList.Count > 0)
                {
                    var searchPaths = engine.GetSearchPaths();
                    foreach (String lib in LibList)
                        searchPaths.Add(@lib);
                    engine.SetSearchPaths(searchPaths);
                }
                scope = engine.CreateScope();

                Console.WriteLine("Run filename- " + PythonFile);
                ScriptSource source = engine.CreateScriptSourceFromFile(PythonFile);
                object result = source.Execute(scope);
                // GA.AddOutput(string param, object value, string path = null);

           //    engine.Runtime.Globals.SetVariable("property_value", "TB Test");
           //     scope.SetVariable("name","value");


            }
            else
                Console.WriteLine("Please provide python file name");

            Console.WriteLine("End RunPython");


            return scope;
        }


        [GingerAction("RunPython", "Run Python script")]
        public ScriptScope RunPythonScript(IGingerAction GA, string script, List<String> LibList)
        {
            ScriptScope scope = null;
            Console.WriteLine("Start RunPythonScript");

            var engine = Python.CreateEngine();
            if (LibList != null && LibList.Count > 0)
            {
                var searchPaths = engine.GetSearchPaths();
                foreach (String lib in LibList)
                    searchPaths.Add(@lib);
                engine.SetSearchPaths(searchPaths);
            }
            ScriptSource source = engine.CreateScriptSourceFromString(script);
            source.Execute();

            Console.WriteLine("End RunPythonScript");
            return scope;
        }

        [GingerAction("RunScript2", "Run Script 2")]
        public void RunScript2(IGingerAction GA, string script)  // replace with List , string[] vars
        {
            Console.WriteLine("start RunPythonScript");
            
            var engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            // scope.SetVariable("A", 4);
            // scope.SetVariable("B", 3);
            

            engine.Execute(script, scope);

            var varsOut = scope.GetItems();
            foreach (var v in varsOut)
            {
                if (v.Key.StartsWith("__")) continue; // ignore internal vars
                GA.AddOutput(v.Key, v.Value);
            }
            // engine.Execute("print A;print B; sum=A+B", scope);
            // int sum = scope.GetVariable("sum");
            
            Console.WriteLine("End RunPythonScript");
        }

    }
}
