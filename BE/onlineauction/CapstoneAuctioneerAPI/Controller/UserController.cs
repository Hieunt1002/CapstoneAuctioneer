﻿using Azure;
using BusinessObject.Model;
using DataAccess;
using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Net.payOS;
using Net.payOS.Types;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly UserService _userService;

        private readonly PaypalClient _paypalClient;
        /// <summary>
        /// The auction service
        /// </summary>
        private readonly AuctionService _auctionService;
        /// <summary>
        /// The user service
        /// </summary>
        private readonly PayOS _payOS;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(UserService userService, PaypalClient paypalClient, AuctionService auctionService, PayOS payOS)
        {
            _userService = userService;
            _paypalClient = paypalClient;
            _auctionService = auctionService;
            _payOS = payOS;
        }
        /// <summary>
        /// Adds the auctionner.
        /// </summary>
        /// <param name="imageAuction">The image auction.</param>
        /// <param name="nameAuction">The name auction.</param>
        /// <param name="description">The description.</param>
        /// <param name="startingPrice">The starting price.</param>
        /// <param name="categoryID">The category identifier.</param>
        /// <param name="signatureImg">The signature img.</param>
        /// <param name="imageVerification">The image verification.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "USER")]
        [Route("addAuctionItem")]
        public async Task<ActionResult> AddAuctionner([FromForm] FormAddAuctionDTO request)
        {
            var register = new RegisterAuctioneerDTO
            {
                Image = request.imageAuction,
                NameAuction = request.nameAuction,
                Description = request.description,
                StartingPrice = request.startingPrice,
                CategoryID = request.categoryID,
                image = request.imageVerification
            };
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.RegiterAuctioneer(userId, register);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Updates the auctionner.
        /// </summary>
        /// <param name="auctionID">The auction identifier.</param>
        /// <param name="imageAuction">The image auction.</param>
        /// <param name="nameAuctionItem">The name auction item.</param>
        /// <param name="description">The description.</param>
        /// <param name="startingPrice">The starting price.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "USER")]
        [Route("UpdateAuctionItem")]
        public async Task<ActionResult> UpdateAuctionner(
            [FromForm] UpdateAuctionDTO updateAuctionDTOrequest)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UpdateAuctioneer(userId, updateAuctionDTOrequest);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Deletes the auctionner.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Withdraws the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Policy = "USER")]
        [Route("withdraw")]
        public async Task<ActionResult> Withdraw(int id)
        {
            var result = await _userService.Withdraw(id);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Registerforauctions the specified aid.
        /// </summary>
        /// <param name="aid">The aid.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "USER")]
        [Route("registerforauction")]
        public async Task<ActionResult> registerforauction(int aid)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.UserRegisterAuction(userId, aid);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }
            var deposit = await _userService.TotalPayDeposit(aid, userId);
            ItemData item = new ItemData(deposit.nameAuction, 1, deposit.priceAuction);
            List<ItemData> items = new List<ItemData>();
            items.Add(item);
            var did = GenerateUniqueId();
            var deposits = new Deposit
            {
                DID = did.ToString(),
                RAID = deposit.IdResgiter,
                PaymentType = "Payos",
                PaymentDate = DateTime.Now.ToString(),
                status = "wait"
            };
            var pay = await _userService.PaymentForDeposit(deposits);
            if (pay != true)
            {
                return BadRequest(StatusCodes.Status500InternalServerError);
            }
            string shortenedName = deposit.nameAuction.Length > 10
                    ? deposit.nameAuction.Substring(0, 10)
                    : deposit.nameAuction;
            var expirationTime = DateTime.Now.AddMinutes(15).ToString("yyyy-MM-dd HH:mm:ss");
            PaymentData paymentData = new PaymentData(did, deposit.priceAuction, $"Tiền Cọc {shortenedName}", items, "http://localhost:5173/cancel", "http://localhost:5173/success", expirationTime);
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
            return Ok(createPayment.checkoutUrl);
        }
        private int GenerateUniqueId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            long longValue = BitConverter.ToInt64(buffer, 0); // Chuyển GUID thành long

            // Giới hạn giá trị trả về chỉ trong phạm vi 5 chữ số
            int uniqueId = (int)(Math.Abs(longValue) % 100000); // Lấy giá trị dương và chỉ lấy 5 chữ số cuối cùng

            return uniqueId;
        }
        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "USER")]
        [Route("placeBid")]
        public async Task<ActionResult> placeBid(RaiseDTO auction)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.PlaceBid(userId, auction);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Auctionregistrationlists the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "USER")]
        [Route("auctionregistrationlist")]
        public async Task<ActionResult> auctionregistrationlist(int status)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.ListYourAuctioneer(userId, status);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Auctionregistrationlists the specified status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "USER")]
        [Route("searchauctionregistrationlist")]
        public async Task<ActionResult> searchauctionregistrationlist(int category, string content)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.SearchListYourAuctioneer(userId, category, content);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Auctionregistrationdetails the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "USER")]
        [Route("auctionregistrationdetail")]
        public async Task<ActionResult> auctionregistrationdetail(int id)
        {
            var result = await _userService.ListYourAutioneerDetail(id);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("joinRoomAuction")]
        public async Task<ActionResult> Auctionroom(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _userService.Auctionroom(id, userId);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// Views the bid history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("viewBidHistory")]
        public async Task<IActionResult> ViewBidHistory(int id)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    while (webSocket.State == WebSocketState.Open)
                    {
                        var AuctionDetails = await _userService.ViewBidHistory(id);
                        // Chuyển đổi chuỗi thành qua kiểu json
                        string jsonString = JsonSerializer.Serialize(AuctionDetails);
                        // Chuyển đổi thời gian còn lại thành mảng byte
                        var bytes = Encoding.UTF8.GetBytes(jsonString);
                        await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                        await Task.Delay(1000); // Gửi dữ liệu mỗi 1 giây
                    }

                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the server", CancellationToken.None);
                    return new EmptyResult(); // Kết thúc WebSocket
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return new BadRequestResult(); // Trả về mã trạng thái lỗi nếu không phải yêu cầu WebSocket
            }
        }
    }
}
