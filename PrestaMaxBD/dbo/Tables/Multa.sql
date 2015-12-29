CREATE TABLE [dbo].[Multa] (
    [multa_id]    INT           IDENTITY (1, 1) NOT NULL,
    [cantidad]    INT           NULL,
    [fecha]       DATETIME      NULL,
    [semana]      INT           NULL,
    [descripcion] VARCHAR (255) NULL,
    [pres_id]     INT           NULL,
    [cli_id]      VARCHAR (5)   NULL,
    [usr_id]      VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([multa_id] ASC),
    FOREIGN KEY ([cli_id]) REFERENCES [dbo].[Cliente] ([cli_id]),
    FOREIGN KEY ([pres_id]) REFERENCES [dbo].[Prestamo] ([pres_id]),
    FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login])
);

