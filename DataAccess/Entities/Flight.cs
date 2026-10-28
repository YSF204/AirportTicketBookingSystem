using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingSystem.DataAccess.Entities;
    public class Flight
    {
        [Display(Name = "Flight ID")]
        [Required]
        [Description("Must be unique")]
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Departure Country")]
        [Required]
        [Description("Must differ from destination country")]
        public string DepartureCountry { get; set; } = string.Empty;

        [Display(Name = "Destination Country")]
        [Required]
        [Description("Must differ from departure country")]
        public string DestinationCountry { get; set; } = string.Empty;

        [Display(Name = "Departure Airport")]
        [Required]
        [Description("Must differ from arrival airport")]
        public string DepartureAirport { get; set; } = string.Empty;

        [Display(Name = "Arrival Airport")]
        [Required]
        [Description("Must differ from departure airport")]
        public string ArrivalAirport { get; set; } = string.Empty;

        [Display(Name = "Economy Price")]
        [Description("Greater than zero and lower than Business Price")]
        public decimal EconomyPrice { get; set; }

        [Display(Name = "Business Price")]
        [Description("Greater than Economy Price and lower than First Class Price")]
        public decimal BusinessPrice { get; set; }

        [Display(Name = "First Class Price")]
        [Description("Greater than zero and higher than Business Price")]
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

        [Display(Name = "Departure Date")]
        [Description("Required, today or later")]
        public DateTime DepartureDate { get; set; }
    }

    
