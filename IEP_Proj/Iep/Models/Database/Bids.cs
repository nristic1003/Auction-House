using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iep.Models.Database{
    public class Bids {
 
        
        public int price {get; set;} //broj ulozenih tokena fakticki
 
        public DateTime bidDate {get; set;}

        public string userId {get; set;}
        public User user {get; set;}

        public int auctionId {get; set;}
        public Auction auction {get; set;}
        
        
 
    }

       public class BidConfiguration : IEntityTypeConfiguration<Bids>
        {
        public void Configure(EntityTypeBuilder<Bids> builder)
        {
            builder.HasKey(
                entity => new {entity.userId, entity.auctionId}  //Kompozitni kljuc :D
            );

            

        }
    }
 
}