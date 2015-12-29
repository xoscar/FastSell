CREATE TABLE [dbo].[Cargo] (
    [cargo_id]          INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]          FLOAT (53)    NULL,
    [fecha_inicio]      DATE          NULL,
    [fecha_vencimiento] DATE          NULL,
    [semanas]           INT           NULL,
    [semana_actual]     INT           NULL,
    [descripcion]       TEXT          NULL,
    [loquidado]         BIT           NULL,
    [cli_id]            VARCHAR (5)   NULL,
    [usr_id]            VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([cargo_id] ASC),
    FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]),
    FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login])
);

