using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Profiles {
    public class CityProfile : Profile {

        public CityProfile() {
            // Will map without city points 
            CreateMap<Entities.City, Models.CityWithoutPoi>();

            // Mapping for city to city
            CreateMap<Entities.City, Models.City>();
        }

    }
}
