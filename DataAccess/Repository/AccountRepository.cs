using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Principal;
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
        private readonly IUploadRepository _uploadRepository;

        public AccountRepository(
            ConnectDB context,
            UserManager<Account> accountManager,
            RoleManager<IdentityRole> roleManager,
            IUploadRepository uploadRepository,
            IConfiguration configuration)
        {
            _context = context;
            _accountManager = accountManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _uploadRepository = uploadRepository;
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

            var token = GenerateNewJsonWebToken(authClaims, TimeSpan.FromDays(1));
            return new ResponseDTO() { IsSucceed = true, Message = token };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims, TimeSpan expiresIn)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.Add(expiresIn),
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
                AccountId = account.Id,
                UserName = account.UserName,
                Avatar = accountDetail.Avatar,
                FrontCCCD= accountDetail.FrontCCCD,
                BacksideCCCD= accountDetail.BacksideCCCD,
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
        private string GetResetPasswordEmailContent(string resetLink)
        {
            string emailContent = @"<!DOCTYPE html>
                            <html lang='en'>
                            <head>
                                <meta charset='UTF-8'>
                                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                <title>Reset Password</title>
                            </head>
                            <body>
                                <p>Dear User,</p>
                                <p>You recently requested to reset your password. Please click the link below to reset your password:</p>
                                <p>Enter the link to change your password <a href='" + resetLink + @"'>Link</a></p>
                                <p>If you did not request a password reset, please ignore this email. Your password will remain unchanged.</p>
                                <p>Best regards,</p>
                                <p>YourApp Team</p>
                            </body>
                            </html>";
            return emailContent;
        }
        public async Task<ResponseDTO> ForgotPassword(string username)
        {
            // Tìm kiếm tài khoản theo email hoặc tên người dùng
            Account account = null;
            if (username.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(username);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(username);
            }

            // Kiểm tra xem tài khoản có tồn tại không
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Tạo token để đặt lại mật khẩu
            var code = await _accountManager.GeneratePasswordResetTokenAsync(account);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Tạo đường link đặt lại mật khẩu
            var resetLink = $"https://www.hienmaugiotmauhong.online/resetpassword/{code}/{account.Email}";

            // Gửi email chứa đường link đặt lại mật khẩu
            await MailUtils.SendMailGoogleSmtp(
                fromEmail: "nguyenanh0978638@gmail.com",
                toEmail: account.Email,
                subject: "Forgot Password",
                body: GetResetPasswordEmailContent(resetLink),
                gmailSend: "nguyenanh0978638@gmail.com",
                gmailPassword: "zwlcvsnblwndpbpe"
            );

            return new ResponseDTO { IsSucceed = true, Message = "Email sent successfully." };
        }



        public async Task<ResponseDTO> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            // Tìm kiếm tài khoản theo email hoặc tên người dùng
            Account account = null;
            if (resetPasswordDTO.UsernameOrEmail.Contains('@'))
            {
                account = await _accountManager.FindByEmailAsync(resetPasswordDTO.UsernameOrEmail);
            }
            else
            {
                account = await _accountManager.FindByNameAsync(resetPasswordDTO.UsernameOrEmail);
            }

            // Kiểm tra xem tài khoản có tồn tại không
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            // Thực hiện đặt lại mật khẩu bằng token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDTO.ResetToken));
            var resetPasswordResult = await _accountManager.ResetPasswordAsync(account, decodedToken, resetPasswordDTO.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                var errorString = string.Join(" ", resetPasswordResult.Errors.Select(e => e.Description));
                return new ResponseDTO { IsSucceed = false, Message = "Password reset failed because: " + errorString };
            }

            return new ResponseDTO { IsSucceed = true, Message = "Password reset successfully" };
        }

        public async Task<ResponseDTO> UpdateUserProfile(string userID, UProfileDTO uProfileDTO)
        {
            var account = await AccountDAO.Instance.ProfileDAO(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            var accountDetail = new AccountDetail
            {
                AccountID = userID,
                FullName = uProfileDTO.FullName,
                Phone = uProfileDTO.Phone,
                City = uProfileDTO.City,
                Ward = uProfileDTO.Ward,
                District = uProfileDTO.District,
                Address = uProfileDTO.Address,
                Avatar = uProfileDTO.Avatar != null ? await SaveFileAsync(uProfileDTO.Avatar, "avatars", userID) : account.Avatar,
                FrontCCCD = uProfileDTO.FrontCCCD != null ? await SaveFileAsync(uProfileDTO.FrontCCCD, "cccd/front", userID) : account.FrontCCCD,
                BacksideCCCD = uProfileDTO.BacksideCCCD != null ? await SaveFileAsync(uProfileDTO.BacksideCCCD, "cccd/back", userID) : account.BacksideCCCD
            };

            try
            {
                await AccountDAO.Instance.UpdateAccountDetail(accountDetail);
                return new ResponseDTO { IsSucceed = true, Message = "Profile updated successfully" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chi tiết hoặc ghi log
                return new ResponseDTO { IsSucceed = false, Message = "Profile update failed: " + ex.Message };
            }
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder, string userId)
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

        public async Task<ResponseDTO> MakeAdminsync(AddAccountDTO updatePermissionDTO)
        {
            var isExistUser = await _accountManager.FindByNameAsync(updatePermissionDTO.UserName);

            if (isExistUser != null)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "UserName already exists" };
            }

            var createAccount = new Account
            {
                UserName = updatePermissionDTO.UserName,
                Email = updatePermissionDTO.Email,
                Warning = 0,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _accountManager.CreateAsync(createAccount, updatePermissionDTO.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = string.Join(" ", createUserResult.Errors.Select(e => e.Description));
                return new ResponseDTO() { IsSucceed = false, Message = "User creation failed because: " + errorString };
            }

            var createAccountDetail = new AccountDetail
            {
                AccountID = createAccount.Id,
                FullName = updatePermissionDTO.FullName,
                Phone = updatePermissionDTO.Phone,
                City = updatePermissionDTO.City,
                Ward = updatePermissionDTO.ward,
                District = updatePermissionDTO.district,
                Address = updatePermissionDTO.Address
            };

            var createAccountDetailResult = await AccountDAO.Instance.AddAccountDetailAsync(createAccountDetail);

            if (!createAccountDetailResult)
            {
                return new ResponseDTO() { IsSucceed = false, Message = "Account detail creation failed" };
            }

            await _accountManager.AddToRoleAsync(createAccount, StaticUserRoles.ADMIN);
            return new ResponseDTO() { IsSucceed = true, Message = "User created successfully" };
        }

        public async Task<ResponseDTO> ListAccount()
        {
            try
            {
                var accounts = await (from acc in _accountManager.Users
                                      join accDetail in _context.AccountDetails on acc.Id equals accDetail.AccountID
                                      select new ProfileDTO
                                      {
                                          AccountId = acc.Id,
                                          UserName = acc.UserName,
                                          Email = acc.Email,
                                          FullName = accDetail.FullName,
                                          Phone = accDetail.Phone,
                                          City = accDetail.City,
                                          Ward = accDetail.Ward,
                                          District = accDetail.District,
                                          Address = accDetail.Address,
                                          Avatar = accDetail.Avatar,
                                          FrontCCCD = accDetail.FrontCCCD,
                                          BacksideCCCD = accDetail.BacksideCCCD,
                                          Warning = acc.Warning,
                                          Status = acc.Status
                                      }).ToListAsync();

                if (accounts == null || !accounts.Any())
                {
                    return new ResponseDTO { IsSucceed = false, Message = "No accounts found" };
                }

                return new ResponseDTO { IsSucceed = true, Result = accounts, Message = "Successfully retrieved accounts" };
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về mã lỗi tương ứng
                return new ResponseDTO { IsSucceed = false, Message = "Internal server error: " + ex.Message };
            }
        }

    }
}
