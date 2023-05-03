using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JS.Abp.DataPermission.Migrations
{
    /// <inheritdoc />
    public partial class addpermissiondesccolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AbpPermissionExtensions",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AbpPermissionExtensions");
        }
    }
}
