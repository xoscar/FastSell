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