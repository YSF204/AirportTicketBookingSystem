using AirportTicketBookingSystem.Models;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
namespace AirportTicketBookingSystem.Util
{
    public class Reader
    {

        public static List<Flight?> ReadCsv(string? path, out List<string> errors)
        {
            string filePath = string.IsNullOrWhiteSpace(path)
                ? Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Flight.csv"))
                : Path.GetFullPath(path);
            errors = new List<string>();
            var flights = new List<Flight?>();

            using var parser = new TextFieldParser(filePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;
            parser.ReadLine();

            while (!parser.EndOfData)
            {
                long row = parser.LineNumber + 1;
                try
                {
                    string[] values = parser.ReadFields() ?? throw new FormatException("No columns were found.");
                    if (values.Length != 9)
                        throw new FormatException($"Expected 9 columns, found {values.Length}.");

                    flights.Add(new Flight
                    {
                        Id = values[0].Trim(),
                        DepartureCountry = values[1].Trim(),
                        DestinationCountry = values[2].Trim(),
                        DepartureAirport = values[3].Trim(),
                        ArrivalAirport = values[4].Trim(),
                        EconomyPrice = decimal.Parse(values[5], CultureInfo.InvariantCulture),
                        BusinessPrice = decimal.Parse(values[6], CultureInfo.InvariantCulture),
                        FirstClassPrice = decimal.Parse(values[7], CultureInfo.InvariantCulture),
                        DepartureDate = DateTime.Parse(values[8], CultureInfo.InvariantCulture)
                    });
                }
                catch (Exception error) when (
                    error is FormatException or IndexOutOfRangeException or MalformedLineException)
                {
                    string message = error is IndexOutOfRangeException
                        ? "Missing one or more columns."
                        : error.Message;
                    errors.Add($"Row {row}: {message}");
                    flights.Add(null);
                }
            }

            return flights;
        }
    }
}
