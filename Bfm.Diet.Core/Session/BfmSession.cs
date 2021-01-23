using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Bfm.Diet.Core.Session
{
    public class BfmSession : IBfmSessionInfo
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _id;

        public BfmSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int Id
        {
            get
            {
                var sid = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
                if (int.TryParse(sid?.Value, out var id)) _id = id;

                return _id;
            }
        }

        public string Adi
        {
            get
            {
                var name = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                return name?.Value;
            }
        }

        public string EMail
        {
            get
            {
                var email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x =>
                    x.Type == ClaimTypes.Email);
                return email?.Value;
            }
        }

        public string SessionId
        {
            get
            {
               return  _httpContextAccessor.HttpContext?.Session.Id;
            }
        }
    }
}