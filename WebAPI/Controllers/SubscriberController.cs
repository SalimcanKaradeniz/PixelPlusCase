using Business.Services;
using Core.Models;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberService _subscriberService;
        private readonly IAppContext _appContext;
        public SubscribersController(ISubscriberService subscriberService, IAppContext appContext)
        {
            _subscriberService = subscriberService;
            _appContext = appContext;
        }

        [HttpPost]
        [Route("/GetSubscribe")]
        public IActionResult GetSubscribe([FromBody] int subscribeNumber)
        {
            if (!_appContext.Type)
                return Forbid();

            return Ok(_subscriberService.GetSubscribe(subscribeNumber));
        }

        [HttpPost]
        [Route("/CreateSubscribe")]
        public IActionResult CreateSubscribe([FromBody] SubscriberRequestModel subscribers)
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();

            if (!_appContext.Type)
                return Forbid();

            returnModel = _subscriberService.CreateSubscribe(subscribers);

            if (returnModel.IsSuccess)
                return Ok(returnModel);
            else
                return BadRequest(returnModel);
        }

        [HttpPost]
        [Route("/SubscriberCancelled")]
        public IActionResult SubscriberCancelled([FromBody] int id) 
        {
            ReturnModel<object> returnModel = new ReturnModel<object>();

            if (!_appContext.Type)
                return Forbid();

            if (id == 0 || id < 0)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Abone bulunamadı";
                return BadRequest(returnModel);
            }

            returnModel = _subscriberService.SubscriberCancelled(id);

            if (returnModel.IsSuccess)
                return Ok(returnModel);
            else
                return BadRequest(returnModel);
        }
    }
}