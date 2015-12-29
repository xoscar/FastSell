CREATE TABLE [dbo].[Prestamo] (
    [pres_id]           INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]          FLOAT (53)    NULL,
    [saldo_actual]      FLOAT (53)    NULL,
    [fecha_inicio]      DATE          NULL,
    [fecha_vencimiento] DATE          NULL,
    [pago_semanal]      FLOAT (53)    NULL,
    [semanas]           INT           NULL,
    [semana_actual]     INT           NULL,
    [pagare]            VARCHAR (100) NULL,
    [descripcion]       TEXT          NULL,
    [loquidado]         BIT           NULL,
    [cli_id]            VARCHAR (5)   NULL,
    [usr_id]            VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([pres_id] ASC),
    FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]),
    FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login])
);


GO
create trigger SaldoActualPres
on Prestamo for update
as
if(update(cantidad))
	update Prestamo 
	set saldo_actual = Prestamo.saldo_actual + (inserted.cantidad - Prestamo.cantidad) 
	from Prestamo, inserted
	where Prestamo.pres_id = inserted.pres_id