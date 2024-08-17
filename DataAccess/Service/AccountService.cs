using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Service
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ResponseDTO> loginService(Login sign)
        {
            var login = await _accountRepository.LoginAsync(sign);
            return login;
        }

        public async Task<ResponseDTO> RegisterService(AddAccountDTO account)
        {
            var register = await _accountRepository.MakeUSERAsync(account);
            return register;

        }
        public async Task<ResponseDTO> ChangePassWordAsync(ChangepassDTO changepassDTO)
        {
            var changepassword = await _accountRepository.ChangePassWord(changepassDTO);
            return changepassword;
        }
        public async Task<ResponseDTO> ProfileUserAsync(string username)
        {
            var changepassword = await _accountRepository.ProfileUser(username);
            return changepassword;
        }
    }
}
