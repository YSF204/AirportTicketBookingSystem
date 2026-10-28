

using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Repositories.Bookings;
using AirportTicketBookingSystem.DataAccess.Repositories.Flights;
using AirportTicketBookingSystem.DataAccess.Repositories.Passengers;
namespace AirportTicketBookingSystem.Business.Services.Bookings;
    public class BookingService : IBookingService
    {
        private readonly IFlightRepository flightRepository;
        private readonly IPassengerRepository passengerRepository;
        private readonly IBookingRepository bookingRepository;

        public BookingService(
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IBookingRepository bookingRepository
            )
        {
            this.flightRepository = flightRepository;
            this.passengerRepository = passengerRepository;
            this.bookingRepository = bookingRepository;
        }

        public Flight? FindFlight(string flightId) => flightRepository.FindFlight(flightId);

        public Passenger? FindPassenger(string passengerId) => passengerRepository.FindPassenger(passengerId);

        public Passenger? RegisterPassenger(string passengerId)
        {
            passengerId = passengerId.Trim();
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return null;
            }

            if (passengerRepository.FindPassenger(passengerId) != null)
            {
                return null;
            }

            var passenger = new Passenger { Id = passengerId };
            passengerRepository.AddPassenger(passenger);
            return passenger;
        }

        public Booking BookFlight(string passengerId, string flightId, TicketClass ticketClass)
        {
            var passenger = FindPassenger(passengerId);
            var flight = FindFlight(flightId);

            if (passenger == null)
            {
                throw new ArgumentException($"Passenger with ID {passengerId} not found.");
            }

            if (flight == null)
            {
                throw new ArgumentException($"Flight with ID {flightId} not found.");
            }

            decimal pricePaid = flight.GetPrice(ticketClass);

            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                PassengerId = passenger.Id,
                FlightId = flight.Id,
                TicketClass = ticketClass,
                PricePaid = pricePaid,
                BookingDate = DateTime.UtcNow
            };

            bookingRepository.AddBooking(booking);

            return booking;
        }

        public List<Booking> GetBookingsForPassenger(string passengerId)
        {
            return bookingRepository.GetBookingsByPassenger(passengerId);
        }

        public void CancelBooking(string bookingId, string passengerId) =>
            bookingRepository.CancelBooking(bookingId, passengerId);

        public void UpdateBooking(string bookingId, string passengerId, string flightId, TicketClass ticketClass)
        {
            if (!Enum.IsDefined(ticketClass))
            {
                throw new ArgumentOutOfRangeException(nameof(ticketClass));
            }

            var flight = FindFlight(flightId);
            if (flight == null)
            {
                throw new KeyNotFoundException($"Flight '{flightId}' was not found.");
            }

            bookingRepository.UpdateBooking(
                bookingId,
                passengerId,
                flight.Id,
                ticketClass,
                flight.GetPrice(ticketClass));
        }

        public Booking? GetBookingForPassenger(string bookingId, string passengerId) =>
            bookingRepository.GetBookingForPassenger(bookingId, passengerId);

        public List<Booking> GetBookingsForFlight(string flightId) =>
            bookingRepository.GetBookingsForFlight(flightId);

        public List<Booking> GetBookingByPriceRange(decimal minPrice, decimal maxPrice)
        {

            if (minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum price cannot be greater than maximum price.");

            }
            return bookingRepository.GetBookingsByPriceRange(minPrice, maxPrice);
        }

        public List<Booking> GetBookingByTicketClass(TicketClass ticketClass) =>
            bookingRepository.GetBookingsByTicketClass(ticketClass);

        public List<Booking> GetBookingByPassenger(string passengerId) =>
            bookingRepository.GetBookingsByPassenger(passengerId);

        public List<Booking> GetBookingsByDepartureDate(DateTime date) =>
            bookingRepository.GetBookingsByDepartureDate(date);

        public List<Booking> GetBookingsByDepartureCountry(string country) =>
            bookingRepository.GetBookingsByDepartureCountry(country);

        public List<Booking> GetBookingsByDestinationCountry(string country) =>
            bookingRepository.GetBookingsByDestinationCountry(country);

        public List<Booking> GetBookingsByDepartureAirport(string airport) =>
            bookingRepository.GetBookingsByDepartureAirport(airport);

        public List<Booking> GetBookingsByArrivalAirport(string airport) =>
            bookingRepository.GetBookingsByArrivalAirport(airport);
    }
    
