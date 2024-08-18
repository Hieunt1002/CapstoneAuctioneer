using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneAuctioneerAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadRepository _uploadRepository;

        public UploadController(IUploadRepository uploadRepository)
        {
            _uploadRepository = uploadRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File không hợp lệ.");
            }

            try
            {
                await _uploadRepository.UploadFile(file);
                return Ok(new { Message = "File đã được tải lên thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi không xác định.");
            }
        }

        [HttpGet("read/{fileName}")]
        public async Task<IActionResult> ReadFile(string fileName)
        {
            try
            {
                var fileStream = await _uploadRepository.ReadFileAsync(fileName);

                // Xác định loại MIME của tệp dựa trên phần mở rộng
                var contentType = "application/octet-stream";
                var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

                contentType = fileExtension switch
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => contentType
                };

                return File(fileStream, contentType, fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi khi đọc file.");
            }
        }
    }
}
