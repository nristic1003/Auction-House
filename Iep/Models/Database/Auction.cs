using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iep.Models.Database{
    public class Auction {
 
        [Key]
        public int Id{get; set;}
        [Required]
        [MaxLength(20)]
        
        public string name{get; set;}
 
          [Required]
       
        public string description{get; set;}
 
        [Required]
        public byte[] image{get; set;}
        
        [Required]
        public int startPrice{get; set;}

        [Required]
        public int currentPrice{get; set;}
 
        [Required]
        public DateTime createDate{get; set;}
 
        [Required]
        public DateTime openDate{get; set;}
 
        [Required]
        public DateTime closeDate{get; set;}
 
        [Required]
        public string state{get; set;} 
        public User winner {get; set;} 
        public User owner {get; set;}

        public ICollection<Bids> BidList{get; set;}

        [Timestamp]
        public byte[] RowVersion { get; set; }
        


    }


    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.Property(auction => auction.Id).ValueGeneratedOnAdd();
            
            
        }
    }
}