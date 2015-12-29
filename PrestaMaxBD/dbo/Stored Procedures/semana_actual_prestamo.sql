create procedure semana_actual_prestamo
@id_prestamo int
as
	select datediff([week], fecha_inicio, getdate())
	from Prestamo
	where pres_id = @id_prestamo