using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Services;

namespace AirportTicketBookingSystem.Menus;

public class PassengerMenu
{
    private readonly FlightService flightService;
    private readonly BookingService bookingService;

    public PassengerMenu(FlightService flightService, BookingService bookingService)
    {
        this.flightService = flightService;
        this.bookingService = bookingService;
    }

    public void Run()
    {
        string? passengerId = ChoosePassenger();
        if (passengerId == null)
        {
            return;
        }

        Console.WriteLine($"Welcome, passenger {passengerId}.");

        while (true)
        {
            Console.WriteLine("\nPassenger Menu");
            Console.WriteLine("1. Search flights and book");
            Console.WriteLine("2. View my bookings");
            Console.WriteLine("3. Cancel a booking");
            Console.WriteLine("4. Change booking class");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1": SearchFlightsAndBook(passengerId); break;
                case "2": ViewBookings(passengerId); break;
                case "3": CancelBooking(passengerId); break;
                case "4": ChangeBookingClass(passengerId); break;
                case "0": return;
                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private string? ChoosePassenger()
    {
        while (true)
        {
            
            Console.WriteLine("1. Existing passenger");
            Console.WriteLine("2. Register with a new ID");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Passenger ID: ");
                    string existingId = Console.ReadLine() ?? "";
                    var passenger = bookingService.FindPassenger(existingId);
                    if (passenger != null)
                    {
                        return passenger.Id;
                    }

                    Console.WriteLine("Passenger not found.");
                    break;

                case "2":
                    Console.Write("Choose an ID: ");
                    string newId = Console.ReadLine() ?? "";
                    passenger = bookingService.RegisterPassenger(newId);
                    if (passenger != null)
                    {
                        Console.WriteLine("Registration successful.");
                        return passenger.Id;
                    }

                    Console.WriteLine("ID is required and must not already be registered.");
                    break;

                case "0":
                    return null;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void SearchFlightsAndBook(string passengerId)
    {
        Console.Write("Departure country (leave blank to skip): ");
        string? departureCountry = ReadOptionalText();
        Console.Write("Destination country (leave blank to skip): ");
        string? destinationCountry = ReadOptionalText();
        DateTime? departureDate = ReadOptionalDate();
        Console.Write("Departure airport (leave blank to skip): ");
        string? departureAirport = ReadOptionalText();
        Console.Write("Arrival airport (leave blank to skip): ");
        string? arrivalAirport = ReadOptionalText();
        TicketClass ticketClass = ReadTicketClass();
        decimal? maxPrice = ReadOptionalPrice();

        var flights = flightService.SearchFlights(
            departureCountry,
            destinationCountry,
            departureDate,
            departureAirport,
            arrivalAirport,
            maxPrice,
            maxPrice.HasValue ? ticketClass : null);

        if (flights.Count == 0)
        {
            Console.WriteLine("No matching flights found.");
            return;
        }

        foreach (var flight in flights)
        {
            Console.WriteLine(
                $"{flight.Id} | {flight.DepartureCountry} ({flight.DepartureAirport}) to " +
                $"{flight.DestinationCountry} ({flight.ArrivalAirport}) | " +
                $"{flight.DepartureDate:dd MMM yyyy, h:mm tt} | {ticketClass}: {flight.GetPrice(ticketClass):C}");
        }

        Console.Write("Enter the flight ID to book (or leave blank to return): ");
        string flightId = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(flightId))
        {
            return;
        }

        var selectedFlight = flights.FirstOrDefault(flight =>
            flight.Id.Equals(flightId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (selectedFlight == null)
        {
            Console.WriteLine("That flight is not in the search results.");
            return;
        }

        var booking = bookingService.BookFlight(passengerId, selectedFlight.Id, ticketClass);
        Console.WriteLine($"Booking created. Booking ID: {booking.Id}. Price: {booking.PricePaid:C}");
    }

    private void ViewBookings(string passengerId)
    {
        var bookings = bookingService.GetBookingsForPassenger(passengerId);
        if (bookings.Count == 0)
        {
            Console.WriteLine("You have no bookings.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void CancelBooking(string passengerId)
    {
        Console.Write("Booking ID to cancel: ");
        string bookingId = Console.ReadLine() ?? "";

        if (bookingService.CancelBooking(bookingId, passengerId))
        {
            Console.WriteLine("Booking cancelled.");
        }
        else
        {
            Console.WriteLine("Booking not found.");
        }
    }

    private void ChangeBookingClass(string passengerId)
    {
        Console.Write("Booking ID to change: ");
        string bookingId = Console.ReadLine() ?? "";
        TicketClass ticketClass = ReadTicketClass();

        if (bookingService.UpdateBookingTicketClass(bookingId, passengerId, ticketClass))
        {
            Console.WriteLine("Booking class updated.");
        }
        else
        {
            Console.WriteLine("Booking not found.");
        }
    }

    private string? ReadOptionalText()
    {
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }

    private DateTime? ReadOptionalDate()
    {
        while (true)
        {
            Console.Write("Departure date yyyy-MM-dd (leave blank to skip): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (DateTime.TryParseExact(input, "yyyy-MM-dd", null,
                    System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                return date;
            }

            Console.WriteLine("Enter the date as yyyy-MM-dd.");
        }
    }

    private TicketClass ReadTicketClass()
    {
        while (true)
        {
            Console.Write("Ticket class (Economy, Business, FirstClass): ");
            string? input = Console.ReadLine();

            if (Enum.TryParse(input, true, out TicketClass ticketClass) &&
                Enum.IsDefined(typeof(TicketClass), ticketClass))
            {
                return ticketClass;
            }

            Console.WriteLine("Please enter Economy, Business, or FirstClass.");
        }
    }

    private decimal? ReadOptionalPrice()
    {
        while (true)
        {
            Console.Write("Maximum price (leave blank for no limit): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (decimal.TryParse(input, out decimal price) && price >= 0)
            {
                return price;
            }

            Console.WriteLine("Enter a valid non-negative price.");
        }
    }

    private void DisplayBookings(List<Booking> bookings)
    {
        foreach (var booking in bookings)
        {
            Console.WriteLine(
                $"Booking ID: {booking.Id} | Flight: {booking.FlightId} | " +
                $"Class: {booking.TicketClass} | Price: {booking.PricePaid:C} | " +
                $"Booked: {booking.BookingDate:dd MMM yyyy, h:mm tt}");
        }
    }
}
