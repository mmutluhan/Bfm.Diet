using System;
using System.Threading.Tasks;
using Bfm.Diet.Authorization.Messages;
using Bfm.Diet.Core.Controller;
using Bfm.Diet.Core.Response;
using Bfm.Diet.Core.Security.Jwt;
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
                return Ok(new SuccessDataResponse<KullaniciDto>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDto loginUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _authService.Login(loginUserDto);
                var result = _authService.CreateAccessToken(user);
                if (string.IsNullOrEmpty(result.Token))
                    throw new ApplicationException(Error.TokenCreationError);

                return Ok(new SuccessDataResponse<AccessToken>(result));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex.Message));
            }
        }
    }
}