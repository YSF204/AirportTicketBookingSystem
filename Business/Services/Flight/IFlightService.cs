using AirportTicketBookingSystem.DataAccess.Entities;

namespace AirportTicketBookingSystem.Business.Services.Flights;
    /// <summary>
    /// this interface defines the operations for the required services for the flight management system.
    /// </summary>
    public interface IFlightService
    {
        /// <summary>Returns the rules used to validate imported flights.</summary>
        List<string> GetValidationDetails();

        /// <summary>Validates and adds flights, returning each row's status and errors.</summary>
        List<(bool Added, List<string> Errors)> AddFlights(List<Flight?> newFlights);

        /// <summary>Returns every flight.</summary>
        List<Flight> GetAllFlights();

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

