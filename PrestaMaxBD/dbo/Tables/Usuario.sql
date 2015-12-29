CREATE TABLE [dbo].[Usuario] (
    [usr_nombre] VARCHAR (255) NOT NULL,
    [usr_login]  VARCHAR (255) NOT NULL,
    [usr_pwd]    VARCHAR (255) NOT NULL,
    [tipo]       CHAR (1)      NULL,
    PRIMARY KEY CLUSTERED ([usr_login] ASC)
);

