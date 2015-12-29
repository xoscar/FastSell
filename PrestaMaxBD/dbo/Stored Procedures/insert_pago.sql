create procedure insert_pago
@cantidad float,
@fecha datetime,
@pres_id int,
@cli_id varchar(5),
@usr_login varchar(255)
as
	insert into Pago values(@cantidad,@fecha,@pres_id,@cli_id,@usr_login)