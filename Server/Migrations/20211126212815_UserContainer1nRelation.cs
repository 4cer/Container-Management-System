using Microsoft.EntityFrameworkCore.Migrations;

namespace ProITM.Server.Migrations
{
    public partial class UserContainer1nRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Containers_ContainersId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ContainersId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49cea4da-b5fd-4d88-971e-00e9ac7ec7af");

            migrationBuilder.DropColumn(
                name: "ContainersId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Containers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d6aff19e-6f24-4b30-9b56-818af0b73d6c", "b597a03e-cc18-47df-b987-b2338e41d729", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ApplicationUserId",
                table: "Containers",
                column: "ApplicationUserId");

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

            migrationBuilder.DropIndex(
                name: "IX_Containers_ApplicationUserId",
                table: "Containers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6aff19e-6f24-4b30-9b56-818af0b73d6c");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Containers");

            migrationBuilder.AddColumn<string>(
                name: "ContainersId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "49cea4da-b5fd-4d88-971e-00e9ac7ec7af", "5d017e37-0c3f-46eb-b2bb-5283482c2f33", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ContainersId",
                table: "AspNetUsers",
                column: "ContainersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Containers_ContainersId",
                table: "AspNetUsers",
                column: "ContainersId",
                principalTable: "Containers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
