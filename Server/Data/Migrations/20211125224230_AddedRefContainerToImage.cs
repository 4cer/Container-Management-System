using Microsoft.EntityFrameworkCore.Migrations;

namespace ProITM.Server.Data.Migrations
{
    public partial class AddedRefContainerToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Containers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ImageId",
                table: "Containers",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Images_ImageId",
                table: "Containers",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Images_ImageId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_ImageId",
                table: "Containers");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Containers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
