---- Procedimientos Almacenados

---- Manejo de semanas prestamo
-- semanas restantes
create procedure semanas_restantes_prestamo
@id_prestamo int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Prestamo
	where pres_id = @id_prestamo

-- semana actual
create procedure semana_actual_prestamo
@id_prestamo int
as
	select datediff([week], fecha_inicio, getdate())
	from Prestamo
	where pres_id = @id_prestamo

------ Saldos Actuales y vencidos para reporte
create procedure Reporte
as
	declare @fecha datetime
	set @fecha = GETDATE()
	select  Cliente.cli_id, nombre, apellido, SUM
	from Cliente left join Prestamo on Prestamo.cli_id = Cliente.cli_id 


--insertar en usuario

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

---- insertar en cliente
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

drop procedure insert_cliente

--- insertar en prestamo
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

exec insert_prestamo 500, 50,12,'C:','videocamara','osre1','Rosio',0
drop procedure insert_prestamo

----- isertar cargo
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

------ insertar pago

create procedure insert_pago
@cantidad float,
@fecha datetime,
@pres_id int,
@cli_id varchar(5),
@usr_login varchar(255)
as
	insert into Pago values(@cantidad,@fecha,@pres_id,@cli_id,@usr_login)

drop procedure insert_pago

------ insertar multa
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

drop procedure insert_multa
----


--- Triggers
---- Triggers manejo de saldo actual
create trigger SaldoActual
on Pago for Insert
as
update Prestamo
set saldo_actual = Prestamo.cantidad - ( select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
from Prestamo, inserted
where Prestamo.pres_id = inserted.pres_id

drop trigger SaldoActual

create trigger SaldoActualDelete
on Pago after delete
as 
    update Prestamo
	set saldo_actual = saldo_actual + deleted.cantidad
	from Prestamo, deleted
	where Prestamo.pres_id = deleted.pres_id

drop trigger SaldoActualDelete

--Trigger para modificar la cantidad del prestamo
create trigger SaldoActualSumar
on Pago after update
as
if(update(cantidad))
	update Prestamo 
	set saldo_actual = Prestamo.cantidad - (select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
	from Prestamo, inserted
	where Prestamo.pres_id = inserted.pres_id

drop trigger SaldoActualSumar

create trigger SaldoActualPres
on Prestamo for update
as
	update Prestamo 
	set Prestamo.saldo_actual = Prestamo.saldo_actual - deleted.cantidad + Prestamo.cantidad
	from Prestamo, deleted
	where Prestamo.pres_id = deleted.pres_id

drop trigger SaldoActualPres
