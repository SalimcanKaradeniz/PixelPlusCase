using Data.DbEntity;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IUserService
    {
        ReturnModel<LoginResponseModel> Login(LoginModel model);
        ReturnModel<Users> CreateUser(string phoneNumber, string password);
    }
}
