$(document).ready(function(){

    $("#changePass").click(function(){
        $oldPass = $("#oldPass").val()
        if($oldPass.length==0) 
        {
            $("#oldPass_error").css({"border":"1px solid red", "display":"block"});
			$("#oldPass").focus();
            return false;

        }

        $pass = $("#password").val()
        var regEx =  /^(?=[A-Za-z0-9@#$%^&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,}).*$/
        if($pass.length==0 || !regEx.test($pass) )
        {
            $("#pass_error").css({"border":"1px solid red", "display":"block"});
			$("#password").focus();
            return false;
        }

        $confPass = $("#confirmedPass").val()
        if($confPass.length==0 || !regEx.test($confPass) || $pass!=confPass )
        {
            $("#confirmPass_error").css({"border":"1px solid red", "display":"block"});
			$("#confirmedPass").focus();
            return false;
        }


       

    })
})