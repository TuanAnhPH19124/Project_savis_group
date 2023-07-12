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
  public class TransactionDetailRepository : ITransactionDetailRepository
  {
    private readonly IDbConnection _connection;

    public TransactionDetailRepository(IConfiguration configuration)
    {
      this._connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
    }

    public void AddRange(List<TransactionDetail> transactionDetails, string transactionId)
    {
      var sql = "insert into \"TransactionDetails\""+
      "(\"Id\", \"TransactionId\", \"RoomId\", \"Amount\", \"Price\")"+
      " values (@Id, @TransactionId, @RoomId, @Amount, @Price)";
      _connection.Open();
      using (var transaction = _connection.BeginTransaction())
      {
        try
        {
            #region add range
                foreach (var item in transactionDetails)
                {
                    item.TransactionId = transactionId;
                    _connection.Execute(sql, item, transaction);
                }
            #endregion
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