$(document).ready(function(){

    $("#regSubmit").click(function(){
       
        var regEx =  /^(?=[A-Za-z0-9@#$%^&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,}).*$/
       
        $firstName = $("#firstName").val()
        if($firstName.length==0) 
        {
            $("#firstName_error").css({"border":"1px solid red", "display":"block"});
			$("#firstName").focus();
            return false;

        }
        $lastName = $("#lastName").val()
        if($lastName.length==0) 
        {
            $("#lastName_error").css({"border":"1px solid red", "display":"block"});
			$("#lastName").focus();
            return false;

        }

        $email = $("#email").val()

        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if($email.length==0 || !regex.test($email)) 
        {
            alert("eail")
            $("#email_error").css({"border":"1px solid red", "display":"block"});
			$("#email").focus();
            return false;

        }

        $gender = $("#lastName").val()
        if($gender.length==0) 
        {
            $("#gender_error").css({"border":"1px solid red", "display":"block"});
			$("#gender").focus();
            return false;

        }

        $username = $("#username").val()
        if($username.length<6) 
        {
            $("#username_error").css({"border":"1px solid red", "display":"block"});
			$("#username").focus();
            return false;

        }

        $password = $("#password").val()
        if($password.length==0 || !regEx.test($password)) 
        {
            $("#password_error").css({"border":"1px solid red", "display":"block"});
			$("#password").focus();
            return false;

        }

        $confirmPass = $("#confirmPass").val()
        if($confirmPass.length==0 || $password!=$confirmPass) 
        {
            $("#confirmPass_error").css({"border":"1px solid red", "display":"block"});
			$("#confirmPass").focus();
            return false;

        }
       

    })
})