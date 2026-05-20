/*
  Agrega modulos y acciones faltantes (idempotente).
  Ejecutar en SSMS si el sidebar solo muestra "System".
*/

SET NOCOUNT ON;
USE [SistemaPaisa];
GO

DECLARE @Now DATETIME2 = SYSUTCDATETIME();
DECLARE @By NVARCHAR(100) = N'seed-modules';

/* SYSTEM */
DECLARE @SystemId INT = (SELECT [Id] FROM [Modules] WHERE [Code] = N'SYSTEM');
IF @SystemId IS NULL
BEGIN
    INSERT INTO [Modules] ([Name],[Description],[Code],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'System',N'Core system module',N'SYSTEM',1,@Now,@By);
    SET @SystemId = SCOPE_IDENTITY();
END
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@SystemId AND [Code]=N'VIEW')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'View',N'VIEW',@SystemId,1,@Now,@By);
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@SystemId AND [Code]=N'MANAGE')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Manage',N'MANAGE',@SystemId,1,@Now,@By);

/* PRODUCTS */
DECLARE @ProductsId INT = (SELECT [Id] FROM [Modules] WHERE [Code] = N'PRODUCTS');
IF @ProductsId IS NULL
BEGIN
    INSERT INTO [Modules] ([Name],[Description],[Code],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Products',N'Product catalog',N'PRODUCTS',1,@Now,@By);
    SET @ProductsId = SCOPE_IDENTITY();
END
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@ProductsId AND [Code]=N'LIST')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'List products',N'LIST',@ProductsId,1,@Now,@By);
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@ProductsId AND [Code]=N'CREATE')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Create product',N'CREATE',@ProductsId,1,@Now,@By);

/* CATEGORIES */
DECLARE @CategoriesId INT = (SELECT [Id] FROM [Modules] WHERE [Code] = N'CATEGORIES');
IF @CategoriesId IS NULL
BEGIN
    INSERT INTO [Modules] ([Name],[Description],[Code],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Categories',N'Product categories',N'CATEGORIES',1,@Now,@By);
    SET @CategoriesId = SCOPE_IDENTITY();
END
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@CategoriesId AND [Code]=N'LIST')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'List categories',N'LIST',@CategoriesId,1,@Now,@By);
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@CategoriesId AND [Code]=N'CREATE')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Create category',N'CREATE',@CategoriesId,1,@Now,@By);

/* USERS */
DECLARE @UsersId INT = (SELECT [Id] FROM [Modules] WHERE [Code] = N'USERS');
IF @UsersId IS NULL
BEGIN
    INSERT INTO [Modules] ([Name],[Description],[Code],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Users',N'User management',N'USERS',1,@Now,@By);
    SET @UsersId = SCOPE_IDENTITY();
END
IF NOT EXISTS (SELECT 1 FROM [Actions] WHERE [ModuleId]=@UsersId AND [Code]=N'REGISTER')
    INSERT INTO [Actions] ([Name],[Code],[ModuleId],[IsActive],[CreatedAt],[CreatedBy])
    VALUES (N'Register user',N'REGISTER',@UsersId,1,@Now,@By);

PRINT N'Modulos listos: SYSTEM, PRODUCTS, CATEGORIES, USERS';
GO
