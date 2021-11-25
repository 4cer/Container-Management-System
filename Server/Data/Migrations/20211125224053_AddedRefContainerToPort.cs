using Microsoft.EntityFrameworkCore.Migrations;

namespace ProITM.Server.Data.Migrations
{
    public partial class AddedRefContainerToPort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PortId",
                table: "Containers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Containers_PortId",
                table: "Containers",
                column: "PortId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_ContainerPorts_PortId",
                table: "Containers",
                column: "PortId",
                principalTable: "ContainerPorts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_ContainerPorts_PortId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_PortId",
                table: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "PortId",
                table: "Containers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
