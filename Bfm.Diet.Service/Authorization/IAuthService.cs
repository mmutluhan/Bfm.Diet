using System.Threading.Tasks;
using Bfm.Diet.Core.Security.Jwt;
using Bfm.Diet.Dto.Authorization;

namespace Bfm.Diet.Service.Authorization
{
    public interface IAuthService
    {
        KullaniciDto Login(LoginUserDto loginUser);

        KullaniciDto Register(KullaniciKayitDto kullanici);
        Task<KullaniciDto> RegisterAsync(KullaniciKayitDto kullanici);

        AccessToken CreateAccessToken(KullaniciDto userDto);
        Task<KullaniciDto> LoginAsync(LoginUserDto loginUser);
        Task<AccessToken> CreateAccessTokenAsync(KullaniciDto userDto);
    }
}