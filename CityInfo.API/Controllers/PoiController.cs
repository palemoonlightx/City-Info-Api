using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/cities/{cityId}/poi")]
    public class PoiController : ControllerBase {


        private readonly ILogger<PoiController> _logger;
        private readonly IMailService _mailService;
        private readonly DataStore _dataStore;


        // Constructor injection (prefferred way)
        public PoiController(ILogger<PoiController> logger, IMailService mailService, DataStore dataStore) {
            // Checking for null exception 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
        }


        /*
         * Validation 
         * - Data annotations
         * - ModelState (a dictionary containing both state of model and model binding validation)
         * 
         * JsonPatch standard - set of rules to apply to data
         */

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterest>> GetAllPoi(int cityId) {

            // Logging exception
            try {

                // Throwing exception for testing
                //throw new Exception("Testing");

                // Checking if city exists
                var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null) {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }


                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex) {
                // Unhandled exceptions automatically return error 500
                // but since we are handling the exception we need to return it manually

                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);

                // This will go back to consumer (but dont send back trace message, or expose implementation)
                return StatusCode(500, "A problem happened while handling your request");
            }
        }



        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterest>> GetPoi(int cityId, int poiId) {
            // Checking if city exists
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == poiId);
            if (poi == null) {
                return NotFound();
            }


            return Ok(poi);
        }



        // Post request - returns created object in response
        [HttpPost]
        public ActionResult<PointOfInterest> CreatePointOfInterest(int cityId, PointOfInterestForCreation poi) {


            // Checking if city exists
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Demo purposes
            var maxPoiId = _dataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPoi = new PointOfInterest {
                Id = maxPoiId + 1,
                Name = poi.Name,
                Description = poi.Description,

            };

            // Adding to list of Point Of Interests
            city.PointsOfInterest.Add(newPoi);

            // Returning 201 - with reference to get method and its paramaters - and newly created poi
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = newPoi.Id }, newPoi);
        }


        // Put request - updates whole object
        [HttpPut("{poiId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int poiId, PointOfInterestForUpdate poi) {

            // Checking if city exists
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (poiExisting == null) {
                return NotFound();
            }

            poiExisting.Name = poi.Name;
            poiExisting.Description = poi.Description;

            return NoContent();

        }

        // Patch request - updates part of data
        [HttpPatch("{poiId}")]
        public ActionResult UpdatePointOfInterestPartially(int cityId, int poiId, JsonPatchDocument<PointOfInterestForUpdate> patchDocument) {
            // Check if city exists
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (poiExisting == null) {
                return NotFound();
            }

            // Applying patch document
            var poiToPatch = new PointOfInterestForUpdate {
                Name = poiExisting.Name,
                Description = poiExisting.Description,
            };

            // Passing model state to show errors if any
            patchDocument.ApplyTo(poiToPatch, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }


            // Checking if poiUpdate is still valid after applying patch document
            if (!TryValidateModel(poiToPatch)) {
                return BadRequest(ModelState);
            }


            poiExisting.Name = poiToPatch.Name;
            poiExisting.Description = poiToPatch.Description;
            return NoContent();
        }


        // Delete request
        [HttpDelete("{poiId}")]
        public ActionResult DeletePointOfInterest(int cityId, int poiId) {
            // Check if city exists
            var city = _dataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointsOfInterest.FirstOrDefault(c => c.Id == poiId);
            if (poiExisting == null) {
                return NotFound();
            }

            city.PointsOfInterest.Remove(poiExisting);

            // Using custom service here
            _mailService.Send("Point of interest deleted", $"Point of interest {poiExisting.Name} with id {poiId} has been deleted ");


            var response = new {
                Message = "Point of Interest deleted successfully."
            };



            return Ok(response);

        }
    }
}
