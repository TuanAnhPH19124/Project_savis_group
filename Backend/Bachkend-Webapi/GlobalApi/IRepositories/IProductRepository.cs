using Dapper;
using GlobalApi.DataTransfer;
using GlobalApi.Models;

namespace GlobalApi.IRepositories;

public interface IRoomRepository
{
    string Insert(Room product);
    List<Room> SelectAll();
    List<Room> SelectByWhereCondition(DynamicParamsDto paramsDto);
    List<Room> SelectbyKeyword(string kedword);
    Room SelectById(string Id);
    bool Update(Room product);
    void Remove(string Id);
    void UpdateAvalibleUnit(Dictionary<string, int> roomDictionary);
}