/*
 * This class is the controller for Main View (Default.aspx)
 * Author: Sohrab Ameli
 * Date: 14 Nov 2015
 */

using System;
using System.Web;
using System.Web.UI;
using System.Text;

namespace OnlineCompiler
{
	public partial class Default : System.Web.UI.Page
	{

		// This method will be called when user clicks on "Run" button from the view
		[System.Web.Services.WebMethod]
		public static string RunCode(string CodeBox)
		{
			byte[] data = Convert.FromBase64String(CodeBox);
			string decodedString = Encoding.UTF8.GetString(data);

			MonoCompiler monoCompiler = new MonoCompiler();

			string resultInJSON = monoCompiler.compileCode(decodedString);
			return resultInJSON;
		}

	}
}

