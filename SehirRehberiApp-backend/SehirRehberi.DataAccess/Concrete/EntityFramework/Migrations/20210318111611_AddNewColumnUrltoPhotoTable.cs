using Microsoft.EntityFrameworkCore.Migrations;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Migrations
{
    public partial class AddNewColumnUrltoPhotoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Photos");
        }
    }
}
