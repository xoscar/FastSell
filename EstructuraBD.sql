create database pruebaV2

drop database pruebaV2

use pruebaV2

----------Ususarios

CREATE TABLE Usuario
(
  usr_nombre varchar(255) NOT NULL,
  usr_login VARCHAR(255) NOT NULL,
  usr_pwd VARCHAR(255) NOT NULL,
  
  tipo char

  primary key(usr_login)
)
---------------

insert into Usuario values('Rosio','papaya@123',NULL)

select * from Usuario

-------------------------------------------------------------------------------------------------

--Estructura tabla Cliente

create table Cliente
(
	cli_id varchar(5), 
	nombre varchar(30), 
	apellido varchar(50), 
	direccion varchar(70), 
	fecha_alta date,
	tel varchar(15),

	usr_id varchar(255),

	primary key(cli_id)
)

alter table Cliente add foreign key(usr_id) references Usuario(usr_login)

drop table Cliente

alter table Cliente 



--Estructura tabla Prestamo

create table Prestamo
(
	pres_id int identity,
	cantidad float,
	saldo_actual float,
	fecha_inicio date,
	fecha_vencimiento date,
	pago_semanal float, 
	semanas int,
	semana_actual int, 
	pagare varchar(100), 
	descripcion text,
	loquidado bit,
	
	cli_id varchar(5),
	usr_id varchar(255), 
	primary key (pres_id)
)

drop table Prestamo

alter table Prestamo add foreign key(cli_id) references Cliente(cli_id)
alter table Prestamo add foreign key(usr_id) references Usuario(usr_login)

------Estructura tabla Cargo Extra
create table Cargo
(
	cargo_id int identity,
	cantidad float,
	fecha_inicio date,
	fecha_vencimiento date, 
	semanas int,
	semana_actual int,  
	descripcion text,
	loquidado bit,
	
	cli_id varchar(5),
	usr_id varchar(255), 
	primary key (cargo_id)
)
drop table Cargo

alter table Cargo add foreign key(cli_id) references Cliente(cli_id)
alter table Cargo add foreign key(usr_id) references Usuario(usr_login)

--Estructura tabla Pago

create table Pago
(
	pago_id int identity, 
	cantidad float, 
	fecha datetime,
	 
	pres_id int, 
	cli_id varchar(5),
	usr_id varchar(255),
	 
	primary key(pago_id)
)

drop table Pago

--foreing keys

alter table Pago add foreign key(pres_id) references Prestamo(pres_id)
alter table Pago add foreign key(cli_id) references Cliente(cli_id)
alter table Pago add foreign key(usr_id) references Usuario(usr_login)

-----Estructura tabla Multas-----------------------
create table Multa
(
	multa_id int identity,
	cantidad int,
	fecha datetime,
	semana int,
	descripcion varchar(255),

	pres_id int,
	cli_id varchar(5),
	usr_id varchar(255),

	primary key(multa_id)
)
drop table Multa

alter table Multa add foreign key(pres_id) references Prestamo(pres_id)
alter table Multa add foreign key(cli_id) references Cliente(cli_id)
alter table Multa add foreign key(usr_id) references Usuario(usr_login)


--Consultas
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

drop trigger SaldoActualSumar
drop trigger SaldoActual

-----------------
select * from Usuario 

-------------
select * from Cliente

delete from Cliente 

----------
select * from Prestamo

------------
select* from Cargo

----------
select * from Pago

---------------------
select * from Multa

--------------------

delete from Cargo

delete from Prestamo

delete from Pago

delete from Multa
---------------------

insert into Cliente values(1, 'Oscar', 'Reyes Gaucin', 'Avenida Libertad #124 Fracc. Santa Rosa',SYSDATETIME(),'1591280')

insert into Prestamo values(1, 1500,15,'','dejo una computadora',1,SYSDATETIME(), 1500)

insert into Prestamo values(2, 1500,15,'','dejo una computadora',1,SYSDATETIME(), 1500)


insert into Pago  values(2,200,SYSDATETIME(),1,1)

select nombre from Cliente where id = (select id from Pago where cantidad = 200)

select count(*) from Prestamo where id_Cliente  in ( select id from Cliente where nombre = 'Oscar')


select count(*) from Pago where id_Prestamo = (select id from Prestamo where id  = 1)

update pago set cantidad = 750 where id = 1

update pago set fecha = SYSDATETIME() where id = 1

exec insert_usuario 'Rosio Gaucin','Rosio','papaya123','A';

declare @fecha datetime
set @fecha = GETDATE()

exec insert_cliente 'ana','villalpando','gomez palacio',@fecha,'1591280','Rosio';

exec insert_prestamo 500, 50,12,'C:','videocamara','osre1','Rosio',0


declare @fecha datetime
set @fecha = GETDATE()

exec insert_pago 50,@fecha,1,'osre1','Rosio'

declare @fecha datetime
set @fecha = GETDATE()

exec insert_multa 50, @fecha,1,'se le olvido',3,'osre1','Rosio'

exec transferir_liquidado 3

select SUM(saldo_actual) from Prestamo where cli_id = 'maju1'

select * from Cliente where cli_id = 'oscar'

select * from Pago where pres_id = 1 
 
update Prestamo set cantidad = 1700,semanas = 13 where pres_id = 1

select Prestamo.cantidad - (select SUM(cantidad) from Pago where pres_id = 17)
from Prestamo,Pago
where Prestamo.pres_id = Pago.pres_id



 select SUM(cantidad) from Pago where pres_id = 17

delete from Prestamo where loquidado = 1

select pres_id, cantidad, fecha_inicio, fecha_vencimiento, usr_id from Prestamo where loquidado = 1 and cli_id = 'osre1'

exec insert_cargo 500,10,'cargo','osre1','Rosio'

declare @fecha datetime
set @fecha = GETDATE()

exec insert_multa 50,@fecha,1,'hola',1,'osre1','Rosio'
delete from Usuario where usr_nombre = '';

select * from Prestamo where pres_id = 11 and loquidado = 0;