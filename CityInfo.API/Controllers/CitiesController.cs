using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase {

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper) {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPoi>>> GetCites() {
            var cityEntites = await _cityInfoRepository.GetCitiesAsync();


            // Manuall mapping (problematic)
            //var results = new List<CityWithoutPoi>();
            //foreach (var item in results){
            //    results.Add(new CityWithoutPoi {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Description = item.Description,
            //    });
            //}

            // Automapper - Maps to CityWithoutPoi  
            return Ok(_mapper.Map<IEnumerable<CityWithoutPoi>>(cityEntites));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false) {

            /*
             * The IActionResult return type is appropriate when multiple ActionResult return types are possible in an action.
             * IActionResults because returning Ok and NotFound
             */


            // Find City
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            // If not found 
            if (city == null) {
                return NotFound();
            }

            // If including city poi then map with poi (created poi profile too)
            if (includePointsOfInterest) {
                return Ok(_mapper.Map<City>(city));
            }


            // Else just map city without poi
            return Ok(_mapper.Map<CityWithoutPoi>(city));
        }


    }
}
