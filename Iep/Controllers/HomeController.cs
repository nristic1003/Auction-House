using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Iep.Models;
using Iep.Models.Database;
using System.Net.Mime;
using System.Threading;

namespace Iep.Controllers
{
    public class HomeController : Controller
    {

    
        private readonly ILogger<HomeController> _logger;
          private AuctionContext context;
          private Timer timer;


        public HomeController( AuctionContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.context = context;
            this.timer = null;
        }


     class Check{

        

         public Check ()
         {

         } 

         public void checkDate(List<DateTime> dates)
         {
            DateTime mostRecent= dates.First();
            if(DateTime.Now == mostRecent)
            {
                dates.Remove(mostRecent);
                // Change Auction state


            }

         }

     }
class StatusChecker
{
    private int invokeCount;
    private int  maxCount;

    public StatusChecker(int count)
    {
        invokeCount  = 0;
        maxCount = count;
    }

    // This method is called by the timer delegate.
    public void CheckStatus(Object stateInfo)
    {
        //AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
        Console.WriteLine("{0} Checking status {1,2}.", 
            DateTime.Now.ToString("h:mm:ss.fff"), 
            (++invokeCount).ToString());

        if(invokeCount == maxCount)
        {
            // Reset the counter and signal the waiting thread.
            invokeCount = 0;
            //autoEvent.Set();
        }
    }
}
     public async Task<IActionResult> Index(
            int minPrice,
            int maxPrice,
            string currentFilter,
            string searchString,
            int? pageNumber,
            string state="Choose...")
        {

           var autoEvent = new AutoResetEvent(false);
        
        var statusChecker = new StatusChecker(10);

        // Create a timer that invokes CheckStatus after one second, 
        // and every 1/4 second thereafter.
        Console.WriteLine("{0:h:mm:ss.fff} Creating timer.\n", 
                          DateTime.Now);
        var stateTimer = new Timer(statusChecker.CheckStatus, 
                                   autoEvent, 1000, 250);

     

    Console.WriteLine(state);
   if(state=="Choose...") state = "";
   else{
       switch(state)
       {
           case "1": state = "READY";
           break;
           case "2": state = "OPEN";
           break;
           case "3": state = "EXPIRED";
           break;

       }
   }
    

    if (searchString != null)
    {
        pageNumber = 1;
    }
    else
    {
        searchString = currentFilter;
    }

    ViewData["CurrentFilter"] = searchString;

    var auction = from s in context.auction
                   select s;
    if (!String.IsNullOrEmpty(searchString))
    {
        auction = auction.Where(s => s.name.Contains(searchString));
    }
    if(minPrice!=0)
    {
        auction = auction.Where(s => s.currentPrice>= minPrice);
    }
    if(maxPrice!=0)
    {
        auction = auction.Where(s => s.currentPrice<=maxPrice);
    }
    if(state!="")
    {
        auction = auction.Where(s => s.state==state);
    }

   

    int pageSize = 12;
    return View(await PaginatedList<Auction>.CreateAsync(auction, pageNumber ?? 1, pageSize));
}
        public IActionResult Privacy()
        {

            return View();
        }

 
        public async Task<IActionResult> AuctionPreview(int pageNumber =1)
        {    
            var data = context.auction;   
            PaginatedList< Auction> auc =   await PaginatedList< Auction>.CreateAsync(data, pageNumber,4);
            if(pageNumber>auc.TotalPages || pageNumber<0)
            return null;
            else{
                 return PartialView(auc);
            }
           
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
