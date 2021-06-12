using Core.Models;
using Core.UnitOfWork;
using Data.Context;
using Data.DbEntity;
using Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<PixelPlusContext> _unitOfWork;
        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWork<PixelPlusContext> unitOfWork, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        public ReturnModel<Users> CreateUser(string phoneNumber, string password) 
        {
            ReturnModel<Users> returnModel = new ReturnModel<Users>();

            try
            {
                Users user = new Users()
                {
                    Phone = phoneNumber,
                    Password = password,
                    Type = false
                };

                _unitOfWork.GetRepository<Users>().Insert(user);
                _unitOfWork.SaveChanges();

                returnModel.Data = user;
                returnModel.IsSuccess = true;
                returnModel.Message = "Kullanıcı oluşturuldu";
            }
            catch (Exception ex)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "Kullanıcı oluşturulamadı";
            }
            return returnModel;
        }
        public ReturnModel<LoginResponseModel> Login(LoginModel model)
        {
            ReturnModel<LoginResponseModel> returnModel = new ReturnModel<LoginResponseModel>();
            LoginResponseModel loginResponseModel = new LoginResponseModel();

            try
            {
                var user = _unitOfWork.GetRepository<Users>().GetFirstOrDefault(predicate: x => x.Phone == model.Phone && x.Password == model.Password);

                if (user == null)
                {
                    returnModel.IsSuccess = false;
                    returnModel.Message = "Kullanıcı bulunamadı";

                    return returnModel;
                }

                var key = Encoding.ASCII.GetBytes(_appSettings.JwtConfiguration.SigningKey);
                var singingKey = new SymmetricSecurityKey(key);

                DateTime expiresDate = DateTime.Now.AddDays(1);

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Audience = _appSettings.JwtConfiguration.Audience,
                    Issuer = _appSettings.JwtConfiguration.Issuer,
                    SigningCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256Signature),
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Phone),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Type", user.Type.ToString())
                    }),
                    Expires = expiresDate,
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);


                loginResponseModel.Token = tokenString;
                loginResponseModel.TokenExpireDate = expiresDate;

                returnModel.IsSuccess = true;
                returnModel.Message = "İşlem Gerçekleştirildi.";
                returnModel.Data = loginResponseModel;
            }
            catch (Exception ex)
            {
                returnModel.IsSuccess = false;
                returnModel.Message = "İşlem Gerçekleştirilemedi";
            }

            return returnModel;
        }
    }
}
