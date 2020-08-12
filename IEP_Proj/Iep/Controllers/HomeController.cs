using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Iep.Models;
using Iep.Models.Database;

namespace Iep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
          private AuctionContext context;

        public HomeController( AuctionContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> AuctionPreview(int pageNumber =1)
        {    
             var data = context.auction;   
            return View(await PaginatedList< Auction>.CreateAsync(data, pageNumber,12));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
