using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Iep.Models.View;
using Microsoft.AspNetCore.Identity;

namespace Iep.Models.Database
{
    public class User:IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string firstName{get; set;}
        [Required]
        [MaxLength(20)]
        public string lastName{get; set;}
        [Required]
        [MaxLength(1)]
        [Column(TypeName = "varchar(1)")]
        public char gender{get; set;}
        [Required]
        [MaxLength(1)]
        [Column(TypeName = "varchar(1)")]
        public char state{get; set;}
        [Required]
        public int  tokens{get; set;}

         public ICollection<Transaction> kolekcjaTransakcija{get;set;}
          public ICollection<Bids> BidList {get; set;}

        public ICollection<Auction> AuctionWinners {get; set;}
        public ICollection<Auction> AuctionOwners {get; set;}
    }
     public class UserProfile : Profile  //Mapper klaasa :D
    {
        public UserProfile()
        {

            base.CreateMap<RegisterModel, User>( )
            .ForMember(
                destination => destination.Email,
                options => options.MapFrom(data=>data.email)
            ) 
            .ForMember(
                destination => destination.UserName,
                options => options.MapFrom(data=>data.username)
            )
            .ReverseMap();
        
        }
    }

}