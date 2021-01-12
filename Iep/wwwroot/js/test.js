
var eventID;
var x =0;

$(document).ready(function(){
  
    $(".buyTokens").click(function(event) {
       
        eventID = event.target.id;
        
        switch(event.target.id)
        {
            case "silver": x = 10;
            break;
            case "gold": x = 100;
            break;
            case "platinum": x = 200;
            break;

        }
        var price =x
        $("#cart").prepend("<tr><td>"+ eventID +"</td> <td>" + price + "</td></tr>");
       var currPrice =  $("#price").text()
       currPrice = parseInt(currPrice)+price;
       $("#price").text(currPrice)
      // alert(currPrice+price)
     

    });
    
    paypal.Buttons ( {
        createOrder: function ( data, actions ) {
            return actions.order.create ( {
                purchase_units: [{
                    amount: { 
                        value: $("#price").text()
                    }
                }]
            } )
        },
        onApprove: function ( data, actions ) {
            return actions.order.capture ( ).then (
                function ( details ) {
                    alert ( "SUCCESS " + details.payer.name.given_name)
                    buyTokens();
 
                }
            )
        }
    } ).render ( "#paypal" )

})
function buyTokens ( ) {
    var verificationToken = $("input[name='__RequestVerificationToken']").val ( ) 
    $.ajax ({ 
        type: "POST", 
        url: "/User/buyTokens", 
        data: {
            "option": eventID, 
            "__RequestVerificationToken" : verificationToken 
        },
        success: function ( response ) {
           
        },
        error: function ( response ) {
            alert ( response ) 
        }
    })
}
