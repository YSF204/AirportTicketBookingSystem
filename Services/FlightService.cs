
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Validators;

namespace AirportTicketBookingSystem.Services
{
    public class FlightService
    {
        private readonly FlightRepository flightRepository;
        private readonly FlightValidator flightValidator;

        public FlightService(
            FlightRepository flightRepository,
            FlightValidator? flightValidator = null)
        {
            this.flightRepository = flightRepository;
            this.flightValidator = flightValidator ?? new FlightValidator();
        }

        public List<string> GetValidationDetails() => flightValidator.GetValidationDetails();

        public List<string> AddFlights(List<Flight?> newFlights, out int addedCount)
        {
            var existingFlights = flightRepository.GetFlights();
            var flightsToCheck = new List<Flight>(existingFlights);
            var errors = new List<string>();
            addedCount = 0;

            for (int i = 0; i < newFlights.Count; i++)
            {
                var flight = newFlights[i];
                if (flight == null)
                {
                    continue;
                }

                var flightErrors = flightValidator.ValidateFlight(flight, flightsToCheck);

                if (flightErrors.Count > 0)
                {
                    foreach (var error in flightErrors)
                    {
                        errors.Add($"Row {i + 2}: {error}");
                    }
                }
                else
                {
                    existingFlights.Add(flight);
                    addedCount++;
                }

                flightsToCheck.Add(flight);
            }

            if (addedCount > 0)
            {
                flightRepository.SaveFlights(existingFlights);
            }

            return errors;
        }


        public List<Flight> GetAllFlights()
        {
            return flightRepository.GetFlights();

        }

        public List<Flight> SearchFlights(string departureCountry , string destinationCountry) {

            var flights = flightRepository.GetFlights();
            return flights.Where(f => f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase) &&
                              f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)).ToList();

        }

        public List<Flight> SearchFlightsByDate(DateTime departureDate)
        {
            var flights = flightRepository.GetFlights();
            return flights.Where(f=> f.DepartureDate.Date == departureDate.Date).ToList();

        }

        public List<Flight> SearchFlightsByMaxPrice(decimal maxPrice , TicketClass ticketClass)
        {
            var flights = flightRepository.GetFlights();
            return flights.Where(f => f.GetPrice(ticketClass) <= maxPrice).ToList();
        }

        public List<Flight> SearchFlightsByAirports(string departureAirport, string arrivalAirport)
        {
            var flights = flightRepository.GetFlights();
            return flights.Where(f => f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase) &&
                              f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)).ToList();

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
            IEnumerable<Flight> flights = flightRepository.GetFlights();

            if (!string.IsNullOrWhiteSpace(departureCountry))
            {
                flights = flights.Where(f =>
                    f.DepartureCountry.Equals(
                        departureCountry.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(destinationCountry))
            {
                flights = flights.Where(f =>
                    f.DestinationCountry.Equals(
                        destinationCountry.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (departureDate.HasValue)
            {
                DateTime date = departureDate.Value.Date;

                flights = flights.Where(f =>
                    f.DepartureDate.Date == date);
            }

            if (!string.IsNullOrWhiteSpace(departureAirport))
            {
                flights = flights.Where(f =>
                    f.DepartureAirport.Equals(
                        departureAirport.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(arrivalAirport))
            {
                flights = flights.Where(f =>
                    f.ArrivalAirport.Equals(
                        arrivalAirport.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (maxPrice.HasValue != ticketClass.HasValue)
            {
                throw new ArgumentException(
                    "Maximum price and ticket class must be provided together.");
            }

            if (maxPrice.HasValue && ticketClass.HasValue)
            {
                if (maxPrice.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(maxPrice), "Maximum price cannot be negative.");
                }

                TicketClass selectedClass = ticketClass.Value;
                decimal priceLimit = maxPrice.Value;

                flights = flights.Where(f =>
                    f.GetPrice(selectedClass) <= priceLimit);
            }

            return flights.ToList();
        }

    }
}
