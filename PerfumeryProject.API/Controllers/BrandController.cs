using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PerfumeryProject.API.DTOs.Brand;
using PerfumeryProject.API.Extensions;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Data.Domain;

namespace PerfumeryProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ODataController
    {
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandService _service;
        private readonly IMapper _mapper;

        public BrandController(ILogger<BrandController> logger, IBrandService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("get-all-brands")]
        [EnableQuery]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var list = await _service.GetAllAsync().ConfigureAwait(false);
                _logger.LogInformation("Tüm marka listesi alınmaya çalışıldı.");
                if(list.HasValue())
                {
                    _logger.LogInformation("Tüm marka listesi alındı.");
                    return Ok(new { IsSuccess = true, data = list.ToList() });
                }
                _logger.LogInformation("Marka bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("get-brand-by-id")]
        public async Task<IActionResult> GetBrandById([FromBody] GetBrandByIdDto input)
        {
            try
            {
                var data = await _service.GetByIdAsync(input.Id).ConfigureAwait(false);
                _logger.LogInformation("Marka listesi id ile alınmaya çalışıldı.");
                if (data != null)
                {
                    _logger.LogInformation("Marka bilgisi alındı.");
                    return Ok(new { IsSuccess = true, data = data });
                }
                _logger.LogInformation("Marka bulunamadı.");
                return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("add-brand")]
        public async Task<IActionResult> AddBrand([FromBody] CreateBrandDto data)
        {
            try
            {
                var mappedData = _mapper.Map<Brand>(data);
                await _service.AddAsync(mappedData).ConfigureAwait(false);
                _logger.LogInformation("Marka kaydedildi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("update-brand")]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if(foundData == null)
                {
                    _logger.LogInformation("Marka bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
                }
                foundData.Name = data.Name;
                await _service.UpdateAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Marka güncellendi.");
                return Ok(new { IsSuccess = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { IsSuccess = false, message = ex.Message });
            }
        }

        [HttpPost("delete-brand")]
        public async Task<IActionResult> DeleteBrand([FromBody] DeleteBrandDto data)
        {
            try
            {
                var foundData = await _service.GetByIdAsync(data.Id).ConfigureAwait(false);
                if (foundData == null)
                {
                    _logger.LogInformation("Marka bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
                }
                await _service.DeleteAsync(foundData).ConfigureAwait(false);
                _logger.LogInformation("Marka silindi.");
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
