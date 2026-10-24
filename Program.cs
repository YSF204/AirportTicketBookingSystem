using AirportTicketBookingSystem.Models;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Services;

var flight = new Flight
{
    Id = "FL1323",
    DepartureCountry = "USA",
    DestinationCountry = "UK",
    DepartureAirport = "JFK",
    ArrivalAirport = "LHR",
    EconomyPrice = 0m,
    BusinessPrice = 1500m,
    FirstClassPrice = 3000m,
    DepartureDate = DateTime.Now.AddDays(10)
}; 
var flightRepository = new FlightRepository();
var flightService = new FlightService(flightRepository);
var errors = flightService.AddFlight(flight);
if (errors.Count == 0)
{
    Console.WriteLine("Flight added.");
}
else
{
    foreach (var error in errors)
        Console.WriteLine($"- {error}");
}