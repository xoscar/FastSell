create procedure semanas_restantes_prestamo
@id_prestamo int
as
	select DATEDIFF([week], getdate(), fecha_vencimiento)
	from Prestamo
	where pres_id = @id_prestamo
