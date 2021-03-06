USE [master]
GO
/****** Object:  Database [pruebaV2]    Script Date: 29/12/2015 1:35:00 ******/
CREATE DATABASE [pruebaV2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'pruebaV2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\pruebaV2.mdf' , SIZE = 4160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'pruebaV2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\pruebaV2_log.ldf' , SIZE = 1040KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [pruebaV2] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [pruebaV2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [pruebaV2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [pruebaV2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [pruebaV2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [pruebaV2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [pruebaV2] SET ARITHABORT OFF 
GO
ALTER DATABASE [pruebaV2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [pruebaV2] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [pruebaV2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [pruebaV2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [pruebaV2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [pruebaV2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [pruebaV2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [pruebaV2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [pruebaV2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [pruebaV2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [pruebaV2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [pruebaV2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [pruebaV2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [pruebaV2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [pruebaV2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [pruebaV2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [pruebaV2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [pruebaV2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [pruebaV2] SET RECOVERY FULL 
GO
ALTER DATABASE [pruebaV2] SET  MULTI_USER 
GO
ALTER DATABASE [pruebaV2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [pruebaV2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [pruebaV2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [pruebaV2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'pruebaV2', N'ON'
GO
USE [pruebaV2]
GO
/****** Object:  User [PrestaMax]    Script Date: 29/12/2015 1:35:01 ******/
CREATE USER [PrestaMax] FOR LOGIN [PrestaMax] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [PrestaMax]
GO
ALTER ROLE [db_datareader] ADD MEMBER [PrestaMax]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [PrestaMax]
GO
/****** Object:  StoredProcedure [dbo].[insert_cargo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_cargo]
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
/****** Object:  StoredProcedure [dbo].[insert_cliente]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_cliente]
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
/****** Object:  StoredProcedure [dbo].[insert_multa]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_multa]
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
/****** Object:  StoredProcedure [dbo].[insert_pago]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_pago]
@cantidad float,
@fecha datetime,
@pres_id int,
@cli_id varchar(5),
@usr_login varchar(255)
as
	insert into Pago values(@cantidad,@fecha,@pres_id,@cli_id,@usr_login)
GO
/****** Object:  StoredProcedure [dbo].[insert_prestamo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_prestamo]
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
/****** Object:  StoredProcedure [dbo].[insert_usuario]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_usuario]
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
/****** Object:  StoredProcedure [dbo].[semana_actual_prestamo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[semana_actual_prestamo]
@id_prestamo int
as
	select datediff([week], fecha_inicio, getdate())
	from Prestamo
	where pres_id = @id_prestamo
GO
/****** Object:  StoredProcedure [dbo].[semanas_restantes_prestamo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[semanas_restantes_prestamo]
@id_prestamo int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Prestamo
	where pres_id = @id_prestamo

GO
/****** Object:  Table [dbo].[Cargo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cargo](
	[cargo_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [float] NULL,
	[fecha_inicio] [date] NULL,
	[fecha_vencimiento] [date] NULL,
	[semanas] [int] NULL,
	[semana_actual] [int] NULL,
	[descripcion] [text] NULL,
	[loquidado] [bit] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[cargo_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cliente](
	[cli_id] [varchar](5) NOT NULL,
	[nombre] [varchar](30) NULL,
	[apellido] [varchar](50) NULL,
	[direccion] [varchar](70) NULL,
	[fecha_alta] [date] NULL,
	[tel] [varchar](15) NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[cli_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Multa]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Multa](
	[multa_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [int] NULL,
	[fecha] [datetime] NULL,
	[semana] [int] NULL,
	[descripcion] [varchar](255) NULL,
	[pres_id] [int] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[multa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pago]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pago](
	[pago_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [float] NULL,
	[fecha] [datetime] NULL,
	[pres_id] [int] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[pago_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Prestamo]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Prestamo](
	[pres_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [float] NULL,
	[saldo_actual] [float] NULL,
	[fecha_inicio] [date] NULL,
	[fecha_vencimiento] [date] NULL,
	[pago_semanal] [float] NULL,
	[semanas] [int] NULL,
	[semana_actual] [int] NULL,
	[pagare] [varchar](100) NULL,
	[descripcion] [text] NULL,
	[loquidado] [bit] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[pres_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 29/12/2015 1:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[usr_nombre] [varchar](255) NOT NULL,
	[usr_login] [varchar](255) NOT NULL,
	[usr_pwd] [varchar](255) NOT NULL,
	[tipo] [char](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[usr_login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Cargo]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Cargo]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Multa]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Multa]  WITH CHECK ADD FOREIGN KEY([pres_id])
REFERENCES [dbo].[Prestamo] ([pres_id])
GO
ALTER TABLE [dbo].[Multa]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD FOREIGN KEY([pres_id])
REFERENCES [dbo].[Prestamo] ([pres_id])
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
USE [master]
GO
ALTER DATABASE [pruebaV2] SET  READ_WRITE 
GO
