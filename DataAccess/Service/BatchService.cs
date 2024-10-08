﻿using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class BatchService
    {
        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="endTime">The end time.</param>
        public void CreateAuction(int id, DateTime endTime)
        {
            Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {endTime}.");
            DateTime notificationTime = endTime.AddSeconds(15);
            TimeSpan delay = notificationTime - DateTime.Now;
            BackgroundJob.Schedule(() => NotifyAuctionComplete(id), delay);
        }
        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="date">The date.</param>
        /// <param name="account">The account.</param>
        public void CreateAuction(int id, DateTime endTime, DateTime date, string account)
        {
            Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {endTime}.");
            DateTime notificationTime = endTime.AddSeconds(15);
            TimeSpan delay = notificationTime - DateTime.Now;
            BackgroundJob.Schedule(() => NotifyAuctionComplete(id, date, account), delay);
        }
        /// <summary>
        /// Creates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="auctioneer">The auctioneer.</param>
        /// <param name="admin">The admin.</param>
        /// <param name="account">The account.</param>
        /// <param name="title">The title.</param>
        /// <param name="date">The date.</param>
        /// <param name="idadmin">The idadmin.</param>
        /// <param name="idauction">The idauction.</param>
        public void CreateAuction(int id, DateTime endTime, string auctioneer, string admin, string account, string title, DateTime date, string idadmin, string idauction)
        {
            Console.WriteLine($"{id} đã được tạo và sẽ kết thúc vào {endTime}.");
            DateTime notificationTime = endTime.AddSeconds(15);
            TimeSpan delay = notificationTime - DateTime.Now;
            BackgroundJob.Schedule(() => NotifyAuctionComplete(id, auctioneer, admin, account, title, date, idadmin, idauction), delay);
        }
        /// <summary>
        /// Notifies the auction complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctioneer">The auctioneer.</param>
        /// <param name="admin">The admin.</param>
        /// <param name="account">The account.</param>
        /// <param name="title">The title.</param>
        /// <param name="date">The date.</param>
        /// <param name="idadmin">The idadmin.</param>
        /// <param name="idauction">The idauction.</param>
        public async void NotifyAuctionComplete(int id, string auctioneer, string admin, string account, string title, DateTime date, string idadmin, string idauction)
        {
            var check = await RegistAuctionDAO.Instance.SecondCheckUsertoPayment(id);

            if (check != true)
            {
                var result = new SetTimeForBatch
                {
                    EmailAdmin = admin,
                    AuctioneerEmail = auctioneer,
                    BidderEmail = null,
                    endTime = date,
                    Price = 0
                };
                var notifications = new Notification
                {
                    AccountID = account,
                    Title = $"Cảnh báo không thanh toán: {title}",
                    Description = "Bạn đã không thanh toán đúng hẹn và bạn sẽ chịu phạt nếu đủ 3 lần tài khoản bạn sẽ bị khóa tài khoản"
                };
                await NotificationDAO.Instance.AddNotification(notifications);
                var adminNotification = new Notification
                {
                    AccountID = idadmin,
                    Title = $"Kết quả buổi đấu giá: {result.Title}",
                    Description = $"Người thứ 2 không thanh toán nên sản phẩm đấu giá thất bại"
                };
                await NotificationDAO.Instance.AddNotification(adminNotification);

                // Thông báo cho auctioneer
                var auctioneerNotification = new Notification
                {
                    AccountID = idauction,
                    Title = $"Kết quả buổi đấu giá: {result.Title}",
                    Description = $"Người thứ 2 không thanh toán nên sản phẩm đấu giá thất bại"
                };
                await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                // Gửi email thông báo đấu giá thất bại cho Auctioneer
                if (result.AuctioneerEmail != null)
                {
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.AuctioneerEmail,
                        subject: "Auction Results - Failure",
                        body: GenerateAuctioneerFailureEmailBody(result)
                    );
                }

                // Gửi email thông báo đấu giá thất bại cho Admin
                if (result.EmailAdmin != null)
                {
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.EmailAdmin,
                        subject: "Auction Results - Failure",
                        body: GenerateAdminFailureEmailBody(result)
                    );
                }
            }
        }
        /// <summary>
        /// Notifies the auction complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="account">The account.</param>
        public async void NotifyAuctionComplete(int id, DateTime date, string account)
        {
            var check = await RegistAuctionDAO.Instance.checkusertopayment(id);
            var result = new SetTimeForBatch
            {
                EmailAdmin = check.EmailAdmin,
                AuctioneerEmail = check.AuctioneerEmail,
                BidderEmail = check?.BidderEmail,
                endTime = date,
                Price = check.Price,
                AccountId = check?.AccountId,
                Title = check.Title,
                AccountAdminId = check?.AccountAdminId,
                AccountAuctionId = check?.AccountAuctionId
            };

            if (check.status != true)
            {

                // Kiểm tra BidderEmail
                if (result.BidderEmail != null)
                {
                    // Gửi email cho Bidder
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.BidderEmail,
                        subject: "Auction Results - Success",
                        body: GenerateSecondBidderEmailBody(result)
                    );

                    // Gửi email cho Auctioneer
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.AuctioneerEmail,
                        subject: "Auction Results - Success",
                        body: GenerateAuctioneerSecondBidderEmailBody(result)
                    );

                    // Gửi email cho Admin
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.EmailAdmin,
                        subject: "Auction Results - Success",
                        body: GenerateSecondAdminEmailBody(result)
                    );

                    // Thông báo cho người không thanh toán
                    var notifications = new Notification
                    {
                        AccountID = account,
                        Title = $"Cảnh báo không thanh toán: {result.Title}",
                        Description = "Bạn đã không thanh toán đúng hẹn và bạn sẽ chịu phạt nếu đủ 3 lần tài khoản bạn sẽ bị khóa."
                    };
                    await NotificationDAO.Instance.AddNotification(notifications);
                    var secondBidderNotification = new Notification
                    {
                        AccountID = result.AccountId,
                        Title = $"Bạn đã trúng đấu giá: {result.Title}",
                        Description = $"Người đầu tiên đã không thanh toán, bạn là người thắng cuộc với giá: {result.Price}."
                    };
                    await NotificationDAO.Instance.AddNotification(secondBidderNotification);
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người thắng cuộc thay đổi qua người đặt giá cao thứ 2 bởi vì người thứ nhất không thanh toán: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Người thắng cuộc thay đổi qua người đặt giá cao thứ 2 bởi vì người thứ nhất không thanh toán: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                    // Gửi email thông báo cho Auctioneer về người thắng cuộc
                    if (result.AuctioneerEmail != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.AuctioneerEmail,
                            subject: "Thông báo trúng đấu giá",
                            body: $"Chúc mừng! {result.BidderEmail} đã trúng đấu giá với giá: {result.Price}."
                        );
                    }

                    // Gửi email thông báo cho Admin về người thắng cuộc
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Thông báo trúng đấu giá",
                            body: $"Chúc mừng! {result.BidderEmail} đã trúng đấu giá với giá: {result.Price}."
                        );
                    }

                    CreateAuction(id, DateTime.Now.AddDays(2), result.AuctioneerEmail, result.EmailAdmin, result.AccountId, result.Title, date, result.AccountAdminId, result.AuctioneerEmail);
                }
                else
                {
                    // Gửi email thông báo đấu giá thất bại cho Auctioneer
                    if (result.AuctioneerEmail != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.AuctioneerEmail,
                            subject: "Auction Results - Failure",
                            body: GenerateAuctioneerFailureEmailBody(result)
                        );
                    }

                    // Gửi email thông báo đấu giá thất bại cho Admin
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Auction Results - Failure",
                            body: GenerateAdminFailureEmailBody(result)
                        );
                    }
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                }
            }
        }


        /// <summary>
        /// Notifies the auction complete.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public async void NotifyAuctionComplete(int id)
        {
            var result = await AuctionDAO.Instance.GetInforSendMail(id);

            if (result != null)
            {
                // Kiểm tra BidderEmail
                if (result.BidderEmail != null)
                {
                    // Gửi email cho Bidder
                    var bidderSuccessBody = new StringBuilder();
                    bidderSuccessBody.AppendLine("Chúc mừng! Bạn đã thắng cuộc đấu giá.");
                    bidderSuccessBody.AppendLine($"Giá đấu thành công: {result.Price}");
                    bidderSuccessBody.AppendLine("Yêu cầu thanh toán trong vòng 2 ngày. Nếu không thanh toán, bạn sẽ bị nhường lại cho người khác.");
                    bidderSuccessBody.AppendLine("Xin lưu ý: Nếu bạn không thanh toán quá 3 lần, tài khoản của bạn sẽ bị khóa.");

                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.BidderEmail,
                        subject: "Auction Results - Success",
                        body: bidderSuccessBody.ToString()
                    );

                    // Gửi email cho Auctioneer
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.AuctioneerEmail,
                        subject: "Auction Results - Success",
                        body: GenerateAuctioneerEmailBody(result)
                    );

                    // Gửi email cho Admin
                    await MailUtils.SendMailGoogleSmtp(
                        fromEmail: "nguyenanh0978638@gmail.com",
                        toEmail: result.EmailAdmin,
                        subject: "Auction Results - Success",
                        body: GenerateAdminEmailBody(result)
                    );

                    RegistAuctionDAO.Instance.UpdateInforPayment(id);
                    var RAID = await AuctionDAO.Instance.UpdateWinningStatus(id);

                    // Tạo thông báo cho người thua cuộc
                    var checkuser = await AuctionDAO.Instance.InformationOfTheLoser(RAID, id);
                    if (checkuser != null)
                    {
                        foreach (var item in checkuser)
                        {
                            var notifications = new Notification
                            {
                                AccountID = item,
                                Title = $"Kết quả buổi đấu giá: {result.Title}",
                                Description = "Xin chia buồn với bạn đã không đấu giá được sản phẩm với mức giá mong muốn."
                            };
                            await NotificationDAO.Instance.AddNotification(notifications);
                        }
                    }
                    else
                    {
                        var notificationForBidder = new Notification
                        {
                            AccountID = result.AccountId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = bidderSuccessBody.ToString()
                        };
                        await NotificationDAO.Instance.AddNotification(notificationForBidder);

                        // Thông báo cho admin
                        var adminNotification = new Notification
                        {
                            AccountID = result.AccountAdminId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}"
                        };
                        await NotificationDAO.Instance.AddNotification(adminNotification);

                        // Thông báo cho auctioneer
                        var auctioneerNotification = new Notification
                        {
                            AccountID = result.AccountAuctionId,
                            Title = $"Kết quả buổi đấu giá: {result.Title}",
                            Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n{bidderSuccessBody}"
                        };
                        await NotificationDAO.Instance.AddNotification(auctioneerNotification);

                        CreateAuction(RAID, DateTime.Now.AddDays(2), result.endTime, result.AccountId);
                    }
                }
                else
                {
                    // Nếu BidderEmail là null, gửi thông báo đấu giá thất bại cho Auctioneer và Admin
                    var failureMessage = "Buổi đấu giá đã thất bại do không có người thắng cuộc.";

                    // Gửi email cho Auctioneer
                    if (result.AuctioneerEmail != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.AuctioneerEmail,
                            subject: "Auction Results - Failure",
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAuctioneerEmailBody(result)}"
                        );
                    }

                    // Gửi email cho Admin
                    if (result.EmailAdmin != null)
                    {
                        await MailUtils.SendMailGoogleSmtp(
                            fromEmail: "nguyenanh0978638@gmail.com",
                            toEmail: result.EmailAdmin,
                            subject: "Auction Results - Failure",
                            body: $"{failureMessage} Thông tin chi tiết: {GenerateAdminEmailBody(result)}"
                        );
                    }
                    var adminNotification = new Notification
                    {
                        AccountID = result.AccountAuctionId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(adminNotification);

                    // Thông báo cho auctioneer
                    var auctioneerNotification = new Notification
                    {
                        AccountID = result.AccountAdminId,
                        Title = $"Kết quả buổi đấu giá: {result.Title}",
                        Description = $"Đấu giá thất bại bởi vì không có người tham gia"
                    };
                    await NotificationDAO.Instance.AddNotification(auctioneerNotification);
                }
            }
        }


        /// <summary>
        /// Generates the second bidder email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateSecondBidderEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Khách,");
            body.AppendLine();
            body.AppendLine("Người đấu giá đầu tiên không thực hiện thanh toán.");
            body.AppendLine("Vì vậy, bạn đã được chọn làm người thắng cuộc thay thế.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");
            body.AppendLine($"Giá đấu của bạn: {result.Price} VND");
            body.AppendLine("Vui lòng thanh toán trong vòng 5 ngày để hoàn tất giao dịch.");
            body.AppendLine("Nếu không thanh toán, cuộc đấu giá sẽ bị hủy.");
            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }
        /// <summary>
        /// Generates the auctioneer second bidder email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerSecondBidderEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Nhà Đấu Giá,");
            body.AppendLine();
            body.AppendLine("Người đấu giá đầu tiên không thực hiện thanh toán trong thời gian quy định.");
            body.AppendLine("Vì vậy, quyền đấu giá đã được chuyển cho người đấu giá thứ hai.");
            body.AppendLine($"Người đấu giá thứ hai đã được chọn với giá: {result.Price} VND.");
            body.AppendLine();
            body.AppendLine("Xin vui lòng theo dõi quá trình thanh toán.");
            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }

        /// <summary>
        /// Generates the second admin email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateSecondAdminEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Quản Trị,");
            body.AppendLine();
            body.AppendLine("Cuộc đấu giá đã chuyển quyền do người thắng cuộc đầu tiên không thanh toán.");
            body.AppendLine($"Người thứ hai đã được chọn làm người thắng cuộc với giá đấu là: {result.Price} VND");
            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }

        /// <summary>
        /// Generates the bidder email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateBidderEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Khách Đấu Giá,");
            body.AppendLine();

            // Thông báo về kết quả đấu giá
            body.AppendLine("Cuộc đấu giá của bạn đã được xử lý.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");

            if (result.BidderEmail != null)
            {
                body.AppendLine("Chúc mừng! Bạn đã thắng cuộc đấu giá.");
                body.AppendLine($"Giá đấu thành công: {result.Price}");
                body.AppendLine("Yêu cầu thanh toán trong vòng 2 ngày. Nếu không thanh toán, bạn sẽ bị nhường lại cho người khác.");
                body.AppendLine("Xin lưu ý: Nếu bạn không thanh toán quá 3 lần, tài khoản của bạn sẽ bị khóa.");
            }
            else
            {
                body.AppendLine("Rất tiếc! Bạn đã không thắng cuộc đấu giá này.");
            }

            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }

        /// <summary>
        /// Generates the admin email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAdminEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Quản Trị,");
            body.AppendLine();

            // Thông báo về cuộc đấu giá
            body.AppendLine("Cuộc đấu giá đã được xử lý.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");

            if (result.BidderEmail != null)
            {
                body.AppendLine("Cuộc đấu giá đã thành công.");
                body.AppendLine($"Người thắng cuộc: {result.BidderEmail}");
                body.AppendLine($"Giá đấu thành công: {result.Price}");
            }
            else
            {
                body.AppendLine("Cuộc đấu giá đã thất bại do không có người thắng cuộc.");
            }

            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }

        /// <summary>
        /// Generates the auctioneer email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerEmailBody(SetTimeForBatch result)
        {
            // Thông tin cơ bản
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Nhà Đấu Giá,");
            body.AppendLine();

            // Thông báo về cuộc đấu giá
            body.AppendLine("Cuộc đấu giá của bạn đã được xử lý.");
            body.AppendLine($"Thời gian kết thúc: {result.endTime}");

            if (result.BidderEmail != null)
            {
                body.AppendLine("Chúc mừng! Cuộc đấu giá đã thành công.");
                body.AppendLine($"Người thắng cuộc: {result.BidderEmail}");
                body.AppendLine($"Giá đấu thành công: {result.Price}");
            }
            else
            {
                body.AppendLine("Rất tiếc! Cuộc đấu giá đã thất bại do không có người thắng cuộc.");
            }

            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }
        /// <summary>
        /// Generates the auctioneer failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAuctioneerFailureEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Nhà Đấu Giá,");
            body.AppendLine();
            body.AppendLine("Rất tiếc! Buổi đấu giá đã không thành công do không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.");
            body.AppendLine("Thông tin đấu giá đã được hủy bỏ.");
            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine();
            body.AppendLine("Xin chân thành cảm ơn!");
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }
        /// <summary>
        /// Generates the admin failure email body.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        private string GenerateAdminFailureEmailBody(SetTimeForBatch result)
        {
            var body = new StringBuilder();

            body.AppendLine("Kính gửi Quý Quản Trị,");
            body.AppendLine();
            body.AppendLine("Buổi đấu giá đã thất bại vì không có người đấu giá tiếp theo sau khi người đầu tiên không thanh toán.");
            body.AppendLine("Thông tin đấu giá đã được hủy bỏ.");
            body.AppendLine();
            body.AppendLine("Xin vui lòng kiểm tra và xử lý nếu cần thiết.");
            body.AppendLine();
            body.AppendLine("Đội ngũ hỗ trợ.");

            return body.ToString();
        }


    }
}
