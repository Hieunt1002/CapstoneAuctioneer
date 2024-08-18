using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UploadRepository : IUploadRepository
    {
        private readonly string destinationFolderPath;

        public UploadRepository(string baseDirectory)
        {
            // Khởi tạo thư mục 'uploads' trong thư mục gốc của dự án
            destinationFolderPath = Path.Combine(baseDirectory, "uploads");

            // Tạo thư mục 'uploads' nếu nó chưa tồn tại
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ.");
            }

            try
            {
                // Tạo tên file mới tại thư mục đích
                string fileName = Path.GetFileName(file.FileName);
                string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

                // Lưu file vào thư mục đích
                using (var stream = new FileStream(destinationFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Đã xảy ra lỗi khi lưu file.", ex);
            }
        }

        public Task<Stream> ReadFileAsync(string fileName)
        {
            // Giải mã URL nếu cần
            fileName = Uri.UnescapeDataString(fileName);

            // Chuẩn hóa đường dẫn để sử dụng với hệ thống tệp của Windows
            var filePath = Path.Combine(destinationFolderPath, fileName.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File không tồn tại.", fileName);
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult((Stream)stream);
        }
        public string ReadFileAsyncs(string fileName)
        {
            // Giải mã URL nếu cần
            fileName = Uri.UnescapeDataString(fileName);

            // Chuẩn hóa đường dẫn để sử dụng với hệ thống tệp của Windows
            var filePath = Path.Combine(destinationFolderPath, fileName.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File không tồn tại.", fileName);
            }
            return filePath;
        }
    }
}
