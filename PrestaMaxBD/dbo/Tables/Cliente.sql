CREATE TABLE [dbo].[Cliente] (
    [cli_id]     VARCHAR (5)   NOT NULL,
    [nombre]     VARCHAR (30)  NULL,
    [apellido]   VARCHAR (50)  NULL,
    [direccion]  VARCHAR (70)  NULL,
    [fecha_alta] DATE          NULL,
    [tel]        VARCHAR (15)  NULL,
    [usr_id]     VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([cli_id] ASC),
    FOREIGN KEY ([usr_id]) REFERENCES [dbo].[Usuario] ([usr_login])
);

