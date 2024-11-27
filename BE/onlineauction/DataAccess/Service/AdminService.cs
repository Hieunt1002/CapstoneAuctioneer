﻿using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminService
    {
        /// <summary>
        /// The admin repository
        /// </summary>
        private readonly IAdminRepository _adminRepository;
        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IAccountRepository _accountRepository;

        private readonly IAuctioneerRepository _auctioneerRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService" /> class.
        /// </summary>
        /// <param name="adminRepository">The admin repository.</param>
        /// <param name="accountRepository">The account repository.</param>
        public AdminService(IAdminRepository adminRepository, IAccountRepository accountRepository, IAuctioneerRepository auctioneerRepository) 
        {
            _adminRepository= adminRepository;
            _accountRepository = accountRepository;
            _auctioneerRepository = auctioneerRepository;
        }
        /// <summary>
        /// Lists the category asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO> ListCategoryAsync()
        {
            var result = await _adminRepository.ListCategory();
            return result;
        }
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AddCategory(string name)
        {
            var result = await _adminRepository.AddCategory(name);
            return result;
        }
        /// <summary>
        /// Accepts the auctioneer for admin.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <param name="idAuction">The identifier auction.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction)
        {
            var result = await _adminRepository.AcceptAuctioneerForAdmin(autioneer, idAuction);
            return result;
        }
        /// <summary>
        /// Updates the category asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Namecategory">The namecategory.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateCategoryAsync(int id, string Namecategory)
        {
            var result = await _adminRepository.UpdateCategory(id, Namecategory);
            return result;
        }
        /// <summary>
        /// Deletes the category asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> DeleteCategoryAsync(int id)
        {
            var result = await _adminRepository.DeleteCategory(id);
            return result;
        }
        /// <summary>
        /// Lists the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListAuction(string id, int status)
        {
            try
            {
                var result = await _adminRepository.ListAuction(id, status);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }

        /// <summary>
        /// Profiles the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ProfileUser(string username)
        {
            var result = await _adminRepository.ProfileUser(username);
            return result;
        }
        /// <summary>
        /// Lists the account.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO> ListAccount()
        {
            var listAccount = await _accountRepository.ListAccount();
            return listAccount;
        }
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AuctionDetail(int id)
        {
            var listAccount = await _adminRepository.AuctionDetail(id);
            return listAccount;
        }
        /// <summary>
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            try
            {
                var result = await _adminRepository.ListYourAuctioneerCategoryAdmin(id, status, category);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> SearchAuctioneerAdmin(string id, string content)
        {
            try
            {
                var result = await _adminRepository.SearchAuctioneerAdmin(id, content);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Auctions the detail batch job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<DAuctioneerDTO> AuctionDetailBatchJob(int id)
        {
            var result = await _adminRepository.AuctionDetailBatchJob(id);
            return result;
        }
        /// <summary>
        /// Res up auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ReUpAuction(int id)
        {
            var result = await _adminRepository.ReUpAuction(id);
            return result;
        }
        /// <summary>
        /// Lists the bidder in auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> listBidderInAuction(int id)
        {
            var result = await _adminRepository.listBidderInAuction(id);
            return result;
        }
        public async Task<ResponseDTO> ListAuctioneerByUser(string id, int status)
        {
            var result = await _auctioneerRepository.ListAuctioneerByUser(id, status);
            return result;
        }

        public async Task<ResponseDTO> ListAuctioneerRegisterByUser(string id, int status)
        {
            var result = await _auctioneerRepository.ListAuctioneerRegisterByUser(id, status);
            return result;
        }
        public async Task<ResponseDTO> AuctionRoomAdmin(int id)
        {
            try
            {
                var result = await _adminRepository.AuctionRoomAdmin(id);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
    }
}