using AirportTicketBookingSystem.Models;
namespace AirportTicketBookingSystem.Util
{
    public class Reader
    {

        public static List<Flight>ReadCsv()
        {
            string filePath = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory, "..", "..", "..", "Flight.csv"));
            List<Flight> flight = File.ReadLines(filePath)
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var values = line.Split(',');
                    return new Flight
                    {
                        Id = values[0],
                        DepartureCountry = values[1],
                        DestinationCountry = values[2],
                        DepartureAirport = values[3],
                        ArrivalAirport = values[4],
                        EconomyPrice = decimal.Parse(values[5]),
                        BusinessPrice = decimal.Parse(values[6]),
                        FirstClassPrice = decimal.Parse(values[7]),
                        DepartureDate = DateTime.Parse(values[8])
                    };
                })
                .ToList();
            return flight;
        }
    }
}
