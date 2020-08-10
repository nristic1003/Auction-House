using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iep.Models.Database
{
    public class Transaction
    {
        [Key]
        public int idT{get;set;}
        [Required]
        public DateTime date{get; set;}
        [Required]
        public string  idUser{get; set;}
        [Required]
        public int idToken{get; set;}
         
        public Tokens token{get; set;}

      
    }
    
    public class TokenConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
              builder.Property(transaction => transaction.idT).ValueGeneratedOnAdd();

            builder.HasOne<Tokens>(item => item.token)
            .WithMany(item => item.kolekcjaTransakcija)
            .HasForeignKey(item => new {item.idToken} );
        }

}
}