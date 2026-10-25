

using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using System.Linq;
using System.Net;
namespace AirportTicketBookingSystem.Services
{
    public class BookingService
    {
        private readonly FlightRepository flightRepository;
        private readonly PassengerRepository passengerRepository;
        private readonly BookingRepository bookingRepository;

        public BookingService(
            FlightRepository flightRepository,
            PassengerRepository passengerRepository,
            BookingRepository bookingRepository
            )
        {
            this.flightRepository = flightRepository;
            this.passengerRepository = passengerRepository;
            this.bookingRepository = bookingRepository;
        }

        public Flight? FindFlight(string flightId)
        {
            var flights = flightRepository.GetFlights();
            return flights.FirstOrDefault(f => f.Id == flightId);
        }

        public Passenger? FindPassenger(string passengerId)
        {
            var passengers = passengerRepository.GetPassengers();
            return passengers.FirstOrDefault(p => p.Id.Equals(
                passengerId,
                StringComparison.OrdinalIgnoreCase));

        }

        public Passenger? RegisterPassenger(string passengerId)
        {
            passengerId = passengerId.Trim();
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return null;
            }

            var passengers = passengerRepository.GetPassengers();
            if (passengers.Any(passenger => passenger.Id.Equals(
                    passengerId,
                    StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }

            var passenger = new Passenger { Id = passengerId };
            passengers.Add(passenger);
            passengerRepository.SavePassengers(passengers);

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

            List<Booking> bookings = bookingRepository.GetBookings();

            var booking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                PassengerId = passengerId,
                FlightId = flightId,
                TicketClass = ticketClass,
                PricePaid = pricePaid,
                BookingDate = DateTime.UtcNow
            };

            bookings.Add(booking);
            bookingRepository.SaveBookings(bookings);

            return booking;
        }



        public List<Booking> GetBookingsForPassenger(string passengerId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.Where(b => b.PassengerId == passengerId).ToList();
        }

        public bool CancelBooking(string bookingId, string passengerId)
        {

            var bookings = bookingRepository.GetBookings();
            var booking = bookings.FirstOrDefault(b => b.Id == bookingId && b.PassengerId == passengerId);
            if (booking == null)
            {
                return false;
            }

            bookings.Remove(booking);
            bookingRepository.SaveBookings(bookings);

            return true;
        }



        public bool UpdateBookingTicketClass(string bookingId, string passengerId, TicketClass ticketClass)
        {
            var booking = bookingRepository.GetBookings();
            var existingBookings = booking.FirstOrDefault(b => b.Id == bookingId && b.PassengerId == passengerId);

            if (existingBookings == null)
            {
                return false;
            }
            ;

            var flight = FindFlight(existingBookings.FlightId);
            if (flight == null)
            {
                return false;
            }


            existingBookings.TicketClass = ticketClass;
            existingBookings.PricePaid = flight.GetPrice(ticketClass);

            bookingRepository.SaveBookings(booking);

            return true;

        }

        public Booking? GetBookingForPassenger(string bookingId, string passengerId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.FirstOrDefault(b => b.Id == bookingId && b.PassengerId == passengerId);
        }

        public List<Booking> GetBookingsForFlight(string flightId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.Where(booking => booking.FlightId.Equals(flightId,
                    StringComparison.OrdinalIgnoreCase)).ToList();

        }

        public List<Booking> GetBookingByPriceRange(decimal minPrice, decimal maxPrice)
        {

            if (minPrice > maxPrice)
            {
                throw new ArgumentException("Minimum price cannot be greater than maximum price.");

            }
            else
            {
                var bookings = bookingRepository.GetBookings();
                return bookings.Where(booking => booking.PricePaid >= minPrice && booking.PricePaid <= maxPrice).ToList();

            }
        }

        public List<Booking> GetBookingByTicketClass(TicketClass ticketClass)
        {

            var bookings = bookingRepository.GetBookings();
            return bookings.Where(booking => booking.TicketClass == ticketClass).ToList();
        }

        public List<Booking> GetBookingByPassenger(string passengerId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.Where(booking => booking.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase)).ToList();

        }

        public List<Booking> GetBookingsByDepartureDate(DateTime date)
        {
            var bookings = bookingRepository.GetBookings();
            var flights = flightRepository.GetFlights();

            return bookings
                .Where(booking => flights.Any(flight =>
                    flight.Id.Equals(
                        booking.FlightId,
                        StringComparison.OrdinalIgnoreCase) &&
                    flight.DepartureDate.Date == date.Date))
                .ToList();
        }

        public List<Booking> GetBookingsByDepartureCountry(string country)
        {
            var bookings = bookingRepository.GetBookings();
            var flights = flightRepository.GetFlights();

            return bookings.Where(booking => flights.Any(flight =>
                flight.Id.Equals(booking.FlightId, StringComparison.OrdinalIgnoreCase) &&
                flight.DepartureCountry.Equals(country, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public List<Booking> GetBookingsByDestinationCountry(string country)
        {
            var bookings = bookingRepository.GetBookings();
            var flights = flightRepository.GetFlights();

            return bookings.Where(booking => flights.Any(flight =>
                flight.Id.Equals(booking.FlightId, StringComparison.OrdinalIgnoreCase) &&
                flight.DestinationCountry.Equals(country, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public List<Booking> GetBookingsByDepartureAirport(string airport)
        {
            var bookings = bookingRepository.GetBookings();
            var flights = flightRepository.GetFlights();

            return bookings.Where(booking => flights.Any(flight =>
                flight.Id.Equals(booking.FlightId, StringComparison.OrdinalIgnoreCase) &&
                flight.DepartureAirport.Equals(airport, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        public List<Booking> GetBookingsByArrivalAirport(string airport)
        {
            var bookings = bookingRepository.GetBookings();
            var flights = flightRepository.GetFlights();

            return bookings.Where(booking => flights.Any(flight =>
                flight.Id.Equals(booking.FlightId, StringComparison.OrdinalIgnoreCase) &&
                flight.ArrivalAirport.Equals(airport, StringComparison.OrdinalIgnoreCase))).ToList();
        }
    }
    }

