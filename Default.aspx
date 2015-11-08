<%@ Page Language="C#" Inherits="OnlineCompiler.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<script src="Scripts/jquery-2.1.4.js"></script>
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


        <script>

        var programText = "class MainClass{ \n" + 
                "\tpublic void Main(){ \n" +
                    "\t\tSystem.Console.WriteLine(\"Hello World\"); \n" +
                "\t} \n" +
            "} \n";

            $('#textbox1').html(programText);
                
            $("#submitCodeFormID").submit(function (e) {
                e.preventDefault();
                console.log("submitCodeFormID clicked");
                var tmp1 = $("#textbox1").val();
                tmp1 = btoa(tmp1);
                
                $.ajax({
                    url: "Default.aspx/CompileStr",
                    type: "POST",
                    data: "{ 'CodeBox': '" + tmp1 + "' }",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        console.log("data here: "+ data.d);
                        $("#outputid").html(data.d);
                    }
                });

            });
            

            //});
        </script>
</body>
</html>
