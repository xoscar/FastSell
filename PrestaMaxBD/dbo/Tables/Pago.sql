CREATE TABLE [dbo].[Pago] (
    [pago_id]  INT           IDENTITY (1, 1) NOT NULL,
    [cantidad] FLOAT (53)    NULL,
    [fecha]    DATETIME      NULL,
    [pres_id]  INT           NULL,
    [cli_id]   VARCHAR (5)   NULL,
    [usr_id]   VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([pago_id] ASC),
    FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]),
    FOREIGN KEY ([pres_id]) REFERENCES [dbo].[Prestamo] ([pres_id]),
    FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login])
);


GO
create trigger SaldoActual
on Pago for Insert
as
update Prestamo
set saldo_actual = Prestamo.cantidad - ( select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
from Prestamo, inserted
where Prestamo.pres_id = inserted.pres_id
GO
create trigger SaldoActualSumar
on Pago after update
as
if(update(cantidad))
	update Prestamo 
	set saldo_actual = Prestamo.cantidad - (select SUM(cantidad) from Pago where pres_id = inserted.pres_id)
	from Prestamo, inserted
	where Prestamo.pres_id = inserted.pres_id
GO
create trigger SaldoActualDelete
on Pago after delete
as 
    update Prestamo
	set saldo_actual = saldo_actual + deleted.cantidad
	from Prestamo, deleted
	where Prestamo.pres_id = deleted.pres_id