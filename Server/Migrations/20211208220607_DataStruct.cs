using Microsoft.EntityFrameworkCore.Migrations;

namespace ProITM.Server.Migrations
{
    public partial class DataStruct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_AspNetUsers_ApplicationUserId1",
                table: "Containers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76a5106f-37b3-4884-a193-ed63b8737697");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId1",
                table: "Containers",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Containers_ApplicationUserId1",
                table: "Containers",
                newName: "IX_Containers_ApplicationUserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "528e9446-5f7b-4cb3-8168-7f071c086b09", "1f505ca0-58cb-47d8-bd42-07147268ab86", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_AspNetUsers_ApplicationUserId",
                table: "Containers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_AspNetUsers_ApplicationUserId",
                table: "Containers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "528e9446-5f7b-4cb3-8168-7f071c086b09");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Containers",
                newName: "ApplicationUserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Containers_ApplicationUserId",
                table: "Containers",
                newName: "IX_Containers_ApplicationUserId1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "76a5106f-37b3-4884-a193-ed63b8737697", "8cc0c87c-72d0-41bf-9b51-fdeb9157b62a", "Admin", "ADMIN" });

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_AspNetUsers_ApplicationUserId1",
                table: "Containers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
