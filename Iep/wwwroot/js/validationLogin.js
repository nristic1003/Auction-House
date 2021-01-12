$(document).ready(function(){

    $("#login").click(function(){
        $username = $("#username").val()
        if($username.length==0) 
        {
            $("#username_error").css({"border":"1px solid red", "display":"block"});
			$("#username").focus();
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


       

    })
})