using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Services
{

    /// <summary>
    /// This interface defines the operations for the required services for the booking system.
    /// </summary>
    public interface IBookingService
    {



        /// <summary>
        /// it finds the flight by the flight Id and returns the flights if exists,
        /// otherwise it returns null.
        /// </summary>
        Flight? FindFlight(string flightId);

        /// <summary>
        /// it finds the passenger by the passenger Id and returns the passenger if exists,
        /// otherwise it returns null.
        /// </summary>
        Passenger? FindPassenger(string passengerId);

        /// <summary>
        /// it registers a new passenger with the provided passenger Id and returns the registered passenger.
        /// </summary>
        Passenger? RegisterPassenger(string passengerId);

        /// <summary>
        /// it books a flight for the specified passenger with the given flight Id and ticket class.
        /// </summary>
        Booking BookFlight(string passengerId, string flightId, TicketClass ticketClass);

        /// <summary>
        /// it retrieves all bookings associated with the specified passenger Id.
        /// </summary>
        List<Booking> GetBookingsForPassenger(string passengerId);

        /// <summary>
        /// it cancels the booking with the specified booking Id for the given passenger Id.
        /// </summary>
        bool CancelBooking(string bookingId, string passengerId);

        /// <summary>
        /// it updates the booking with the specified booking Id for the given passenger Id, flight Id, and ticket class.
        /// </summary>
        bool UpdateBooking(string bookingId, string passengerId, string flightId, TicketClass ticketClass);

        /// <summary>
        /// it retrieves the booking with the specified booking Id for the given passenger Id.
        /// </summary>
        Booking GetBookingForPassenger(string bookingId, string passengerId);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>
        List<Booking> GetBookingsForFlight(string flightId);

        /// <summary>
        /// it retrieves all bookings associated with the specified ticket class.
        /// </summary>
        List<Booking> GetBookingByPriceRange(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// it retrieves all bookings associated with the specified ticket class.
        /// </summary>
        List<Booking> GetBookingByTicketClass(TicketClass ticketClass);

        /// <summary>
        /// it retrieves all bookings associated with the specified passenger Id.
        /// </summary>
        List<Booking> GetBookingByPassenger(string passengerId);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>
        List<Booking> GetBookingsByDepartureDate(DateTime date);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>      
        List<Booking> GetBookingsByDepartureCountry(string country);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>
        List<Booking> GetBookingsByDestinationCountry(string country);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>
        List<Booking> GetBookingsByDepartureAirport(string airport);

        /// <summary>
        /// it retrieves all bookings associated with the specified flight Id.
        /// </summary>
        List<Booking> GetBookingsByArrivalAirport(string airport);





    }
}
