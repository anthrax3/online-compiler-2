/*
 * This class contains the core functionalities for evaluating and executing CSharp code from client
 * Author: Sohrab Ameli
 * Date: 14 Nov 2015
 */

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

		/*
		 * This class contains structure to store compiler results
		 */
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

		static TextWriter originalConsoleOut_global;
		static CompilerOutput compilerOutput_global;
		static int timeout_global;

		// This function converts the given object to a string in JSON format
		public static string toJSON(object tmp){
			var jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			return jsSerializer.Serialize(tmp);
		}

		// Constructor
		public MonoCompiler (){
			timeout_global = 5000; // In milliseconds
		}

		private Boolean isDangerousCode(string code){
			List<string> prohibitedListCaseInsensitive = new List<string> () {
				"System.IO", "IO.File", "IO.Directory",
				"System.Runtime", "Runtime.CompilerServices", "System.Reflection",
				"Microsoft.CSharp", "CSharpCodeProvider", "System.CodeDom.Compiler", "CodeDom.Compiler",
				"System.Management", "Microsoft.Win32", "System.Security", "System.Security.Permissions"
			};

			foreach(string tmp in prohibitedListCaseInsensitive){
				if(code.IndexOf(tmp, StringComparison.OrdinalIgnoreCase) >= 0){
					return true;
				}
			}

			List<string> prohibitedListCaseSensitive = new List<string> () {
				"IO.", "CompilerServices."
			};

			foreach (string tmp2 in prohibitedListCaseSensitive) {
				if (code.Contains (tmp2)) {
					return true;
				}
			}

			return false;
		}

		// This method accepts a string which contains CSharp code then calls another functions to evaluate and execute the code
		// Output of this function is a string in JSON format
		public string compileCode(string code){
			CompilerOutput result = new CompilerOutput();
			if (isDangerousCode (code)) {
				result.errors = "Possible dangerous code";
			} else {
				result = run (code);
			}

			string jsontmp = toJSON (result);
			return jsontmp;
		}

		// Input: code to compile, return: CompilerOutput object
		public CompilerOutput run(string code){
			string result = "";
			Thread workerThread = new Thread(() => threadedFunction(code));
			workerThread.Start();

			bool finished = workerThread.Join(TimeSpan.FromMilliseconds(timeout_global));
			if (finished) {
				Console.WriteLine("Worker thread finished.");
				return compilerOutput_global;

			} else {
				workerThread.Abort ();
				if (originalConsoleOut_global != null) {
					Console.SetOut (originalConsoleOut_global);
					compilerOutput_global.criticalErrors = "RESTORED CONSOLE.\n";
				}
				compilerOutput_global.criticalErrors += "Worker thread was aborted.\n";

				return compilerOutput_global;
			}

			return compilerOutput_global;
		}

		// This method runs within a thread to compile code
		static void threadedFunction(string code)
		{
			//Thread.Sleep(1000);

			try{
				compilerOutput_global = evaluateCode (code);
			}catch(Exception ex){
				//System.Console.WriteLine("Caught: " + ex.ToString());

			}
		}


		// This method evaluates the given code and returns a CompilerOutput object
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

			var myString = "";
			originalConsoleOut_global = Console.Out; // preserve the original stream
			using(var writer = new StringWriter())
			{
				Console.SetOut(writer);

				evaluator.Run (code);
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

			Console.SetOut(originalConsoleOut_global); // restore Console.Out

			return compilerOutput;
		}

	}

}

