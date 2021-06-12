using Microsoft.AspNetCore.Http;
using System;

namespace Core.Models
{
    public interface IAppContext
    {
        int UserId { get; }
        bool Type { get; }
    }

    public class AppContext : IAppContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                try
                {
                    string userId = _httpContextAccessor.HttpContext.User.FindFirst("UserId").Value;

                    if (int.TryParse(userId, out int UserId))
                        return UserId;
                    else
                        return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public bool Type
        {
            get
            {
                try
                {
                    string type = _httpContextAccessor.HttpContext.User.FindFirst("Type").Value;

                    if (bool.TryParse(type, out bool Type))
                        return Type;
                    else
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
