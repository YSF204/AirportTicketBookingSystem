namespace AirportTicketBookingSystem.Models
{
    public class Flight
    {
        public string Id { get; set; } = string.Empty;

        public string DepartureCountry { get; set; } = string.Empty;

        public string DestinationCountry { get; set; } = string.Empty;

        public string DepartureAirport { get; set; } = string.Empty;

        public string ArrivalAirport { get; set; } = string.Empty;

        public decimal EconomyPrice { get; set; }

        public decimal BusinessPrice { get; set; }

        public decimal FirstClassPrice { get; set; }

        public decimal GetPrice(TicketClass ticketClass)
        {
            return ticketClass switch
            {
                TicketClass.Economy => EconomyPrice,
                TicketClass.Business => BusinessPrice,
                TicketClass.FirstClass => FirstClassPrice,
                _ => throw new ArgumentOutOfRangeException(nameof(ticketClass))
            };
        }

        public DateTime DepartureDate { get; set; }
    }

    
}