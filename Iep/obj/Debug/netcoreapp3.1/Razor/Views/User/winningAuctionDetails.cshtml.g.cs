#pragma checksum "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6185437b7fa5fc593a91ddc16c4cf3596445af5f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_winningAuctionDetails), @"mvc.1.0.view", @"/Views/User/winningAuctionDetails.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\_ViewImports.cshtml"
using Iep;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\_ViewImports.cshtml"
using Iep.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6185437b7fa5fc593a91ddc16c4cf3596445af5f", @"/Views/User/winningAuctionDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"03acc3d30d6c79b0b00585f7d97dcd8d5b57a4a2", @"/Views/_ViewImports.cshtml")]
    public class Views_User_winningAuctionDetails : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Iep.Models.Database.Auction>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
  
    var  base64 = Convert.ToBase64String(Model.image);
  


#line default
#line hidden
#nullable disable
            WriteLiteral("    \r\n\r\n\r\n    \r\n<div id = \"forDisable\">\r\n<div class = \"row\">\r\n    <div class = \"col\">\r\n              <div class=\"d-flex justify-content-center \"><h1> ");
#nullable restore
#line 15 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                                                          Write(Model.name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n              </div>  \r\n    </div>\r\n  \r\n</div>\r\n\r\n<div class = \"row\">\r\n    <div class = \"col-sm-2\">  </div>\r\n       <div class = \"col-sm-4\">\r\n             <div class=\"d-flex justify-content-center \">\r\n                  <img class=\"img-fluid\"");
            BeginWriteAttribute("src", " src = \"", 516, "\"", 554, 2);
            WriteAttributeValue("", 524, "data:image/png;base64,", 524, 22, true);
#nullable restore
#line 25 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
WriteAttributeValue(" ", 546, base64, 547, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" >
            </div>
     </div>
       <div class=""col-sm-5 col-md-4 d-flex justify-content-center justify-content-md-start"">
            <table class=""table table-stripped"">
             
                  
                    <tr>
                    <td>Winner:</td>
                    <td> ");
#nullable restore
#line 34 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                    Write(Html.Raw("<span id='winner" + @Model.Id +"'>"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" \r\n");
#nullable restore
#line 35 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                            if(@Model.winner!=null)
                           {
                             

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                        Write(Model.winner.UserName);

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                                                     
                            } else{
                                 

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                            Write(Html.Raw("NULL"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 39 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                                                  ;
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </td>\r\n                       \r\n                </tr>\r\n                 </tr>\r\n                    <tr>\r\n                    <td>Owner:</td>\r\n                    <td>");
#nullable restore
#line 47 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                   Write(Model.owner.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </tr>\r\n                 </tr>\r\n                    <tr>\r\n                    <td>Current price:</td>\r\n                    <td>  ");
#nullable restore
#line 52 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                     Write(Html.Raw("<span id='cena" + @Model.Id +"'>"));

#line default
#line hidden
#nullable disable
            WriteLiteral("  ");
#nullable restore
#line 52 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                                                                    Write(Model.currentPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("  ");
#nullable restore
#line 52 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
                                                                                         Write(Html.Raw("</div>"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@" <span> tokens </span>  </td>
                    
                </tr>
                
            </table>
        </div>
         </div>
 

 
  
  
    
        <div class= ""row my-4""> 
            <div class = ""col-sm-3""></div>
       <div class=""col-sm-3 col-md-5 d-flex justify-content-center justify-content-md-start"">
           <h4>Description:
               </h4>
               
        </div>
    </div>
        <div class= ""row my-4""> 
            <div class = ""col-sm-3""></div>
       <div class=""col-sm-5 col-md-6 d-flex justify-content-center justify-content-md-start"">
            <span>
                 ");
#nullable restore
#line 77 "C:\Users\Nikola\Desktop\IEP_Proj\Iep\Views\User\winningAuctionDetails.cshtml"
            Write(Model.description);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </span>\r\n               \r\n        </div>\r\n        </div>\r\n    \r\n\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Iep.Models.Database.Auction> Html { get; private set; }
    }
}
#pragma warning restore 1591
