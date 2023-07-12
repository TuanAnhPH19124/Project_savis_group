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
  [Route("[controller]")]
  public class BookingsController : Controller
  {
    private readonly ILogger<BookingsController> _logger;
    private readonly IBookingRepository _repository;

    public BookingsController(ILogger<BookingsController> logger, IBookingRepository repository)
    {
      _logger = logger;
      _repository = repository;
    }

    [HttpGet("getBookingDetail/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetAllBookingDetail(string id){
      var bookingDetailList = _repository.GetBookingDetails(id);
      Console.Write(bookingDetailList);
      return Ok(bookingDetailList);
    }

    [HttpPost("add")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult AddToBooking([FromBody]BookingPostRequestDto booking)
    {
      if (!ModelState.IsValid){
        return BadRequest();
      }
      var newBooking = booking.Adapt<Booking>();
        _repository.Insert(newBooking);
        return Ok();
    }

    [HttpGet("updateAmount/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult UpdateBookingAmount(string id,string amount){
      _repository.Update(id, int.Parse(amount));
      return Ok();
    }

    [HttpGet("gettotalitem/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetTotalItemInBooking(string id){
      return Ok(_repository.GetTotalItemInBooking(id));
    }

    [HttpDelete("remove/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult RemoveBooking(string id){
      _repository.Remove(id);
      return NoContent();
    }


  }
}