using ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ASP.NetCore_WebAPI_MongoDB_EntityFrameworkCore.Context
{
    public class MongoDbContext : DbContext
    {
        public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options)
        {
              
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToCollection("countries");
        }

        public DbSet<Country> Countries { get; set; }

    }
}
 