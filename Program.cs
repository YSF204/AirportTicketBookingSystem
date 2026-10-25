using AirportTicketBookingSystem.Menus;
using AirportTicketBookingSystem.Repositories;
using AirportTicketBookingSystem.Services;
using AirportTicketBookingSystem.Util;

var flightRepository = new FlightRepository();
var bookingRepository = new BookingRepository();
var passengerRepository = new PassengerRepository();


var flightService = new FlightService(flightRepository);
var bookingService = new BookingService(
    flightRepository,
    passengerRepository,
    bookingRepository);
var reader = new Reader();

var managerMenu = new ManagerMenu(flightService, bookingService);
managerMenu.Run();
