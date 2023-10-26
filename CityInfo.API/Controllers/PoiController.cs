using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {

    [ApiController]
    [Route("api/cities/{cityId}/poi")]
    public class PoiController : ControllerBase {


        /*
         * Validation 
         * - Data annotations
         * - ModelState (a dictionary containing both state of model and model binding validation)
         * 
         * JsonPatch standard - set of rules to apply to data
         */

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterest>> GetAllPoi(int cityId) {

            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }


            return Ok(city.PointOfInterests);
        }



        [HttpGet("{poiId}", Name = "GetPointOfInterest")]
        public ActionResult<IEnumerable<PointOfInterest>> GetPoi(int cityId, int poiId) {
            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            var poi = city.PointOfInterests.FirstOrDefault(p => p.Id == poiId);
            if (poi == null) {
                return NotFound();
            }


            return Ok(poi);
        }



        // Post request - returns created object in response
        [HttpPost]
        public ActionResult<PointOfInterest> CreatePointOfInterest(int cityId, PointOfInterestForCreation poi) {


            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Demo purposes
            var maxPoiId = DataStore.Current.Cities.SelectMany(c => c.PointOfInterests).Max(p => p.Id);

            var newPoi = new PointOfInterest {
                Id = maxPoiId + 1,
                Name = poi.Name,
                Description = poi.Description,

            };

            // Adding to list of Point Of Interests
            city.PointOfInterests.Add(newPoi);

            // Returning 201 - with reference to get method and its paramaters - and newly created poi
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = newPoi.Id }, newPoi);
        }


        // Put request - updates whole object
        [HttpPut("{poiId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int poiId, PointOfInterestForUpdate poi) {

            // Checking if city exists
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointOfInterests.FirstOrDefault(c => c.Id == poiId);
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
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointOfInterests.FirstOrDefault(c => c.Id == poiId);
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
            var city = DataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            // Checking if poi exists
            var poiExisting = city.PointOfInterests.FirstOrDefault(c => c.Id == poiId);
            if (poiExisting == null) {
                return NotFound();
            }

            city.PointOfInterests.Remove(poiExisting);

            var response = new {
                Message = "Point of Interest deleted successfully."
            };

            return Ok(response);

        }
    }
}
