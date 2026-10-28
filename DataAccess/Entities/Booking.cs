

namespace AirportTicketBookingSystem.DataAccess.Entities;
    public class Booking
    {
      public  string Id { get; set; } = string.Empty;
      public  string PassengerId { get; set; } = string.Empty;
      public  string FlightId { get; set; } = string.Empty;
      public  TicketClass TicketClass { get; set; } = TicketClass.Economy;
      public  decimal PricePaid { get; set; } = 0m;
      public  DateTime BookingDate { get; set; } = DateTime.Now;

    }

