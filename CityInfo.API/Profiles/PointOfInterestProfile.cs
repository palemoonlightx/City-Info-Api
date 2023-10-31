using AutoMapper;

namespace CityInfo.API.Profiles {
    public class PointOfInterestProfile : Profile {
        public PointOfInterestProfile() {

            CreateMap<Entities.PointOfInterest, Models.PointOfInterest>();

            // Mapping for PointOfInterestForCreation
            CreateMap<Models.PointOfInterestForCreation, Entities.PointOfInterest>();

            // Mapping for PointOfInterestForUpdate
            CreateMap<Models.PointOfInterestForUpdate, Entities.PointOfInterest>();

            // Mapping for PointOfInterestForUpdate Partial
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdate>();
        }
    }
}
