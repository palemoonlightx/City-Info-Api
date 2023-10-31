using AutoMapper;
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

        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;


        // Constructor injection (prefferred way)
        public PoiController(ILogger<PoiController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper) {
            // Checking for null exception 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /*
         * Validation 
         * - Data annotations
         * - ModelState (a dictionary containing both state of model and model binding validation)
         * 
         * JsonPatch standard - set of rules to apply to data
         */




        // Get all request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterest>>> GetPointsOfInterest(int cityId) {
            // Checking first if city exists calling the CityExistsMethod
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                _logger.LogInformation($"City with id {cityId} does not exist.");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterest>>(pointsOfInterestForCity));
        }





        // Single get request - named request so can refer to it later

        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<IEnumerable<PointOfInterest>>> GetPointOfInterest(int cityId, int poiId) {

            // Checking if city exists
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                return NotFound();
            }

            // Getting point of interest
            var poi = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, poiId);
            if (poi == null) {
                return NotFound();
            }

            // Mapping 
            return Ok(_mapper.Map<PointOfInterest>(poi));
        }






        // Post request - returns created object in response
        [HttpPost]
        public async Task<ActionResult<PointOfInterest>> CreatePointOfInterest(int cityId, PointOfInterestForCreation poi) {


            // Checking if city exists
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                return NotFound();
            }

            var newPoi = _mapper.Map<Entities.PointOfInterest>(poi);

            // Adding to list of Point Of Interests
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, newPoi);

            // Saving changes
            await _cityInfoRepository.SaveChangesAsync();

            // Created point of interest to return  (mapping entity back to model)
            var createdPoi = _mapper.Map<Models.PointOfInterest>(newPoi);

            // Returning 201 - with reference to get method and its paramaters - and newly created poi
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = createdPoi.Id }, createdPoi);
        }






        // Put request - updates whole object
        [HttpPut("{poiId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int poiId, PointOfInterestForUpdate poi) {

            // Checking if city exists
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                return NotFound();
            }

            // Checking if poi exists
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, poiId);
            if (pointOfInterestEntity == null) {
                return NotFound();
            }

            /*
             * If we pass the source object (poi) passed via request body.
             * And we pass the entity which is destination object (pointOfInterestEntity)
             * Automapper will automatically override the destination object with values of the source object
             */
            _mapper.Map(poi, pointOfInterestEntity);

            // Saving changes
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

        }







        // Patch request - updates part of data
        [HttpPatch("{poiId}")]
        public async Task<ActionResult> UpdatePointOfInterestPartially(int cityId, int poiId, JsonPatchDocument<PointOfInterestForUpdate> patchDocument) {

            // Check if city exists
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                return NotFound();
            }


            // Checking if poi exists
            var poiExisting = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, poiId);
            if (poiExisting == null) {
                return NotFound();
            }



            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdate>(poiExisting);

            // Passing model state to show errors if any
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);


            // Validation on dto after patch is applied
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }


            // Checking if poiUpdate is still valid after applying patch document
            if (!TryValidateModel(pointOfInterestToPatch)) {
                return BadRequest(ModelState);
            }

            // Updating in entity
            _mapper.Map(pointOfInterestToPatch, poiExisting);

            // Saving changes
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }





        //// Delete request
        [HttpDelete("{poiId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int poiId) {

            // Check if city exists
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, poiId);
            if (poiExisting == null) {
                return NotFound();
            }

            // Deleting
            _cityInfoRepository.DeletePointOfInterest(poiExisting);

            // Saving changes
            await _cityInfoRepository.SaveChangesAsync();

            // Using custom service here
            _mailService.Send("Point of interest deleted", $"Point of interest {poiExisting.Name} with id {poiId} has been deleted ");


            var response = new {
                Message = "Point of Interest deleted successfully."
            };


            return Ok(response);
        }
    }
}
