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