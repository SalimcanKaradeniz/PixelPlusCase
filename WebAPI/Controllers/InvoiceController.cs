using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IAppContext _appContext;
        public InvoiceController(IInvoiceService invoiceService, IAppContext appContext)
        {
            _invoiceService = invoiceService;
            _appContext = appContext;
        }

        #region Invoice

        [HttpGet]
        [Route("/GetInvoices")]
        public IActionResult GetInvoices()
        {
            return Ok(_invoiceService.GetInvoices(_appContext.UserId));
        }

        [HttpPost]
        [Route("/InvoicePayment")]
        public IActionResult InvoicePayment([FromBody] InvoicePaymentRequestModel model)
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();

            #region Parameter Check

            if (model.InvoiceNumber == 0)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Fatura Numarası Bilginiz Eksiktir.Eksik Alanları Doldurup Lütfen Tekrar Deneyiniz";
                return BadRequest(returnModel);
            }

            if (model.TotalPrice == 0)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Ödeme Miktarı Bilginiz Eksiktir.Eksik Alanları Doldurup Lütfen Tekrar Deneyiniz";
                return BadRequest(returnModel);
            }

            #endregion

            returnModel = _invoiceService.InvoicePayment(model);

            if (returnModel.IsSuccess)
                return Ok(returnModel);
            else
                return BadRequest(returnModel);
        }

        #endregion
    }
}
