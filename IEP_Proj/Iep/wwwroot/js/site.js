// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var eventID;
var x =0;
$(document).ready(function() {
    $(".buyTokens").click(function(event) {
        alert(event.target.id);
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
        var price = x.toString();
     alert(x);

    });
    
    paypal.Buttons ( {
        createOrder: function ( data, actions ) {
            return actions.order.create ( {
                purchase_units: [{
                    amount: { 
                        value: x
                    }
                }]
            } )
        },
        onApprove: function ( data, actions ) {
            return actions.order.capture ( ).then (
                function ( details ) {
                    alert ( "SUCCESS " + details.payer.name.given_name)
                    buyTokens(); //i ovde kada je PayPal uspeo, prikaze se alert da je sve uspesno, obavi se upisu bazu i refreshuje se stranica :D moz
 
                }
            )
        }
    } ).render ( "#paypal" )
       
    function buyTokens ( ) {
        var verificationToken = $("input[name='__RequestVerificationToken']").val ( ) //Znaci ovde ga dohvatimo, on se nalazi na svakoj starnici
        alert(eventID);
        $.ajax ({  //on se ovako pise
            type: "POST", //koji je tip zahteva POST
            url: "/User/buyTokens", //Sta se pozviva, /ControllerName/ControllerMethodName
            data: { //Podaci koji se salju metodi kontrolera i to ka JSON objekat
                "bagName": eventID, //to je onaj parametar sto on trazi bagName
                "__RequestVerificationToken" : verificationToken // i za kraj taj token za verifikaciju
            },
            success: function ( response ) {
                localStorage.removeItem()
                location.reload(); //Sta se desava kada je metoda uspesna, kada vrati JSON true objekat, ja sam mu reko da se stranice refreshuje, kako bi se osvezilo stanje, mozemo to da uradimo i na mnogo bolji nacin, provbacemo, brzo se radi
            },
            error: function ( response ) {
                alert ( response ) // i sta se desava kada funkcija nije uspela, ispisace se alert :D
            }
        })
    }
        
    });
