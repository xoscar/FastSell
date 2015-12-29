create database Prueba

use Prueba

drop database Prueba

----------Ususarios

CREATE TABLE Usuario
(
  usr_nombre varchar(255) NOT NULL,
  usr_login VARCHAR(255) NOT NULL,
  usr_pwd VARCHAR(255) NOT NULL,
  eliminado bit not null,
  tipo char not null,

  primary key(usr_login)
)

---------------

drop table Usuario

insert into Usuario values('Rosio','papaya@123',NULL)

select * from Usuario

-------------------------------------------------------------------------------------------------

--Estructura tabla Cliente

create table Cliente
(
	cli_id varchar(5), 
	nombre varchar(30), 
	apellido varchar(50),
	negocio varchar(100), 
	direccion varchar(70), 
	fecha_alta date,
	tel varchar(15),

	usr_id varchar(255),

	primary key(cli_id)
)

alter table Cliente add foreign key(usr_id) references Usuario(usr_login) 

------ Estructura tabla Productos

create table Producto
(
	prod_id int identity,
	nombre varchar(100),
	precio_general  float not null,
	cantidad int,
	tipo_acum int not null,
	fecha_ingreso date not  null,
	descripcion text,

	usr_id varchar(255)

	primary key(prod_id, nombre)
)

alter table Producto add foreign key(usr_id) references Usuario(usr_login)

drop table Producto

select * from Producto


------ Estructura tabla Repartido
create table Repartidor
(
	repar_id varchar(5),
	nombre varchar(20),
	apellido varchar(30),
	direccion varchar(255),
	telefono varchar(15),
	fecha_alta date,

	usr_id varchar(255),

	primary key(repar_id)
)

alter table Repartidor add foreign key(usr_id) references Usuario(usr_login)

------- Estructura tabla Pedido

create table Pedido
(
	pedido_id  int identity,
	fecha_ped datetime,
	fecha_entrega datetime,
	precio_total float,
	liquidado bit,
	contado bit, 
	usr_id varchar(255),
	cli_id varchar(5),
	repar_id  varchar(5),
	fecha_liquidacion datetime,

	primary key( pedido_id)
)

alter table Pedido add foreign key(usr_id) references Usuario(usr_login)
alter table Pedido add foreign key(cli_id) references Cliente(cli_id)
alter table Pedido add foreign key(repar_id) references Repartidor(repar_id)

drop table Pedido 
---- Estructura tabla Nota
create table Nota
(
	nota_id int identity,
	cantidad float,
	saldo_actual float,
	fecha_inicio datetime,
	fecha_vencimiento datetime,
	semanas int,
	nota varchar(100),
	descripcion text,
	liquidado bit,
	fecha_liquidacion date,

	cli_id varchar(5),
	usr_id varchar(255),
	pedido_id int,

	primary key( nota_id)
)

drop table Nota

alter table Nota add foreign key(cli_id) references Cliente(cli_id)
alter table Nota add foreign key(usr_id) references Usuario(usr_login)
alter table Nota add foreign key(pedido_id) references Pedido(pedido_id)

---Estructura tabla Abono
create table Abono
(
	abono_id int identity, 
	cantidad float, 
	fecha datetime,
	 
	nota_id int, 
	cli_id varchar(5),
	usr_id varchar(255),
	pedido_id int,
	 
	primary key(abono_id)
)

drop table Abono

alter table Abono add foreign key(nota_id) references Nota(nota_id)
alter table Abono add foreign key(cli_id) references Cliente(cli_id)
alter table Abono add foreign key(usr_id) references Usuario(usr_login)
alter table Abono add foreign key(pedido_id) references Pedido(pedido_id)


----Estructura producto_cliente
create table Producto_Cliente_Precio
(
	pcp_id int identity,
	precio float,

	prod_id int,
	nombre_prod varchar(100),
	cli_id varchar(5),

	primary key(pcp_id, prod_id, cli_id)
)

drop table Producto_Cliente_Precio

alter table Producto_Cliente_Precio add foreign key(prod_id, nombre_prod) references Producto(prod_id, nombre)
alter table Producto_Cliente_Precio add foreign key(cli_id) references Cliente(cli_id)


-----Estructura tabla Pedido_Producto
create table Pedido_Producto
(
	cantidad int,
	
	prod_id int,
	nombre_prod  varchar(100),
	pedido_id  int,
	precio float,

	primary key(prod_id, pedido_id)
)

drop table Pedido_Producto

alter table Pedido_Producto add foreign key(prod_id, nombre_prod) references Producto(prod_id, nombre)
alter table Pedido_Producto add foreign key(pedido_id) references Pedido(pedido_id)

create table Proveedor
(
	prov_id int identity,
	nombre varchar(50),
	appellido varchar(50),
	negocio varchar(100),
	telefono varchar(20),
	direccion varchar(255),

	primary key(prov_id)
)

drop table Proveedor

select * from Usuario

select * from Pedido_Producto

select * from Cliente

select * from Pedido

select * from Repartidor

select * from Producto_Cliente_Precio

select * from Producto

select * from Nota

select * from Abono

select * from Proveedor


insert into Usuario values('Oscar Reyes','Oscar','12345',0, 'A')

insert into Cliente values('MaGu1','Maria','Guadalupe','Gomez Palacio',GETDATE(),'1591280','Oscar')

insert into Repartidor values('DaBo1','David','Bolsas','Torreon','1594545',GETDATE(), 'Oscar')

--insert into Producto_Cliente_Precio(50,


delete from Cliente where cli_id = 'MaRe2'

declare @fecha datetime
set @fecha = GETDATE()

exec insert_cliente 'Maria','Reyes','gomez palacio',@fecha,'155645','Oscar'

select  name from Prueba.sys.tables


select count(*) from Pedido where fecha_ped <= GETDATE() and liquidado = 0

select * from Cliente where cli_id = 'MaGu1'


declare @fecha datetime
set @fecha = GETDATE()

exec insert_pedido '350','Oscar','MaGu1','DaBo1',0,@fecha

exec insert_nota '350',5,'c:','hola',0,'MaGu1','Oscar',1,null

exec insert_producto 'charola','200',30,1,'charola pequeña','Oscar'

exec insert_producto_cliente_precio '150',3,'charola','MaRe1'


update Producto set nombre = 'Bolsas Grandes' where prod_id = 1

update Producto set cantidad = 50

update Nota set saldo_actual = 2100 

update Usuario set eliminado = 0

exec insert_producto 'vasos',150,10,0,'vaso del otro','Oscar'

select * from Producto_Cliente_Precio where prod_id = 4 and nombre_prod = 'vasos'

delete from Abono
delete from Pedido
delete from Nota
delete from Cliente

delete from Repartidor
delete from Pedido
delete from Producto

delete from Pedido_Producto