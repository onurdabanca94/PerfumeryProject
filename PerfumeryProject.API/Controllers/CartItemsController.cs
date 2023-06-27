using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PerfumeryProject.API.DTOs.CartItems;
using PerfumeryProject.API.DTOs.Parfume;
using PerfumeryProject.API.Extensions;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Business.Concrete;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly ICartItemService _service;
        private readonly ILogger<CartItemsController> _logger;
        private readonly IUserService _userService;

        public CartItemsController(IMapper mapper, ICartItemService service, ILogger<CartItemsController> logger, IUserService userService)
        {
            _mapper = mapper;
            _service = service;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("get-items")]
        public async Task<IActionResult> GetItems([FromBody] GetItemsByUserIdAndCartNoDto input)
        {
            try
            {
                var data = _service.GetListByExpressionAsync(x => x.UserId == input.UserId && x.IsOrdered == false);
                _logger.LogInformation("Sepet bilgisi alınmaya çalışıldı.");
                if (data.HasValue())
                {
                    _logger.LogInformation("Sepet listesi alındı.");
                    return Ok(new { IsSuccess = true, data = data });
                }
                _logger.LogInformation("Sepet bulunamadı ya da ödemesi alındı.");
                return NotFound(new { IsSuccess = false, message = "Sepet bulunamadı ya da ödemesi alındı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("save-or-update-cart-item")]
        public async Task<IActionResult> SaveOrUpdateCartItem([FromBody] SaveOrUpdateCartItemDto data)
        {
            try
            {
                var itemData = _service.GetListByExpressionAsync(x=> x.UserId == data.UserId && x.IsOrdered == false && x.BrandName == data.BrandName && x.Name == data.Name).FirstOrDefault();
                if (itemData == null) //Save at
                {
                    try
                    {
                        var user = await _userService.GetByIdAsync(data.UserId).ConfigureAwait(false);
                        if (user == null)
                        {
                            _logger.LogInformation("Kullanıcı bulunamadı.");
                            return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                        }
                        var orderedCartList = _service.GetListByExpressionAsync(x => x.UserId == data.UserId && x.IsOrdered == true).ToList();
                        if (!orderedCartList.HasValue())
                        {
                            data.CartNumber = 1;
                        }
                        else
                        {
                            data.CartNumber = orderedCartList.LastOrDefault()!.CartNumber + 1;
                        }
                        var mappedData = _mapper.Map<CartItem>(data);
                        mappedData.CreatedDate = DateTime.Now;
                        await _service.AddAsync(mappedData).ConfigureAwait(false);
                        string logString = $"Sepete {data.UserId} id'li kişi tarafından {data.BrandName} markasından {data.Name} isimli parfüm {data.Quantity} adet, {DateTime.Now} tarihinde, {data.Price * data.Quantity} fiyatına eklendi.";
                        _logger.LogInformation(logString);
                        _logger.LogInformation("Sepete ürün kaydedildi.");
                        return Ok(new { IsSuccess = true });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return BadRequest(new { IsSuccess = false, message = ex.Message });
                    }
                }
                else //Update at
                {
                    try
                    {
                        itemData.Quantity += data.Quantity;
                        itemData.UpdatedDate = DateTime.Now;
                        await _service.UpdateAsync(itemData).ConfigureAwait(false);
                        string logString = $"Sepete {itemData.UserId} id'li kişi tarafından {itemData.BrandName} markasından {itemData.Name} isimli parfüm {data.Quantity} adet, {DateTime.Now} tarihinde, {data.Quantity * data.Price} fiyatına güncellendi.";
                        _logger.LogInformation(logString);
                        _logger.LogInformation("Sepette ürün güncellendi.");
                        return Ok(new { IsSuccess = true });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        return BadRequest(new { IsSuccess = false, message = ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("create-cart-item")]
        public async Task<IActionResult> CreateCartItem([FromBody] CreateCartItemsDto data)
        {
            try
            {
                var user = await _userService.GetByIdAsync(data.UserId).ConfigureAwait(false);
                if(user == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }
                var orderedCartList = _service.GetListByExpressionAsync(x => x.UserId == data.UserId && x.IsOrdered == true).ToList();
                if(!orderedCartList.HasValue())
                {
                    data.CartNumber = 1;
                }
                else
                {
                    data.CartNumber = orderedCartList.LastOrDefault()!.CartNumber + 1;
                }
                var mappedData = _mapper.Map<CartItem>(data);
                mappedData.CreatedDate = DateTime.Now;
                await _service.AddAsync(mappedData).ConfigureAwait(false);
                string logString = $"Sepete {data.UserId} id'li kişi tarafından {data.BrandName} markasından {data.Name} isimli parfüm {data.Quantity} adet, {DateTime.Now} tarihinde, {data.Price * data.Quantity} fiyatına eklendi.";
                _logger.LogInformation(logString);
                _logger.LogInformation("Sepete ürün kaydedildi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("update-cart-item")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemsDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Ürün bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Ürün bulunamadı." });
                }
                foundData.Quantity = data.Quantity;
                foundData.UpdatedDate = DateTime.Now;
                await _service.UpdateAsync(foundData).ConfigureAwait(false);
                string logString = $"Sepete {foundData.UserId} id'li kişi tarafından {foundData.BrandName} markasından {foundData.Name} isimli parfüm {data.Quantity} adet, {DateTime.Now} tarihinde, {data.Quantity * data.Price} fiyatına güncellendi.";
                _logger.LogInformation(logString);
                _logger.LogInformation("Sepette ürün güncellendi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("delete-cart-item")]
        public async Task<IActionResult> DeleteCartItem([FromBody] DeleteCartItemsDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Ürün bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Ürün bulunamadı." });
                }
                await _service.DeleteAsync(foundData).ConfigureAwait(false);
                string logString = $"Sepete {data.Id} id'li ürün {foundData.UserId} id'li kişi tarafından {DateTime.Now} tarihinde silindi.";
                _logger.LogInformation(logString);
                _logger.LogInformation("Sepetten ürün silindi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }


    }
}
