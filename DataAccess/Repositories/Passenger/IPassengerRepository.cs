using AirportTicketBookingSystem.DataAccess.Entities;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Passengers;

public interface IPassengerRepository
{
    /// <summary>Returns all stored passengers.</summary>
    List<Passenger> GetPassengers();

    /// <summary>Finds a passenger by ID.</summary>
    Passenger? FindPassenger(string passengerId);

    /// <summary>Adds a passenger.</summary>
    /// <exception cref="ArgumentException">A passenger with the same ID already exists.</exception>
    void AddPassenger(Passenger passenger);

    /// <summary>Saves the supplied passengers.</summary>
    void SavePassengers(List<Passenger> passengers);
}