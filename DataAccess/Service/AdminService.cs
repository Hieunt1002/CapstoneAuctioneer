using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository) 
        {
            _adminRepository= adminRepository;
        }
        public async Task<ResponseDTO> ListCategoryAsync()
        {
            var result = await _adminRepository.ListCategory();
            return result;
        }
        public async Task<ResponseDTO> AddCategory(string name)
        {
            var result = await _adminRepository.AddCategory(name);
            return result;
        }
        public async Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction)
        {
            var result = await _adminRepository.AcceptAuctioneerForAdmin(autioneer, idAuction);
            return result;
        }
        public async Task<ResponseDTO> UpdateCategoryAsync(int id, string Namecategory)
        {
            var result = await _adminRepository.UpdateCategory(id, Namecategory);
            return result;
        }
        public async Task<ResponseDTO> DeleteCategoryAsync(int id)
        {
            var result = await _adminRepository.DeleteCategory(id);
            return result;
        }
        public async Task<List<AuctionnerAdminDTO>> ListAuction(string id, int status)
        {
            var result = await _adminRepository.ListAuction(id, status);
            return result;
        }
    }
}
