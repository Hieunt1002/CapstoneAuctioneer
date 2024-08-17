using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAccountRepository
    {
        Task<ResponseDTO> LoginAsync(Login loginDTO);
        Task<ResponseDTO> MakeUSERAsync(AddAccountDTO updatePermissionDTO);
        Task<ResponseDTO> ChangePassWord(ChangepassDTO changepassDTO);
        Task<ResponseDTO> ProfileUser(string username);
    }
}
