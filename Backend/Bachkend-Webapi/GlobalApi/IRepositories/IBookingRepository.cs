using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.DataTransfer;
using GlobalApi.Models;

namespace GlobalApi.IRepositories
{
    public interface IBookingRepository
    {
        void Insert(Booking booking);
        
        void Update(string id, int Amount);
        int GetTotalItemInBooking(string cusomterId);
        IEnumerable<BookingGetRequestDto> GetBookingDetails(string id);
        void UpdateAmount(string id);
        Booking GetById(string id);

        void Remove(string id);
        void RemoveRange(List<TransactionDetailPostRequestDto> bookings );

    }
}