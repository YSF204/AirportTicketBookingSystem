
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Validators
{
    public class FlightValidator
    {
        public List<string> ValidateFlight(
            Flight flight,
            IEnumerable<Flight>? existingFlights = null)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(flight.Id))
            {
                errors.Add("Flight ID is required.");
            }
            else if (existingFlights != null &&
                existingFlights.Any(f =>
                    string.Equals(
                        f.Id,
                        flight.Id,
                        StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add($"Flight ID '{flight.Id}' already exists.");
            }

            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
            {
                errors.Add("Departure country is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
            {
                errors.Add("Destination country is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
            {
                errors.Add("Departure airport is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport))
            {
                errors.Add("Arrival airport is required.");
            }

            if (flight.EconomyPrice <= 0 ||
                flight.BusinessPrice <= 0 ||
                flight.FirstClassPrice <= 0)
            {
                errors.Add("All ticket prices must be greater than zero.");
            }

            if (flight.DepartureDate < DateTime.Now)
            {
                errors.Add("Departure date cannot be in the past.");
            }

            if (string.Equals(
                flight.DepartureAirport,
                flight.ArrivalAirport,
                StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Departure and arrival airports cannot be the same.");
            }

            if (string.Equals(
                flight.DepartureCountry,
                flight.DestinationCountry,
                StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Departure and destination countries cannot be the same.");
            }

            return errors;
        }
    }
}
