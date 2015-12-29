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