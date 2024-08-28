using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAdminRepository
    {
        Task<List<AuctionnerAdminDTO>> ListAuction(string accountID, int status);
        Task<ResponseDTO> AddCategory(string name);
        Task<ResponseDTO> ListCategory();
        Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction);
        Task<ResponseDTO> UpdateCategory(int id, string Namecategory);
        Task<ResponseDTO> DeleteCategory(int id);
    }
}
