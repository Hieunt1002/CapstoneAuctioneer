using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ConnectDB _context;
        private readonly UserManager<Account> _accountManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(
            ConnectDB context,
            UserManager<Account> accountManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _context = context;
            _accountManager = accountManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> LoginAsync(Login loginDTO)
        {
            Account account = null;

            if (loginDTO.username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(loginDTO.username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(loginDTO.username);
            }

            if (account == null || !await _accountManager.CheckPasswordAsync(account, loginDTO.password))
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Invalid credentials" };
            }

            var userRoles = await _accountManager.GetRolesAsync(account);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);
            return new ResponseDTO() { IsSucceed = true, Message = token };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        public async Task<ResponseDTO> MakeUSERAsync(AddAccountDTO account)
        {
            var isExistUser = await _accountManager.FindByNameAsync(account.UserName);

            if (isExistUser != null)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "UserName already exists" };
            }

            var createAccount = new Account
            {
                UserName = account.UserName,
                Email = account.Email,
                Warning = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _accountManager.CreateAsync(createAccount, account.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = string.Join(" ", createUserResult.Errors.Select(e => e.Description));
                return new ResponseDTO() { IsSucceed = false, Message = "User creation failed because: " + errorString };
            }

            var createAccountDetail = new AccountDetail
            {
                AccountID = createAccount.Id,
                FullName = account.FullName,
                Phone = account.Phone,
                City = account.City,
                Ward = account.ward,
                District = account.district,
                Address = account.Address
            };

            var createAccountDetailResult = await AccountDAO.Instance.AddAccountDetailAsync(createAccountDetail);

            if (!createAccountDetailResult)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account detail creation failed" };
            }

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.USER);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> ChangePassWord(ChangepassDTO changepassDTO)
        {
            Account account = null;

            if (changepassDTO.username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(changepassDTO.username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(changepassDTO.username);
            }
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }
            var isPasswordValid = await _accountManager.CheckPasswordAsync(account, changepassDTO.oldpassword);
            if (!isPasswordValid)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Old password is incorrect" };
            }
            var changePasswordResult = await _accountManager.ChangePasswordAsync(account, changepassDTO.oldpassword, changepassDTO.newpassword);
            if (!changePasswordResult.Succeeded)
            {
                var errorString = string.Join(" ", changePasswordResult.Errors.Select(e => e.Description));
                return new ResponseDTO { IsSucceed = false, Message = "Password change failed because: " + errorString };
            }
            return new ResponseDTO { IsSucceed = true, Message = "Password changed successfully" };
        }

        public async Task<ResponseDTO> ProfileUser(string username)
        {
            ProfileDTO profileDTO = null;
            Account account = null;
            AccountDetail accountDetail = null;
            if (username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(username);
            }
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }
            accountDetail = await AccountDAO.Instance.ProfileDAO(account.Id);
            if (accountDetail == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account details not found" };
            }
            profileDTO = new ProfileDTO
            {
                UserName = account.UserName,
                Avatar = accountDetail.Avatar,
                Email = account.Email,
                FullName = accountDetail.FullName,
                Phone = accountDetail.Phone,
                City = accountDetail.City,
                Ward = accountDetail.Ward,
                District = accountDetail.District,
                Address = accountDetail.Address
            };
            return new ResponseDTO { Result = profileDTO, IsSucceed = true, Message = "Successfully" };
        }
    }
}
