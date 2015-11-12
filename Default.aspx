<%@ Page Language="C#" Inherits="OnlineCompiler.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<link href="./bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="style.css" rel="stylesheet">
	<script src="./Scripts/jquery-2.1.4.js"></script>
    <script src="./bootstrap/js/bootstrap.min.js"></script>
	<title>Default</title>
</head>
<body>
    <div class="main">
        <div class="container">
    
        <div class="row">
            <div class="col-lg-7 col-lg-offset-2 col-md-8 col-md-offset-2 col-sm-10 col-sm-offset-1">
            
            
	<h3>Code:</h3>
	<form role="form" id="submitCodeFormID" method="post" name="codebox">
		<div class="form-group">
		<textarea class="form-control" rows="10" id="textbox1" name="codebox">Main code here</textarea>
<br>
<button id="submitbtnID" value="Run" type="submit" class="btn btn-success">Run Code</button>
		<br/>
		</div>
	</form>

	<div class="form-group">
	  <label for="comment">Output:</label>
	  <textarea class="form-control" rows="5" id="outputid" readonly="readonly"></textarea>
	</div>
	<!-- <textarea id="outputid" readonly="readonly" rows="10" cols="80">Errors here</textarea> -->


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
                    url: "Default.aspx/RunCode",
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
        
       			 <br>
                <div id="footer" class="text-center">
                    &copy; Sohrab Ameli 2015
                </div>
                <br>
             
            </div>
        
        </div> <!-- end row -->
    </div> <!-- end container -->
</div> <!-- end main -->
</body>
</html>
