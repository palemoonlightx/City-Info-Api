﻿using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models {
    public class PointOfInterestForUpdate {

        // Custom Error Message
        [Required(ErrorMessage = "Please provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

    }
}
