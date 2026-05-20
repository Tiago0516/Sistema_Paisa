using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddProfileModulesAndRenameActionsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "Actions",
            newName: "rel_Actions_Modules");

        migrationBuilder.RenameIndex(
            name: "IX_Actions_ModuleId_Code",
            table: "rel_Actions_Modules",
            newName: "IX_rel_Actions_Modules_ModuleId_Code");

        migrationBuilder.CreateTable(
            name: "rel_Modules_Profiles",
            columns: table => new
            {
                ProfileId = table.Column<int>(type: "int", nullable: false),
                ModuleId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_rel_Modules_Profiles", x => new { x.ProfileId, x.ModuleId });
                table.ForeignKey(
                    name: "FK_rel_Modules_Profiles_Modules_ModuleId",
                    column: x => x.ModuleId,
                    principalTable: "Modules",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_rel_Modules_Profiles_Profiles_ProfileId",
                    column: x => x.ProfileId,
                    principalTable: "Profiles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_rel_Modules_Profiles_ModuleId",
            table: "rel_Modules_Profiles",
            column: "ModuleId");

        migrationBuilder.Sql("""
            INSERT INTO [rel_Modules_Profiles] ([ProfileId], [ModuleId])
            SELECT [Id], [ModuleId]
            FROM [Profiles]
            WHERE [ModuleId] > 0
              AND NOT EXISTS (
                  SELECT 1
                  FROM [rel_Modules_Profiles] rp
                  WHERE rp.[ProfileId] = [Profiles].[Id]
                    AND rp.[ModuleId] = [Profiles].[ModuleId]
              );
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "rel_Modules_Profiles");

        migrationBuilder.RenameTable(
            name: "rel_Actions_Modules",
            newName: "Actions");

        migrationBuilder.RenameIndex(
            name: "IX_rel_Actions_Modules_ModuleId_Code",
            table: "Actions",
            newName: "IX_Actions_ModuleId_Code");
    }
}
