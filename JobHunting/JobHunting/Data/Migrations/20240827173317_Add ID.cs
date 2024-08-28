using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHunting.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NationalID",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalID",
                table: "AspNetUsers");
        }
    }
}
