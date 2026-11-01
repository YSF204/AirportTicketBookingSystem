using AirportTicketBookingSystem.DataAccess.Entities;
using AirportTicketBookingSystem.DataAccess.Repositories.Bookings;
using AirportTicketBookingSystem.DataAccess.Repositories.Flights;
using AirportTicketBookingSystem.DataAccess.Repositories.Passengers;

namespace AirportTicketBookingSystem.Business.Services.Bookings;

public class BookingService : IBookingService
{
    private readonly IBookingRepository bookingRepository;
    private readonly IFlightRepository flightRepository;
    private readonly IPassengerRepository passengerRepository;

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

    public Flight? FindFlight(string flightId)
    {
        return flightRepository.FindFlight(flightId);
    }

    public Passenger? FindPassenger(string passengerId)
    {
        return passengerRepository.FindPassenger(passengerId);
    }

    public Passenger? RegisterPassenger(string passengerId)
    {
        passengerId = passengerId.Trim();
        if (string.IsNullOrWhiteSpace(passengerId)) return null;

        if (passengerRepository.FindPassenger(passengerId) != null) return null;

        var passenger = new Passenger { Id = passengerId };
        passengerRepository.AddPassenger(passenger);
        return passenger;
    }

    public Booking BookFlight(string passengerId, string flightId, TicketClass ticketClass)
    {
        var passenger = FindPassenger(passengerId);
        var flight = FindFlight(flightId);

        if (passenger == null) throw new ArgumentException($"Passenger with ID {passengerId} not found.");

        if (flight == null) throw new ArgumentException($"Flight with ID {flightId} not found.");

        var pricePaid = flight.GetPrice(ticketClass);

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

    public void CancelBooking(string bookingId, string passengerId)
    {
        var booking = bookingRepository.GetBookingForPassenger(bookingId, passengerId);
        if (booking == null)
            throw new KeyNotFoundException($"Booking '{bookingId}' was not found for passenger '{passengerId}'.");

        bookingRepository.CancelBooking(booking);
    }

    public void UpdateBooking(string bookingId, string passengerId, string flightId, TicketClass ticketClass)
    {
        var booking = bookingRepository.GetBookingForPassenger(bookingId, passengerId);
        if (booking == null)
            throw new KeyNotFoundException($"Booking '{bookingId}' was not found for passenger '{passengerId}'.");

        if (!Enum.IsDefined(ticketClass)) throw new ArgumentOutOfRangeException(nameof(ticketClass));

        var flight = FindFlight(flightId);
        if (flight == null) throw new KeyNotFoundException($"Flight '{flightId}' was not found.");

        booking.FlightId = flight.Id;
        booking.TicketClass = ticketClass;
        booking.PricePaid = flight.GetPrice(ticketClass);
        bookingRepository.UpdateBooking(booking);
    }

    public Booking? GetBookingForPassenger(string bookingId, string passengerId)
    {
        return bookingRepository.GetBookingForPassenger(bookingId, passengerId);
    }

    public List<Booking> GetBookingsForFlight(string flightId)
    {
        return bookingRepository.GetBookingsForFlight(flightId);
    }

    public List<Booking> GetBookingByPriceRange(decimal minPrice, decimal maxPrice)
    {
        if (minPrice > maxPrice) throw new ArgumentException("Minimum price cannot be greater than maximum price.");
        return bookingRepository.GetBookingsByPriceRange(minPrice, maxPrice);
    }

    public List<Booking> GetBookingByTicketClass(TicketClass ticketClass)
    {
        return bookingRepository.GetBookingsByTicketClass(ticketClass);
    }

    public List<Booking> GetBookingByPassenger(string passengerId)
    {
        return bookingRepository.GetBookingsByPassenger(passengerId);
    }

    public List<Booking> GetBookingsByDepartureDate(DateTime date)
    {
        return bookingRepository.GetBookingsByDepartureDate(date);
    }

    public List<Booking> GetBookingsByDepartureCountry(string country)
    {
        return bookingRepository.GetBookingsByDepartureCountry(country);
    }

    public List<Booking> GetBookingsByDestinationCountry(string country)
    {
        return bookingRepository.GetBookingsByDestinationCountry(country);
    }

    public List<Booking> GetBookingsByDepartureAirport(string airport)
    {
        return bookingRepository.GetBookingsByDepartureAirport(airport);
    }

    public List<Booking> GetBookingsByArrivalAirport(string airport)
    {
        return bookingRepository.GetBookingsByArrivalAirport(airport);
    }
}
