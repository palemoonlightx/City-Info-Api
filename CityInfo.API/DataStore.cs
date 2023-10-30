using CityInfo.API.Models;

namespace CityInfo.API {
    public class DataStore {
        public List<City> Cities { get; set; }
        //public static DataStore Current { get; } = new DataStore();

        public DataStore() {
            Cities = new List<City> {
                new City { Id = 1, Name = "New York City", Description = "The one with that big park.", PointsOfInterest = new List<PointOfInterest>{
                    new PointOfInterest {Id=1, Name="Central Park", Description="The most visited urban park in the United States."},
                    new PointOfInterest {Id=2, Name="Empire State Building", Description="A 102-story skyscraper located in Midtown Manhattan."}
                } },

                new City{Id = 2,Name = "Los Angeles",Description = "The city of angels.",PointsOfInterest = new List<PointOfInterest>{
                    new PointOfInterest { Id = 3, Name = "Hollywood Walk of Fame", Description = "A historic sidewalk with stars embedded in honor of celebrities." },
                    new PointOfInterest { Id = 4, Name = "Griffith Observatory", Description = "A popular spot for stargazing and city views." }
                }},

                new City{Id = 3,Name = "Chicago",Description = "The windy city.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 5, Name = "Millennium Park", Description = "Known for its public art and outdoor events." },
                        new PointOfInterest { Id = 6, Name = "Willis Tower", Description = "Formerly known as the Sears Tower." }
                }},

                new City{Id = 4,Name = "San Francisco",Description = "The city by the bay.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 7, Name = "Golden Gate Bridge", Description = "A famous suspension bridge." },
                        new PointOfInterest { Id = 8, Name = "Alcatraz Island", Description = "A former federal prison." }
                }},
                new City{Id = 5,Name = "Miami",Description = "The Magic City.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 9, Name = "South Beach", Description = "Famous for its colorful art deco architecture." },
                        new PointOfInterest { Id = 10, Name = "Art Deco Historic District", Description = "Preserving the Miami Beach architectural style." }
                }},
                new City{Id = 6,Name = "Las Vegas",Description = "The entertainment capital of the world.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 11, Name = "The Strip", Description = "A stretch of South Las Vegas Boulevard known for its resorts and casinos." },
                        new PointOfInterest { Id = 12, Name = "Fremont Street Experience", Description = "A pedestrian mall and attraction in downtown Las Vegas." }
                }},
                new City{
                    Id = 7,Name = "Washington, D.C.",Description = "The nation's capital.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 13, Name = "The White House", Description = "The official residence and workplace of the President of the United States." },
                        new PointOfInterest { Id = 14, Name = "The National Mall", Description = "A landscaped park within the city." }
                }},
                new City{Id = 8,Name = "Seattle",Description = "The Emerald City.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 15, Name = "Space Needle", Description = "An iconic observation tower." },
                        new PointOfInterest { Id = 16, Name = "Pike Place Market", Description = "A public market overlooking Elliott Bay waterfront." }
                }},
                new City{Id = 9,Name = "Boston",Description = "The Cradle of Liberty.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 17, Name = "Freedom Trail", Description = "A 2.5-mile-long path through historic sites." },
                        new PointOfInterest { Id = 18, Name = "Fenway Park", Description = "Home of the Boston Red Sox." }
                }},
                new City{Id = 10,Name = "New Orleans",Description = "The Big Easy.",PointsOfInterest = new List<PointOfInterest>{
                        new PointOfInterest { Id = 19, Name = "French Quarter", Description = "The oldest neighborhood in the city." },
                        new PointOfInterest { Id = 20, Name = "Café du Monde", Description = "Famous for beignets and café au lait." }
                }}
            };
        }
    }
}
