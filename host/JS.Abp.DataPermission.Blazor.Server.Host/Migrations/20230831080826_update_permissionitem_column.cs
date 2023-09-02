using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JS.Abp.DataPermission.Blazor.Server.Host.Migrations
{
    /// <inheritdoc />
    public partial class updatepermissionitemcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpPermissionItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "AbpPermissionItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpPermissionItems");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AbpPermissionItems");
        }
    }
}
