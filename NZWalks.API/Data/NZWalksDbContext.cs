using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        //public DbSet<Image> Images { get; set; }
    }
}