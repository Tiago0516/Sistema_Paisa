using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPaisa.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AlignRelationTablesSpanishSchema : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            IF OBJECT_ID(N'[rel_Actions_Modules]', N'U') IS NOT NULL
               AND COL_LENGTH(N'rel_Actions_Modules', N'Name') IS NOT NULL
            BEGIN
                EXEC sp_rename N'rel_Actions_Modules', N'Actions';
            END
            """);

        migrationBuilder.Sql("""
            IF OBJECT_ID(N'[rel_Actions_Modules]', N'U') IS NULL
            BEGIN
                CREATE TABLE [rel_Actions_Modules] (
                    [id_rel_Actions_Modules] INT IDENTITY(1,1) NOT NULL,
                    [id_Actions]             INT NOT NULL,
                    [id_Modules]             INT NOT NULL,
                    [estado]                 VARCHAR(50) NOT NULL CONSTRAINT [DF_rel_Actions_Modules_estado] DEFAULT ('ACTIVO'),
                    [id_usuario_creador]     INT NOT NULL,
                    [fecha_creacion]         DATETIME NOT NULL CONSTRAINT [DF_rel_Actions_Modules_fecha_creacion] DEFAULT (GETDATE()),
                    [id_usuario_modifica]    INT NULL,
                    [fecha_modificacion]     DATETIME NULL,
                    CONSTRAINT [PK_rel_Actions_Modules] PRIMARY KEY ([id_rel_Actions_Modules]),
                    CONSTRAINT [FK_rel_Actions_Modules_Actions_id_Actions]
                        FOREIGN KEY ([id_Actions]) REFERENCES [Actions]([Id]) ON DELETE NO ACTION,
                    CONSTRAINT [FK_rel_Actions_Modules_Modules_id_Modules]
                        FOREIGN KEY ([id_Modules]) REFERENCES [Modules]([Id]) ON DELETE NO ACTION
                );

                CREATE UNIQUE INDEX [IX_rel_Actions_Modules_id_Actions_id_Modules]
                    ON [rel_Actions_Modules]([id_Actions], [id_Modules]);

                IF COL_LENGTH(N'Actions', N'ModuleId') IS NOT NULL
                BEGIN
                    INSERT INTO [rel_Actions_Modules] (
                        [id_Actions], [id_Modules], [estado], [id_usuario_creador], [fecha_creacion])
                    SELECT
                        [Id],
                        [ModuleId],
                        CASE WHEN [IsActive] = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END,
                        1,
                        COALESCE([CreatedAt], GETDATE())
                    FROM [Actions]
                    WHERE [ModuleId] IS NOT NULL;

                    ALTER TABLE [Actions] DROP CONSTRAINT [FK_Actions_Modules_ModuleId];
                    DROP INDEX [IX_Actions_ModuleId_Code] ON [Actions];
                    ALTER TABLE [Actions] DROP COLUMN [ModuleId];
                END
            END
            """);

        migrationBuilder.Sql("""
            IF OBJECT_ID(N'[rel_Modules_Profiles]', N'U') IS NOT NULL
               AND COL_LENGTH(N'rel_Modules_Profiles', N'id_rel_Modules_Profiles') IS NULL
            BEGIN
                CREATE TABLE [rel_Modules_Profiles_new] (
                    [id_rel_Modules_Profiles] INT IDENTITY(1,1) NOT NULL,
                    [id_Modules]              INT NOT NULL,
                    [id_Profiles]             INT NOT NULL,
                    [estado]                  VARCHAR(50) NOT NULL CONSTRAINT [DF_rel_Modules_Profiles_estado] DEFAULT ('ACTIVO'),
                    [id_usuario_creador]      INT NOT NULL,
                    [fecha_creacion]          DATETIME NOT NULL CONSTRAINT [DF_rel_Modules_Profiles_fecha_creacion] DEFAULT (GETDATE()),
                    [id_usuario_modifica]     INT NULL,
                    [fecha_modificacion]      DATETIME NULL,
                    CONSTRAINT [PK_rel_Modules_Profiles_new] PRIMARY KEY ([id_rel_Modules_Profiles]),
                    CONSTRAINT [FK_rel_Modules_Profiles_new_Modules]
                        FOREIGN KEY ([id_Modules]) REFERENCES [Modules]([Id]) ON DELETE NO ACTION,
                    CONSTRAINT [FK_rel_Modules_Profiles_new_Profiles]
                        FOREIGN KEY ([id_Profiles]) REFERENCES [Profiles]([Id]) ON DELETE CASCADE
                );

                INSERT INTO [rel_Modules_Profiles_new] (
                    [id_Modules], [id_Profiles], [estado], [id_usuario_creador], [fecha_creacion])
                SELECT [ModuleId], [ProfileId], 'ACTIVO', 1, GETDATE()
                FROM [rel_Modules_Profiles];

                DROP TABLE [rel_Modules_Profiles];
                EXEC sp_rename N'rel_Modules_Profiles_new', N'rel_Modules_Profiles';
            END
            ELSE IF OBJECT_ID(N'[rel_Modules_Profiles]', N'U') IS NULL
            BEGIN
                CREATE TABLE [rel_Modules_Profiles] (
                    [id_rel_Modules_Profiles] INT IDENTITY(1,1) NOT NULL,
                    [id_Modules]              INT NOT NULL,
                    [id_Profiles]             INT NOT NULL,
                    [estado]                  VARCHAR(50) NOT NULL CONSTRAINT [DF_rel_Modules_Profiles_estado] DEFAULT ('ACTIVO'),
                    [id_usuario_creador]      INT NOT NULL,
                    [fecha_creacion]          DATETIME NOT NULL CONSTRAINT [DF_rel_Modules_Profiles_fecha_creacion] DEFAULT (GETDATE()),
                    [id_usuario_modifica]     INT NULL,
                    [fecha_modificacion]      DATETIME NULL,
                    CONSTRAINT [PK_rel_Modules_Profiles] PRIMARY KEY ([id_rel_Modules_Profiles]),
                    CONSTRAINT [FK_rel_Modules_Profiles_Modules]
                        FOREIGN KEY ([id_Modules]) REFERENCES [Modules]([Id]) ON DELETE NO ACTION,
                    CONSTRAINT [FK_rel_Modules_Profiles_Profiles]
                        FOREIGN KEY ([id_Profiles]) REFERENCES [Profiles]([Id]) ON DELETE CASCADE
                );

                INSERT INTO [rel_Modules_Profiles] (
                    [id_Modules], [id_Profiles], [estado], [id_usuario_creador], [fecha_creacion])
                SELECT [ModuleId], [Id], 'ACTIVO', 1, GETDATE()
                FROM [Profiles]
                WHERE [ModuleId] > 0;
            END
            """);

        migrationBuilder.Sql("""
            IF OBJECT_ID(N'[rel_Profiles_Roles]', N'U') IS NULL
            BEGIN
                CREATE TABLE [rel_Profiles_Roles] (
                    [id_rel_Profiles_Roles] INT IDENTITY(1,1) NOT NULL,
                    [id_Roles]              INT NOT NULL,
                    [id_Profiles]           INT NOT NULL,
                    [estado]                VARCHAR(50) NOT NULL CONSTRAINT [DF_rel_Profiles_Roles_estado] DEFAULT ('ACTIVO'),
                    [id_usuario_creador]    INT NOT NULL,
                    [fecha_creacion]        DATETIME NOT NULL CONSTRAINT [DF_rel_Profiles_Roles_fecha_creacion] DEFAULT (GETDATE()),
                    [id_usuario_modifica]   INT NULL,
                    [fecha_modificacion]    DATETIME NULL,
                    CONSTRAINT [PK_rel_Profiles_Roles] PRIMARY KEY ([id_rel_Profiles_Roles]),
                    CONSTRAINT [FK_rel_Profiles_Roles_Roles]
                        FOREIGN KEY ([id_Roles]) REFERENCES [Roles]([Id]) ON DELETE CASCADE,
                    CONSTRAINT [FK_rel_Profiles_Roles_Profiles]
                        FOREIGN KEY ([id_Profiles]) REFERENCES [Profiles]([Id]) ON DELETE NO ACTION
                );

                CREATE UNIQUE INDEX [IX_rel_Profiles_Roles_id_Roles]
                    ON [rel_Profiles_Roles]([id_Roles]);

                IF COL_LENGTH(N'Roles', N'ProfileId') IS NOT NULL
                BEGIN
                    INSERT INTO [rel_Profiles_Roles] (
                        [id_Roles], [id_Profiles], [estado], [id_usuario_creador], [fecha_creacion])
                    SELECT [Id], [ProfileId], 'ACTIVO', 1, GETDATE()
                    FROM [Roles]
                    WHERE [ProfileId] > 0;

                    ALTER TABLE [Roles] DROP CONSTRAINT [FK_Roles_Profiles_ProfileId];
                    DROP INDEX [IX_Roles_ProfileId] ON [Roles];
                    ALTER TABLE [Roles] DROP COLUMN [ProfileId];
                END
            END
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("SELECT 1");
    }
}
