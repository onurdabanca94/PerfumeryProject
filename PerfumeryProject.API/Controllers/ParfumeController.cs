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
using System.Collections.Generic;

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

        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<GetParfumeWithBrandDto>> Get()
        {
            try
            {
                var list = await _service.GetAllAsync().ConfigureAwait(false);
                _logger.LogInformation("Tüm parfümlerin listesi alınmaya çalışıldı.");
                if (list.HasValue())
                {
                    _logger.LogInformation("Tüm parfümlerin listesi alındı.");
                    var response = (from p in list.ToList()
                                    join b in await _brandService.GetAllAsync().ConfigureAwait(false)
                                    on p.BrandId equals b.Id
                                    select new GetParfumeWithBrandDto()
                                    {
                                        Id = p.Id,
                                        BrandId = p.BrandId,
                                        Name = p.Name,
                                        Price = p.Price,
                                        BrandName = b.Name
                                    }).ToList();
                    return response;
                }
                _logger.LogInformation("Parfüm bulunamadı.");
                return new List<GetParfumeWithBrandDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<GetParfumeWithBrandDto>();
            }
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
                    var response = (from p in list.ToList()
                                    join b in await _brandService.GetAllAsync().ConfigureAwait(false)
                                    on p.BrandId equals b.Id
                                    select new GetParfumeWithBrandDto()
                                    {
                                        Id = p.Id,
                                        BrandId = p.BrandId,
                                        Name = p.Name,
                                        Price = p.Price,
                                        BrandName = b.Name
                                    }).ToList();
                    return Ok(new { IsSuccess = true, data = response });
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
                    var brand = await _brandService.GetByIdAsync(data.BrandId).ConfigureAwait(false);
                    if (brand == null)
                    {
                        _logger.LogInformation("Marka bulunamadı.");
                        return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
                    }
                    var response = new GetParfumeWithBrandDto()
                    {
                        BrandId = data.BrandId,
                        Id = data.Id,
                        Name = data.Name,
                        Price = data.Price,
                        BrandName = brand.Name
                    };
                    return Ok(new { IsSuccess = true, data = response });
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
                    var response = (from p in data.ToList()
                                    join b in await _brandService.GetAllAsync().ConfigureAwait(false)
                                    on p.BrandId equals b.Id
                                    select new GetParfumeWithBrandDto()
                                    {
                                        Id = p.Id,
                                        BrandId = p.BrandId,
                                        Name = p.Name,
                                        Price = p.Price,
                                        BrandName = b.Name
                                    }).ToList();
                    return Ok(new { IsSuccess = true, data = response });
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
                if (brand == null)
                {
                    _logger.LogInformation("Marka bulunamadı.");
                    return NotFound(new { IsSuccess = false, message = "Marka bulunamadı." });
                }
                var mappedData = _mapper.Map<Parfum>(data);
                mappedData.BrandName = brand;
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
