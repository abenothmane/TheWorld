using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TheWorld.Models
{
    public class WorldRepository: IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public Trip GetTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.Username == username).FirstOrDefault();
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting All Trips from the Database");

            return _context.Trips.ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public void AddStop(string tripName, string username, Stop stop)
        {
            var trip = GetTripByName(tripName, username);
            if (trip!= null)
            {
                trip.Stops.Add(stop);
                _context.Stops.Add(stop); 
            }
        }

        public async Task<Boolean> SaveChangesAsync()
        {

            return (await _context.SaveChangesAsync()) > 0;
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();
            }

            catch ( Exception ex)
            {
                _logger.LogError("Could not get trips from the database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetUserTripsWithStops(string name)
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .Where(t=> t.Username == name)
                    .ToList();
            }

            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from the database", ex);
                return null;
            }
        }
    }
}
