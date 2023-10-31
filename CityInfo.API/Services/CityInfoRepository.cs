using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services {
    public class CityInfoRepository : ICityInfoRepository {

        private readonly Context _context;

        public CityInfoRepository(Context context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync() {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }


        // Filtering and searching cities and paging
        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize) {

            // Commented because want to implement paging regardless
            //if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(searchQuery)) {
            //    return await GetCitiesAsync();
            //}


            //https://josipmisko.com/posts/c-sharp-iqueryable-vs-ienumerable
            // Casting dbset to iqueryable (creates an expression tree) can query database directly instead of loading first into memory
            var collection = _context.Cities as IQueryable<City>;


            // Basically building a query

            if (!string.IsNullOrWhiteSpace(name)) {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }


            if (!string.IsNullOrWhiteSpace(searchQuery)) {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery) || c.Description != null && c.Description.Contains(searchQuery));
            }

            // Sent to the database only at the end
            // Paging functionality should be added at the end
            return await collection.OrderBy(c => c.Name).Skip(pageSize * (pageNumber - 1)).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest) {
            if (includePointsOfInterest) {
                return await _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int poiId) {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == poiId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId) {
            return await _context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId) {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest) {
            var city = await GetCityAsync(cityId, false);
            if (city != null) {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync() {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest) {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }
    }
}
