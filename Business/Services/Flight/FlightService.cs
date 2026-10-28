
using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Repositories.Flights;
using AirportTicketBookingSystem.Business.Validation;

namespace AirportTicketBookingSystem.Business.Services.Flights;
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository flightRepository;
        private readonly FlightValidator flightValidator;

        public FlightService(
            IFlightRepository flightRepository,
            FlightValidator? flightValidator = null)
        {
            this.flightRepository = flightRepository;
            this.flightValidator = flightValidator ?? new FlightValidator();
        }

        public List<string> GetValidationDetails() => flightValidator.GetValidationDetails();

        public List<(bool Added, List<string> Errors)> AddFlights(List<Flight?> newFlights)
        {
            var existingFlights = flightRepository.GetFlights();
            var flightsToCheck = new List<Flight>(existingFlights);
            var results = new List<(bool Added, List<string> Errors)>(newFlights.Count);

            for (int i = 0; i < newFlights.Count; i++)
            {
                var flight = newFlights[i];
                if (flight == null)
                {
                    results.Add((false, []));
                    continue;
                }

                var flightErrors = flightValidator.ValidateFlight(flight, flightsToCheck);

                if (flightErrors.Count > 0)
                {
                    results.Add((false, flightErrors.Select(error => $"Row {i + 2}: {error}").ToList()));
                }
                else
                {
                    existingFlights.Add(flight);
                    results.Add((true, []));
                }

                flightsToCheck.Add(flight);
            }

            if (results.Any(result => result.Added))
            {
                flightRepository.SaveFlights(existingFlights);
            }

            return results;
        }

        public List<Flight> GetAllFlights()
        {
            return flightRepository.GetFlights();
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
            if (maxPrice.HasValue != ticketClass.HasValue)
            {
                throw new ArgumentException(
                    "Maximum price and ticket class must be provided together.");
            }

            if (maxPrice.HasValue && maxPrice.Value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxPrice), "Maximum price cannot be negative.");
            }

            return flightRepository.SearchFlights(
                departureCountry,
                destinationCountry,
                departureDate,
                departureAirport,
                arrivalAirport,
                maxPrice,
                ticketClass);
        }

    }

