using System;
using System.Collections.Generic;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core.Controller;
using Bfm.Diet.Core.Response;
using Bfm.Diet.Core.Security.Hashing;
using Bfm.Diet.Dto.Authorization;
using Bfm.Diet.Dto.Extensions;
using Bfm.Diet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bfm.Diet.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BfmControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = _userService.GetAllList();
                var result = users.Map<IEnumerable<Kullanici>, IEnumerable<KullaniciDto>>();
                var response = new SuccessDataResponse<IEnumerable<KullaniciDto>>(result);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDataResponse<string>(e.Message));
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _userService.FirstOrDefault(x => x.Id == id);
                var result = user.Map<Kullanici, KullaniciDto>();
                var response = new SuccessDataResponse<KullaniciDto>(result);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDataResponse<string>(e.Message));
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] KullaniciDto kullanici)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = kullanici.Map<KullaniciDto, Kullanici>();
                _userService.Insert(user);
                var result = user.Map<Kullanici, KullaniciDto>();
                var response = new SuccessDataResponse<KullaniciDto>(result);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDataResponse<string>(e.Message));
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] KullaniciDto kullanici)
        {
            try
            {
                var user = kullanici.Map<KullaniciDto, Kullanici>();
                Hashing.CreatePasswordHash(kullanici.Parola, out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                _userService.Update(user);
                var result = user.Map<Kullanici, KullaniciDto>();
                var response = new SuccessDataResponse<KullaniciDto>(result);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDataResponse<string>(e.Message));
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete(u => u.Id == id);
                var response = new SuccessDataResponse<string>(id + ", Kullanci silindi");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorDataResponse<string>(e.Message));
            }
        }
    }
}