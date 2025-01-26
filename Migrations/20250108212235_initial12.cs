using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcFilm.Migrations
{
    /// <inheritdoc />
    public partial class initial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UserAccount_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_UserAccount_UserAccountId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "UserAccount");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserAccountId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Favorites");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserAccountId",
                table: "Favorites",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Email",
                table: "UserAccount",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_UserName",
                table: "UserAccount",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UserAccount_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_UserAccount_UserAccountId",
                table: "Favorites",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id");
        }
    }
}
