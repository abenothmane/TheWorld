using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModel;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ILogger _logger;
        private IWorldRepository _repository;
        private GeoCoordsService _coordsService;

        public StopsController(IWorldRepository repository, ILogger<StopsController> logger, GeoCoordsService coordsService )
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;

        }


        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody] StopViewModel stop)
        {

            try
            {

                var newStop = Mapper.Map<Stop>(stop);

                // Lookup the Geocodes

                var result = await _coordsService.GetCoordsAsync(newStop.Name);
                if (!result.Success)
                {
                    _logger.LogError(result.Message);
                }

                else
                {

                    newStop.Latitude = result.Latitude;
                    newStop.Longitude = result.Longitude;


                    // Save to the database 
                    _repository.AddStop(tripName,User.Identity.Name, newStop);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Created($"api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                    }

                }


            }
            catch (Exception ex)
            {
                
                _logger.LogError("Failed to save new stop: {0}", ex);
            }

        

            return BadRequest("Failed To Save new stop");

        
        }

            

        


        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName, User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList()));
            }

            catch (Exception ex)
            {
                _logger.LogError("Failed To Get Stops: {0}", ex);

            }

            return BadRequest("Failed To Get Stops");

        }
    }
}
