-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 04-01-2016 a las 06:21:40
-- Versión del servidor: 5.6.17
-- Versión de PHP: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `prestamax_db`
--

DELIMITER $$
--
-- Procedimientos
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `AbonosMes`()
begin
select SUM(cantidad)
from Abono
where fecha between DATEADD(month,-1,fecha) and NOW()
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `AbonosTotal`(
p_fecha date)
begin
select SUM(cantidad)
from Abono
where  CAST(DATE_FORMAT(fecha,112) as date) = p_fecha
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `BuscarIdProducto`(
p_cli_id varchar(5),
p_id int)
begin
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = p_cli_id and Producto_Cliente_Precio.prod_id  = p_id
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = p_cli_id) and Producto.prod_id = p_id
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `BuscarNombreProducto`(
p_cli_id varchar(5),
p_nombre varchar(100))
begin
	select Producto.prod_id, nombre_prod, precio, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = p_cli_id and nombre_prod = p_nombre
	union
	select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = p_cli_id) and nombre = p_nombre
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_abono`(IN `p_cantidad` DOUBLE, IN `p_fecha` DATETIME, IN `p_nota_id` INT, IN `p_cli_id` VARCHAR(5), IN `p_usr_login` VARCHAR(255), IN `p_pedido_id` INT)
begin
	insert into Abono values(DEFAULT,p_cantidad,p_fecha,p_nota_id,p_cli_id,p_usr_login,p_pedido_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_cliente`(IN `p_nombre` VARCHAR(30), IN `p_apellido` VARCHAR(50), IN `p_negocio` VARCHAR(100), IN `p_direc` VARCHAR(70), IN `p_fecha_alta` DATE, IN `p_tel` VARCHAR(15), IN `p_user_id` VARCHAR(255))
begin
	declare v_aux_ int;
	declare v_cli_ varchar(5);
	declare v_cli2 varchar(5);
  declare v_cli3 varchar(5);
	set v_cli_ = CONCAT(substring(p_nombre,1,2),substring(p_apellido,1,2), '%');
	set v_cli2 = CONCAT(substring(p_nombre,1,2) , substring(p_apellido,1,2));
	set v_aux_ = (select count(*) + 1  from Cliente where cli_id like v_cli_);
	set v_cli3 = CONCAT(v_cli2 , CONVERT(v_aux_ , char(5)));
	insert into Cliente values(v_cli3, p_nombre, p_apellido, p_negocio, p_direc,p_fecha_alta,p_tel,p_user_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_nota`(IN `p_cantidad` DOUBLE, IN `p_semanas` INT, IN `p_nota` VARCHAR(100), IN `p_descripcion` LONGTEXT, IN `p_liquidado` TINYINT, IN `p_cli_id` VARCHAR(5), IN `p_user` VARCHAR(255), IN `p_pedido_id` INT)
begin
	declare v_fec_ date;
	set v_fec_ = DATE_ADD(now(),INTERVAL p_semanas week);
	insert into Nota values(DEFAULT,p_cantidad,p_cantidad,now(),v_fec_,p_semanas,p_nota,p_descripcion,p_liquidado,null,p_cli_id,p_user,p_pedido_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_pedido`(IN `p_precio_total` DOUBLE, IN `p_usr_id` VARCHAR(255), IN `p_cli_id` VARCHAR(5), IN `p_repar_id` VARCHAR(5), IN `p_contado` TINYINT, IN `p_fecha_entrega` DATETIME)
begin
	declare v_fec_ datetime(3);
	set v_fec_ = NOW();
	insert into Pedido values(DEFAULT,v_fec_,p_fecha_entrega,p_precio_total,0,p_contado,p_usr_id,p_cli_id,p_repar_id, null);
	select pedido_id from Pedido where fecha_ped = v_fec_ and cli_id = p_cli_id
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_pedido_producto`(
p_cantidad int,
p_prod_id int,
p_nombre_prod varchar(100),
p_pedido_id int,
p_precio double)
begin
	insert into Pedido_Producto values(p_cantidad,p_prod_id,p_nombre_prod,p_pedido_id,p_precio)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_producto`(IN `p_nombre` VARCHAR(100), IN `p_precio` DOUBLE, IN `p_cantidad` INT, IN `p_tipo_acum` INT, IN `p_descripcion` LONGTEXT, IN `p_usr_id` VARCHAR(255))
begin 
	insert into Producto values(DEFAULT,p_nombre,p_precio,p_cantidad,p_tipo_acum,now(),p_descripcion,p_usr_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_producto_cliente_precio`(IN `p_precio` DOUBLE, IN `p_prod_id` INT, IN `p_nombre_prod` VARCHAR(100), IN `p_cli_id` VARCHAR(5))
begin
	insert into Producto_Cliente_Precio values(DEFAULT,p_precio,p_prod_id,p_nombre_prod,p_cli_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_proveedor`(IN `p_nombre` VARCHAR(50), IN `p_apellido` VARCHAR(50), IN `p_negocio` VARCHAR(100), IN `p_telefono` VARCHAR(20), IN `p_direccion` VARCHAR(255))
begin
	insert into Proveedor values(DEFAULT,p_nombre,p_apellido,p_negocio,p_telefono,p_direccion)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_repartidor`(IN `p_nombre` VARCHAR(20), IN `p_apellido` VARCHAR(30), IN `p_direccion` VARCHAR(255), IN `p_telefono` VARCHAR(15), IN `p_usr_id` VARCHAR(255))
begin
	declare v_aux_ int;
	declare v_cli_ varchar(5);
	declare v_cli2 varchar(5);
    declare v_cli3 varchar(5);
	set v_cli_ = CONCAT(substring(p_nombre,1,2),substring(p_apellido,1,2), '%');
	set v_cli2 = CONCAT(substring(p_nombre,1,2) , substring(p_apellido,1,2));
	set v_aux_ = (select (count(*) + 1)  from Repartidor where repar_id like v_cli_);
	set v_cli3 = CONCAT(v_cli2 , CONVERT(v_aux_ , char(5)));
	insert into Repartidor values(v_cli3,p_nombre,p_apellido,p_direccion,p_telefono,now(),p_usr_id)	
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_usuario`(
p_nombre varchar(255),
p_login varchar(255),
p_pswd varchar(255),
p_tipo char)
begin
	if(p_login not in (select usr_login from Usuario)) then
		insert into Usuario values(p_nombre,p_login,p_pswd,0,p_tipo);

	end if;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `PedidosFechaEntrega`(
p_fecha_entrega date)
begin
	select * from Pedido where CAST(DATE_FORMAT(fecha_entrega,112) as date) = p_fecha_entrega and liquidado = 0
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `PedidosFechaPedido`(
p_fecha_pedido date)
begin
	select * from Pedido where CAST(DATE_FORMAT(fecha_ped,112) as date) = p_fecha_pedido and liquidado = 0
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `PedidosMes`()
begin
	select SUM(precio_total)
	from Pedido
	where contado = 0 and fecha_ped between  DATEADD(month,-1,fecha_ped) and NOW()
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `PreciosCliente`(
p_cli_id varchar(5))
begin
	select precio, Producto.prod_id, nombre_prod, Producto.cantidad, Producto.tipo_acum  from Producto_Cliente_Precio left join Producto on Producto.prod_id =Producto_Cliente_Precio.prod_id where cli_id = p_cli_id 
	union
	select precio_general, prod_id, nombre, cantidad, tipo_acum from Producto where prod_id not in( select prod_id from Producto_Cliente_Precio where cli_id = p_cli_id)
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ProductosPedido`(
p_pedido_id int)
begin
	select Pedido_Producto.prod_id, Pedido_Producto.precio, nombre_prod, Pedido_Producto.cantidad, Producto.tipo_acum from Pedido_Producto 
	left join Producto on Pedido_Producto.prod_id = Producto.prod_id where Pedido_Producto.pedido_id = p_pedido_id 
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `semanas_restantes_prestamo`(
p_id_nota int)
begin
	select TIMESTAMPDIFF(week, now(), fecha_vencimiento)
	from Nota
	where nota_id = p_id_nota
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `semana_actual_prestamo`(
p_id_nota int)
begin
	select timestampdiff(week, fecha_inicio, now())
	from Nota
	where nota_id = p_id_nota
;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `VentaTotalPedidos`(
p_fecha date)
begin
select SUM(precio_total)
from Pedido
where contado = 0 and CAST(DATE_FORMAT(fecha_ped,112) as date)  = p_fecha
;
end$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `abono`
--

CREATE TABLE IF NOT EXISTS `abono` (
  `abono_id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidad` double DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  `nota_id` int(11) DEFAULT NULL,
  `cli_id` varchar(5) DEFAULT NULL,
  `usr_id` varchar(255) DEFAULT NULL,
  `pedido_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`abono_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Disparadores `abono`
--
DROP TRIGGER IF EXISTS `SaldoActual`;
DELIMITER //
CREATE TRIGGER `SaldoActual` AFTER INSERT ON `abono`
 FOR EACH ROW begin 
        	update Nota 
            set Nota.saldo_actual = Nota.cantidad - (select SUM(cantidad) from Abono where nota_id = NEW.nota_id) 
            where Nota.nota_id = NEW.nota_id; 
        end
//
DELIMITER ;
DROP TRIGGER IF EXISTS `SaldoActualDelete`;
DELIMITER //
CREATE TRIGGER `SaldoActualDelete` AFTER DELETE ON `abono`
 FOR EACH ROW begin
    update Nota
	set saldo_actual = saldo_actual + old.cantidad
	where Nota.nota_id  = old.nota_id;
    end
//
DELIMITER ;
DROP TRIGGER IF EXISTS `SaldoActualSumar`;
DELIMITER //
CREATE TRIGGER `SaldoActualSumar` AFTER UPDATE ON `abono`
 FOR EACH ROW begin
	if new.cantidad <> old.cantidad
    	then
            update Nota 
            set saldo_actual = Nota.cantidad - (select SUM(cantidad) from Abono where nota_id = new.nota_id)
            where Nota.nota_id = new.nota_id;
    end if;
    end
//
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

CREATE TABLE IF NOT EXISTS `cliente` (
  `cli_id` varchar(5) NOT NULL,
  `nombre` varchar(30) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `negocio` varchar(100) DEFAULT NULL,
  `direccion` varchar(70) DEFAULT NULL,
  `fecha_alta` date DEFAULT NULL,
  `tel` varchar(15) DEFAULT NULL,
  `usr_id` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`cli_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `cliente`
--

INSERT INTO `cliente` (`cli_id`, `nombre`, `apellido`, `negocio`, `direccion`, `fecha_alta`, `tel`, `usr_id`) VALUES
('Osos1', 'Oscar', 'oscar', 'oscar', 'direccion', '2015-12-29', '(123)123-5115', '1'),
('Osos2', 'Oscar', 'oscar', 'oscar', 'direccion', '2015-12-29', '123123', '1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `nota`
--

CREATE TABLE IF NOT EXISTS `nota` (
  `nota_id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidad` double DEFAULT NULL,
  `saldo_actual` double DEFAULT NULL,
  `fecha_inicio` datetime DEFAULT NULL,
  `fecha_vencimiento` datetime DEFAULT NULL,
  `semanas` int(11) DEFAULT NULL,
  `nota` varchar(100) DEFAULT NULL,
  `descripcion` text,
  `liquidado` tinyint(4) DEFAULT NULL,
  `fecha_liquidacion` date DEFAULT NULL,
  `cli_id` varchar(5) DEFAULT NULL,
  `usr_id` varchar(255) DEFAULT NULL,
  `pedido_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`nota_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Disparadores `nota`
--
DROP TRIGGER IF EXISTS `SaldoActualPres`;
DELIMITER //
CREATE TRIGGER `SaldoActualPres` AFTER UPDATE ON `nota`
 FOR EACH ROW begin
if new.cantidad <> old.cantidad then
	update Nota 
	set saldo_actual = Nota.saldo_actual + (new.cantidad - Nota.cantidad) 
	where Nota.nota_id = new.nota_id;
end if;
    end
//
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedido`
--

CREATE TABLE IF NOT EXISTS `pedido` (
  `pedido_id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_ped` datetime DEFAULT NULL,
  `fecha_entrega` datetime DEFAULT NULL,
  `precio_total` double DEFAULT NULL,
  `liquidado` tinyint(4) DEFAULT NULL,
  `contado` tinyint(4) DEFAULT NULL,
  `usr_id` varchar(255) DEFAULT NULL,
  `cli_id` varchar(5) DEFAULT NULL,
  `repar_id` varchar(5) DEFAULT NULL,
  `fecha_liquidacion` datetime DEFAULT NULL,
  PRIMARY KEY (`pedido_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedido_producto`
--

CREATE TABLE IF NOT EXISTS `pedido_producto` (
  `cantidad` int(11) DEFAULT NULL,
  `prod_id` int(11) NOT NULL,
  `nombre_prod` varchar(100) DEFAULT NULL,
  `pedido_id` int(11) NOT NULL,
  `precio` double DEFAULT NULL,
  PRIMARY KEY (`prod_id`,`pedido_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto`
--

CREATE TABLE IF NOT EXISTS `producto` (
  `prod_id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `precio_general` double NOT NULL,
  `cantidad` int(11) DEFAULT NULL,
  `tipo_acum` int(11) NOT NULL,
  `fecha_ingreso` date NOT NULL,
  `descripcion` text,
  `usr_id` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`prod_id`,`nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `producto_cliente_precio`
--

CREATE TABLE IF NOT EXISTS `producto_cliente_precio` (
  `pcp_id` int(11) NOT NULL AUTO_INCREMENT,
  `precio` double DEFAULT NULL,
  `prod_id` int(11) NOT NULL,
  `nombre_prod` varchar(100) DEFAULT NULL,
  `cli_id` varchar(5) NOT NULL,
  PRIMARY KEY (`pcp_id`,`prod_id`,`cli_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proveedor`
--

CREATE TABLE IF NOT EXISTS `proveedor` (
  `prov_id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `appellido` varchar(50) DEFAULT NULL,
  `ne;cio` varchar(100) DEFAULT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`prov_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `repartidor`
--

CREATE TABLE IF NOT EXISTS `repartidor` (
  `repar_id` varchar(5) NOT NULL,
  `nombre` varchar(20) DEFAULT NULL,
  `apellido` varchar(30) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `telefono` varchar(15) DEFAULT NULL,
  `fecha_alta` date DEFAULT NULL,
  `usr_id` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`repar_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `repartidor`
--

INSERT INTO `repartidor` (`repar_id`, `nombre`, `apellido`, `direccion`, `telefono`, `fecha_alta`, `usr_id`) VALUES
('Osos1', 'Oscar', 'oscar', 'direccion', '123123', '2015-12-29', '1'),
('Osos2', 'Oscar', 'oscar', 'direccion', '123123', '2015-12-29', '1');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE IF NOT EXISTS `usuario` (
  `usr_nombre` varchar(255) NOT NULL,
  `usr_login` varchar(255) NOT NULL,
  `usr_pwd` varchar(255) NOT NULL,
  `eliminado` tinyint(4) NOT NULL,
  `tipo` varchar(1) NOT NULL,
  PRIMARY KEY (`usr_login`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`usr_nombre`, `usr_login`, `usr_pwd`, `eliminado`, `tipo`) VALUES
('oscar', 'oscar', 'oscar', 0, 'A');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
