/*
  Test login:
    Email:    user@sistemapaisa.com
    Password: Password123!
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

USE [SistemaPaisa];
GO

BEGIN TRANSACTION;

DECLARE @Now DATETIME2 = SYSUTCDATETIME();
DECLARE @SeedUser NVARCHAR(100) = N'seed-script';

DECLARE @ModuleId INT;
DECLARE @ClientId INT;
DECLARE @ProfileId INT;
DECLARE @RoleId INT;

/* -------------------------------------------------------------------------- */
/* Module                                                                     */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [Code] = N'SYSTEM')
BEGIN
    INSERT INTO [Modules] ([Name], [Description], [Code], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'System', N'Core system module', N'SYSTEM', 1, @Now, @SeedUser);

    SET @ModuleId = SCOPE_IDENTITY();
END
ELSE
    SELECT @ModuleId = [Id] FROM [Modules] WHERE [Code] = N'SYSTEM';

/* -------------------------------------------------------------------------- */
/* Actions                                                                    */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @ModuleId AND [Code] = N'VIEW')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'View', N'VIEW', @ModuleId, 1, @Now, @SeedUser);
END

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @ModuleId AND [Code] = N'MANAGE')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Manage', N'MANAGE', @ModuleId, 1, @Now, @SeedUser);
END

/* Products module */
DECLARE @ProductsModuleId INT;
IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [Code] = N'PRODUCTS')
BEGIN
    INSERT INTO [Modules] ([Name], [Description], [Code], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Products', N'Product catalog', N'PRODUCTS', 1, @Now, @SeedUser);
    SET @ProductsModuleId = SCOPE_IDENTITY();
END
ELSE
    SELECT @ProductsModuleId = [Id] FROM [Modules] WHERE [Code] = N'PRODUCTS';

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @ProductsModuleId AND [Code] = N'LIST')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'List products', N'LIST', @ProductsModuleId, 1, @Now, @SeedUser);
END

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @ProductsModuleId AND [Code] = N'CREATE')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Create product', N'CREATE', @ProductsModuleId, 1, @Now, @SeedUser);
END

/* Categories module */
DECLARE @CategoriesModuleId INT;
IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [Code] = N'CATEGORIES')
BEGIN
    INSERT INTO [Modules] ([Name], [Description], [Code], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Categories', N'Product categories', N'CATEGORIES', 1, @Now, @SeedUser);
    SET @CategoriesModuleId = SCOPE_IDENTITY();
END
ELSE
    SELECT @CategoriesModuleId = [Id] FROM [Modules] WHERE [Code] = N'CATEGORIES';

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @CategoriesModuleId AND [Code] = N'LIST')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'List categories', N'LIST', @CategoriesModuleId, 1, @Now, @SeedUser);
END

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @CategoriesModuleId AND [Code] = N'CREATE')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Create category', N'CREATE', @CategoriesModuleId, 1, @Now, @SeedUser);
END

/* Users module */
DECLARE @UsersModuleId INT;
IF NOT EXISTS (SELECT 1 FROM [Modules] WHERE [Code] = N'USERS')
BEGIN
    INSERT INTO [Modules] ([Name], [Description], [Code], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Users', N'User management', N'USERS', 1, @Now, @SeedUser);
    SET @UsersModuleId = SCOPE_IDENTITY();
END
ELSE
    SELECT @UsersModuleId = [Id] FROM [Modules] WHERE [Code] = N'USERS';

IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId] = @UsersModuleId AND [Code] = N'REGISTER')
BEGIN
    INSERT INTO [Actions] ([Name], [Code], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Register user', N'REGISTER', @UsersModuleId, 1, @Now, @SeedUser);
END

/* -------------------------------------------------------------------------- */
/* Client                                                                     */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Clients] WHERE [Code] = N'PAISA')
BEGIN
    INSERT INTO [Clients] ([Name], [Code], [Identification], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Paisa Foods', N'PAISA', N'900123456-1', 1, @Now, @SeedUser);

    SET @ClientId = SCOPE_IDENTITY();
END
ELSE
    SELECT @ClientId = [Id] FROM [Clients] WHERE [Code] = N'PAISA';

/* -------------------------------------------------------------------------- */
/* Profile                                                                    */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Profiles] WHERE [ModuleId] = @ModuleId AND [Name] = N'Operations')
BEGIN
    INSERT INTO [Profiles] ([Name], [Description], [ModuleId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Operations', N'Catalog and daily operations', @ModuleId, 1, @Now, @SeedUser);

    SET @ProfileId = SCOPE_IDENTITY();
END
ELSE
    SELECT @ProfileId = [Id] FROM [Profiles] WHERE [ModuleId] = @ModuleId AND [Name] = N'Operations';

/* -------------------------------------------------------------------------- */
/* Role (linked to Client + Profile)                                          */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Roles] WHERE [ClientId] = @ClientId AND [Name] = N'Store Operator')
BEGIN
    INSERT INTO [Roles] ([Name], [Description], [ProfileId], [ClientId], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Store Operator', N'Can manage products and categories', @ProfileId, @ClientId, 1, @Now, @SeedUser);

    SET @RoleId = SCOPE_IDENTITY();
END
ELSE
    SELECT @RoleId = [Id] FROM [Roles] WHERE [ClientId] = @ClientId AND [Name] = N'Store Operator';

/* -------------------------------------------------------------------------- */
/* User                                                                       */
/* Password: Password123! (ASP.NET Core Identity PasswordHasher v3)           */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Email] = N'user@sistemapaisa.com')
BEGIN
    INSERT INTO [Users] (
        [FirstName],
        [LastName],
        [Email],
        [PasswordHash],
        [RoleId],
        [IsActive],
        [CreatedAt],
        [CreatedBy]
    )
    VALUES (
        N'Juan',
        N'Perez',
        N'user@sistemapaisa.com',
        N'AQAAAAEAACcQAAAAEDXvqzbG1qsA/BUMTHqW0mTD1+ENvcEzExy9V6xsl9WwLgR5E0DLpywr16QROn0LIw==',
        @RoleId,
        1,
        @Now,
        @SeedUser
    );
END

/* -------------------------------------------------------------------------- */
/* Optional: sample category and product                                      */
/* -------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Dairy')
BEGIN
    INSERT INTO [Categories] ([Name], [Description], [IsActive], [CreatedAt], [CreatedBy])
    VALUES (N'Dairy', N'Dairy products', 1, @Now, @SeedUser);
END

DECLARE @CategoryId INT = (SELECT TOP 1 [Id] FROM [Categories] WHERE [Name] = N'Dairy');

IF @CategoryId IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM [Products] WHERE [Name] = N'Milk 1L')
BEGIN
    INSERT INTO [Products] (
        [Name],
        [Description],
        [Price],
        [Stock],
        [CategoryId],
        [IsActive],
        [CreatedAt],
        [CreatedBy]
    )
    VALUES (
        N'Milk 1L',
        N'Whole milk one liter',
        4500.00,
        100,
        @CategoryId,
        1,
        @Now,
        @SeedUser
    );
END

COMMIT TRANSACTION;

PRINT N'Seed completed.';
PRINT N'Login -> Email: user@sistemapaisa.com | Password: Password123!';
GO
