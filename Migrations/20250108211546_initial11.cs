using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcFilm.Migrations
{
    /// <inheritdoc />
    public partial class initial11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_UserAccount_UserId",
                table: "Favorites");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "Favorites",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserAccountId",
                table: "Favorites",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_UserAccount_UserAccountId",
                table: "Favorites",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_UserAccount_UserAccountId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserAccountId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Favorites");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_UserAccount_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
