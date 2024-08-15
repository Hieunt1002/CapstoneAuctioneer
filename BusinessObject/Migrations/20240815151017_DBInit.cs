using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class DBInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameCategory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Warning = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FrontCCCD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BacksideCCCD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_AccountDetails_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListAuctioneer",
                columns: table => new
                {
                    ListAuctioneerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<int>(type: "int", nullable: true),
                    Manager = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAuctioneer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatusAuction = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListAuctioneer", x => x.ListAuctioneerID);
                    table.ForeignKey(
                        name: "FK_ListAuctioneer_Account_Creator",
                        column: x => x.Creator,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListAuctioneer_Account_Manager",
                        column: x => x.Manager,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notications",
                columns: table => new
                {
                    NoticationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notications", x => x.NoticationID);
                    table.ForeignKey(
                        name: "FK_Notications_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctioneerDetail",
                columns: table => new
                {
                    ListAuctioneerID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    StartDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberofAuctionRounds = table.Column<int>(type: "int", nullable: false),
                    TimePerLap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceStep = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctioneerDetail", x => x.ListAuctioneerID);
                    table.ForeignKey(
                        name: "FK_AuctioneerDetail_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctioneerDetail_ListAuctioneer_ListAuctioneerID",
                        column: x => x.ListAuctioneerID,
                        principalTable: "ListAuctioneer",
                        principalColumn: "ListAuctioneerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistAuctioneer",
                columns: table => new
                {
                    RAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    ListAuctioneerID = table.Column<int>(type: "int", nullable: false),
                    PaymentTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuctionStatus = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistAuctioneer", x => x.RAID);
                    table.ForeignKey(
                        name: "FK_RegistAuctioneer_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistAuctioneer_ListAuctioneer_ListAuctioneerID",
                        column: x => x.ListAuctioneerID,
                        principalTable: "ListAuctioneer",
                        principalColumn: "ListAuctioneerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachments",
                columns: table => new
                {
                    FileAID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListAuctioneerID = table.Column<int>(type: "int", nullable: false),
                    FileAuctioneer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachments", x => x.FileAID);
                    table.ForeignKey(
                        name: "FK_FileAttachments_AuctioneerDetail_ListAuctioneerID",
                        column: x => x.ListAuctioneerID,
                        principalTable: "AuctioneerDetail",
                        principalColumn: "ListAuctioneerID");
                });

            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    BetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAID = table.Column<int>(type: "int", nullable: false),
                    PriceBit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.BetID);
                    table.ForeignKey(
                        name: "FK_Bet_RegistAuctioneer_RAID",
                        column: x => x.RAID,
                        principalTable: "RegistAuctioneer",
                        principalColumn: "RAID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    RAID = table.Column<int>(type: "int", nullable: false),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SatisfactionLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.RAID);
                    table.ForeignKey(
                        name: "FK_Feedback_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedback_RegistAuctioneer_RAID",
                        column: x => x.RAID,
                        principalTable: "RegistAuctioneer",
                        principalColumn: "RAID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAID = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PayID);
                    table.ForeignKey(
                        name: "FK_Payment_RegistAuctioneer_RAID",
                        column: x => x.RAID,
                        principalTable: "RegistAuctioneer",
                        principalColumn: "RAID");
                });

            migrationBuilder.CreateTable(
                name: "TImage",
                columns: table => new
                {
                    TImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileAID = table.Column<int>(type: "int", nullable: false),
                    Imange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TImage", x => x.TImageId);
                    table.ForeignKey(
                        name: "FK_TImage_FileAttachments_FileAID",
                        column: x => x.FileAID,
                        principalTable: "FileAttachments",
                        principalColumn: "FileAID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_RoleId",
                table: "Account",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctioneerDetail_CategoryID",
                table: "AuctioneerDetail",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_RAID",
                table: "Bet",
                column: "RAID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_AccountID",
                table: "Feedback",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_ListAuctioneerID",
                table: "FileAttachments",
                column: "ListAuctioneerID");

            migrationBuilder.CreateIndex(
                name: "IX_ListAuctioneer_Creator",
                table: "ListAuctioneer",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_ListAuctioneer_Manager",
                table: "ListAuctioneer",
                column: "Manager");

            migrationBuilder.CreateIndex(
                name: "IX_Notications_AccountID",
                table: "Notications",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RAID",
                table: "Payment",
                column: "RAID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistAuctioneer_AccountID",
                table: "RegistAuctioneer",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_RegistAuctioneer_ListAuctioneerID",
                table: "RegistAuctioneer",
                column: "ListAuctioneerID");

            migrationBuilder.CreateIndex(
                name: "IX_TImage_FileAID",
                table: "TImage",
                column: "FileAID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails");

            migrationBuilder.DropTable(
                name: "Bet");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Notications");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "TImage");

            migrationBuilder.DropTable(
                name: "RegistAuctioneer");

            migrationBuilder.DropTable(
                name: "FileAttachments");

            migrationBuilder.DropTable(
                name: "AuctioneerDetail");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "ListAuctioneer");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
