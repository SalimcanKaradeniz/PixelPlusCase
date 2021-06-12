using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Services;
using Data.DbEntity;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            ReturnModel<LoginResponseModel> returnModel = new ReturnModel<LoginResponseModel>();

            if (ModelState.IsValid)
            {
                var userLogin = _userService.Login(model);

                if (!userLogin.IsSuccess)
                    return Unauthorized(returnModel);

                returnModel.IsSuccess = true;
                returnModel.Data = userLogin.Data;

                return Ok(returnModel);
            }
            else
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Model doğrulanamadı";

                return BadRequest();
            }
        }
    }
}