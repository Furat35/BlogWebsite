using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YoutubeBlog.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ArticleComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleComments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ArticleComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8da9a416-cda8-4d0d-9087-6ea08174e2e2"), "09374461-7c70-498c-ae35-d373b29680a5", false, "User", "USER" },
                    { new Guid("c35e880e-17c4-4726-a20e-ff817fbb16ae"), "800bc536-6515-4886-b2d9-b811adbba554", false, "Superadmin", "SUPERADMIN" },
                    { new Guid("c6992ca2-86d3-40be-a85d-257fb72bbbeb"), "9294b10f-e4a2-4f0e-b54d-1d8dbf19ce3b", false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "ModifiedBy", "ModifiedDate", "Name", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("13349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2287), null, null, null, null, "SQL", false },
                    { new Guid("21349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2283), null, null, null, null, "C#", false },
                    { new Guid("53349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2278), null, null, null, null, "Python", false },
                    { new Guid("58349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2291), null, null, null, null, "Javascript", false },
                    { new Guid("66349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2269), null, null, null, null, "Visual Studio", false },
                    { new Guid("98349229-d8aa-4430-8f3a-2bec4b6816ae"), "Firat Ortac", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2265), null, null, null, null, "Asp.net Core", false }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "FileName", "FileType", "ModifiedBy", "ModifiedDate", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("242a5458-7d57-4dd6-abea-c5e99e626e87"), "SuperAdmin", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2560), null, null, "~/images/user-images/FiratOrtac_2439664.png", "image/png", null, null, false },
                    { new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), "SuperAdmin", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2568), null, null, "~/images/article-images/emptyImage.jpg", "image/jpg", null, null, false },
                    { new Guid("44349229-d8aa-4430-8f3a-2bec4b6816ae"), "Admin Test", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2555), null, null, "~/images/testimage", "jpg", null, null, false },
                    { new Guid("54349229-d8aa-4430-8f3a-2bec4b6816ae"), "Admin Test", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2544), null, null, "~/images/testimage", "jpg", null, null, false },
                    { new Guid("77349229-d8aa-4430-8f3a-2bec4b6816ae"), "SuperAdmin", new DateTime(2023, 5, 8, 23, 54, 13, 811, DateTimeKind.Local).AddTicks(2551), null, null, "~/images/testimage", "jpg", null, null, false }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageId", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("20461ba2-1457-4303-aef9-15173ddbb9b5"), 0, "e18ad048-2042-4a8e-9504-0a727f14bb38", "superadmin@gmail.com", true, "Firat", new Guid("242a5458-7d57-4dd6-abea-c5e99e626e87"), false, "Ortac", false, null, "SUPERADMIN@GMAIL.COM", "SUPERADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEPsG+tWA3QSfZDOeLLsmwCLMn9zcfMKxvuSh8r9UNjfLSAVNHUnNcr/22pvidvcrew==", "+905375655978", true, "eb05c3c3-d84f-49d5-a184-6b47b40e6e54", false, "superadmin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageId", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("84461ba2-1457-4303-aef9-15173ddbb9b5"), 0, "0f595b0d-0e2a-450d-bd72-62b9d04b1fb2", "admin@gmail.com", true, "Ahmet", new Guid("242a5458-7d57-4dd6-abea-c5e99e626e87"), false, "Demir", false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEE6KZHBfExpStSyRYBullynua+wZSL33pTySwoIJYoxaIxeoo6PuyfTAuCyiYvJQyw==", "+90537543546", true, "4d658389-c36e-4b61-9212-014ba120ae3c", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "ImageId", "IsDeleted", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5"), 0, "39ec69af-e6ff-472d-b8f6-b9088933e853", "adminDeneme@gmail.com", true, "Ayşe", new Guid("242a5458-7d57-4dd6-abea-c5e99e626e87"), false, "Ulu", false, null, "ADMINDENEME@GMAIL.COM", "ADMINDENEME@GMAIL.COM", "AQAAAAEAACcQAAAAEPqGU5UGVy4dEZuJvjnHsJoP/NRepDXot+ccxnpxiShz+41x461khMnGq2C++SPgGg==", "+905375534555", true, "fce40b53-7c71-40e1-8de3-23ad01035f99", false, "adminDeneme@gmail.com" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "ImageId", "ModifiedBy", "ModifiedDate", "Title", "UserId", "ViewCount", "isDeleted" },
                values: new object[,]
                {
                    { new Guid("081f033b-13da-4ae4-8540-fd5a869052cd"), new Guid("58349229-d8aa-4430-8f3a-2bec4b6816ae"), "Javascript Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Javascript Nedir?", new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("2616566b-84c7-4f29-8b25-b16395e4ddcc"), new Guid("98349229-d8aa-4430-8f3a-2bec4b6816ae"), "C# Hakkinda Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "C# Hakkinda", new Guid("20461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("2d06b3f6-3f91-4480-aea8-400a8aecb772"), new Guid("21349229-d8aa-4430-8f3a-2bec4b6816ae"), ".Net'de Api Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, ".Net'de Api Kullanımı", new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("6a9f89ff-7573-435a-a5b8-681b1304e28c"), new Guid("13349229-d8aa-4430-8f3a-2bec4b6816ae"), "Veritabanında Procedure Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Veritabanında Procedure Kullanımı", new Guid("20461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("8e9815be-a6bf-45ea-b424-5e6484b50367"), new Guid("98349229-d8aa-4430-8f3a-2bec4b6816ae"), ".Net Developer Olmak Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, ".Net Developer Olmak", new Guid("84461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("bd0e0b24-18cf-48f5-a7b3-16b0d758c9f6"), new Guid("58349229-d8aa-4430-8f3a-2bec4b6816ae"), "Web Geliştiricilerin Bilmesi Gerekenler Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Web Geliştiricilerin Bilmesi Gerekenler", new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("d12e3d5a-4605-4b15-a701-825b81cd32a1"), new Guid("98349229-d8aa-4430-8f3a-2bec4b6816ae"), "Identity Framework Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Identity Framework Kullanımı", new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("db5a9535-7d8f-4176-96e9-c30592811cf8"), new Guid("66349229-d8aa-4430-8f3a-2bec4b6816ae"), "Visual studio, Blog orem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Firat Ortac", new DateTime(2023, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Visual Studio'ya Giriş", new Guid("20461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false },
                    { new Guid("e0a5e1d6-53da-4d43-b96f-63bb5f787475"), new Guid("53349229-d8aa-4430-8f3a-2bec4b6816ae"), "Python Fonksiyon Yazımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı", "Admin test", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, new Guid("342a5458-7d57-4dd6-abea-c5e99e626e87"), null, null, "Python Fonksiyon Yazımı", new Guid("84461ba2-1457-4303-aef9-15173ddbb9b5"), 0, false }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("c35e880e-17c4-4726-a20e-ff817fbb16ae"), new Guid("20461ba2-1457-4303-aef9-15173ddbb9b5") },
                    { new Guid("c6992ca2-86d3-40be-a85d-257fb72bbbeb"), new Guid("84461ba2-1457-4303-aef9-15173ddbb9b5") },
                    { new Guid("c6992ca2-86d3-40be-a85d-257fb72bbbeb"), new Guid("99461ba2-1457-4303-aef9-15173ddbb9b5") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_ArticleId",
                table: "ArticleComments",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleComments_CommentId",
                table: "ArticleComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ImageId",
                table: "Articles",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserId",
                table: "Articles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageId",
                table: "AspNetUsers",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleComments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
