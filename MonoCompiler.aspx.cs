using System;

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
			Object result = new Object (); //TODO
			string jsontmp = toJSON (result);
			return jsontmp;
		}
	}
}

