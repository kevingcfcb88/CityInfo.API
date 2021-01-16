using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using CityInfo.API.Services;
using AutoMapper;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mail;
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mail, ICityInfoRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mail = mail ?? throw new ArgumentNullException(nameof(mail));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_repository.CityExists(cityId))
                {
                    _logger.LogInformation($"cityId {cityId} wasn't found");
                    return NotFound();
                }
                var pointsOfInterest = _repository.GetPointsOfInterestForCity(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Custom Error {ex.Message}");
                return StatusCode(500, "Internal custom error");
            }
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var point = _repository.GetPointOfInterestForCity(cityId, id);

            if (point == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(point));
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Description should be different than Name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPoint = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _repository.AddPointOfInterestForCity(cityId, finalPoint);

            _repository.Save();

            var createdPointOfInterest = _mapper.Map<Models.PointOfInterestDto>(finalPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId, id = createdPointOfInterest.Id }, createdPointOfInterest);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto point)
        {
            if (point.Description == point.Name)
            {
                ModelState.AddModelError("Description", "Description should be different than Name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestInRepo = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestInRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(point, pointOfInterestInRepo);

            _repository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestInRepo = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestInRepo == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestInRepo);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "Description should be different than Name");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest();
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestInRepo);

            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestInRepo = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestInRepo == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(pointOfInterestInRepo);

            _repository.Save();

            _mail.SendMail("Deleting object", $"pointOfInterestId: {pointOfInterestInRepo.Id}");

            return NoContent();
        }
    }
}
