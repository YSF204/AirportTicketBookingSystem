using System;
using System.Collections.Generic;
using System.Text;

namespace AirportTicketBookingSystem.DataAccess.Entities;
    public class Passenger
    {
      public string Id { get; set; } = string.Empty;
      public string FullName { get; set; } = string.Empty;
        
      public string Email { get; set; } = string.Empty;
    }

