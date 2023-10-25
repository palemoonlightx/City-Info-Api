using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/cities")]
    public class CitiesController  : ControllerBase {

        [HttpGet] 
        public ActionResult<IEnumerable<City>> GetCites() {
            return Ok(DataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id) {
            // Find City
            var city = DataStore.Current.Cities.FirstOrDefault(x => x.Id == id);

            // If not found 
            if(city == null) {
                return NotFound();
            }

            return Ok(city);
        }


    }
}
