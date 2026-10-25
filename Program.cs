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
managerMenu.Run();
