using AirportTicketBookingSystem.Models;

namespace AirportTicketBookingSystem.Repositories
{
    public class BookingRepository
    {
        private readonly JsonRepository<Booking> bookingRepository = new("bookings.json");

        public List<Booking> GetBookings()
        {
            return bookingRepository.GetItems();
        }

        public void SaveBookings(List<Booking> bookings)
        {
            bookingRepository.SaveItems(bookings);
        }
    }
}
