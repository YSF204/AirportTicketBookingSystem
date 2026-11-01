using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Repositories.Flights;
using AirportTicketBookingSystem.DataAccess.Storage;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Bookings;
    public class BookingRepository : IBookingRepository
    {
        private readonly IFlightRepository flightRepository;
        private readonly JsonRepository<Booking> bookingRepository = new("bookings.json");

        public BookingRepository(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public List<Booking> GetBookings()
        {
            return bookingRepository.GetItems();
        }

        public void AddBooking(Booking booking)
        {
            var bookings = GetBookings();
            bookings.Add(booking);
            SaveBookings(bookings);
        }

        public Booking? GetBookingForPassenger(string bookingId, string passengerId)
        {
            var bookings = GetActiveBookings();
            return FindBooking(bookings, bookingId, passengerId);
        }

        public List<Booking> GetBookingsForFlight(string flightId)
        {
            var bookings = GetActiveBookings();
            return bookings.Where(booking => booking.FlightId.Equals(
                flightId,
                StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Booking> GetBookingsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var bookings = GetActiveBookings();
            return bookings.Where(booking =>
                booking.PricePaid >= minPrice && booking.PricePaid <= maxPrice).ToList();
        }

        public List<Booking> GetBookingsByTicketClass(TicketClass ticketClass)
        {
            var bookings = GetActiveBookings();
            return bookings.Where(booking => booking.TicketClass == ticketClass).ToList();
        }

        public List<Booking> GetBookingsByPassenger(string passengerId)
        {
            var bookings = GetActiveBookings();
            return bookings.Where(booking => booking.PassengerId.Equals(
                passengerId,
                StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Booking> GetBookingsByDepartureDate(DateTime date) =>
            GetBookingsForFlights(flight => flight.DepartureDate.Date == date.Date);

        public List<Booking> GetBookingsByDepartureCountry(string country) =>
            GetBookingsForFlights(flight => flight.DepartureCountry.Equals(
                country,
                StringComparison.OrdinalIgnoreCase));

        public List<Booking> GetBookingsByDestinationCountry(string country) =>
            GetBookingsForFlights(flight => flight.DestinationCountry.Equals(
                country,
                StringComparison.OrdinalIgnoreCase));

        public List<Booking> GetBookingsByDepartureAirport(string airport) =>
            GetBookingsForFlights(flight => flight.DepartureAirport.Equals(
                airport,
                StringComparison.OrdinalIgnoreCase));

        public List<Booking> GetBookingsByArrivalAirport(string airport) =>
            GetBookingsForFlights(flight => flight.ArrivalAirport.Equals(
                airport,
                StringComparison.OrdinalIgnoreCase));

        public void UpdateBooking(Booking booking)
        {
            var bookings = GetBookings();
            var index = bookings.FindIndex(existing =>
                existing.Id.Equals(booking.Id, StringComparison.OrdinalIgnoreCase) &&
                existing.PassengerId.Equals(booking.PassengerId, StringComparison.OrdinalIgnoreCase));
            if (index < 0)
            {
                return;
            }

            bookings[index] = booking;
            SaveBookings(bookings);
        }

        public void SaveBookings(List<Booking> bookings)
        {
            bookingRepository.SaveItems(bookings);
        }

        private List<Booking> GetActiveBookings()
        {
            return GetBookings().Where(booking => !booking.IsCancelled).ToList();
        }

        private List<Booking> GetBookingsForFlights(Func<Flight, bool> matches)
        {
            var bookings = GetActiveBookings();
            var flights = flightRepository.GetFlights();
            var matchingFlightIds = flights
                .Where(matches)
                .Select(flight => flight.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return bookings.Where(booking => matchingFlightIds.Contains(booking.FlightId)).ToList();
        }

        private static Booking? FindBooking(
            IEnumerable<Booking> bookings,
            string bookingId,
            string passengerId) =>
            bookings.FirstOrDefault(booking =>
                booking.Id.Equals(bookingId.Trim(), StringComparison.OrdinalIgnoreCase) &&
                booking.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase));
    }