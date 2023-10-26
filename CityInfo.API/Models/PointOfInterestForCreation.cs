using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models {
    public class PointOfInterestForCreation {

        //docs.fluentvalidation.net/en/latest/ - docs for more complex validation

        // Custom Error Message
        [Required(ErrorMessage = "Please provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }


    }
}
