using Azure;
using DataAccess;
using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Authorize(Policy = "USER")]
        [Route("addAuctioneer")]
        public async Task<ActionResult> AddAuctionner(
            IFormFile Image,
            string NameAuctioneer,
            string Description,
            decimal StartingPrice,
            int CategoryID,
            string StartDay,
            string StartTime,
            string EndDay,
            string EndTime,
            decimal PriceStep,
            IFormFile file,
            IFormFile signatureImg,
            IFormFile image
            )
        {
            var register = new RegisterAuctioneerDTO
            {
                Image = Image,
                NameAuctioneer = NameAuctioneer,
                Description = Description,
                StartingPrice = StartingPrice,
                CategoryID = CategoryID,
                StartDay = StartDay,
                StartTime = StartTime,
                EndDay = EndDay,
                EndTime = EndTime,
                PriceStep = PriceStep,
                file = file,
                signatureImg = signatureImg,
                image = image
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var result = await _userService.RegiterAuctioneer(userId, register);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpPut]
        [Authorize(Policy = "USER")]
        [Route("UpdateAuctionner")]
        public async Task<ActionResult> UpdateAuctionner(
            int AuctionID,
            IFormFile Image,
            string NameAuctioneer,
            string Description,
            decimal StartingPrice
            )
        {
            var update = new UDAuctionDTO
            {
                AuctionID = AuctionID,
                Image= Image,
                NameAuctioneer = NameAuctioneer,
                Description = Description,
                StartingPrice = StartingPrice
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var result = await _userService.UpdateAuctioneer(userId, update);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = "USER")]
        [Route("DeleteAuctionner")]
        public async Task<ActionResult> DeleteAuctionner(int id)
        {
            var result = await _userService.DeleteAuctioneer(id);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
