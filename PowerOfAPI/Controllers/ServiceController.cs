using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerOf.Application.DTO_s;
using PowerOf.Application.IServices;
using PowerOf.Core.Entities;
using PowerOf.Core.IUnitOfWork;

namespace PowerOfAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ServiceController(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        [HttpGet("GetAllServices")]
        public async Task<IActionResult> ViewAllService()
        {
            var Service = await _unitOfWork._ServiceRepository.GetAllEntityAsync();
            return Ok(Service);
        }
        [HttpGet("GetServiceBy/{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var Service = await _unitOfWork._ServiceRepository.GetEntityByIdAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            return Ok(Service);
        }
        [HttpPost("CreateService")]
        public async Task<IActionResult> CreateService([FromForm] ServiceDTO ServiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Service = _mapper.Map<Service>(ServiceDTO);


            if (ServiceDTO.Img != null)
            {
                var path = @"Images/Services/";
                Service.Img = await _imageService.SaveImageAsync(ServiceDTO.Img, path);
            }

            await _unitOfWork._ServiceRepository.AddEntityAsync(Service);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetServiceById), new { id = Service.Id }, Service);
        }
        [HttpPut("EditServiceBy/{id}")]
        public async Task<IActionResult> EditService(int id, [FromForm] ServiceDTO ServiceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Service = await _unitOfWork._ServiceRepository.GetEntityByIdAsync(id);
            if (Service == null)
            {
                return NotFound();
            }

            var oldServiceImg = Service.Img;

            _mapper.Map(ServiceDTO, Service);

            if (ServiceDTO.Img != null)
            {
                if (!string.IsNullOrEmpty(oldServiceImg))
                {
                    await _imageService.DeleteFileAsync(oldServiceImg);
                }
                var path = @"Images/Services/";
                Service.Img = await _imageService.SaveImageAsync(ServiceDTO.Img, path);
            }

            await _unitOfWork._ServiceRepository.UpdateEntityAsync(Service);
            await _unitOfWork.CompleteAsync();
            return Ok(new
            {
                Message = "Service updated successfully!",
                NewData = Service
            });
        }

        [HttpDelete("DeleteServiceBy/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var Service = await _unitOfWork._ServiceRepository.GetEntityByIdAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            await _unitOfWork._ServiceRepository.DeleteEntityAsync(id);
            return Ok(new { message = "Service Deleted successfully!" });
        }



        
    }
}
