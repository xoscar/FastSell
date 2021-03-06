USE [master]
GO
/****** Object:  Database [Prueba]    Script Date: 29/12/2015 12:05:38 ******/
CREATE DATABASE [Prueba]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Prueba', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Prueba.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Prueba_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Prueba_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Prueba] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Prueba].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Prueba] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Prueba] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Prueba] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Prueba] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Prueba] SET ARITHABORT OFF 
GO
ALTER DATABASE [Prueba] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Prueba] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Prueba] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Prueba] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Prueba] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Prueba] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Prueba] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Prueba] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Prueba] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Prueba] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Prueba] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Prueba] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Prueba] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Prueba] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Prueba] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Prueba] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Prueba] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Prueba] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Prueba] SET RECOVERY FULL 
GO
ALTER DATABASE [Prueba] SET  MULTI_USER 
GO
ALTER DATABASE [Prueba] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Prueba] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Prueba] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Prueba] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Prueba', N'ON'
GO
USE [Prueba]
GO
/****** Object:  User [FastSell]    Script Date: 29/12/2015 12:05:38 ******/
CREATE USER [FastSell] FOR LOGIN [FastSell] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [FastSell]
GO
ALTER ROLE [db_datareader] ADD MEMBER [FastSell]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [FastSell]
GO
/****** Object:  StoredProcedure [dbo].[AbonosMes]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AbonosMes]
as
select SUM(cantidad)
from Abono
where fecha between DATEADD(month,-1,fecha) and GETDATE()
GO
/****** Object:  StoredProcedure [dbo].[AbonosTotal]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[AbonosTotal]
@fecha date
as
select SUM(cantidad)
from Abono
where  CAST(CONVERT(varchar(8),fecha,112) as date) = @fecha
GO
/****** Object:  StoredProcedure [dbo].[BuscarIdProducto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[BuscarIdProducto]
@cli_id varchar(5),
@id int
as
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id and Producto_Cliente_Precio.prod_id  = @id
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id) and Producto.prod_id = @id
GO
/****** Object:  StoredProcedure [dbo].[BuscarNombreProducto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[BuscarNombreProducto]
@cli_id varchar(5),
@nombre varchar(100)
as
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id and nombre_prod = @nombre
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id) and nombre = @nombre
GO
/****** Object:  StoredProcedure [dbo].[insert_abono]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_abono]
@cantidad float,
@fecha datetime,
@nota_id int,
@cli_id varchar(5),
@usr_login varchar(255),
@pedido_id int
as
	insert into Abono values(@cantidad,@fecha,@nota_id,@cli_id,@usr_login,@pedido_id)
GO
/****** Object:  StoredProcedure [dbo].[insert_cliente]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_cliente]
@nombre varchar(30),
@apellido varchar(50),
@negocio varchar(100),
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
	insert into Cliente values(@cli_id2, @nombre, @apellido, @negocio, @direc,@fecha_alta,@tel,@user_id)
GO
/****** Object:  StoredProcedure [dbo].[insert_nota]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--- insertar en Nota
create procedure [dbo].[insert_nota]
@cantidad float,
@semanas int,
@nota varchar(100),
@descripcion text,
@liquidado  bit,
@cli_id varchar(5),
@user varchar(255),
@pedido_id int
as
	declare @fecha_vencimiento date
	set @fecha_vencimiento = DATEADD(week, @semanas, GETDATE())
	insert into Nota values(@cantidad,@cantidad,getdate(),@fecha_vencimiento,@semanas,@nota,@descripcion,@liquidado,null,@cli_id,@user,@pedido_id)
GO
/****** Object:  StoredProcedure [dbo].[insert_pedido]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_pedido]
@precio_total float,
@usr_id varchar(255),
@cli_id varchar(5),
@repar_id varchar(5),
@contado bit,
@fecha_entrega datetime
as
	declare @fecha datetime
	set @fecha = GETDATE()
	insert into Pedido values(@fecha,@fecha_entrega,@precio_total,0,@contado,@usr_id,@cli_id,@repar_id, null)
	select pedido_id from Pedido where fecha_ped = @fecha and cli_id = @cli_id
GO
/****** Object:  StoredProcedure [dbo].[insert_pedido_producto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_pedido_producto]
@cantidad int,
@prod_id int,
@nombre_prod varchar(100),
@pedido_id int,
@precio float
as
	insert into Pedido_Producto values(@cantidad,@prod_id,@nombre_prod,@pedido_id,@precio)
GO
/****** Object:  StoredProcedure [dbo].[insert_producto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_producto]
@nombre varchar(100),
@precio float,
@cantidad int,
@tipo_acum int,
@descripcion text,
@usr_id varchar(255)
as 
	insert into Producto values(@nombre,@precio,@cantidad,@tipo_acum,getdate(),@descripcion,@usr_id)
GO
/****** Object:  StoredProcedure [dbo].[insert_producto_cliente_precio]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_producto_cliente_precio]
@precio float,
@prod_id int,
@nombre_prod varchar(100),
@cli_id varchar(5)
as
	insert into Producto_Cliente_Precio values(@precio,@prod_id,@nombre_prod,@cli_id)
GO
/****** Object:  StoredProcedure [dbo].[insert_proveedor]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_proveedor]
@nombre varchar(50),
@apellido varchar(50),
@negocio varchar(100),
@telefono varchar(20),
@direccion varchar(255)
as
	insert into Proveedor values(@nombre,@apellido,@negocio,@telefono,@direccion)
GO
/****** Object:  StoredProcedure [dbo].[insert_repartidor]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[insert_repartidor]
@nombre varchar(20),
@apellido varchar(30),
@direccion varchar(255),
@telefono varchar(15),
@usr_id varchar(255)
as
	declare @aux int
	declare @cli_id varchar(5)
	declare @cli_id2 varchar(5)
	set @cli_id = substring(@nombre,0,3) + substring(@apellido,0,3) + '%'
	set @cli_id2 = substring(@nombre,0,3) + substring(@apellido,0,3)
	set @aux = (select count(*) + 1  from Repartidor where repar_id like @cli_id)
	set @cli_id2 = @cli_id2 + cast(@aux as varchar(10))
	insert into Repartidor values(@cli_id2,@nombre,@apellido,@direccion,@telefono,getdate(),@usr_id)	
GO
/****** Object:  StoredProcedure [dbo].[insert_usuario]    Script Date: 29/12/2015 12:05:38 ******/
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
		insert into Usuario values(@nombre,@login,@pswd,0,@tipo)
	else
		print 'Nombre de usuario ya en uso'
GO
/****** Object:  StoredProcedure [dbo].[PedidosFechaEntrega]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[PedidosFechaEntrega]
@fecha_entrega date
as
	select * from Pedido where CAST(CONVERT(varchar(8),fecha_entrega,112) as date) = @fecha_entrega and liquidado = 0
GO
/****** Object:  StoredProcedure [dbo].[PedidosFechaPedido]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[PedidosFechaPedido]
@fecha_pedido date
as
	select * from Pedido where CAST(CONVERT(varchar(8),fecha_ped,112) as date) = @fecha_pedido and liquidado = 0
GO
/****** Object:  StoredProcedure [dbo].[PedidosMes]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[PedidosMes]
as
	select SUM(precio_total)
	from Pedido
	where contado = 0 and fecha_ped between  DATEADD(month,-1,fecha_ped) and GETDATE()
GO
/****** Object:  StoredProcedure [dbo].[PreciosCliente]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[PreciosCliente]
@cli_id varchar(5)
as
	select precio, Producto.prod_id, nombre_prod, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id 
	union
	select precio_general, prod_id, nombre, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id)
GO
/****** Object:  StoredProcedure [dbo].[ProductosPedido]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[ProductosPedido]
@pedido_id int
as
	select Pedido_Producto.prod_id, Pedido_Producto.precio, nombre_prod, Pedido_Producto.cantidad, Producto.tipo_acum from Pedido_Producto 
	left join Producto on Pedido_Producto.prod_id = Producto.prod_id where Pedido_Producto.pedido_id = @pedido_id 
GO
/****** Object:  StoredProcedure [dbo].[semana_actual_prestamo]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- semana actual
create procedure [dbo].[semana_actual_prestamo]
@id_nota int
as
	select datediff([week], fecha_inicio, getdate())
	from Nota
	where nota_id = @id_nota
GO
/****** Object:  StoredProcedure [dbo].[semanas_restantes_prestamo]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[semanas_restantes_prestamo]
@id_nota int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Nota
	where nota_id = @id_nota
GO
/****** Object:  StoredProcedure [dbo].[VentaTotalPedidos]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---Venta Hoy pedidos
create procedure [dbo].[VentaTotalPedidos]
@fecha date
as
select SUM(precio_total)
from Pedido
where contado = 0 and CAST(CONVERT(varchar(8),fecha_ped,112) as date)  = @fecha
GO
/****** Object:  Table [dbo].[Abono]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Abono](
	[abono_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [float] NULL,
	[fecha] [datetime] NULL,
	[nota_id] [int] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
	[pedido_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[abono_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 29/12/2015 12:05:38 ******/
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
	[negocio] [varchar](100) NULL,
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
/****** Object:  Table [dbo].[Nota]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Nota](
	[nota_id] [int] IDENTITY(1,1) NOT NULL,
	[cantidad] [float] NULL,
	[saldo_actual] [float] NULL,
	[fecha_inicio] [datetime] NULL,
	[fecha_vencimiento] [datetime] NULL,
	[semanas] [int] NULL,
	[nota] [varchar](100) NULL,
	[descripcion] [text] NULL,
	[liquidado] [bit] NULL,
	[fecha_liquidacion] [date] NULL,
	[cli_id] [varchar](5) NULL,
	[usr_id] [varchar](255) NULL,
	[pedido_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[nota_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pedido]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pedido](
	[pedido_id] [int] IDENTITY(1,1) NOT NULL,
	[fecha_ped] [datetime] NULL,
	[fecha_entrega] [datetime] NULL,
	[precio_total] [float] NULL,
	[liquidado] [bit] NULL,
	[contado] [bit] NULL,
	[usr_id] [varchar](255) NULL,
	[cli_id] [varchar](5) NULL,
	[repar_id] [varchar](5) NULL,
	[fecha_liquidacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[pedido_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pedido_Producto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pedido_Producto](
	[cantidad] [int] NULL,
	[prod_id] [int] NOT NULL,
	[nombre_prod] [varchar](100) NULL,
	[pedido_id] [int] NOT NULL,
	[precio] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[prod_id] ASC,
	[pedido_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Producto]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Producto](
	[prod_id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[precio_general] [float] NOT NULL,
	[cantidad] [int] NULL,
	[tipo_acum] [int] NOT NULL,
	[fecha_ingreso] [date] NOT NULL,
	[descripcion] [text] NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[prod_id] ASC,
	[nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Producto_Cliente_Precio]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Producto_Cliente_Precio](
	[pcp_id] [int] IDENTITY(1,1) NOT NULL,
	[precio] [float] NULL,
	[prod_id] [int] NOT NULL,
	[nombre_prod] [varchar](100) NULL,
	[cli_id] [varchar](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[pcp_id] ASC,
	[prod_id] ASC,
	[cli_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Proveedor]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Proveedor](
	[prov_id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NULL,
	[appellido] [varchar](50) NULL,
	[negocio] [varchar](100) NULL,
	[telefono] [varchar](20) NULL,
	[direccion] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[prov_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Repartidor]    Script Date: 29/12/2015 12:05:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Repartidor](
	[repar_id] [varchar](5) NOT NULL,
	[nombre] [varchar](20) NULL,
	[apellido] [varchar](30) NULL,
	[direccion] [varchar](255) NULL,
	[telefono] [varchar](15) NULL,
	[fecha_alta] [date] NULL,
	[usr_id] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[repar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 29/12/2015 12:05:38 ******/
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
	[eliminado] [bit] NOT NULL,
	[tipo] [char](1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[usr_login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Abono]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Abono]  WITH CHECK ADD FOREIGN KEY([nota_id])
REFERENCES [dbo].[Nota] ([nota_id])
GO
ALTER TABLE [dbo].[Abono]  WITH CHECK ADD FOREIGN KEY([pedido_id])
REFERENCES [dbo].[Pedido] ([pedido_id])
GO
ALTER TABLE [dbo].[Abono]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Nota]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Nota]  WITH CHECK ADD FOREIGN KEY([pedido_id])
REFERENCES [dbo].[Pedido] ([pedido_id])
GO
ALTER TABLE [dbo].[Nota]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD FOREIGN KEY([repar_id])
REFERENCES [dbo].[Repartidor] ([repar_id])
GO
ALTER TABLE [dbo].[Pedido]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Pedido_Producto]  WITH CHECK ADD FOREIGN KEY([pedido_id])
REFERENCES [dbo].[Pedido] ([pedido_id])
GO
ALTER TABLE [dbo].[Pedido_Producto]  WITH CHECK ADD FOREIGN KEY([prod_id], [nombre_prod])
REFERENCES [dbo].[Producto] ([prod_id], [nombre])
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
ALTER TABLE [dbo].[Producto_Cliente_Precio]  WITH CHECK ADD FOREIGN KEY([cli_id])
REFERENCES [dbo].[Cliente] ([cli_id])
GO
ALTER TABLE [dbo].[Producto_Cliente_Precio]  WITH CHECK ADD FOREIGN KEY([prod_id], [nombre_prod])
REFERENCES [dbo].[Producto] ([prod_id], [nombre])
GO
ALTER TABLE [dbo].[Repartidor]  WITH CHECK ADD FOREIGN KEY([usr_id])
REFERENCES [dbo].[Usuario] ([usr_login])
GO
USE [master]
GO
ALTER DATABASE [Prueba] SET  READ_WRITE 
GO
