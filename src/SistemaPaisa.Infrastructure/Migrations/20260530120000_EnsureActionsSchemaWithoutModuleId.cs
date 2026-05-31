using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations;

/// <inheritdoc />
public partial class EnsureActionsSchemaWithoutModuleId : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            IF COL_LENGTH(N'Actions', N'ModuleId') IS NOT NULL
            BEGIN
                IF OBJECT_ID(N'[rel_Actions_Modules]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [rel_Actions_Modules] (
                        [id_rel_Actions_Modules] INT IDENTITY(1,1) NOT NULL,
                        [id_Actions]             INT NOT NULL,
                        [id_Modules]             INT NOT NULL,
                        [estado]                 VARCHAR(50) NOT NULL CONSTRAINT [DF_rel_Actions_Modules_estado_fix] DEFAULT ('ACTIVO'),
                        [id_usuario_creador]     INT NOT NULL,
                        [fecha_creacion]         DATETIME NOT NULL CONSTRAINT [DF_rel_Actions_Modules_fecha_fix] DEFAULT (GETDATE()),
                        [id_usuario_modifica]    INT NULL,
                        [fecha_modificacion]     DATETIME NULL,
                        CONSTRAINT [PK_rel_Actions_Modules_fix] PRIMARY KEY ([id_rel_Actions_Modules]),
                        CONSTRAINT [FK_rel_Actions_Modules_Actions_fix]
                            FOREIGN KEY ([id_Actions]) REFERENCES [Actions]([Id]) ON DELETE NO ACTION,
                        CONSTRAINT [FK_rel_Actions_Modules_Modules_fix]
                            FOREIGN KEY ([id_Modules]) REFERENCES [Modules]([Id]) ON DELETE NO ACTION
                    );

                    CREATE UNIQUE INDEX [IX_rel_Actions_Modules_id_Actions_id_Modules_fix]
                        ON [rel_Actions_Modules]([id_Actions], [id_Modules]);
                END

                INSERT INTO [rel_Actions_Modules] (
                    [id_Actions], [id_Modules], [estado], [id_usuario_creador], [fecha_creacion])
                SELECT
                    a.[Id],
                    a.[ModuleId],
                    CASE WHEN a.[IsActive] = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END,
                    1,
                    COALESCE(a.[CreatedAt], GETDATE())
                FROM [Actions] a
                WHERE a.[ModuleId] IS NOT NULL
                  AND NOT EXISTS (
                      SELECT 1
                      FROM [rel_Actions_Modules] ram
                      WHERE ram.[id_Actions] = a.[Id]
                        AND ram.[id_Modules] = a.[ModuleId]);

                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Actions_Modules_ModuleId')
                    ALTER TABLE [Actions] DROP CONSTRAINT [FK_Actions_Modules_ModuleId];

                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Actions_ModuleId_Code' AND object_id = OBJECT_ID(N'Actions'))
                    DROP INDEX [IX_Actions_ModuleId_Code] ON [Actions];

                ALTER TABLE [Actions] DROP COLUMN [ModuleId];
            END
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Schema alignment is not reversed automatically.
    }
}
