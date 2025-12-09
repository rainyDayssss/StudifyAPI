using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudifyAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addednumberoffriendsattributeorcolumnforuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfFriends",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfFriends",
                table: "Users");
        }
    }
}
