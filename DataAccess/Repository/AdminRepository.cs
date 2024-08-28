using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public async Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction)
        {
            try
            {
                await AuctionDAO.Instance.AcceptAuctioneerForAdmin(autioneer, idAuction);
                return new ResponseDTO { IsSucceed = true, Message = "successfully" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
        }

        public async Task<ResponseDTO> AddCategory(string name)
        {
            try
            {
                var category = new Category
                {
                    NameCategory = name
                };
                var result = await CategoryDAO.Instance.AddCategory(category);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Add Category successfully" };
                }
                return new ResponseDTO { IsSucceed = false, Message = "Add Category failed" };
            }
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Add Category failed" };
            }
        }

        public async Task<ResponseDTO> DeleteCategory(int id)
        {
            try
            {
                var result = await CategoryDAO.Instance.DeleteCategory(id);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Delete Category successfully" };
                }
                return new ResponseDTO { IsSucceed = false, Message = "Delete Category failed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Delete Category failed" };
            }
        }

        public async Task<List<AuctionnerAdminDTO>> ListAuction(string accountID, int status)
        {
            var auctioneerList = new List<AuctionnerAdminDTO>();
            var result = await AuctionDAO.Instance.ListYourAuctioneerAdmin(accountID, status);
            foreach (var item in result)
            {
                // Determine if we should use Start or End day/time
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                    TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Parse both the date and time together
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var startDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new AuctionnerAdminDTO(); // Create a new object in each iteration

                    string formattedTimeRemaining = "";
                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }

                    auctioneer.AuctioneerID = item.ListAuctioneerID;
                    auctioneer.Category = item.Category;
                    auctioneer.Name = item.Name;
                    auctioneer.Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Image}";
                    auctioneer.NameAuctioneer = item.NameAuctioneer;
                    auctioneer.StartingPrice = item.StartingPrice;
                    auctioneer.StartDay = item.StartDay;
                    auctioneer.StartTime = item.StartTime;
                    auctioneer.EndDay = item.EndDay;
                    auctioneer.EndTime = item.EndTime;
                    auctioneer.StatusAuction = item.StatusAuction;
                    auctioneer.Time = formattedTimeRemaining;

                    auctioneerList.Add(auctioneer); // Add to the list
                }
            }
            return auctioneerList;
        }
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days * 24 + timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        public async Task<ResponseDTO> ListCategory()
        {
            try
            {
                var result = await CategoryDAO.Instance.CategoryAsync();
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "List category successfully" };
            }
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "List category failed" };
            }
        }

        public async Task<ResponseDTO> UpdateCategory(int id, string Namecategory)
        {
            try
            {
                var category = new Category()
                {
                    CategoryID = id,
                    NameCategory = Namecategory
                };
                var result = await CategoryDAO.Instance.UpdateCategory(category);
                if (result)
                {
                    return new ResponseDTO {IsSucceed = true, Message = "Update category successfully" };
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Update category failed" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Update category failed" };
            }
        }
    }
}
