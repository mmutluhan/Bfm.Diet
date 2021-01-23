using System.Threading.Tasks;
using Bfm.Diet.Authorization.Messages;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Authorization.Service;
using Bfm.Diet.Core.Attributes;
using Bfm.Diet.Core.Exceptions;
using Bfm.Diet.Core.Security.Hashing;
using Bfm.Diet.Core.Security.Jwt;
using Bfm.Diet.Core.Services;
using Bfm.Diet.Dto.Authorization;
using Bfm.Diet.Dto.Extensions;

namespace Bfm.Diet.Service.Authorization
{
    [Log]
    public class AuthService : IAuthService
    {
        private readonly IMailService _mailService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService(IUserService userService, ITokenService tokenService, IMailService mailService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mailService = mailService;
        }

        public async Task<KullaniciDto> Login(LoginUserDto loginUser)
        {
            //var result = _userService.GetKullaniciByEMail(loginUser.Kullanici); 
            var result = await _userService.GetKullaniciByEMailAsync(loginUser.Kullanici);
            if (result == null)
                throw new ApplicationException(Error.UserNotFound);

            if (!Hashing.VerifyPasswordHash(loginUser.Parola, result.PasswordHash, result.PasswordSalt))
                throw new ApplicationException(Error.PasswordError);

            if (!result.Durum)
                throw new ApplicationException(Error.UserAccountNotActive);
            var user = result.Map<Kullanici, KullaniciDto>();
            return await Task.FromResult(user);
        }

        public KullaniciDto Register(KullaniciKayitDto kullanici)
        {
            var result = _userService.FirstOrDefault(u => u.Email == kullanici.Email);
            if (result != null)
                throw new ApplicationException(Error.UserAlreadyExists);

            Hashing.CreatePasswordHash(kullanici.Parola, out var passwordHash, out var passwordSalt);
            var user = kullanici.Map<KullaniciKayitDto, Kullanici>();
            user.Durum = true;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _userService.Insert(user);
            if (!user.IsTransient())
                _mailService.SendAsync(user.Email, null, "Kayıt", "Kaydetme başarılı");

            var retKullanici = user.Map<Kullanici, KullaniciDto>();
            retKullanici.Parola = string.Empty;
            return retKullanici;
        }

        public async Task<KullaniciDto> RegisterAsync(KullaniciKayitDto kullanici)
        {
            var result = await _userService.FirstOrDefaultAsync(u => u.Email == kullanici.Email);
            if (result != null)
                throw new ApplicationException(Error.UserAlreadyExists);

            Hashing.CreatePasswordHash(kullanici.Parola, out var passwordHash, out var passwordSalt);
            var user = kullanici.Map<KullaniciKayitDto, Kullanici>();
            user.Durum = true;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _userService.InsertAsync(user);
            if (!user.IsTransient())
                await _mailService.SendAsync(user.Email, null, "Kayıt", "Kaydetme başarılı");

            var retKullanici = user.Map<Kullanici, KullaniciDto>();
            retKullanici.Parola = string.Empty;
            return retKullanici;
        }

        public AccessToken CreateAccessToken(KullaniciDto userDto)
        {
            var user = userDto.Map<KullaniciDto, Kullanici>();
            var token = _tokenService.CreateToken(user);
            return token;
        }

        public async Task<KullaniciDto> LoginAsync(LoginUserDto loginUser)
        {
            var result = await _userService.FirstOrDefaultAsync(u => u.Email == loginUser.Kullanici);
            var user = result.Map<Kullanici, KullaniciDto>();
            return user;
        }

        public async Task<AccessToken> CreateAccessTokenAsync(KullaniciDto userDto)
        {
            var user = userDto.Map<KullaniciDto, Kullanici>();
            var token = _tokenService.CreateToken(user);
            return await Task.FromResult(token);
        }
    }
}