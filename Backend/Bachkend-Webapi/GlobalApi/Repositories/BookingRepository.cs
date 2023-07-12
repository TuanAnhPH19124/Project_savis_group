using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GlobalApi.DataTransfer;
using GlobalApi.IRepositories;
using GlobalApi.Models;
using Npgsql;

namespace GlobalApi.Repositories
{
  internal sealed class BookingRepository : IBookingRepository
  {
    private readonly IDbConnection _connection;

    public BookingRepository(IConfiguration configuration)
    {
      this._connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
    }

    public void Insert(Booking booking)
    {
      var sql = "insert into \"Bookings\" (\"CustomerId\", \"RoomId\", \"Amount\", \"Id\") "
      +"values (@CustomerId, @RoomId, @Amount, @Id);";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction())
      {
        try
        {
          var checkAmount = CheckBooking(booking.CustomerId, booking.RoomId);
          if (checkAmount > 0)
          {
            booking.Amount += checkAmount;
            var updateSql = "update \"Bookings\" set \"Amount\" = @Amount "+
            "where \"CustomerId\" = @CustomerId and \"RoomId\" = @RoomId";
            _connection.Execute(updateSql, new { 
              @Amount = booking.Amount, 
              @CustomerId = booking.CustomerId, 
              @RoomId = booking.RoomId });
          }
          else
          {
            _connection.Execute(sql, booking, transaction);
          }
          transaction.Commit();
          _connection.Close();
        }
        catch (System.Exception)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public void Update(string id, int Amount)
    {
      var sql = "update \"Bookings\" set \"Amount\" = @Amount where \"Id\" = @Id";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction())
      {
        try
        {
          _connection.Execute(sql, new { @Amount = Amount, @Id = id });
          transaction.Commit();
          _connection.Close();
        }
        catch (System.Exception)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public int CheckBooking(string customerId, string roomId)
    {
      var sql = "select b.\"Amount\"  from \"Bookings\" b where b.\"CustomerId\" = @CustomerId and b.\"RoomId\" = @RoomId";
      var amount = _connection.QuerySingleOrDefault<int>(sql, new
      {
        @CustomerId = customerId,
        @RoomId = roomId
      });
      Console.WriteLine(amount);
      return amount;
    }

    public int GetTotalItemInBooking(string cusomterId)
    {
      var sql = "select count(b.\"RoomId\") from \"Bookings\" b where \"CustomerId\" = @CustomerId";
      var count = _connection.QuerySingleOrDefault<int>(sql, new { @CustomerId = cusomterId });
      return count;
    }

    public IEnumerable<BookingGetRequestDto> GetBookingDetails(string id)
    {
      var sql = "select b.\"Id\", r.\"Price\", r.\"Name\" , b.\"Amount\" , concat(r.\"City\",',',r.\"District\",',',r.\"Ward\") Location, b.\"RoomId\" from \"Bookings\" b left join \"Rooms\" r on b.\"RoomId\" = r.\"Id\" where b.\"CustomerId\" = @CustomerId";
      return _connection.Query<BookingGetRequestDto>(sql, new {@CustomerId = id}).ToList();
    }

    public void UpdateAmount(string id)
    {
      var sql = "Update Rooms Set \"Amount\" = @Amount where \"Id\" = @Id";
      var booking = GetById(id);
      booking.Amount++;
      _connection.Open();
      using( var transaction = _connection.BeginTransaction()){
        try
        {
          _connection.Execute(sql, new { @Amount = booking.Amount, @Id = id}, transaction);
          transaction.Commit();
          _connection.Close();
        }
        catch (System.Exception)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public Booking GetById(string id)
    {
      var sql = "select * from \"Bookings\" where \"Id\" = @Id";
      var booking = _connection.Query<Booking>(sql, new { @Id = id}).Single();
      return booking;
    }

    public void Remove(string id)
    {
      var sql = "delete from \"Bookings\" where \"Id\" = @Id";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction())
      {
        try
        {
          _connection.Execute(sql, new {@Id = id}, transaction);
          transaction.Commit();
          _connection.Close();
        }
        catch (System.Exception)
        {
          transaction.Rollback();
          throw;
        }
      }
    }

    public void RemoveRange(List<TransactionDetailPostRequestDto> bookings )
    {
      var sql = "delete from \"Bookings\" where \"Id\" = @Id";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction())
      {
        try
        {
          foreach (var item in bookings)
          {
            _connection.Execute(sql, new {@Id = item.BookingId}, transaction);
          }
          transaction.Commit();
          _connection.Close();
        }
        catch (System.Exception)
        {
          transaction.Rollback();
          throw;
        }
      }
    }
  }
}