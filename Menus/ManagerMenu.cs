using System.Globalization;
using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.Util;

namespace AirportTicketBookingSystem.Menus;

public class ManagerMenu
{
    private readonly FlightService flightService;
    private readonly BookingService bookingService;

    public ManagerMenu(FlightService flightService, BookingService bookingService)
    {
        this.flightService = flightService;
        this.bookingService = bookingService;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\nManager Menu");
            Console.WriteLine("10. Find bookings by arrival airport");
            Console.WriteLine("9. Find bookings by departure airport");
            Console.WriteLine("8. Find bookings by destination country");
            Console.WriteLine("7. Find bookings by departure country");
            Console.WriteLine("6. Find bookings by departure date");
            Console.WriteLine("5. Find bookings by passenger");
            Console.WriteLine("4. Find bookings by ticket class");
            Console.WriteLine("3. Find bookings by price range");
            Console.WriteLine("2. Find bookings by flight");
            Console.WriteLine("1. Add a flight");
            Console.WriteLine("0. Exit");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("A. Load Flights from CSV file");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine()?.ToUpper())
            {
                case "A": LoadFlightsFromCSV(); break;
                case "10": GetBookingsByArrivalAirport(); break;
                case "9": GetBookingsByDepartureAirport(); break;
                case "8": GetBookingsByDestinationCountry(); break;
                case "7": GetBookingsByDepartureCountry(); break;
                case "6": GetBookingsByDepartureDate(); break;
                case "5": GetBookingsByPassenger(); break;
                case "4": GetBookingByTicketClass(); break;
                case "3": FindBookingByPrice(); break;
                case "2": FindBookingsByFlight(); break;
                case "1": AddFlight(); break;
                case "0": return;
                default:
                    Console.WriteLine("Invalid option. Please choose a listed option.");
                    break;
            }
        }
    }

    private void AddFlight()
    {
        Console.Write("Departure country: ");
        string departureCountry = Console.ReadLine() ?? "";
        Console.Write("Destination country: ");
        string destinationCountry = Console.ReadLine() ?? "";
        Console.Write("Departure airport: ");
        string departureAirport = Console.ReadLine() ?? "";
        Console.Write("Arrival airport: ");
        string arrivalAirport = Console.ReadLine() ?? "";

        decimal economyPrice = ReadDecimal("Economy price: ");
        decimal businessPrice = ReadDecimal("Business price: ");
        decimal firstClassPrice = ReadDecimal("First Class price: ");
        DateTime departureDate = ReadDepartureDateTime();

        var flight = new Flight
        {
            Id = Guid.NewGuid().ToString(),
            DepartureCountry = departureCountry,
            DestinationCountry = destinationCountry,
            DepartureAirport = departureAirport,
            ArrivalAirport = arrivalAirport,
            EconomyPrice = economyPrice,
            BusinessPrice = businessPrice,
            FirstClassPrice = firstClassPrice,
            DepartureDate = departureDate
        };

        List<string> errors = flightService.AddFlight(flight);
        if (errors.Count == 0)
        {
            Console.WriteLine("Flight added successfully.");
            return;
        }

        Console.WriteLine("Flight was not added:");
        foreach (string error in errors)
        {
            Console.WriteLine($"- {error}");
        }
    }

    private decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid number.");
        }
    }

    private DateTime ReadDepartureDateTime()
    {
        while (true)
        {
            Console.Write("Departure date (e.g. 30 Oct 2026): ");
            string? dateInput = Console.ReadLine();
            Console.Write("Departure time (e.g. 2:30 PM): ");
            string? timeInput = Console.ReadLine();

            if (DateTime.TryParse($"{dateInput} {timeInput}", out DateTime departureDate))
            {
                return departureDate;
            }

            Console.WriteLine("I couldn't understand that date and time. Please try again.\n");
        }
    }

    private void FindBookingsByFlight()
    {
        Console.Write("Enter the flight ID to find bookings for: ");
        string flightId = Console.ReadLine() ?? "";
        var bookings = bookingService.GetBookingsForFlight(flightId);

        if (bookings.Count == 0)
        {
            Console.WriteLine($"No bookings found for flight ID {flightId}.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void FindBookingByPrice()
    {
        decimal minPrice = ReadDecimal("Minimum price: ");
        decimal maxPrice = ReadDecimal("Maximum price: ");
        var bookings = bookingService.GetBookingByPriceRange(minPrice, maxPrice);

        if (bookings.Count == 0)
        {
            Console.WriteLine($"No bookings found in the price range {minPrice} - {maxPrice}.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void GetBookingByTicketClass()
    {
        Console.WriteLine("Enter the ticket class to find bookings for (Economy, Business, FirstClass):");
        string? ticketClassInput = Console.ReadLine();
        if (!Enum.TryParse<TicketClass>(ticketClassInput, true, out TicketClass ticketClass))
        {
            Console.WriteLine("Invalid ticket class. Please enter Economy, Business, or FirstClass.");
            return;
        }

        var bookings = bookingService.GetBookingByTicketClass(ticketClass);
        if (bookings.Count == 0)
        {
            Console.WriteLine($"No bookings found for ticket class {ticketClass}.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void GetBookingsByPassenger()
    {
        Console.Write("Enter the passenger ID to find bookings for: ");
        string passengerId = Console.ReadLine() ?? "";
        var bookings = bookingService.GetBookingByPassenger(passengerId);

        if (bookings.Count == 0)
        {
            Console.WriteLine($"No bookings found for passenger ID {passengerId}.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void GetBookingsByDepartureDate()
    {
        Console.Write("Enter departure date (yyyy-MM-dd): ");
        string? dateInput = Console.ReadLine();
        if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
            return;
        }

        var bookings = bookingService.GetBookingsByDepartureDate(date);
        if (bookings.Count == 0)
        {
            Console.WriteLine($"No bookings found for departure date {date:yyyy-MM-dd}.");
            return;
        }

        DisplayBookings(bookings);
    }

    private void GetBookingsByDepartureCountry()
    {
        Console.Write("Departure country: ");
        string country = Console.ReadLine() ?? "";
        DisplayBookings(bookingService.GetBookingsByDepartureCountry(country));
    }

    private void GetBookingsByDestinationCountry()
    {
        Console.Write("Destination country: ");
        string country = Console.ReadLine() ?? "";
        DisplayBookings(bookingService.GetBookingsByDestinationCountry(country));
    }

    private void GetBookingsByDepartureAirport()
    {
        Console.Write("Departure airport: ");
        string airport = Console.ReadLine() ?? "";
        DisplayBookings(bookingService.GetBookingsByDepartureAirport(airport));
    }

    private void GetBookingsByArrivalAirport()
    {
        Console.Write("Arrival airport: ");
        string airport = Console.ReadLine() ?? "";
        DisplayBookings(bookingService.GetBookingsByArrivalAirport(airport));
    }

    private void DisplayBookings(List<Booking> bookings)
    {
        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found.");
            return;
        }

        Console.WriteLine(new string('-', 50));
        foreach (var booking in bookings)
        {
            Console.WriteLine(
                $"Booking ID:   {booking.Id}\n" +
                $"Passenger ID: {booking.PassengerId}\n" +
                $"Flight ID:    {booking.FlightId}\n" +
                $"Ticket Class: {booking.TicketClass}\n" +
                $"Price Paid:   {booking.PricePaid:C}\n" +
                $"Booking Date: {booking.BookingDate:dd MMM yyyy, h:mm tt}");
            Console.WriteLine(new string('-', 50));
        }

    }

    private void LoadFlightsFromCSV()
    {
        var flights = Reader.ReadCsv();

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights were loaded from the CSV file.");
            return;
        }

        var errors = flightService.AddFlights(flights, out int addedCount);
        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }

        Console.WriteLine($"Imported {addedCount} of {flights.Count} flights.");
    }
}
