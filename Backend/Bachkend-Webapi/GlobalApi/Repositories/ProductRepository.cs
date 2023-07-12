using Dapper;
using GlobalApi.DataTransfer;
using GlobalApi.IRepositories;
using GlobalApi.Models;
using Npgsql;
using System.Data;

namespace GlobalApi.Repositories;

internal sealed class RoomRepository : IRoomRepository
{
  private readonly IDbConnection _connection;

  public RoomRepository(IConfiguration configuration)
  {
    this._connection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
  }
  public string Insert(Room product)
  {
    var sql = "INSERT INTO products (\"Name\", \"Photo\", \"State\", \"AvailableUnits\", \"Id\", \"City\", \"Laundry\", \"Wifi\") VALUES(@Name, @Photo, @State, @AvailableUnits, @Id, @City, @Laundry, @Wifi) RETURNING \"Id\";";
    _connection.Open();
    using (var transaction = _connection.BeginTransaction())
    {
      try
      {
        var id = _connection.QuerySingle<string>(sql, product, transaction);
        transaction.Commit();
        _connection.Close();
        return id;
      }
      catch (System.Exception)
      {
        transaction.Rollback();
        throw;
      }

    }

  }
  public void Remove(string Id)
  {
    var sql = "DELETE FROM products WHERE \"Id\" = @Id";
    try
    {
      _connection.Execute(sql, new { @Id = Id });


    }
    catch (System.Exception)
    {

      throw;
    }


  }
  public List<Room> SelectAll()
  {
    var sql = "select r.* from \"Rooms\" r";
    return _connection.Query<Room>(sql).ToList();
  }
  public Room SelectById(string Id)
  {
    var sql = "select * from \"Rooms\" p where p.\"Id\" = @id;";
    return _connection.Query<Room>(sql, new
    {
      @id = Id
    }).Single();
  }
  public List<Room> SelectbyKeyword(string keyword)
  {
    var sql = $"Select * from \"Rooms\" p where p.\"Name\" like '%{keyword}%'";
    return _connection.Query<Room>(sql).ToList();
  }
  public List<Room> SelectByWhereCondition(DynamicParamsDto paramsDto)
  {
    #region Create sql query
      var sql = "select r.* from \"Rooms\" r Where 1 = 1";
      var parameters = new DynamicParameters();
      if (paramsDto.Wifi == true)
      {
        sql += " AND r.\"Wifi\" = @Wifi";
        parameters.Add("@Wifi", paramsDto.Wifi.Value);
      }
      if (paramsDto.Laundry == true)
      {
        sql += " AND r.\"Laundry\" = @Laundry";
        parameters.Add("@Laundry", paramsDto.Laundry.Value);
      }
      if (!string.IsNullOrEmpty(paramsDto.City))
      {
        sql += " AND r.\"City\" = @City";
        parameters.Add("@City", paramsDto.City);
      }
      if (!string.IsNullOrEmpty(paramsDto.Ward))
      {
        sql += " AND r.\"Ward\" = @Ward";
        parameters.Add("@Ward", paramsDto.Ward);
      }
      if (!string.IsNullOrEmpty(paramsDto.District))
      {
        sql += " AND r.\"District\" = @District";
        parameters.Add("@District", paramsDto.District);
      }
      if (paramsDto.LowPrice > 0 && paramsDto.HighPrice > 0){
        sql += " AND r.\"Price\" BETWEEN @MinPrice AND @MaxPrice";
        parameters.Add("@MinPrice", paramsDto.LowPrice);
        parameters.Add("@MaxPrice", paramsDto.HighPrice);
      }
      #endregion
    return _connection.Query<Room>(sql, param: parameters).ToList();
  }
  public bool Update(Room product)
  {
    var sql = "update \"products\"  \r\nset \"Name\" = @Name,\r\n\"Photo\" = @Photo,\r\n\"State\" = @State,\r\n\"AvailableUnits\" = @AvailableUnits,\r\n\"City\" = @City,\r\n\"Laundry\" = @Laundry,\r\n\"Wifi\" = @Wifi \r\nwhere \"Id\" = @Id;";
    _connection.Open();
    using (var transaction = _connection.BeginTransaction())
    {
      try
      {
        var rowEffected = _connection.Execute(sql, product, transaction);
        transaction.Commit();
        _connection.Close();
        return true;
      }
      catch (System.Exception)
      {
        transaction.Rollback();
        throw;
      }
    }
  }

  public void UpdateAvalibleUnit(Dictionary<string, int> roomDictionary)
  {
    var sql = "update \"Rooms\" set \"AvailableUnits\" = @newUnit where \"Id\" = @Id";
    _connection.Open();
    using (var transaction = _connection.BeginTransaction())
    {
      try
      {
        foreach (var item in roomDictionary)
        {
          var currentUnit = GetAvalibleUnit(item.Key);
          _connection.Execute(sql, new { 
            @newUnit = currentUnit - item.Value,
            @Id = item.Key}, transaction);
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

  private int GetAvalibleUnit(string roomId){
    var sql = "select r.\"AvailableUnits\" from \"Rooms\" r where r.\"Id\" = @Id";
    return _connection.QuerySingle<int>(sql, new { @Id = roomId});
  }
}