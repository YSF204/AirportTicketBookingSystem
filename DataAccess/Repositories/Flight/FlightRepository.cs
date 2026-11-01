using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Storage;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Flights;
    public class FlightRepository : IFlightRepository
    {
        private readonly JsonRepository<Flight> flightRepository = new("flights.json");

        public List<Flight> GetFlights()
        {
            return flightRepository.GetItems();
        }

        public Flight? FindFlight(string flightId)
        {
            var flights = GetFlights();
            return flights.FirstOrDefault(flight =>
                flight.Id.Equals(flightId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public List<Flight> SearchFlights(
            string? departureCountry = null,
            string? destinationCountry = null,
            DateTime? departureDate = null,
            string? departureAirport = null,
            string? arrivalAirport = null,
            decimal? maxPrice = null,
            TicketClass? ticketClass = null)
        {
            var flights = GetFlights();
            IEnumerable<Flight> matchingFlights = flights;

            if (!string.IsNullOrWhiteSpace(departureCountry))
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.DepartureCountry.Equals(
                        departureCountry.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(destinationCountry))
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.DestinationCountry.Equals(
                        destinationCountry.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (departureDate.HasValue)
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.DepartureDate.Date == departureDate.Value.Date);
            }

            if (!string.IsNullOrWhiteSpace(departureAirport))
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.DepartureAirport.Equals(
                        departureAirport.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(arrivalAirport))
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.ArrivalAirport.Equals(
                        arrivalAirport.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (maxPrice.HasValue && ticketClass.HasValue)
            {
                matchingFlights = matchingFlights.Where(flight =>
                    flight.GetPrice(ticketClass.Value) <= maxPrice.Value);
            }

            return matchingFlights.ToList();
        }

        public void SaveFlights(List<Flight> flights)
        {
            flightRepository.SaveItems(flights);
        }
    }