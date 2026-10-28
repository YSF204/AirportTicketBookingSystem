using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Storage;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Passengers;
    public class PassengerRepository : IPassengerRepository
    {
        private readonly JsonRepository<Passenger> passengerRepository = new("passengers.json");

        public List<Passenger> GetPassengers()
        {
            return passengerRepository.GetItems();
        }

        public Passenger? FindPassenger(string passengerId)
        {
            var passengers = GetPassengers();
            return passengers.FirstOrDefault(passenger =>
                passenger.Id.Equals(passengerId, StringComparison.OrdinalIgnoreCase));
        }

        public void AddPassenger(Passenger passenger)
        {
            var passengers = GetPassengers();
            if (passengers.Any(existingPassenger => existingPassenger.Id.Equals(
                    passenger.Id,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Passenger with ID '{passenger.Id}' is already registered.", nameof(passenger));
            }

            passengers.Add(passenger);
            SavePassengers(passengers);
        }

        public void SavePassengers(List<Passenger> passengers)
        {
            passengerRepository.SaveItems(passengers);
        }
    }