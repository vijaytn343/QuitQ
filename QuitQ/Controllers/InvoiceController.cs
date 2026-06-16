using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.Models;
using QuitQ.Services.InvoiceFeature;
using System.Security.Claims;

namespace QuitQ.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(
            IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

   

[Authorize]
    [HttpGet("order/{orderId}")]
    public IActionResult DownloadInvoice(int orderId)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

        var pdf =
            _invoiceService.GenerateInvoice(
                orderId,
                userId);

        return File(
            pdf,
            "application/pdf",
            $"invoice_{orderId}.pdf");
    }
}
}
