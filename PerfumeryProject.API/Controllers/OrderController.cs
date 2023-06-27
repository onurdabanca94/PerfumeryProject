using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using PerfumeryProject.API.DTOs.Order;
using PerfumeryProject.API.Extensions;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _service;
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<OrderController> _logger;
        private readonly IUserService _userService;

        public OrderController(IMapper mapper, IOrderService service, ICartItemService cartItemService, ILogger<OrderController> logger, IUserService userService)
        {
            _mapper = mapper;
            _service = service;
            _cartItemService = cartItemService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto data)
        {
            try
            {
                var order = new Order() { OrderDate = DateTime.Now };
                await _service.AddAsync(order).ConfigureAwait(false);
                _logger.LogInformation("Sipariş kaydedildi.");

                var user = await _userService.GetByIdAsync(data.UserId).ConfigureAwait(false);
                if (user == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }

                var cartItems = _cartItemService.GetListByExpressionAsync(x => x.UserId == data.UserId && x.IsOrdered == false);
                if(!cartItems.HasValue())
                {
                    _logger.LogInformation("Sepet bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sepet bulunamadı." });
                }
                var lastOrder = (await _service.GetAllAsync().ConfigureAwait(false)).LastOrDefault();
                if (lastOrder == null)
                {
                    _logger.LogInformation("Sipariş bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sipariş bulunamadı." });
                }

                foreach (var item in cartItems)
                {
                    item.OrderId = lastOrder!.Id;
                    item.UpdatedDate = DateTime.Now;
                    item.IsOrdered = true;
                    await _cartItemService.UpdateAsync(item).ConfigureAwait(false);
                }

                _logger.LogInformation($"{lastOrder!.Id} sipariş id'li sepetin siparişi tamamlandı.");

                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }


        }

        [HttpPost("get-last-order")]
        public async Task<IActionResult> GetLastOrder()
        {
            try
            {
                var lastOrder = (await _service.GetAllAsync().ConfigureAwait(false)).LastOrDefault();
                if(lastOrder == null)
                {
                    _logger.LogInformation("Sipariş bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sipariş bulunamadı." });
                }
                var cartItems = _cartItemService.GetListByExpressionAsync(x => x.OrderId == lastOrder.Id);
                if (!cartItems.HasValue())
                {
                    _logger.LogInformation("Sepet bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sepet bulunamadı." });
                }
                var userId = cartItems.FirstOrDefault()!.UserId;
                var userInfo = await _userService.GetByIdAsync(userId).ConfigureAwait(false);
                if (userInfo == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }

                var response = (from ci in cartItems.ToList()
                                join o in await _service.GetAllAsync().ConfigureAwait(false)
                                on ci.OrderId equals o.Id
                                join u in await _userService.GetAllAsync().ConfigureAwait(false)
                                on ci.UserId equals u.Id
                                select new GetOrderCartListDto()
                                {
                                    UserId = ci.UserId,
                                    TotalPrice = ci.TotalPrice,
                                    BrandName = ci.BrandName,
                                    Fullname = u.Name + " " + u.Surname,
                                    OrderDate = o.OrderDate,
                                    OrderId = ci.OrderId,
                                    PerfumeName = ci.Name,
                                    Quantity = ci.Quantity,
                                    Price = ci.Price
                                }).Where(x => x.UserId == userId && x.OrderId == lastOrder.Id!).ToList();

                _logger.LogInformation("Sipariş özeti listelendi.");
                return Ok(new { IsSuccess = true, data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-selected-order")]
        public async Task<IActionResult> GetSelectedOrder([FromBody] GetSelectedOrderDto data)
        {
            try
            {
                var cartItems = _cartItemService.GetListByExpressionAsync(x => x.OrderId == data.OrderId);
                if (!cartItems.HasValue())
                {
                    _logger.LogInformation("Sepet bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sepet bulunamadı." });
                }
                var userId = cartItems.FirstOrDefault()!.UserId;
                var userInfo = await _userService.GetByIdAsync(userId).ConfigureAwait(false);
                if (userInfo == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }

                var response = (from ci in cartItems.ToList()
                                join o in await _service.GetAllAsync().ConfigureAwait(false)
                                on ci.OrderId equals o.Id
                                join u in await _userService.GetAllAsync().ConfigureAwait(false)
                                on ci.UserId equals u.Id
                                select new GetOrderCartListDto()
                                {
                                    UserId = ci.UserId,
                                    TotalPrice = ci.TotalPrice,
                                    BrandName = ci.BrandName,
                                    Fullname = u.Name + " " + u.Surname,
                                    OrderDate = o.OrderDate,
                                    OrderId = ci.OrderId,
                                    PerfumeName = ci.Name,
                                    Quantity = ci.Quantity,
                                    Price = ci.Price
                                }).Where(x => x.UserId == userId && x.OrderId == data.OrderId).ToList();

                _logger.LogInformation("Sipariş özeti listelendi.");
                return Ok(new { IsSuccess = true, data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-order-history")]
        public async Task<IActionResult> GetOrderHistory()
        {
            try
            {
                var orderList = (await _service.GetAllAsync().ConfigureAwait(false)).ToList();
                if (!orderList.HasValue())
                {
                    _logger.LogInformation("Sipariş geçmişi bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Sipariş geçmişi bulunamadı." });
                }

                var response = new List<GetOrderHistoryListDto>();

                for (int i = 0; i < orderList.Count(); i++)
                {
                    var data = new GetOrderHistoryListDto()
                    {
                        OrderId = orderList[i].Id,
                        OrderName = $"{i + 1}. sipariş"
                    };
                    response.Add(data);
                }

                _logger.LogInformation("Kullanıcı siparişleri listelendi.");
                return Ok(new { IsSuccess = true, data = response });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

    }
}
