/*
Script de implementación para PrestaMaxBD

Una herramienta generó este código.
Los cambios realizados en este archivo podrían generar un comportamiento incorrecto y se perderán si
se vuelve a generar el código.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "PrestaMaxBD"
:setvar DefaultFilePrefix "PrestaMaxBD"
:setvar DefaultDataPath "C:\Users\oscar\AppData\Local\Microsoft\VisualStudio\SSDT\PrestaMaxv2\"
:setvar DefaultLogPath "C:\Users\oscar\AppData\Local\Microsoft\VisualStudio\SSDT\PrestaMaxv2\"

GO
:on error exit
GO
/*
Detectar el modo SQLCMD y deshabilitar la ejecución del script si no se admite el modo SQLCMD.
Para volver a habilitar el script después de habilitar el modo SQLCMD, ejecute lo siguiente:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'El modo SQLCMD debe estar habilitado para ejecutar correctamente este script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creando [dbo].[Cargo]...';


GO
CREATE TABLE [dbo].[Cargo] (
    [cargo_id]          INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]          FLOAT (53)    NULL,
    [fecha_inicio]      DATE          NULL,
    [fecha_vencimiento] DATE          NULL,
    [semanas]           INT           NULL,
    [semana_actual]     INT           NULL,
    [descripcion]       TEXT          NULL,
    [loquidado]         BIT           NULL,
    [cli_id]            VARCHAR (5)   NULL,
    [usr_id]            VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([cargo_id] ASC)
);


GO
PRINT N'Creando [dbo].[Cliente]...';


GO
CREATE TABLE [dbo].[Cliente] (
    [cli_id]     VARCHAR (5)   NOT NULL,
    [nombre]     VARCHAR (30)  NULL,
    [apellido]   VARCHAR (50)  NULL,
    [direccion]  VARCHAR (70)  NULL,
    [fecha_alta] DATE          NULL,
    [tel]        VARCHAR (15)  NULL,
    [usr_id]     VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([cli_id] ASC)
);


GO
PRINT N'Creando [dbo].[Multa]...';


GO
CREATE TABLE [dbo].[Multa] (
    [multa_id]    INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]    INT           NULL,
    [fecha]       DATETIME      NULL,
    [semana]      INT           NULL,
    [descripcion] VARCHAR (255) NULL,
    [pres_id]     INT           NULL,
    [cli_id]      VARCHAR (5)   NULL,
    [usr_id]      VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([multa_id] ASC)
);


GO
PRINT N'Creando [dbo].[Pago]...';


GO
CREATE TABLE [dbo].[Pago] (
    [pago_id]  INT           IDENTITY (1, 1) NOT NULL,
    [cantidad] FLOAT (53)    NULL,
    [fecha]    DATETIME      NULL,
    [pres_id]  INT           NULL,
    [cli_id]   VARCHAR (5)   NULL,
    [usr_id]   VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([pago_id] ASC)
);


GO
PRINT N'Creando [dbo].[Prestamo]...';


GO
CREATE TABLE [dbo].[Prestamo] (
    [pres_id]           INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]          FLOAT (53)    NULL,
    [saldo_actual]      FLOAT (53)    NULL,
    [fecha_inicio]      DATE          NULL,
    [fecha_vencimiento] DATE          NULL,
    [pago_semanal]      FLOAT (53)    NULL,
    [semanas]           INT           NULL,
    [semana_actual]     INT           NULL,
    [pagare]            VARCHAR (100) NULL,
    [descripcion]       TEXT          NULL,
    [loquidado]         BIT           NULL,
    [cli_id]            VARCHAR (5)   NULL,
    [usr_id]            VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([pres_id] ASC)
);


GO
PRINT N'Creando [dbo].[Usuario]...';


GO
CREATE TABLE [dbo].[Usuario] (
    [usr_nombre] VARCHAR (255) NOT NULL,
    [usr_login]  VARCHAR (255) NOT NULL,
    [usr_pwd]    VARCHAR (255) NOT NULL,
    [tipo]       CHAR (1)      NULL,
    PRIMARY KEY CLUSTERED ([usr_login] ASC)
);


GO
PRINT N'Creando Clave externa en [dbo].[Cargo]....';


GO
ALTER TABLE [dbo].[Cargo] WITH NOCHECK
    ADD FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Cargo]....';


GO
ALTER TABLE [dbo].[Cargo] WITH NOCHECK
    ADD FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login]);


GO
PRINT N'Creando Clave externa en [dbo].[Cliente]....';


GO
ALTER TABLE [dbo].[Cliente] WITH NOCHECK
    ADD FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login]);


GO
PRINT N'Creando Clave externa en [dbo].[Multa]....';


GO
ALTER TABLE [dbo].[Multa] WITH NOCHECK
    ADD FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Multa]....';


GO
ALTER TABLE [dbo].[Multa] WITH NOCHECK
    ADD FOREIGN KEY ([pres_id]) REFERENCES [dbo].[Prestamo] ([pres_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Multa]....';


GO
ALTER TABLE [dbo].[Multa] WITH NOCHECK
    ADD FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login]);


GO
PRINT N'Creando Clave externa en [dbo].[Pago]....';


GO
ALTER TABLE [dbo].[Pago] WITH NOCHECK
    ADD FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Pago]....';


GO
ALTER TABLE [dbo].[Pago] WITH NOCHECK
    ADD FOREIGN KEY ([pres_id]) REFERENCES [dbo].[Prestamo] ([pres_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Pago]....';


GO
ALTER TABLE [dbo].[Pago] WITH NOCHECK
    ADD FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login]);


GO
PRINT N'Creando Clave externa en [dbo].[Prestamo]....';


GO
ALTER TABLE [dbo].[Prestamo] WITH NOCHECK
    ADD FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]);


GO
PRINT N'Creando Clave externa en [dbo].[Prestamo]....';


GO
ALTER TABLE [dbo].[Prestamo] WITH NOCHECK
    ADD FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login]);


GO
PRINT N'Creando [dbo].[SaldoActual]...';


GO
create trigger SaldoActual
on Pago for Insert
as
update Prestamo
set saldo_actual = Prestamo.cantidad - ( select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
from Prestamo, inserted
where Prestamo.pres_id = inserted.pres_id
GO
PRINT N'Creando [dbo].[SaldoActualSumar]...';


GO
create trigger SaldoActualSumar
on Pago after update
as
if(update(cantidad))
	update Prestamo 
	set saldo_actual = Prestamo.cantidad - (select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
	from Prestamo, inserted
	where Prestamo.pres_id = inserted.pres_id
GO
PRINT N'Creando [dbo].[SaldoActualDelete]...';


GO
create trigger SaldoActualDelete
on Pago after delete
as 
    update Prestamo
	set saldo_actual = saldo_actual + deleted.cantidad
	from Prestamo, deleted
	where Prestamo.pres_id = deleted.pres_id
GO
PRINT N'Creando [dbo].[SaldoActualPres]...';


GO
create trigger SaldoActualPres
on Prestamo for update
as
if(update(cantidad))
	update Prestamo 
	set saldo_actual = Prestamo.saldo_actual + (inserted.cantidad - Prestamo.cantidad) 
	from Prestamo, inserted
	where Prestamo.pres_id = inserted.pres_id
GO
PRINT N'Creando [dbo].[insert_cargo]...';


GO
create procedure insert_cargo
@cantidad float,
@semanas int,
@descripcion text,
@cli_id varchar(5),
@user varchar(255)
as
	declare @fecha_vencimiento date
	set @fecha_vencimiento = DATEADD(week, @semanas, GETDATE())
	insert into Cargo values(@cantidad,getdate(),@fecha_vencimiento,@semanas,1,@descripcion,0,@cli_id,@user)
GO
PRINT N'Creando [dbo].[insert_cliente]...';


GO
create procedure insert_cliente
@nombre varchar(30),
@apellido varchar(50),
@direc varchar(70),
@fecha_alta date,
@tel varchar(15),
@user_id varchar(255)
as
	declare @aux int
	declare @cli_id varchar(5)
	declare @cli_id2 varchar(5)
	set @cli_id = substring(@nombre,0,3) + substring(@apellido,0,3) + '%'
	set @cli_id2 = substring(@nombre,0,3) + substring(@apellido,0,3)
	set @aux = (select count(*) + 1  from Cliente where cli_id like @cli_id)
	set @cli_id2 = @cli_id2 + cast(@aux as varchar(10))
	insert into Cliente values(@cli_id2, @nombre, @apellido, @direc,@fecha_alta,@tel,@user_id)
GO
PRINT N'Creando [dbo].[insert_multa]...';


GO
create procedure insert_multa
@cantidad float,
@fecha datetime,
@semana int,
@descripcion varchar(255),
@pres_id int,
@cli_id varchar(5),
@usr_login varchar(255)
as
	insert into Multa values(@cantidad,@fecha,@semana,@descripcion,@pres_id,@cli_id,@usr_login)
GO
PRINT N'Creando [dbo].[insert_pago]...';


GO
create procedure insert_pago
@cantidad float,
@fecha datetime,
@pres_id int,
@cli_id varchar(5),
@usr_login varchar(255)
as
	insert into Pago values(@cantidad,@fecha,@pres_id,@cli_id,@usr_login)
GO
PRINT N'Creando [dbo].[insert_prestamo]...';


GO
create procedure insert_prestamo
@cantidad float,
@pago_semanal float,
@semanas int,
@pagare varchar(100),
@descripcion text,
@cli_id varchar(5),
@user varchar(255),
@liquidado  bit
as
	declare @fecha_vencimiento date
	set @fecha_vencimiento = DATEADD(week, @semanas, GETDATE())
	insert into Prestamo values(@cantidad,@cantidad,getdate(),@fecha_vencimiento,@pago_semanal,@semanas,1,@pagare,@descripcion,@liquidado,@cli_id,@user)
GO
PRINT N'Creando [dbo].[insert_usuario]...';


GO
create procedure insert_usuario
@nombre varchar(255),
@login varchar(255),
@pswd varchar(255),
@tipo char
as
	if(@login not in (select usr_login from Usuario))
		insert into Usuario values(@nombre,@login,@pswd,@tipo)
	else
		print 'Nombre de usuario ya en uso'
GO
PRINT N'Creando [dbo].[semana_actual_prestamo]...';


GO
create procedure semana_actual_prestamo
@id_prestamo int
as
	select datediff([week], fecha_inicio, getdate())
	from Prestamo
	where pres_id = @id_prestamo
GO
PRINT N'Creando [dbo].[semanas_restantes_prestamo]...';


GO
create procedure semanas_restantes_prestamo
@id_prestamo int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Prestamo
	where pres_id = @id_prestamo
GO
PRINT N'Comprobando los datos existentes con las restricciones recién creadas';


GO
USE [$(DatabaseName)];


GO
CREATE TABLE [#__checkStatus] (
    id           INT            IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
    [Schema]     NVARCHAR (256),
    [Table]      NVARCHAR (256),
    [Constraint] NVARCHAR (256)
);

SET NOCOUNT ON;

DECLARE tableconstraintnames CURSOR LOCAL FORWARD_ONLY
    FOR SELECT SCHEMA_NAME([schema_id]),
               OBJECT_NAME([parent_object_id]),
               [name],
               0
        FROM   [sys].[objects]
        WHERE  [parent_object_id] IN (OBJECT_ID(N'dbo.Cargo'), OBJECT_ID(N'dbo.Cliente'), OBJECT_ID(N'dbo.Multa'), OBJECT_ID(N'dbo.Pago'), OBJECT_ID(N'dbo.Prestamo'))
               AND [type] IN (N'F', N'C')
                   AND [object_id] IN (SELECT [object_id]
                                       FROM   [sys].[check_constraints]
                                       WHERE  [is_not_trusted] <> 0
                                       UNION
                                       SELECT [object_id]
                                       FROM   [sys].[foreign_keys]
                                       WHERE  [is_not_trusted] <> 0);

DECLARE @schemaname AS NVARCHAR (256);

DECLARE @tablename AS NVARCHAR (256);

DECLARE @checkname AS NVARCHAR (256);

DECLARE @is_not_trusted AS INT;

DECLARE @statement AS NVARCHAR (1024);

BEGIN TRY
    OPEN tableconstraintnames;
    FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
    WHILE @@fetch_status = 0
        BEGIN
            PRINT N'Comprobando restricción: {0} [{1}].[{2}]' + @checkname + N' [' + @schemaname + N'].[' + @tablename + N']';
            SET @statement = N'ALTER TABLE [' + @schemaname + N'].[' + @tablename + N'] WITH ' + CASE @is_not_trusted WHEN 0 THEN N'CHECK' ELSE N'NOCHECK' END + N' CHECK CONSTRAINT [' + @checkname + N']';
            BEGIN TRY
                EXECUTE [sp_executesql] @statement;
            END TRY
            BEGIN CATCH
                INSERT  [#__checkStatus] ([Schema], [Table], [Constraint])
                VALUES                  (@schemaname, @tablename, @checkname);
            END CATCH
            FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
        END
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') >= 0
    CLOSE tableconstraintnames;

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') = -1
    DEALLOCATE tableconstraintnames;

SELECT N'Error de comprobación de restricción:' + [Schema] + N'.' + [Table] + N',' + [Constraint]
FROM   [#__checkStatus];

IF @@ROWCOUNT > 0
    BEGIN
        DROP TABLE [#__checkStatus];
        RAISERROR (N'Error al comprobar las restricciones', 16, 127);
    END

SET NOCOUNT OFF;

DROP TABLE [#__checkStatus];


GO
PRINT N'Actualización completada.';


GO
