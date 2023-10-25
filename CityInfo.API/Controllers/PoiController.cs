using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/cities/{cityId}/poi")]
    public class PoiController : ControllerBase {

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterest>> GetAllPoi(int cityId) {

            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }


            return Ok(city.PointOfInterests);
        }



        [HttpGet("{poiId}")]
        public ActionResult<IEnumerable<PointOfInterest>> GetPoi(int cityId, int poiId) {
            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            var poi = city.PointOfInterests.FirstOrDefault(p => p.Id == poiId);
            if (poi== null) {
                return NotFound();
            }


            return Ok(poi);
        }


    }
}
