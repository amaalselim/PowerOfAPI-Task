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
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> ViewAllProduct()
        {
            var product = await _unitOfWork._ProductRepository.GetAllEntityAsync();
            return Ok(product);
        }
        [HttpGet("GetProductBy/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetEntityByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDTO);

            if (productDTO.Img != null)
            {
                var path = @"Images/Products/";
                product.Img = await _imageService.SaveImageAsync(productDTO.Img, path);
            }

            await _unitOfWork._ProductRepository.AddEntityAsync(product);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        [HttpPut("EditProductBy/{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromForm] ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _unitOfWork._ProductRepository.GetEntityByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var oldProductImg = product.Img;

            _mapper.Map(productDTO, product);

            if (productDTO.Img != null)
            {
                if (!string.IsNullOrEmpty(oldProductImg))
                {
                    await _imageService.DeleteFileAsync(oldProductImg);
                }
                var path = @"Images/Products/";
                product.Img = await _imageService.SaveImageAsync(productDTO.Img, path);
            }

            await _unitOfWork._ProductRepository.UpdateEntityAsync(product);
            await _unitOfWork.CompleteAsync();
            return Ok(new
            {
                Message = "Product updated successfully!",
                NewData = product
            });
        }

        [HttpDelete("DeleteProductBy/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork._ProductRepository.GetEntityByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _unitOfWork._ProductRepository.DeleteEntityAsync(id);
            return Ok(new { message = "Product Deleted successfully!" });
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SeachProducts([FromQuery] ProductSearchDTO productSearchDTO)
        {
            var products = await _unitOfWork._ProductRepository.SearchProductAsync(productSearchDTO.SearchTerm);
            if (!products.Any())
            {
                return NotFound("No products found matching the search term.");
            }

            return Ok(products);
        }



    }
}
