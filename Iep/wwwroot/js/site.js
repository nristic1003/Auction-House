// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



var connection = new signalR.HubConnectionBuilder ( ).withUrl ( "/update" ).build ( );

function handleError(error)
{
    alert(error)
}
connection.start().catch(handleError);

connection.on (
    "updateAuction",
    function (username, price, id ,closeDate ) {
            $( "#cena"+id).text(price + " din")
            $("#winner"+id).text(username)
            $("#time"+id ).parent().find(">:first-child").val(closeDate)
       
    }
)

$(document).ready(function() {

   
    });
 

    function bid()
    {
        var x ="" +event.target.id ;
        var stringNumber = $( "#cena"+x).text();
        var price = parseInt(stringNumber)
        var id = parseInt(event.target.id)
        var verificationToken = $("input[name='__RequestVerificationToken']").val ( )




        if(verificationToken==undefined)
        {
            $("#userLog").text("Morate se ulogovati da bi bidovali");
            $("#userLog").css({"border":"1px solid red", "display":"block" , "text-align":"center"});
            return false
        }


       var span = $("#time"+x ).parent().find(">:first-child");
       var now = new Date(); //now
      
       var newDate = new Date(span.val()) //closeDate
      alert(newDate-0);
       
       if((newDate-now) <10000) {
        newDate.setSeconds(newDate.getSeconds(),10000-newDate);
        
       }
      // newDate.setSeconds(10);

        $.ajax({
            type: "POST",
            url: "https://localhost:5001/User/bid",
            data: {
                "id": event.target.id,
                "price":price,
                "__RequestVerificationToken" : verificationToken,
                "newDate": newDate.toUTCString()
            },
            dataType:"json",
            success:function(data)
            {
                if(data.success)
                {
                    connection.invoke ( "auctionBid", data.winner, data.currPrice, id,newDate.toUTCString() ).catch ( handleError )
                }else{
                    $("#userLog").text("Nemate dovoljno tokena");
                    $("#userLog").css({"color":"red"});
                }
            
            },
            error: function(response)
            {
            }
        })
    }

    function expired(id){
       
        $.ajax({
            type: "POST",
            url: "User/expired",
            data: {
                "id": id
            },
            success:function(response)
            {
               
            },
            error: function(response)
            {
               
            }
        })
    }

    function bidAuctionDetails(){
         var x ="" +event.target.id ;
        var stringNumber = $( "#cena"+x).text();
       
        var price = parseInt(stringNumber)
        console.log(price);
        
        var id = parseInt(event.target.id)
        var myBid = $("#auctionDetailsBid").val();
        var flag =  $("#flag").val();
        var verificationToken = $("input[name='__RequestVerificationToken']").val ( )
        if(verificationToken==undefined)
        {
            $("#badPrice").text("You must be logged in" );
            $("#badPrice").css({"border":"1px solid red", "display":"block" , "text-align":"center"});
           
            return false
        }
        if(myBid<0)
        {
            $("#badPrice").text("You don't have enough tokens");
            $("#badPrice").css({"border":"1px solid red", "display":"block" , "text-align":"center"});
            $("#flag").val(1);
            return false;
        }

        if(flag==1)
        {
            $("#flag").val();
            $("#badPrice").empty();
        }

        
        
       var span = $("#time"+x ).parent().find(">:first-child");
         
       var now = new Date(); //now
      
       var newDate = new Date(span.val()) //closeDate
       if((newDate-now) <10000) {
        newDate.setSeconds(newDate.getSeconds(),10000);
        
       }
       newDate.setSeconds(newDate.getSeconds(),10000);


        $.ajax({
            type: "POST",
            url: "https://localhost:5001/User/bid",
            data: {
                "id": id,
                "price":price,
                "myBid":myBid,
                "__RequestVerificationToken" : verificationToken,
                "newDate": newDate.toUTCString()

            },
            dataType:"json",
            success:function(data)
            {
                if(data.success)
                {
                    connection.invoke ( "auctionBid", data.winner, data.currPrice, id,newDate.toUTCString() ).catch ( handleError )
                }else{
                    $("#userLog").text("Nemate dovoljno tokena");
                    $("#userLog").css({"color":"red"});
                }
            
            },
            error: function(response)
            {
            }
        })
    }

    




