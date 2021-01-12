using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iep.Models.Database{

    public class AuctionContext : IdentityDbContext<User>{


        public DbSet<Tokens> tokens {get; set;}
        public DbSet<Transaction> transaction {get; set;}
        public DbSet<Auction> auction {get; set;}
        public DbSet<Bids> bids {get; set;}      
            public DbSet<User> user {get; set;}      

        public AuctionContext(DbContextOptions options):base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

                builder.Entity<User>()
                .Property(u => u.tokens)
                .HasDefaultValue(0);

                builder.Entity<User>()
                .Property(u => u.state)
                .HasDefaultValue("A");

                builder.Entity<User>()
                .HasMany(u => u.AuctionWinners)
                .WithOne(a => a.winner);

                builder.Entity<User>()
                .HasMany(u => u.AuctionOwners)
                .WithOne(a => a.owner);

                 builder.Entity<Auction>()
            .Property(a => a.RowVersion)
            .IsConcurrencyToken()
            .ValueGeneratedOnAddOrUpdate();

            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new BagTokenConfiguration());
            builder.ApplyConfiguration(new TokenConfiguration());           
            builder.ApplyConfiguration(new AuctionConfiguration());
            builder.ApplyConfiguration(new BidConfiguration());

            

            

        }



    }


}