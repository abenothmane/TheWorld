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
using TheWorld.ViewModel;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller

    {

        private IWorldRepository _repository;
        private ILogger _logger;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            
                var trips = _repository.GetUserTripsWithStops(User.Identity.Name);

                var results  = Mapper.Map<IEnumerable<TripViewModel>>(trips);
                return Json(results);
            
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {

            if(ModelState.IsValid)
            { 
                var newTrip = Mapper.Map<Trip>(theTrip);

                newTrip.Username = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }

            
            }

            return BadRequest("Failed To Save Changes To The Database");
        }

    }
}
