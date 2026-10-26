using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{
    /// <summary>
    /// this interface defines the operations for the required services for the flight management system.
    /// </summary>
    public interface IFlightService
    {
        /// <summary>Returns the rules used to validate imported flights.</summary>
        List<string> GetValidationDetails();

        /// <summary>Validates and adds the supplied flights, returning errors and the number added.</summary>
        List<string> AddFlights(List<Flight?> newFlights, out int addedCount);

        /// <summary>Returns every flight.</summary>
        List<Flight> GetAllFlights();

        /// <summary>Finds flights by departure and destination country.</summary>
        List<Flight> SearchFlights(string departureCountry, string destinationCountry);

        /// <summary>Finds flights departing on the specified calendar date.</summary>
        List<Flight> SearchFlightsByDate(DateTime departureDate);

        /// <summary>Finds flights whose price in the selected class does not exceed the limit.</summary>
        List<Flight> SearchFlightsByMaxPrice(decimal maxPrice, TicketClass ticketClass);

        /// <summary>Finds flights by departure and arrival airport.</summary>
        List<Flight> SearchFlightsByAirports(string departureAirport, string arrivalAirport);

        /// <summary>Finds flights matching any supplied filters; price and ticket class must be supplied together.</summary>
        List<Flight> SearchFlights(
            string? departureCountry = null,
            string? destinationCountry = null,
            DateTime? departureDate = null,
            string? departureAirport = null,
            string? arrivalAirport = null,
            decimal? maxPrice = null,
            TicketClass? ticketClass = null);
    }
}
