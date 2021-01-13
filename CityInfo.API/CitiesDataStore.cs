using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id  = 1,
                    Name = "Barcelona",
                    Description = "Camp Nou",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Parque",
                            Description = "Parque Central"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Cine",
                            Description = "Cinemark"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Supermercado",
                            Description = "El Corte Ingles"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Cadiz",
                    Description = "Ramon de Carranza",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Museo",
                            Description = "Museo Central"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Catedral",
                            Description = "Catedral Principal"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 6,
                            Name = "Teatro",
                            Description = "La ultima gala"
                        }
                    }
                }
            };
        }
    }
}
