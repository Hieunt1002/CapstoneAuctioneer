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
using System.Security.Claims;
using Azure;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        [Route("account/login")]
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
        [Route("account/register")]
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
        [HttpPost]
        [Route("Admin/make-admin")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> MakeAdminsync(AddAccountDTO account)
        {
            try
            {
                var result = await _accountService.MakeAdminsync(account);
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
        [Route("UserOrAdmin/changepassword")]
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
        [Route("UserOrAdmin/profile")]
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
        [HttpGet]
        [Route("account/forgot")]
        public async Task<ActionResult> ForgotPassword(string username)
        {
            try
            {
                var result = await _accountService.ForgotPassword(username);
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
        [HttpPut]
        [Route("account/resetPass")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                var result = await _accountService.ResetPasswordAsync(resetPasswordDTO);
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
        [HttpPut("UserOrAdmin/update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(
            IFormFile? Avatar,
            string? FullName,
            string? Phone,
            IFormFile? FrontCCCD,
            IFormFile? BacksideCCCD,
            string? City,
            string? Ward,
            string? District,
            string? Address
            )
        {
            var uProfileDTO = new UProfileDTO()
            {
                Avatar = Avatar,
                FullName = FullName,
                Phone = Phone,
                FrontCCCD = FrontCCCD,
                BacksideCCCD = BacksideCCCD,
                City = City,
                Ward = Ward,
                District = District,
                Address = Address
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims
            var response = await _accountService.UpdateUserProfile(userId, uProfileDTO);

            if (!response.IsSucceed)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }
        [HttpGet]
        [Route("Admin/listAccount")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> ListAccount()
        {
            try
            {
                var result = await _accountService.ListAccount();
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
    }
}
