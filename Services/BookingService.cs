

using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using System.Linq;
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
            return passengers.FirstOrDefault(p => p.Id == passengerId);

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

        

        public List<Booking> GetBookingForPassenger(string passengerId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.Where(b => b.PassengerId == passengerId).ToList();
        }

        public bool CancelBooking(string bookingId , string passengerId)
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



        public bool UpdateBookingTicketClass(string bookingId , string passengerId, TicketClass ticketClass)
        {
            var booking = bookingRepository.GetBookings();
            var existingBookings = booking.FirstOrDefault(b => b.Id == bookingId && b.PassengerId == passengerId);

            if(existingBookings == null)
            {
                return false;
            };

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

        public Booking? GetBookingForPassenger(string bookingId , string passengerId)
        {
            var bookings = bookingRepository.GetBookings();
            return bookings.FirstOrDefault(b => b.Id == bookingId && b.PassengerId == passengerId);
        }
    }
        

}

