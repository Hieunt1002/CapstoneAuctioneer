using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IUploadRepository
    {
        public Task UploadFile(IFormFile file);
        public Task<Stream> ReadFileAsync(string fileName);
    }
}
