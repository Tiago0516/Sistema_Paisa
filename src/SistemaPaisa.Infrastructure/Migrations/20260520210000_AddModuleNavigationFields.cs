using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddModuleNavigationFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Icon",
            table: "Modules",
            type: "nvarchar(60)",
            maxLength: 60,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ControllerName",
            table: "Modules",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CreateActionName",
            table: "Modules",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsLanding",
            table: "Modules",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Icon",             table: "Modules");
        migrationBuilder.DropColumn(name: "ControllerName",   table: "Modules");
        migrationBuilder.DropColumn(name: "CreateActionName", table: "Modules");
        migrationBuilder.DropColumn(name: "IsLanding",        table: "Modules");
    }
}
