using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class AccountDAO
    {
        private static AccountDAO _instance = null;
        private static readonly object _instanceLock = new object();

        private AccountDAO() { }

        public static AccountDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AccountDAO();
                    }
                    return _instance;
                }
            }
        }
        public async Task<bool> AddAccountDetailAsync(AccountDetail accountDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.AccountDetails.Add(accountDetail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle exception
                return false;
            }
        }
        public async Task<AccountDetail> ProfileDAO(string accountID)
        {
            AccountDetail accountDetail = null;
            try
            {
                using (var context = new ConnectDB())
                {
                    accountDetail = await context.AccountDetails.FirstOrDefaultAsync(a => a.AccountID == accountID);
                    return accountDetail;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
