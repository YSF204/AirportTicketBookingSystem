using System.Text.Json;
using AirportTicketBookingSystem.Menus;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Services;

var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository();
var passengerRepository = new PassengerRepository();


var flightService = new FlightService(flightRepository);
var bookingService = new BookingService(
    flightRepository,
    passengerRepository,
    bookingRepository);
var managerMenu = new ManagerMenu(flightService, bookingService);
var passengerMenu = new PassengerMenu(flightService, bookingService);

Console.WriteLine("1. Manager");
Console.WriteLine("2. Passenger");
Console.Write("Choose a menu: ");

try
{
    switch (Console.ReadLine())
    {
        case "1":
            managerMenu.Run();
            break;
        case "2":
            passengerMenu.Run();
            break;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}
catch (JsonException error)
{
    Console.WriteLine($"A data file contains invalid JSON: {error.Message}");
}
catch (IOException error)
{
    Console.WriteLine($"A data file could not be read or saved: {error.Message}");
}
catch (UnauthorizedAccessException error)
{
    Console.WriteLine($"A data file could not be accessed: {error.Message}");
}
