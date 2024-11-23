using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerOf.Core.DTO_s;
using PowerOf.Core.IUnitOfWork;
using System.Security.Claims;

namespace PowerOfAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost("CreateInvoiceFor/{orderId}")]
        public async Task<IActionResult> CreateInvoice(int orderId)
        {
            if (orderId== null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var invoice = await _unitOfWork._invoiceRepository.CreateInvoiceAsync(orderId);
                var invoicesDTO = _mapper.Map<InvoiceDTO>(invoice);

                return CreatedAtAction(nameof(GetInvoiceById), new { invoiceId = invoice.Id },invoicesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetInvoiceByUserId")]
        public async Task<IActionResult> GetInvoicesByUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var invoices = await _unitOfWork._invoiceRepository.GetInvoicesByUserIdAsync(userId);
            var invoiceDTOs = _mapper.Map<IEnumerable<InvoiceDTO>>(invoices);

            return Ok(invoiceDTOs);
        }

        [HttpGet("GetInvoiceBy/{invoiceId}")]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            var invoice = await _unitOfWork._invoiceRepository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            var invoiceDTO = _mapper.Map<InvoiceDTO>(invoice);
            return Ok(invoiceDTO);
        }
    }
}
