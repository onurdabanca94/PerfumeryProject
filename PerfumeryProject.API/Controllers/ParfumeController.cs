using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PerfumeryProject.API.DTOs.Parfume;
using PerfumeryProject.API.DTOs.User;
using PerfumeryProject.API.Extensions;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParfumeController : ODataController
    {

        private readonly IMapper _mapper;
        private readonly IParfumService _service;
        private readonly IBrandService _brandService;
        private readonly ILogger<ParfumeController> _logger;
        public ParfumeController(IMapper mapper, IParfumService service, IBrandService brandService, ILogger<ParfumeController> logger)
        {
            _mapper = mapper;
            _service = service;
            _brandService = brandService;
            _logger = logger;
        }

        [HttpPost("get-all-parfumes")]
        [EnableQuery]
        public async Task<IActionResult> GetAllParfumes()
        {
            try
            {
                var list = await _service.GetAllAsync().ConfigureAwait(false);
                _logger.LogInformation("Tüm parfümlerin listesi alınmaya çalışıldı.");
                if (list.HasValue())
                {
                    _logger.LogInformation("Tüm parfümlerin listesi alındı.");
                    return Ok(new { IsSuccess = true, data = list.ToList() });
                }
                _logger.LogInformation("Parfüm bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Parfüm bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-parfume")]
        public async Task<IActionResult> GetParfumeById([FromBody] GetParfumeByIdDto input)
        {
            try
            {
                var data = await _service.GetByIdAsync(input.Id).ConfigureAwait(false);
                _logger.LogInformation("Parfüm bilgisi id ile alınmaya çalışıldı.");
                if (data != null)
                {
                    _logger.LogInformation("Parfüm bilgisi alındı.");
                    return Ok(new { IsSuccess = true, data = data });
                }
                _logger.LogInformation("Parfüm bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Parfüm bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-parfumes-by-brand-id")]
        public async Task<IActionResult> GetParfumesByBrandId([FromBody] GetParfumesByBrandIdDto input)
        {
            try
            {
                var data = _service.GetListByExpressionAsync(x => x.BrandId == input.BrandId);
                _logger.LogInformation("Parfüm bilgileri marka idsi ile alınmaya çalışıldı.");
                if (data.HasValue())
                {
                    _logger.LogInformation("Parfüm listesi alındı.");
                    return Ok(new { IsSuccess = true, data = data });
                }
                _logger.LogInformation("Parfüm bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Parfüm bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }


        [HttpPost("create-parfume")]
        public async Task<IActionResult> CreateParfume([FromBody] CreateParfumeDto data)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(data.BrandId).ConfigureAwait(false);
                if(brand == null)
                {
                    _logger.LogInformation("Marka bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
                }
                var mappedData = _mapper.Map<Parfum>(data);
                await _service.AddAsync(mappedData).ConfigureAwait(false);
                _logger.LogInformation("Parfüm kaydedildi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("update-parfume")]
        public async Task<IActionResult> UpdateParfume([FromBody] UpdateParfumeDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Parfüm bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Parfüm bulunamadı." });
                }
                foundData.Name = data.Name;
                foundData.Price = data.Price;
                await _service.UpdateAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Parfüm güncellendi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("delete-parfume")]
        public async Task<IActionResult> DeleteParfume([FromBody] DeleteParfumeDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Parfüm bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Parfüm bulunamadı." });
                }
                await _service.DeleteAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Parfüm silindi.");
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
