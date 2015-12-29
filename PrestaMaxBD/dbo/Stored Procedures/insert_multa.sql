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