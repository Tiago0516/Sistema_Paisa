using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations;

/// <inheritdoc />
public partial class SyncModelSnapshot : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_rel_Actions_Modules_id_Modules",
            table: "rel_Actions_Modules",
            column: "id_Modules");

        migrationBuilder.CreateIndex(
            name: "IX_rel_Profiles_Roles_id_Profiles",
            table: "rel_Profiles_Roles",
            column: "id_Profiles");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_rel_Actions_Modules_id_Modules",
            table: "rel_Actions_Modules");

        migrationBuilder.DropIndex(
            name: "IX_rel_Profiles_Roles_id_Profiles",
            table: "rel_Profiles_Roles");
    }
}
