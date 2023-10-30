using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API {
    public class Context : DbContext {

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<City>().HasData(
                new City("New York") { Id = 1, Description = "The one with that big park." },
                new City("Paris") { Id = 2, Description = "The one with the cathedral." },
                new City("Tokoyo") { Id = 3, Description = "The one with the forest" }
                );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park") { Id = 1, CityId = 1, Description = "The most visited urban park in the US." },
                new PointOfInterest("Paris Park") { Id = 2, CityId = 2, Description = "The most visited urban park in the France." },
                new PointOfInterest("Tokoyo Park") { Id = 3, CityId = 1, Description = "The most visited urban park in the Japan." }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
