------------ Procedimientos Almacenados

-----Pedidos por fecha entrega
create procedure PedidosFechaEntrega
@fecha_entrega date
as
	select * from Pedido where CAST(CONVERT(varchar(8),fecha_entrega,112) as date) = @fecha_entrega and liquidado = 0

drop procedure PedidosFechaEntrega

-----Pedidos por fecha entrega
create procedure PedidosFechaPedido
@fecha_pedido date
as
	select * from Pedido where CAST(CONVERT(varchar(8),fecha_ped,112) as date) = @fecha_pedido and liquidado = 0

drop procedure PedidosFechaPedido

-------- Precios de un cliente
create procedure PreciosCliente
@cli_id varchar(5)
as
	select precio, Producto.prod_id, nombre_prod, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id 
	union
	select precio_general, prod_id, nombre, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id)

drop procedure PreciosCliente

exec PreciosCliente  'MaRe1'
exec PreciosCliente 'MaGu1'
exec PreciosCliente 'OsRa1'

select * from Producto_Cliente_Precio


----- Productos de un cliente buscados por nombre

create procedure BuscarNombreProducto
@cli_id varchar(5),
@nombre varchar(100)
as
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id and nombre_prod = @nombre
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id) and nombre = @nombre


drop procedure BuscarNombreProducto
exec BuscarNombreProducto 'MaGu1','vasos'

select * from Producto
select * from Producto_Cliente_Precio


------- Busqueda producto por id precio
create procedure BuscarIdProducto
@cli_id varchar(5),
@id int
as
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = @cli_id and Producto_Cliente_Precio.prod_id  = @id
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = @cli_id) and Producto.prod_id = @id

exec BuscarIdProducto 'OsRe1',6

------- Productos de un pedido
create procedure ProductosPedido
@pedido_id int
as
	select Pedido_Producto.prod_id, Pedido_Producto.precio, nombre_prod, Pedido_Producto.cantidad, Producto.tipo_acum from Pedido_Producto 
	left join Producto on Pedido_Producto.prod_id = Producto.prod_id where Pedido_Producto.pedido_id = @pedido_id 

exec ProductosPedido 1

drop procedure ProductosPedido

select * from Pedido

select * from Pedido_Producto

select * from Producto

------ Reportes
---Venta Hoy pedidos
create procedure VentaTotalPedidos
@fecha date
as
select SUM(precio_total)
from Pedido
where contado = 0 and CAST(CONVERT(varchar(8),fecha_ped,112) as date)  = @fecha

declare @fecha date
set @fecha = GETDATE()
exec VentaTotalPedidos @fecha
 
drop procedure VentaTotalPedidos

----- Abonos hoy total
create procedure AbonosTotal
@fecha date
as
select SUM(cantidad)
from Abono
where  CAST(CONVERT(varchar(8),fecha,112) as date) = @fecha

---- Abonos del mes
create procedure AbonosMes
as
select SUM(cantidad)
from Abono
where fecha between DATEADD(month,-1,fecha) and GETDATE()

drop procedure AbonosMes

----Pedidos al mes
create procedure PedidosMes
as
	select SUM(precio_total)
	from Pedido
	where contado = 0 and fecha_ped between  DATEADD(month,-1,fecha_ped) and GETDATE()

drop procedure PedidosMes

declare @fecha date
set @fecha = GETDATE()
exec AbonosTotal @fecha

select * from Pedido

select * from Abono


----- Manejo de semanas prestamo
---- semanas restantes

create procedure semanas_restantes_prestamo
@id_nota int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Nota
	where nota_id = @id_nota

-- semana actual
create procedure semana_actual_prestamo
@id_nota int
as
	select datediff([week], fecha_inicio, getdate())
	from Nota
	where nota_id = @id_nota


--insertar en usuario

create procedure insert_usuario
@nombre varchar(255),
@login varchar(255),
@pswd varchar(255),
@tipo char
as
	if(@login not in (select usr_login from Usuario))
		insert into Usuario values(@nombre,@login,@pswd,0,@tipo)
	else
		print 'Nombre de usuario ya en uso'


----- insertar producto
create procedure insert_producto
@nombre varchar(100),
@precio float,
@cantidad int,
@tipo_acum int,
@descripcion text,
@usr_id varchar(255)
as 
	insert into Producto values(@nombre,@precio,@cantidad,@tipo_acum,getdate(),@descripcion,@usr_id)

exec insert_producto 'Bolsas 3x3',50,10,0,'Bolsas Azules','Oscar'


select * from Producto

---- insert Repartidor
create procedure insert_repartidor
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

drop procedure insert_repartidor

exec insert_repartidor 'Oscar','Reyes','Lerdo','121545','Oscar'

select * from Repartidor


----- Insert Producto_Cliente_Precio
create procedure insert_producto_cliente_precio
@precio float,
@prod_id int,
@nombre_prod varchar(100),
@cli_id varchar(5)
as
	insert into Producto_Cliente_Precio values(@precio,@prod_id,@nombre_prod,@cli_id)

exec insert_producto_cliente_precio 150,1,'Bolsas 3x3','MaRe1'

select * from Producto_Cliente_Precio

select * from Producto


---- insertar en Pedido_Producto
create procedure insert_pedido_producto
@cantidad int,
@prod_id int,
@nombre_prod varchar(100),
@pedido_id int,
@precio float
as
	insert into Pedido_Producto values(@cantidad,@prod_id,@nombre_prod,@pedido_id,@precio)

exec insert_pedido_producto '5','1','Bolsas 3x3','1'

select * from Pedido_Producto
drop procedure insert_pedido_producto

----- insertar pedido
create procedure insert_pedido
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

drop procedure insert_pedido

exec insert_pedido '5000','Oscar','MaRe1','OsRe1',0

select * from Pedido


---- insertar en cliente
create procedure insert_cliente
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


select * from Cliente

--- insertar en Nota
create procedure insert_nota
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


select * from Nota
drop procedure insert_nota

----Insertar Abono
create procedure insert_abono
@cantidad float,
@fecha datetime,
@nota_id int,
@cli_id varchar(5),
@usr_login varchar(255),
@pedido_id int
as
	insert into Abono values(@cantidad,@fecha,@nota_id,@cli_id,@usr_login,@pedido_id)

select * from Abono

----- insertar Proveedor
create procedure insert_proveedor
@nombre varchar(50),
@apellido varchar(50),
@negocio varchar(100),
@telefono varchar(20),
@direccion varchar(255)
as
	insert into Proveedor values(@nombre,@apellido,@negocio,@telefono,@direccion)

exec insert_proveedor 'oscar','reyes','pollos','1591280'

drop procedure insert_proveedor
select * from Proveedor

--- Triggers
---- Triggers manejo de saldo actual

create trigger SaldoActual
on Abono for Insert
as
update Nota
set saldo_actual = Nota.cantidad - ( select SUM(cantidad) from Abono where nota_id = inserted.nota_id)
from Nota, inserted
where Nota.nota_id = inserted.nota_id

drop trigger SaldoActual

create trigger SaldoActualDelete
on  Abono after delete
as 
    update Nota
	set saldo_actual = saldo_actual + deleted.cantidad
	from Nota, deleted
	where Nota.nota_id  = deleted.nota_id

drop trigger SaldoActualDelete

--Trigger para modificar la cantidad del prestamo
create trigger SaldoActualSumar
on Abono after update
as
if(update(cantidad))
	update Nota 
	set saldo_actual = Nota.cantidad - (select SUM(cantidad) from Abono where nota_id = inserted.nota_id)
	from Nota, inserted
	where Nota.nota_id = inserted.nota_id

drop trigger SaldoActualSumar

create trigger SaldoActualPres
on Nota for update
as
if(update(cantidad))
	update Nota 
	set saldo_actual = Nota.saldo_actual + (inserted.cantidad - Nota.cantidad) 
	from Nota, inserted
	where Nota.nota_id = inserted.nota_id

drop trigger SaldoActualPres


delete from Pedido

delete from Nota
delete from Abono
