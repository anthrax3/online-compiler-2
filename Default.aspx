<%@ Page Language="C#" Inherits="OnlineCompiler.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<title>Default</title>
</head>
<body>
	<h3>Code:</h3>
	<form id="submitCodeFormID" method="post" name="codebox">
		<div>
		<textarea id="textbox1" rows="10" cols="80" name="codebox">Main code here</textarea>
		<input id="submitbtnID" type="Submit" value="Run" /><br/>
		</div>
	</form>

	<h3>Errors Output:</h3>
	<textarea id="outputid" readonly="readonly" rows="10" cols="80">Errors here</textarea>

</body>
</html>
