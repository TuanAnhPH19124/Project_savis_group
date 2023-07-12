using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.DataTransfer;
using GlobalApi.IRepositories;
using GlobalApi.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlobalApi.Controllers
{
  [Route("api/[controller]")]
  public class TransactionController : Controller
  {
    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionDetailRepository _transactionDetailRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;

    public TransactionController(ILogger<TransactionController> logger, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IRoomRepository roomRepository, IBookingRepository bookingRepository)
    {
      _logger = logger;
      _transactionRepository = transactionRepository;
      _transactionDetailRepository = transactionDetailRepository;
      _roomRepository = roomRepository;
      _bookingRepository = bookingRepository;
    }


    [HttpPost("order")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult OrderTransaction([FromBody] TransactionPostRequestDto _transactionDto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      #region transaction
      var newTransaction = _transactionDto.Adapt<Transaction>();
      Console.WriteLine(newTransaction.ToString());
      var transactionId = _transactionRepository.Add(newTransaction);
      #endregion

      #region transaction detail
      var transactiondetailList = _transactionDto.TransactionDetails.Adapt<List<TransactionDetail>>();
      _transactionDetailRepository.AddRange(transactiondetailList, transactionId);
      #endregion

      #region update unit of room
      var roomDictionary = new Dictionary<string, int>();
      foreach (var item in transactiondetailList)
      {
        roomDictionary.Add(item.RoomId, item.Amount);
      }
      _roomRepository.UpdateAvalibleUnit(roomDictionary);
      #endregion

      #region clear booking
      _bookingRepository.RemoveRange(_transactionDto.TransactionDetails);
      #endregion
      return Ok();
    }
  }
}