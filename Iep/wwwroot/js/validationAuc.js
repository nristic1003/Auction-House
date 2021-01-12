$(document).ready(function(){

    $("#newAuction").click(function(){
        $name = $("#name").val()
        if($name.length==0) 
        {
            $("#name_error").css({"border":"1px solid red", "display":"block"});
			$("#name").focus();
            return false;

        }

        $description = $("#description").val()
      
       
        if($description.length==1 )
        {
         
            $("#description_error").css({"border":"1px solid red", "display":"block"});
			$("#description").focus();
            return false;
        }

        $file = $("#file").val()
      
        if($file.length==0)
        {
            $("#file_error").css({"border":"1px solid red", "display":"block"});
			$("#file").focus();
            return false;
        }


       

    })
    
})