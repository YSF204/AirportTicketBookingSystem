
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Validators
{
    public class FlightValidator
    {
        public List<string> GetValidationDetails()
        {
            var details = new List<string>();
            foreach (PropertyInfo property in typeof(Flight).GetProperties())
            {
                string fieldName = property.GetCustomAttribute<DisplayAttribute>()?.Name ?? property.Name;
                string type = GetFieldType(property.PropertyType);
                string constraint = string.Join(", ", GetConstraints(property));

                details.Add($"{fieldName}:\n  Type: {type}\n  Constraint: {constraint}");
            }

            return details;
        }

        private static string GetFieldType(Type propertyType)
        {
            if (propertyType == typeof(string)) return "Free Text";
            if (propertyType == typeof(decimal)) return "Decimal";
            if (propertyType == typeof(DateTime)) return "Date Time";
            return propertyType.Name;
        }

        private static List<string> GetConstraints(PropertyInfo property)
        {
            var constraints = new List<string>();
            if (property.IsDefined(typeof(RequiredAttribute)))
            {
                constraints.Add("Required");
            }

            string? description = property.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                constraints.Add(description);
            }

            return constraints;
        }

        public List<string> ValidateFlight(
            Flight flight,
            IEnumerable<Flight>? existingFlights = null)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(flight.Id))
            {
                errors.Add("Flight ID is required.");
            }
            else if (existingFlights != null &&
                existingFlights.Any(f =>
                    string.Equals(
                        f.Id,
                        flight.Id,
                        StringComparison.OrdinalIgnoreCase)))
            {
                errors.Add($"Flight ID '{flight.Id}' already exists.");
            }

            if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
            {
                errors.Add("Departure country is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.DestinationCountry))
            {
                errors.Add("Destination country is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.DepartureAirport))
            {
                errors.Add("Departure airport is required.");
            }

            if (string.IsNullOrWhiteSpace(flight.ArrivalAirport))
            {
                errors.Add("Arrival airport is required.");
            }

            if (flight.EconomyPrice <= 0)
            {
                errors.Add("Economy price must be greater than zero.");
            }
            if (flight.BusinessPrice <= 0)
            {
                errors.Add("Business price must be greater than zero.");
            }
            if (flight.FirstClassPrice <= 0)
            {
                errors.Add("First Class price must be greater than zero.");
            }
            if (flight.EconomyPrice >= flight.BusinessPrice || flight.BusinessPrice >= flight.FirstClassPrice)
            {
                errors.Add("Prices must increase from Economy to Business to First Class.");
            }

            if (flight.DepartureDate == default || flight.DepartureDate.Date < DateTime.Today)
            {
                errors.Add("Departure date is required and cannot be before today.");
            }

            if (string.Equals(
                flight.DepartureAirport,
                flight.ArrivalAirport,
                StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Departure and arrival airports cannot be the same.");
            }

            if (string.Equals(
                flight.DepartureCountry,
                flight.DestinationCountry,
                StringComparison.OrdinalIgnoreCase))
            {
                errors.Add("Departure and destination countries cannot be the same.");
            }

            return errors;
        }
    }
}
