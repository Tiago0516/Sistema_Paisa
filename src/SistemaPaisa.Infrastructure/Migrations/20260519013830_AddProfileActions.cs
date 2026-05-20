using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileActions",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    ActionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileActions", x => new { x.ProfileId, x.ActionId });
                    table.ForeignKey(
                        name: "FK_ProfileActions_Actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "Actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileActions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileActions_ActionId",
                table: "ProfileActions",
                column: "ActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileActions");
        }
    }
}
