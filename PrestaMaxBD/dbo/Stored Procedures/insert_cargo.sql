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