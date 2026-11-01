using AirportTicketBookingSystem.DataAccess.Entities;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Flights;

public interface IFlightRepository
{
    /// <summary>Returns all stored flights.</summary>
    List<Flight> GetFlights();

    /// <summary>Finds a flight by its ID.</summary>
    Flight? FindFlight(string flightId);

    /// <summary>Returns flights matching the supplied filters.</summary>
    List<Flight> SearchFlights(
        string? departureCountry = null,
        string? destinationCountry = null,
        DateTime? departureDate = null,
        string? departureAirport = null,
        string? arrivalAirport = null,
        decimal? maxPrice = null,
        TicketClass? ticketClass = null);

    /// <summary>Saves the supplied flights.</summary>
    void SaveFlights(List<Flight> flights);
}