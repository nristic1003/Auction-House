using System.Runtime.CompilerServices;
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

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Iep.Controllers
{




    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AuctionContext context;
      

        public static bool timerStarted=false;

        public HomeController( AuctionContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.context = context;
         
        }

      
     public async Task<IActionResult> Index(int minPrice, int maxPrice,string currentFilter,string searchString,int? pageNumber,string state="Choose...")
        {
              /*  var auction = from s in context.auction
                        select s;*/
             var x = DateTime.Now.AddHours(0);
             var auction=from s in context.auction select s;
             auction = this.context.auction.OrderByDescending( s=> s.createDate);
            if(state=="Choose...")
            {
             auction=this.context.auction.Include(a => a.winner).Where(s => s.openDate <x && s.closeDate>DateTime.Now && (s.state=="READY" || s.state=="OPEN")).OrderByDescending(s => s.createDate);
                state = "";
                
            } 
            else{
                 switch(state) {
                         case "1": state = "READY";
                         break;
                         case "2": state = "OPEN";
                         break;
                         case "3": state = "EXPIRED";
                         break;
                    }
               } 
              ViewData["state"]=state;           
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
             ViewData["minPrice"]=minPrice;
             ViewData["maxPrice"]=maxPrice;

            ViewData["CurrentFilter"] = searchString;
            
            foreach(var item in auction.ToList())
            {
                
                if(item.state == "READY")
                {      
                    item.state ="OPEN";
                    this.context.Update(item);
                    await this.context.SaveChangesAsync();
                 }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                auction = auction.Include(a => a.winner).Where(s => s.name.Contains(searchString));
            }
            if(minPrice!=0)
            {
                auction = auction.Include(a => a.winner).Where(s => s.currentPrice>= minPrice);
            }
            if(maxPrice!=0)
            {
                auction = auction.Include(a => a.winner).Where(s => s.currentPrice<=maxPrice);
            }
            if(state!="")
            {
                auction = auction.Include(a => a.winner).Where(s => s.state==state);
            }

            int pageSize = 12;
    
            return View(await PaginatedList<Auction>.CreateAsync(auction, pageNumber ?? 1, pageSize));
        }
        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult auctionDetails(int id)
        {
              Auction auction  = this.context.auction.Include( a=> a.winner).Include(a => a.owner).Where( a => a.Id == id).FirstOrDefault();

              if(auction!=null)
              {
                  IList<Bids> bids =  this.context.bids.Include( s => s.user).Where( b => b.auctionId == id).OrderByDescending(b => b.bidDate).Take(5).ToList();
                    ViewBag.userID = bids;
                   return View(auction);
              }
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
