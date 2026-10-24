using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Repositories
{
    public class FlightRepository
    {
        private readonly JsonRepository<Flight> flightRepository = new("flights.json");

        public List<Flight> GetFlights()
        {
            return flightRepository.GetItems();
        }

        public void SaveFlights(List<Flight> flights)
        {
            flightRepository.SaveItems(flights);
        }
    }
}
