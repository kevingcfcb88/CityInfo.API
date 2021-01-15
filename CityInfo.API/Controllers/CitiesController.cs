using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;

        public CitiesController(ICityInfoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var citiesFromRepo = _repository.GetCities();
            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach (var city in citiesFromRepo)
            {
                results.Add(new CityWithoutPointsOfInterestDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                });
            }

            return Ok(results);

        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var cityFromRepo = _repository.GetCity(id, includePointsOfInterest);

            if (cityFromRepo == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest == true)
            {
                var cityResult = new CityDto()
                {
                    Id = cityFromRepo.Id,
                    Name = cityFromRepo.Name,
                    Description = cityFromRepo.Description
                };

                foreach (var point in cityFromRepo.PointsOfInterest)
                {
                    cityResult.PointsOfInterest.Add(new PointOfInterestDto()
                    {
                        Id = point.Id,
                        Name = point.Name,
                        Description = point.Description
                    });
                }

                return Ok(cityResult);
            }

            var cityResultWithoutPoints = new CityWithoutPointsOfInterestDto()
            {
                Id = cityFromRepo.Id,
                Name = cityFromRepo.Name,
                Description = cityFromRepo.Description
            };

            return Ok(cityResultWithoutPoints);
        }
    }
}
