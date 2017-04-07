using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        Trip GetTripByName(string tripName, string username);

        IEnumerable<Trip> GetAllTrips();

        void AddTrip(Trip trip);

        void AddStop(string tripName,string username, Stop stop);

        Task<bool> SaveChangesAsync();

        IEnumerable<Trip> GetAllTripsWithStops();

        IEnumerable<Trip> GetUserTripsWithStops(string name);


    }
}