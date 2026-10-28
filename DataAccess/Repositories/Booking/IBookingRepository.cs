using AirportTicketBookingSystem.DataAccess.Entities;

namespace AirportTicketBookingSystem.DataAccess.Repositories.Bookings;

public interface IBookingRepository
{
    /// <summary>Returns all stored bookings.</summary>
    List<Booking> GetBookings();

    /// <summary>Adds and saves a booking.</summary>
    void AddBooking(Booking booking);

    /// <summary>Finds a booking belonging to the specified passenger.</summary>
    Booking? GetBookingForPassenger(string bookingId, string passengerId);

    /// <summary>Returns bookings for the specified flight.</summary>
    List<Booking> GetBookingsForFlight(string flightId);

    /// <summary>Returns bookings with prices inside the inclusive range.</summary>
    List<Booking> GetBookingsByPriceRange(decimal minPrice, decimal maxPrice);

    /// <summary>Returns bookings for the specified ticket class.</summary>
    List<Booking> GetBookingsByTicketClass(TicketClass ticketClass);

    /// <summary>Returns bookings for the specified passenger.</summary>
    List<Booking> GetBookingsByPassenger(string passengerId);

    /// <summary>Returns bookings for flights departing on the specified date.</summary>
    List<Booking> GetBookingsByDepartureDate(DateTime date);

    /// <summary>Returns bookings for flights departing from the specified country.</summary>
    List<Booking> GetBookingsByDepartureCountry(string country);

    /// <summary>Returns bookings for flights arriving in the specified country.</summary>
    List<Booking> GetBookingsByDestinationCountry(string country);

    /// <summary>Returns bookings for flights departing from the specified airport.</summary>
    List<Booking> GetBookingsByDepartureAirport(string airport);

    /// <summary>Returns bookings for flights arriving at the specified airport.</summary>
    List<Booking> GetBookingsByArrivalAirport(string airport);

    /// <summary>Cancels a booking belonging to the specified passenger
    /// it throws an exception if the booking does not exist for this passenger.
    /// </summary>
    void CancelBooking(string bookingId, string passengerId);

    /// <summary>Updates a booking's flight, ticket class, and paid price
    /// it thorws an exception if the booking does not exist for this passenger.
    /// </summary>
    void UpdateBooking(string bookingId, string passengerId, string flightId, TicketClass ticketClass, decimal pricePaid);

    /// <summary>Saves the supplied bookings.</summary>
    void SaveBookings(List<Booking> bookings);
}