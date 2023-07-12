using Dapper;
using GlobalApi.DataTransfer;
using GlobalApi.IRepositories;
using GlobalApi.Models;
using GlobalApi.Services;
using GlobalApi.Ultilities;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GlobalApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RoomsController : ControllerBase
  {
    private readonly ILogger<RoomsController> _logger;
    private readonly ICacheService _cacheService;
    private readonly IRoomRepository _productRepository;

    public RoomsController(ILogger<RoomsController> logger, ICacheService cacheService, IRoomRepository productRepository)
    {
      _logger = logger;
      _cacheService = cacheService;
      _productRepository = productRepository;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string keyword)
    {
      var data = _productRepository.SelectbyKeyword(keyword);
      return await Task.FromResult(Ok(data));
    }

    [HttpGet("getall")]
    public IActionResult GetAll()
    {
      var cacheData = _cacheService.GetData<IEnumerable<Room>>("products_home");

      if (cacheData != null && cacheData.Count() > 0)
      {
        return Ok(cacheData);
      }

      cacheData = _productRepository.SelectAll();
      //set expiry time
      var expiryTime = DateTimeOffset.Now.AddSeconds(15);
      _cacheService.SetData<IEnumerable<Room>>("products_home", cacheData, expiryTime);
      return Ok(cacheData);
    }

    [HttpGet("get/{id}")]
    public IActionResult GetById(string id)
    {
      var cacheData = _cacheService.GetData<Room>($"products:{id}");
      if (cacheData != null)
      {
        return Ok(cacheData);
      }

      cacheData = _productRepository.SelectById(id);
      var expiryTime = DateTimeOffset.Now.AddSeconds(60);
      _cacheService.SetData<Room>($"products:{id}", cacheData, expiryTime);
      return Ok(cacheData);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [HttpPost("insert")]
    public IActionResult Insert([FromForm] InsertProductRequestDto productDto)
    {
      var newProduct = productDto.Adapt<Room>();

      if (productDto.Image != null)
      {
        UploadService.UploadImages(productDto.Image, newProduct.Id);
        // newProduct.Photo = GetImages(newProduct.Id).FirstOrDefault();
      }
      var addedObj = _productRepository.Insert(newProduct);

      var expiryTime = DateTimeOffset.Now.AddSeconds(60);
      _cacheService.SetData<Room>($"products{newProduct.Id}", newProduct, expiryTime);
      return Ok(newProduct);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [HttpDelete("delete/{id}")]
    public IActionResult Delete(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        _productRepository.Remove(id);
        _cacheService.RemoveData("products");

        return Ok(new { message = "Xóa thành công" });
      }
      return NotFound();
    }

    [HttpPut("update/{id}")]
    public IActionResult Update(string id, [FromForm] UpdateProductRequestDto product)
    {
      if (ModelState.IsValid)
      {
        try
        {
          var jsConvertData = System.Text.Json.JsonSerializer.Deserialize<Room>(product.Housing, new JsonSerializerOptions
          {
            PropertyNameCaseInsensitive = true
          });
          if (product.Images != null)
          {
            RemoveImage(id);
            UploadService.UploadImages(product.Images, id);
            // jsConvertData.Photo = GetImages(id).FirstOrDefault();
          }
          _productRepository.Update(jsConvertData!);

          var cacheData = _productRepository.SelectAll();
          var expiryTime = DateTimeOffset.Now.AddSeconds(60);
          _cacheService.SetData<IEnumerable<Room>>("products", cacheData, expiryTime);

          return Ok(new { message = "Cập nhật thành công" });
        }
        catch (Exception)
        {

          throw;
        }
      }
      return BadRequest(product);

    }

    [HttpPost("filter")]
    public IActionResult Filter([FromBody] DynamicParamsDto param)
    {
      var data = _productRepository.SelectByWhereCondition(param);
      return Ok(data);
    }

    [NonAction]
    private IEnumerable<string> GetImages(string Id)
    {
      var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", Id);
      if (Directory.Exists(uploadFolder))
      {
        var imageFiles = Directory.GetFiles(uploadFolder);
        var imageUrls = imageFiles.Select(file =>
        {
          var imageName = Path.GetFileName(file);
          return Url.Link("GetImage", new { id = Id, imageName = imageName });
        });
        return imageUrls;
      }
      return new List<string>() { "https://loremflickr.com/640/480/business" };
    }

    [NonAction]
    private void RemoveImage(string id)
    {
      var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", id);
      if (Directory.Exists(uploadFolder))
      {
        Directory.Delete(uploadFolder, true);
      }
    }
  }
}
