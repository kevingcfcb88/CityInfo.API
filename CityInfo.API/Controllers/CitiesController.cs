using AutoMapper;
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
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var citiesFromRepo = _repository.GetCities();

            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(citiesFromRepo));

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
                return Ok(_mapper.Map<CityDto>(cityFromRepo));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(cityFromRepo));
        }
    }
}
