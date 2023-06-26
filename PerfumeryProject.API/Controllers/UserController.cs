using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PerfumeryProject.API.DTOs.User;
using PerfumeryProject.API.Extensions;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;
        private readonly ILogger<UserController> _logger;

        public UserController(IMapper mapper, IUserService service, ILogger<UserController> logger)
        {
            _mapper = mapper;
            _service = service;
            _logger = logger;
        }

        [HttpPost("get-all-users")]
        [EnableQuery]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var list = await _service.GetAllAsync().ConfigureAwait(false);
                _logger.LogInformation("Tüm kullanıcı listesi alınmaya çalışıldı.");
                if (list.HasValue())
                {
                    _logger.LogInformation("Tüm kullanıcı listesi alındı.");
                    return Ok(new { IsSuccess = true, data = list.ToList() });
                }
                _logger.LogInformation("Kullanıcı bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-user")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdDto input)
        {
            try
            {
                var data = await _service.GetByIdAsync(input.Id).ConfigureAwait(false);
                _logger.LogInformation("Kullanıcı bilgisi id ile alınmaya çalışıldı.");
                if (data != null)
                {
                    _logger.LogInformation("Kullanıcı bilgisi alındı.");
                    return Ok(new { IsSuccess = true, data = data });
                }
                _logger.LogInformation("Kullanıcı bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto data)
        {
            try
            {
                var mappedData = _mapper.Map<User>(data);
                await _service.AddAsync(mappedData).ConfigureAwait(false);
                _logger.LogInformation("Kullanıcı kaydedildi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }
                foundData.Name = data.Name;
                foundData.Surname = data.Surname;
                foundData.Email = data.Email;
                foundData.Username = data.Username;
                await _service.UpdateAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Kullanıcı güncellendi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Kullanıcı bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Kullanıcı bulunamadı." });
                }
                await _service.DeleteAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Kullanıcı silindi.");
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
