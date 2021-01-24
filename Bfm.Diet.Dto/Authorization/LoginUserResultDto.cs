using Bfm.Diet.Core.Security.Jwt;

namespace Bfm.Diet.Dto.Authorization
{
    public class LoginUserResultDto
    {
        public KullaniciDto Kullanici { get; set; }
        public AccessToken  AccessToken { get; set; }
    }
}