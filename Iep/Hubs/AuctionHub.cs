using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Iep.Hubs{
    public class AuctionHub:Hub{

        public async Task auctionBid(string username, int price, int id, String closeDate)
        {
          await Clients.All.SendAsync("updateAuction", username, price,id,closeDate);
        }
    }
}