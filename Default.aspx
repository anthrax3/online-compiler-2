<%@ Page Language="C#" Inherits="OnlineCompiler.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
	<link href="./bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="style.css" rel="stylesheet">
	<script src="./Scripts/jquery-2.1.4.js"></script>
    <script src="./bootstrap/js/bootstrap.min.js"></script>
	<title>Online C# Compiler</title>
</head>
<body>
    <div class="main">
        <div class="container">
    
        <div class="row">
            <div class="col-lg-7 col-lg-offset-2 col-md-8 col-md-offset-2 col-sm-10 col-sm-offset-1">
            
            <div id="mainHeader" class="text-center">
                    <b>Online C# Compiler</b>
            </div>
            <br>
            
	<form role="form" id="submitCodeFormID" method="post" name="codebox">
		<div class="form-group">
		<label for="textbox1">Code:</label>
		<textarea class="form-control" rows="10" id="textbox1" name="codebox">Main code here</textarea>
<br>
<button id="submitbtnID" value="Run" type="submit" class="btn btn-success">Run Code</button>
		<br/>
		</div>
	</form>

	<div class="form-group">
	  <label for="outputid">Output:</label>
	  <textarea class="form-control" rows="5" id="outputid" readonly="readonly"></textarea>
	</div>
	<!-- <textarea id="outputid" readonly="readonly" rows="10" cols="80">Errors here</textarea> -->


        <script>
        
        // to fix the tab in the textarea:
		$(document).delegate('#textbox1', 'keydown', function(e) {
		  var keyTmp = e.keyCode || e.which;

		  if (keyTmp == 9) {
		    e.preventDefault();
		    var start = $(this).get(0).selectionStart;
		    var end = $(this).get(0).selectionEnd;

		    $(this).val($(this).val().substring(0, start) + "\t" + $(this).val().substring(end));

		    $(this).get(0).selectionStart =
		    $(this).get(0).selectionEnd = start + 1;
		  }
		});

        var programText = "// You can write your C# code here and hit \"Run Code\" to send the code to server and wait for compiling and execution result \n" +
        				"// Your code should always start from a \"main\" method of a class called \"MainClass\" \n"+
        	"class MainClass{ \n" + 
                "\tpublic void Main(){ \n" +
                    "\t\tSystem.Console.WriteLine(\"Hello World\"); \n" +
                "\t} \n" +
            "} \n";

            $('#textbox1').html(programText);
                
            $("#submitCodeFormID").submit(function (e) {
                e.preventDefault();
                //console.log("submitCodeFormID clicked");
                var tmp1 = $("#textbox1").val();
                tmp1 = btoa(tmp1);
                
                $.ajax({
                    url: "Default.aspx/RunCode",
                    type: "POST",
                    data: "{ 'CodeBox': '" + tmp1 + "' }",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                    	var jsonData = jQuery.parseJSON(data.d);
                        //console.log("data: "+ jsonData);
                        //$("#outputid").html(jsonData);
                        printToConsule(jsonData);
                    }
                });

            });
            
            function printToConsule(jsonData){
            	
            	var consoleOut = jsonData.consoleOut;
            	var errors = jsonData["errors"];
            	var criticalErrors = jsonData["criticalErrors"];
            	
            	if(errors == ""){
            		$("#outputid").css('color', 'black');
            		$("#outputid").html(consoleOut);
            	
            	}
            	if(criticalErrors != ""){
            		var msg = "Compile failed:\nYour code took too long to compile. Due to server limits we have restricted" +
            		" compile time to 5 seconds.";
            		$("#outputid").css('color', 'red');
            		$("#outputid").html(msg);
            	}else if(errors != ""){
            		var msg = "Compile failed:\n";
            		$("#outputid").css('color', 'red');
            		$("#outputid").html(msg + errors);
            	}
            	
            	/*
            	console.log("consoleOut: "+ consoleOut);
            	console.log("errors: "+ errors);
            	console.log("criticalErrors: "+ criticalErrors);
            	*/
            }
            

            //});
        </script>
        
       			 <br>
                <div id="footer" class="text-center">
                <a href="https://github.com/sameli/online-compiler">Source Code</a>
                <br>
                    &copy; Sohrab Ameli 2015
                </div>
                <br>
             
            </div>
        
        </div> <!-- end row -->
    </div> <!-- end container -->
</div> <!-- end main -->
</body>
</html>
