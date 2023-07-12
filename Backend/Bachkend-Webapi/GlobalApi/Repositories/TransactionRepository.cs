using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GlobalApi.IRepositories;
using GlobalApi.Models;
using Npgsql;

namespace GlobalApi.Repositories
{
  public class TransactionRepository : ITransactionRepository
  {
    private readonly IDbConnection _connection;

    public TransactionRepository(IConfiguration configuration)
    {
      this._connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
    }

    public string Add(Transaction transactions)
    {
      var sql = "insert into \"Transactions\" "+
      "(\"Id\", \"CreatedDate\", \"CheckInDate\", \"Total\", \"PayMethod\", \"Status\", \"CustomerId\")"+
      " values (@Id, @CreatedDate, @CheckInDate, @Total, @PayMethod, @Status, @CustomerId) RETURNING \"Id\"";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction()){
        try
        {
            var transactionId = _connection.QuerySingle<string>(sql, transactions, transaction);
            transaction.Commit();
            _connection.Close();
            return transactionId;
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