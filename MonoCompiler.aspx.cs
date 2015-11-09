using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections;
using Mono.CSharp;

namespace OnlineCompiler
{
	public class MonoCompiler
	{

		public class CompilerOutput{
			public string consoleOut { get; set; }
			public string errors { get; set; }
			public string criticalErrors { get; set; }

			public CompilerOutput(){
				consoleOut = "";
				errors = "";
				criticalErrors = "";
			}
		}

		public static string toJSON(object tmp){
			var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			return jsSerializer.Serialize(tmp);
		}

		public MonoCompiler ()
		{

		}

		public string compileCode(string code){
			/*
			Object result = new Object (); //TODO
			string jsontmp = toJSON (result);
			return jsontmp;
			*/

			CompilerOutput compilerOutput = new CompilerOutput ();
			/*
		var compilerContext = new CompilerContext(new CompilerSettings(), new ConsoleReportPrinter());

		var evaluator = new Evaluator(compilerContext);
		*/
			var reportWriter = new StringWriter();
			var settings = new CompilerSettings();
			var printer = new ConsoleReportPrinter(reportWriter);

			var compilerContext = new CompilerContext (settings, printer);
			var reports = new Report(compilerContext, printer);
			var evaluator = new Evaluator(compilerContext);
			//evaluator.Run ("MainClass m1 = new MainClass(); m1.Main();");
			//evaluator.Compile (mainCode);
		}
	}
}

