using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.Security.Jwt;

namespace Bfm.Diet.Authorization.Service
{
    public interface ITokenService
    {
        AccessToken CreateToken(Kullanici user);
    }
}