using Microsoft.EntityFrameworkCore.Migrations;

namespace ProITM.Server.Migrations
{
    public partial class AdminSeedQM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "49cea4da-b5fd-4d88-971e-00e9ac7ec7af", "5d017e37-0c3f-46eb-b2bb-5283482c2f33", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49cea4da-b5fd-4d88-971e-00e9ac7ec7af");
        }
    }
}
