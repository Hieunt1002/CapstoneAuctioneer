﻿using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DataAccess.IRepository.IUploadRepository" />
    public class UploadRepository : IUploadRepository
    {
        /// <summary>
        /// The destination folder path
        /// </summary>
        private readonly string destinationFolderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadRepository"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
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

        /// <summary>
        /// Saves the file asynchronous.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<string> SaveFileAsync(IFormFile file, string folder, string userId)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads", folder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{userId}_{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{folder}/{fileName}";
        }

        /// <summary>
        /// Reads the file asynchronous.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">File không tồn tại.</exception>
        public Task<Stream> ReadFileAsync(string filePath)
        {
            // Chuyển đổi dấu gạch chéo `/` thành gạch chéo ngược `\` cho hệ thống tệp của Windows
            var fullPath = Path.Combine(destinationFolderPath, filePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("File không tồn tại.", filePath);
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult((Stream)stream);
        }
        /// <summary>
        /// Reads the file asyncs.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">File không tồn tại.</exception>
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
