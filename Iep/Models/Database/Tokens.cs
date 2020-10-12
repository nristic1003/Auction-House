using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iep.Models.Database
{
    public class Tokens
    {
        [Key]
        public int  idToken{get; set;}
        [Required]
        public string  name{get; set;}
         [Required]
        public int  amount{get; set;}
         [Required]
        public int price{get; set;}   

        public ICollection<Transaction> kolekcjaTransakcija{get;set;}
    }

     public class BagTokenConfiguration : IEntityTypeConfiguration<Tokens>
    {
        public void Configure(EntityTypeBuilder<Tokens> builder)
        {
            builder.Property( item => item.idToken).ValueGeneratedOnAdd();

            builder.HasData(
                new Tokens[]{
                    new Tokens()
                    {
                        idToken = 1,
                        name = "Silver",
                        amount = 5,
                        price = 7
                      
                    },
                    new Tokens()
                    {
                        idToken = 2,
                        name = "Gold",
                        amount = 10,
                        price = 12
                      
                    },
                    new Tokens()
                    {
                        idToken = 3,
                        name = "Platinum",
                        amount = 20,
                        price = 19
                      
                    } 
                } 

            );
        }
    }

    }