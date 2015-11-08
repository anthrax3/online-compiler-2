using System;
using System.Web;
using System.Web.UI;
using System.Text;

namespace OnlineCompiler
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Default : System.Web.UI.Page
	{

		[System.Web.Services.WebMethod]
		public static string CompileStr(string CodeBox)
		{
			return "compiler result";
		}

	}
}

