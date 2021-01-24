using System;
using System.Threading.Tasks;
using Bfm.Diet.Authorization.Messages;
using Bfm.Diet.Core.Controller;
using Bfm.Diet.Core.Response;
using Bfm.Diet.Dto.Authorization;
using Bfm.Diet.Service.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bfm.Diet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BfmControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(KullaniciKayitDto kullanici)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RegisterAsync(kullanici);
                return Ok(new SuccessDataResponse<KullaniciDto>(result, Success.UserRegistrationSuccessful));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, Error.UserRegistrationFailed));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _authService.Login(loginUserDto);
                var result = await _authService.CreateAccessTokenAsync(user);
                if (string.IsNullOrEmpty(result.Token))
                    throw new ApplicationException(Error.TokenCreationError);
                var loginResult = new LoginUserResultDto
                {
                    AccessToken = result,
                    Kullanici = user
                };
                return Ok(new SuccessDataResponse<LoginUserResultDto>(loginResult));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}