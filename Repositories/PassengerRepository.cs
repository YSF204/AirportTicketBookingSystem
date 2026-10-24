using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Repositories
{
    public class PassengerRepository
    {
        private readonly JsonRepository<Passenger> passengerRepository = new("passengers.json");

        public List<Passenger> GetPassengers()
        {
            return passengerRepository.GetItems();
        }

        public void SavePassengers(List<Passenger> passengers)
        {
            passengerRepository.SaveItems(passengers);
        }
    }
}
