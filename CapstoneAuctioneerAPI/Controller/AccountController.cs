using DataAccess.Service;
using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(Login login)
        {
            try
            {
                var result = await _accountService.loginService(login);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result); // Return 400 with the error message
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(AddAccountDTO account)
        {
            try
            {
                var result = await _accountService.RegisterService(account);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result); // Return 400 with the error message
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ResponseDTO() { IsSucceed = false, Message = "Internal server error: " + ex.Message });
            }
        }
        [HttpPut]
        [Authorize]
        [Route("changepassword")]
        public async Task<ActionResult> ChangePassword(ChangepassDTO changepassDTO)
        {
            try
            {
                var result = await _accountService.ChangePassWordAsync(changepassDTO);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<ActionResult> ProfileUser(string username)
        {
            try
            {
                var result = await _accountService.ProfileUserAsync(username);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
