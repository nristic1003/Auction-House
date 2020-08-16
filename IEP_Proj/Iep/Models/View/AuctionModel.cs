using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Iep.Models.View{
    public class AuctionModel
    {
      
        public int Id{get; set;}
          [Required]
        [Display(Name = "Name")]
        public string name{get; set;}

        [Required]
        [Display(Name = "Description")]
        public string description{get; set;}

        [Required]
          [Display(Name = "Image")]
        public IFormFile file{get; set;} //ovo je za sliku, slika se cuva kao niz bajtova

        [Required]
        [Display(Name = "Start Price")]
        public int startPrice{get; set;}
        
        [Required]
        [Display(Name = "Create Date")]
        public DateTime createDate{get; set;}
 
        [Required]
        [Display(Name = "Open Date")]
        public DateTime openDate{get; set;}
 
        [Required]
        [Display(Name = "Close Date")]
        public DateTime closeDate{get; set;}

        
    }
}