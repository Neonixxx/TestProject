using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.UserGroupId);
                });

            migrationBuilder.CreateTable(
                name: "UserState",
                columns: table => new
                {
                    UserStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserState", x => x.UserStateId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UserGroupId = table.Column<int>(nullable: false),
                    UserStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "UserGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_UserState_UserStateId",
                        column: x => x.UserStateId,
                        principalTable: "UserState",
                        principalColumn: "UserStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Login",
                table: "User",
                column: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserGroupId",
                table: "User",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserStateId",
                table: "User",
                column: "UserStateId");
            
            AddInitialData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserState");
        }

        private void AddInitialData(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "UserGroup",
                new string[] { "UserGroupId", "Code" },
                new object[] { 1, 0 });
            
            migrationBuilder.InsertData(
                "UserGroup",
                new string[] { "UserGroupId", "Code" },
                new object[] { 2, 1 });
            
            migrationBuilder.InsertData(
                "UserState",
                new string[] { "UserStateId", "Code" },
                new object[] { 1, 0 });
            
            migrationBuilder.InsertData(
                "UserState",
                new string[] { "UserStateId", "Code" },
                new object[] { 2, 1 });
            
            migrationBuilder.InsertData(
                "User",
                new string[] { "UserId", "Login", "Password", "CreatedDate", "UserGroupId", "UserStateId" },
                new object[] { 1, "admin", "AQAAAAEAACcQAAAAEGKg8qDUQKSgMN0TX/TOMeuN27O6T0p4dYw9/EjRlvMJ6xIbxj7OXmAEcD4rE3CS+Q==", DateTime.UtcNow, 2, 2});
        }
    }
}
