using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections;
using Mono.CSharp;
using System.Threading;

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


		static TextWriter originalConsoleOut;
		static CompilerOutput compilerOutput_global;

		public static string toJSON(object tmp){
			var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			return jsSerializer.Serialize(tmp);
		}

		public MonoCompiler ()
		{

		}

		public string compileCode(string code){
			CompilerOutput result = run(code);
			string jsontmp = toJSON (result);
			return jsontmp;
		}

		// input: code to compoile, return: json
		public CompilerOutput run(string code){
			string result = "";
			Thread workerThread = new Thread(() => LongRunningOperation(code));
			workerThread.Start();

			bool finished = workerThread.Join(TimeSpan.FromMilliseconds(1000));
			if (finished) {
				Console.WriteLine("Worker thread finished.");
				return compilerOutput_global;

			} else {
				workerThread.Abort ();
				if (originalConsoleOut != null) {
					Console.SetOut (originalConsoleOut);
					compilerOutput_global.criticalErrors = "RESTORED CONSOLE.\n";
				}
				compilerOutput_global.criticalErrors += "Worker thread was aborted.\n";

				return compilerOutput_global;
			}

			return compilerOutput_global;
		}


		static void LongRunningOperation(string code)
		{
			//Thread.Sleep(1000);

			try{
				compilerOutput_global = evaluateCode (code);
			}catch(Exception ex){
				//System.Console.WriteLine("catched: " + ex.ToString());

			}
		}

		public static CompilerOutput evaluateCode (string code)
		{

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



			string mainCode = code;


			//bool ress;
			//object res;
			//evaluator.Run ("using System; using System.Linq;");


			var myString = "";
			originalConsoleOut = Console.Out; // preserve the original stream
			using(var writer = new StringWriter())
			{
				Console.SetOut(writer);

				//Console.WriteLine("some stuff"); // or make your DLL calls :)
				evaluator.Run (mainCode);
				evaluator.Run ("MainClass m1 = new MainClass(); m1.Main();");

				//bConsole.WriteLine ("after executing code");

				if (reports.Errors > 0) {
					Console.WriteLine ("reportWriter.ToString: \n" + reportWriter.ToString ());
					compilerOutput.errors = reportWriter.ToString ();
				}

				writer.Flush(); // make sure everything is written out of consule

				myString = writer.GetStringBuilder().ToString();

				compilerOutput.consoleOut = myString;

			}

			Console.SetOut(originalConsoleOut); // restore Console.Out

			//CompiledMethod t1 = evaluator.Compile (mainCode);
			//string s = evaluator.Evaluate (mainCode, out res, out ress);
			//s += evaluator.Evaluate ("MainClass m1 = new MainClass(); m1.Main();", out res, out ress);

			//Console.WriteLine ("result: " + s);

			/*
		foreach (var v in (IEnumerable) res){
			Console.Write (v);
			Console.Write (' ');
		}
		*/
			//evaluator.Run ("MainClass m1 = new MainClass(); m1.Main();");
			//evaluator.Compile (mainCode);


			/*
		Console.WriteLine ("aaaa");
		Console.WriteLine (">> " + myString);
		Console.WriteLine ("compilerOutput.errors: " + compilerOutput.errors);
		Console.WriteLine ("compilerOutput.consoleOut: " + compilerOutput.consoleOut);
		*/


			return compilerOutput;
		}

	}

}

