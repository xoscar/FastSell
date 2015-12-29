using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Fast_SellX.Properties;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlTypes;

namespace Fast_SellX
{
    public class Conexion
    {
        SqlConnection _conexion;
        bool _conectada;
        SqlCommand _comando;
        SqlDataReader _reader;

        //Constructores

        public Conexion()
        {
            _conexion = new SqlConnection();
            _conexion.ConnectionString = Settings.Default.PruebaConnectionString; //"data source = OFICINA-PC\\OSCAR; initial catalog = pruebaV2; MultipleActiveResultSets = True; user id = sa; password = system62";
            _conectada = false;
        }

        public Conexion(string co)
        {
            _conexion = new SqlConnection();
            _conexion.ConnectionString = co;
            _conectada = false;
        }

        //Metodos
        //Abrir y cerrar conexion
        public string Abrir()
        {
            try
            {
                _conexion.Open();
                _conectada = true;
                return "conexion establecida";
            }
            catch (Exception ex)
            {
                _conexion.Close();
                _conectada = false;
                return ex.Message + "conexion no completada";
            }
        }

        public bool Cerrar()
        {
            try
            {
                _conexion.Close();
                _conectada = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //propiedades

        public bool Conectada
        {
            get { return _conectada; }
        }

        //Funciones especificas a la base de datos
        //verifica un usuario de la BD para logear en el sistema
        public bool VerificarUsuario(string login, string password, ref string res, ref Usuario us)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                us = new Usuario();
                return false;
            }
            else
            {
                bool _encontrado = false;
                string _consulta = "select * from Usuario where eliminado != 1";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();

                while (_reader.Read())
                {
                    if (_reader.GetValue(1).ToString() == login && _reader.GetValue(2).ToString() == password)
                    {
                        switch (Convert.ToChar(_reader.GetValue(4)))
                        {
                            case 'A':
                                us = new Usuario(_reader.GetValue(1).ToString(), _reader.GetValue(0).ToString(), 0, _reader.GetValue(2).ToString(), false);
                                break;
                            case 'U':
                                us = new Usuario(_reader.GetValue(1).ToString(), _reader.GetValue(0).ToString(), 1, _reader.GetValue(2).ToString(), false);
                                break;
                        }
                        _encontrado = true;
                    }
                }
                if (_encontrado)
                {
                    res = "Sesion iniciada: " + us.Nombre;
                    return true;
                }
                else
                {
                    res = "Error en el usuario o contraseña";
                    return false;
                }
            }
        }


        //Muestra los clientes en un dgv especifico
        public bool MostrarClientes(DataGridView dgv, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                dgv.Rows.Clear();
                string _consulta = "select cli_id,  nombre, apellido, tel from Cliente";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    double _adeudo = 0.0;
                    string _cliId = _reader.GetValue(0).ToString();
                    string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "' and liquidado = 0";
                    SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                    SqlDataReader _readerAux = _comAux.ExecuteReader();
                    string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '"+_cliId+"' and liquidado = 0";
                    SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                    SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                    _readerPrestamo.Read();
                    int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                    _readerAux.Read();
                    if (_readerAux.GetValue(0) != DBNull.Value)
                        _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                    dgv[0, i].Value = _cliId;
                    dgv[1, i].Value = _reader.GetValue(1);
                    dgv[2, i].Value = _reader.GetValue(2);
                    dgv[3, i].Value = _reader.GetValue(3);
                    dgv[4, i].Value = _adeudo;
                    dgv[5, i].Value = _pedidos;
                    dgv[6, i++].Value = "--->";
                }
                if (i == 0)
                    res = "No hay clientes para cargar";
                else
                    res = "Datos existosamente cargados";
                return true;
            }
        }

        //busca clientes dependiendo categoria y parametro
        public bool BuscCliente(DataGridView dgv, string parametro, int tipo, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                bool _encontrado = false;
                dgv.Rows.Clear();
                switch (tipo)
                {
                    case 0:
                        string _consulta = "select cli_id,  nombre, apellido, tel, usr_id from Cliente where cli_id = '" + parametro + "'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado = _reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _reader.GetValue(3);
                            dgv[4, 0].Value = _adeudo;
                            dgv[5, 0].Value = _pedidos;
                            dgv[6, 0].Value = "--->";
                            res = "cliente encontrado";
                            return true;
                        }
                        else
                        {
                            res = "cliente no encontrado";
                            return false;
                        }

                    case 1:
                        _consulta = "select cli_id,  nombre, apellido,tel, usr_id from Cliente where nombre = '" + parametro + "'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado = _reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _reader.GetValue(3);
                            dgv[4, 0].Value = _adeudo;
                            dgv[5, 0].Value = _pedidos;
                            dgv[6, 0].Value = "--->";
                            res = "cliente encontrado";
                            return true;
                        }
                        else
                        {
                            res = "cliente no encontrado";
                            return false;
                        }

                    case 2:
                        _consulta = "select cli_id,  nombre, apellido, tel, usr_id from Cliente where apellido = '" + parametro + "'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado = _reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _reader.GetValue(3);
                            dgv[4, 0].Value = _adeudo;
                            dgv[5, 0].Value = _pedidos;
                            res = "cliente encontrado";
                            dgv[6, 0].Value = "--->";
                            return true;
                        }
                        else
                        {
                            res = "cliente no encontrado";
                            return false;
                        }
                }
                return false;
            }
        }

        //Ordenamiento segun parametro especifico

        public bool Ordenar(DataGridView dgv, int tipo, ref string res, string desc)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                dgv.Rows.Clear();
                switch (tipo)
                {
                    case 0:
                        string _consulta = "select cli_id,  nombre, apellido, tel, usr_id from Cliente order by cli_id " + desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        int i = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _reader.GetValue(3);
                            dgv[4, i].Value = _adeudo;
                            dgv[5, i].Value = _pedidos;
                            dgv[6, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;

                    case 1:
                        _consulta = "select cli_id,  nombre, apellido, tel, usr_id from Cliente order by nombre " + desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        i = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _reader.GetValue(3);
                            dgv[4, i].Value = _adeudo;
                            dgv[5, i].Value = _pedidos;
                            dgv[6, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;

                    case 2:
                        _consulta = "select cli_id,  nombre, apellido, tel, usr_id from Cliente order by apellido " + desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        i = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Nota where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            string _pedido = "select count(*) from Pedido where fecha_ped <= GETDATE() and cli_id = '" + _cliId + "' and liquidado = 0";
                            SqlCommand _comPrestamo = new SqlCommand(_pedido, _conexion);
                            SqlDataReader _readerPrestamo = _comPrestamo.ExecuteReader();
                            _readerPrestamo.Read();
                            int _pedidos = Convert.ToInt32(_readerPrestamo.GetValue(0));
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _reader.GetValue(3);
                            dgv[4, i].Value = _adeudo;
                            dgv[5, i].Value = _pedidos;
                            dgv[6, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;
                }
                res = "Carga Fallida";
                return false;
            }
        }

        //Mostrar Pedidos
        public bool MostrarPedidos(DataGridView dgv, string cli_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pedido_id, precio_total, fecha_ped, fecha_entrega, contado, repar_id, usr_id  from Pedido where cli_id = '" + cli_id + "' and liquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 7; j++)
                    {
                        if (j == 4)
                        {
                            if (Convert.ToInt32(_reader.GetValue(j)) == 0)
                                dgv[j, i].Value = "SI";
                            else
                                dgv[j, i].Value = "NO";
                        }
                        else
                            dgv[j, i].Value = _reader.GetValue(j).ToString();
                    }
                    dgv[7, i].Value = "...";
                    dgv[8, i++].Value = "***";
                }
                res = "Pedidos Cargados con Exito";
                return true;
            }
        }

        //Atrapar Cliente
        public bool AtraparCliente(string cli_id, ref Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                ArrayList prod = new ArrayList();
                string _consulta = "select * from Cliente where cli_id = '" + cli_id + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                string _precios = "PreciosCliente";
                SqlCommand _comPre = new SqlCommand(_precios, _conexion);
                _comPre.CommandType = CommandType.StoredProcedure;
                _comPre.Parameters.AddWithValue("@cli_id", cli_id);
                SqlDataReader _rePre = _comPre.ExecuteReader();
                while (_rePre.Read())
                {
                    Producto _producto = new Producto();
                    _producto.Precio_General = Convert.ToDouble(_rePre.GetValue(0));
                    _producto.Id_Producto = Convert.ToInt32(_rePre.GetValue(1));
                    _producto.Nombre = _rePre.GetValue(2).ToString();
                    _producto.Cantidad = Convert.ToInt32(_rePre.GetValue(3));
                    _producto.Tipo = Convert.ToInt32(_rePre.GetValue(4));
                    prod.Add(_producto);
                }

                cli = new Cliente();
                cli.Id_Cliente = _reader.GetString(0);
                cli.Nombre = _reader.GetString(1);
                cli.Apellido = _reader.GetString(2);
                cli.Negocio = _reader.GetString(3);
                cli.Direccion = _reader.GetString(4);
                cli.Fecha_Alta = Convert.ToDateTime(_reader.GetValue(5));
                cli.Telefono = _reader.GetValue(6).ToString();
                cli.Id_Usuario = _reader.GetString(7);
                cli.Precios = prod;
               
                res = "Cliente cargado con exito";
                return true;
            }
        }

        //Mostrar Notas en dgv
        public bool MostrarNotas(DataGridView dgv, string cli_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select nota_id, cantidad, saldo_actual, fecha_vencimiento, pedido_id from Nota where cli_id =  '" + cli_id + "' and liquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 5; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[5, i].Value = "+";
                    dgv[6, i].Value = "...";
                    dgv[7, i++].Value = "***";
                }
                res = "Notas Cargadas con Exito";
                return true;
            }
        }

        //Modificar Cliente
        public bool ModificarCliente(Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Cliente set nombre = '" + cli.Nombre
                    + "', apellido = '" + cli.Apellido
                    + "', direccion = '" + cli.Direccion
                    + "', tel = '" + cli.Telefono + "', negocio = '"+cli.Negocio+"' where  cli_id = '" + cli.Id_Cliente + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Cliente modificado: " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }

        //Agregar Precio Especial
        public bool AgregarPrecio(Cliente cli, string nombre, int prod_id, double precio, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Producto_Cliente_Precio where nombre_prod = '" + nombre + "' and cli_id = '" + cli.Id_Cliente + "' and prod_id = "+prod_id;
                bool _encontrado = false;
                _comando = new SqlCommand(_consulta,_conexion);
                _reader = _comando.ExecuteReader();
                _encontrado = _reader.Read();
                if (_encontrado)
                {
                    _consulta = "update Producto_Cliente_Precio set precio =  @cantidad where nombre_prod = '" + nombre + "'  and prod_id = "+prod_id;
                    SqlCommand _comAux = new SqlCommand(_consulta, _conexion);
                    _comAux.Parameters.Add("@cantidad", SqlDbType.Money).Value = Convert.ToDouble(precio);
                    _comAux.ExecuteNonQuery();
                    res = "Precio Modificado";
                    return true;
                }
                else
                {
                    _consulta = "select prod_id from Producto where nombre = '"+nombre+"' and prod_id = "+prod_id;
                    SqlCommand _comando2 = new SqlCommand(_consulta,_conexion);
                    SqlDataReader _reader2 = _comando2.ExecuteReader();
                    _reader2.Read();
                    prod_id = Convert.ToInt32(_reader2.GetValue(0));
                    _consulta = "insert_producto_cliente_precio";
                    SqlCommand _comAux = new SqlCommand(_consulta, _conexion);
                    _comAux.CommandType = CommandType.StoredProcedure;
                    _comAux.Parameters.AddWithValue("@precio", precio);
                    _comAux.Parameters.AddWithValue("@prod_id", prod_id);
                    _comAux.Parameters.AddWithValue("@nombre_prod", nombre);
                    _comAux.Parameters.AddWithValue("@cli_id", cli.Id_Cliente);
                    _comAux.ExecuteNonQuery();
                    res = "Precio Agregado";
                    return true;
                }
            }
        }

        //Atrapar Pedido
        public bool AtraparPedido(int ped_id, ref Pedido ped, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from pedido where pedido_id = " + ped_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                ped = new Pedido();
                ped.Id_Pedido = Convert.ToInt32(_reader.GetValue(0));
                ped.Fecha_Pedido = Convert.ToDateTime(_reader.GetValue(1));
                ped.Fecha_Entrega = Convert.ToDateTime(_reader.GetValue(2));
                ped.Precio_Total = Convert.ToDouble(_reader.GetValue(3));
                ped.Liquidado = Convert.ToBoolean(_reader.GetValue(4));
                ped.Contado = Convert.ToBoolean(_reader.GetValue(5));
                if(_reader.GetValue(9) != DBNull.Value)
                    ped.Fecha_Liquidacion = Convert.ToDateTime(_reader.GetValue(9));
                ped.Id_Usuario = _reader.GetValue(6).ToString();
                ped.Id_Cliente = _reader.GetValue(7).ToString();
                ped.Id_Repartidor = _reader.GetValue(8).ToString();
                _consulta = "ProductosPedido";
                ArrayList _productos = new ArrayList();
                SqlCommand _comAux = new SqlCommand(_consulta, _conexion);
                _comAux.CommandType = CommandType.StoredProcedure;
                _comAux.Parameters.AddWithValue("@pedido_id", ped_id);
                SqlDataReader _readerAux = _comAux.ExecuteReader();
                while (_readerAux.Read())
                {
                    Producto _aux = new Producto();
                    _aux.Id_Producto = Convert.ToInt32(_readerAux.GetValue(0));
                    _aux.Nombre = _readerAux.GetValue(2).ToString();
                    _aux.Precio_General = Convert.ToDouble(_readerAux.GetValue(1));
                    _aux.Cantidad = Convert.ToInt32(_readerAux.GetValue(3));
                    _aux.Tipo = Convert.ToInt32(_readerAux.GetValue(4));
                    _productos.Add(_aux);
                }
                ped.Productos = _productos;
                res = "Mostrado con exito";
                return true;
            }
        }   

        //Atrapar Producto
        public bool AtraparProducto(int prod_id, ref Producto prod, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select nombre, precio_general, cantidad, tipo_acum, fecha_ingreso, descripcion, usr_id from Producto where prod_id =" + prod_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                prod = new Producto();
                prod.Id_Producto = prod_id;
                prod.Nombre = _reader.GetValue(0).ToString();
                prod.Precio_General = Convert.ToDouble(_reader.GetValue(1));
                prod.Cantidad = Convert.ToInt32(_reader.GetValue(2));
                prod.Tipo = Convert.ToInt32(_reader.GetValue(3));
                prod.Fecha_Ingreso = Convert.ToDateTime(_reader.GetValue(4));
                prod.Descripcion = _reader.GetValue(5).ToString();
                prod.Id_User = _reader.GetValue(6).ToString();
                res = "Cargado con exito";
                return true;
            }
        }

        //Id repartidores en combobox

        public bool RepartidorCombo(ComboBox cb, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select repar_id from Repartidor";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                cb.Items.Clear();
                while(_reader.Read())
                {
                    cb.Items.Add(_reader.GetValue(0));
                }
                res = "Cargados con exito";
                return true;
            }                
        }

        //Insertar pedido
        public bool AgregarPedido(Pedido ped, ref string res, ref int ped_id)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_pedido";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@precio_total", ped.Precio_Total);
                _comando.Parameters.AddWithValue("@usr_id", ped.Id_Usuario);
                _comando.Parameters.AddWithValue("@cli_id", ped.Id_Cliente);
                _comando.Parameters.AddWithValue("@repar_id", ped.Id_Repartidor);
                _comando.Parameters.AddWithValue("@contado", (ped.Contado == true?0:1));
                _comando.Parameters.AddWithValue("@fecha_entrega", ped.Fecha_Entrega);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                ped_id = Convert.ToInt32(_reader.GetValue(0));
                for (int i = 0; i < ped.Productos.Count; i++)
                {
                    Producto _aux = ((Producto)ped.Productos[i]);
                    if(_aux.Id_Producto != 0)
                    {
                        _consulta = "insert_pedido_producto";
                        SqlCommand _comPed = new SqlCommand(_consulta, _conexion);
                        _comPed.CommandType = CommandType.StoredProcedure;
                        _comPed.Parameters.AddWithValue("@cantidad", _aux.Cantidad);
                        _comPed.Parameters.AddWithValue("@prod_id", _aux.Id_Producto);
                        _comPed.Parameters.AddWithValue("@nombre_prod", _aux.Nombre);
                        _comPed.Parameters.AddWithValue("@pedido_id", ped_id);
                        _comPed.Parameters.AddWithValue("@precio", _aux.Precio_General);
                        _comPed.ExecuteNonQuery();
                    }
                }
                res = "Prestamo insertado con exito";
                return true;
            }
        }

        //Update producto
        public bool ModificarProductoCantidad(Producto prod, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Producto set cantidad = "+prod.Cantidad+ " where prod_id = "+prod.Id_Producto;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "exito";
                return true;
            }
        }

        //Insertar Nota
        public bool AgregarNota(Nota no, ref string res)
        {

            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_nota";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@cantidad", no.Cantidad);
                _comando.Parameters.AddWithValue("@semanas", no.Semanas);
                _comando.Parameters.AddWithValue("@nota", no.Pagare);
                _comando.Parameters.AddWithValue("@descripcion", no.Descripcion);
                _comando.Parameters.AddWithValue("@liquidado", 0);
                _comando.Parameters.AddWithValue("@cli_id", no.Id_Cliente);
                _comando.Parameters.AddWithValue("@user", no.Id_User);
                _comando.Parameters.AddWithValue("@pedido_id", no.Id_Pedido);
                _comando.ExecuteNonQuery();
                res = "exito";
                return true;
            }       
        }

        //Liquidar pedido
        public bool LiquidarPedido(Pedido ped, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Pedido set liquidado = 1, fecha_liquidacion = '"+ DateTime.Now.ToShortDateString() + "' where pedido_id = " + ped.Id_Pedido;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Prestamo liquidado con exito";
                return true;
            }
        }

        //Mostrar pedidos liquidados en dgv
        public bool MostrarPedidosLiquidados(DataGridView dgv, Cliente cli, ref string res )
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pedido_id, fecha_ped, fecha_entrega, fecha_liquidacion, cli_id from Pedido where cli_id = '" + cli.Id_Cliente + "'  and liquidado = 1";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for(int j = 0; j < 5; j++)
                    {
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    }
                    dgv[5, i++].Value = "...";
                }
                res = "exito";
                return true;
            }       
        }

        //Atrapar Nota
        public bool AtraparNota(int nota_id, ref Nota no, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Nota where nota_id = " + nota_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                no.Id_Nota = nota_id;
                no.Cantidad = Convert.ToDouble(_reader.GetValue(1));
                no.Fecha_Inicio = Convert.ToDateTime(_reader.GetValue(3));
                no.Fecha_Vencimiento = Convert.ToDateTime(_reader.GetValue(4));
                no.Semanas = Convert.ToInt32(_reader.GetValue(5));
                no.Pagare = _reader.GetValue(6).ToString();
                no.Descripcion = _reader.GetValue(7).ToString();
                no.Liquidado = Convert.ToBoolean(_reader.GetValue(8));
                if (_reader.GetValue(9) != DBNull.Value)
                    no.Fecha_Liquidacion = Convert.ToDateTime(_reader.GetValue(9));
                no.Id_Cliente = _reader.GetValue(10).ToString();
                no.Id_User = _reader.GetValue(11).ToString();
                no.Id_Pedido = Convert.ToInt32(_reader.GetValue(12));
                res = "Nota Atrapada";
                return true;
            }
        }

        //Mostrar Notas Liquidada
        public bool MostrarNotasLiquidadas(DataGridView dgv, Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select nota_id, cantidad, fecha_inicio, fecha_vencimiento, fecha_liquidacion from Nota where liquidado = 1 and cli_id = '" + cli.Id_Cliente + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 5; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[5, i++].Value = "...";
                }
                res = "exito";
                return true;
            }    
        }

        //Liquidar Nota
        public bool LiquidarNota(Nota no, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Nota set liquidado = 1, fecha_liquidacion = '"+DateTime.Now.ToShortDateString() +"' where nota_id = " + no.Id_Nota;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Nota Liquidada";
                return true;
            }
        }

        //Agregar Abono
        public bool AgregarAbono(Abono _abo, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_abono";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@cantidad", _abo.Cantidad);
                _comando.Parameters.AddWithValue("@fecha", DateTime.Now);
                _comando.Parameters.AddWithValue("@nota_id", _abo.Id_Nota);
                _comando.Parameters.AddWithValue("@cli_id", _abo.Id_Cliente);
                _comando.Parameters.AddWithValue("@usr_login", _abo.Id_User);
                _comando.Parameters.AddWithValue("@pedido_id", _abo.Id_Pedido);
                _comando.ExecuteNonQuery();
                res = "Abono Agregado";
                return true;
            }
        }

        //Mostrar Abonos en dgv
        public bool MostrarAbonos( DataGridView dgv, Nota nota, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select abono_id, cantidad, fecha from Abono where nota_id = " + nota.Id_Nota;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 3; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[3, i++].Value = "X";
                }

                res = "Exito";
                return true;
            }
        }

        //Modificar Nota
        public bool ModificarNota( string desc, string fecha, string pagare, ref Nota nota, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Nota set descripcion = '" + desc + "', fecha_vencimiento = '" + fecha + "', nota = '" + pagare + "' where nota_id = " + nota.Id_Nota;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();

                nota.Descripcion = desc;
                nota.Fecha_Vencimiento = Convert.ToDateTime(fecha);
                nota.Pagare = pagare;
                res = "Nota Mofidicada";
                return true;
            }
        }

        //Quitar abono
        public bool QuitarAbono(Abono abono, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "delete from Abono where abono_id = " + abono.Id_Abono;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Abono Eliminado";
                return true;
            }
        }

        //Agregar Cliente
        public bool AgregarCliente(Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_cliente";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@nombre", cli.Nombre);
                _comando.Parameters.AddWithValue("@apellido", cli.Apellido);
                _comando.Parameters.AddWithValue("@direc", cli.Direccion);
                _comando.Parameters.AddWithValue("@fecha_alta", DateTime.Now.ToShortDateString());
                _comando.Parameters.AddWithValue("@tel", cli.Telefono);
                _comando.Parameters.AddWithValue("@user_id", cli.Id_Usuario);
                _comando.Parameters.AddWithValue("@negocio", cli.Negocio);
                _comando.ExecuteNonQuery();
                res = "Cliente: " + cli.Nombre + " " + cli.Apellido + " agregado con exito";
                return true;
            }
        }

        //Agregar Repartidor
        public bool AgregarRepartidor(Repartidor rep, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_repartidor";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@nombre", rep.Nombre);
                _comando.Parameters.AddWithValue("@apellido", rep.Apellido);
                _comando.Parameters.AddWithValue("@direccion", rep.Direccion);
                _comando.Parameters.AddWithValue("@telefono", rep.Telefono);
                _comando.Parameters.AddWithValue("@usr_id", rep.Id_User);
                _comando.ExecuteNonQuery();
                res = "Repartidor: " + rep.Nombre + " " + rep.Apellido + " agregado exitosamente";
                return true;
            }
        }

        //Agregar Usuario
        public bool AgregarUsuario(Usuario us, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_usuario";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@nombre", us.Nombre);
                _comando.Parameters.AddWithValue("@login", us.Login);
                _comando.Parameters.AddWithValue("@pswd", us.Password);
                _comando.Parameters.AddWithValue("@tipo", us.CharTipo[us.NumTipo]);
                _comando.ExecuteNonQuery();
                res = "Usuario: " + us.Nombre + " con login: " + us.Login + " agregado con exito.";
                return true;
            }
        }

        //BuscarUsuario
        public bool BuscarUsuario(Usuario us, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Usuario where usr_login = '" + us.Login + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                if (_reader.Read())
                {
                    res = "Usuario ya en el base de datos";
                    return true;
                }
                else
                {
                    res = "Usuario no encontrado";
                    return false;
                }
            }
        }

        //Bloquear usuario
        public bool Bloquearusuario(Usuario us, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Usuario set eliminado = 1 where usr_login = '" + us.Login + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Usuario Bloqueado";
                return true;
            }
        }

        //Desbloquear usuario
        public bool DesbloquearUsuario(Usuario us, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Usuario set eliminado = 0 where usr_login = '" + us.Login + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Usuario desbloqueado";
                return true;
            }
        }

        //Mostrar Productos
        public bool MostrarProductos(DataGridView dgv, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                Producto _aux = new Producto();
                string _consulta = "select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 4; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    _aux.Tipo = Convert.ToInt32(_reader.GetValue(4));
                    dgv[4, i].Value = _aux.Acumulacion[_aux.Tipo];
                    dgv[5, i].Value = "+";
                    dgv[6, i++].Value = "--->";
                }
                res = "exito";
                return true;
            }
        }

        //Buscar producto
        public bool BuscarProducto(DataGridView dgv, string parametro, int tipo, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                switch(tipo)
                {
                        //Caso ID
                    case 0:
                        try
                        {
                            string _consulta = "select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where prod_id =" + parametro;
                            _comando = new SqlCommand(_consulta, _conexion);
                            _reader = _comando.ExecuteReader();
                            bool _encontro = _reader.Read();
                            if (_encontro)
                            {
                                dgv.Rows.Clear();
                                dgv.Rows.Add();
                                Producto _aux = new Producto();
                                for (int j = 0; j < 4; j++)
                                    dgv[j, 0].Value = _reader.GetValue(j).ToString();
                                _aux.Tipo = Convert.ToInt32(_reader.GetValue(4));
                                dgv[4, 0].Value = _aux.Acumulacion[_aux.Tipo];
                                dgv[5, 0].Value = "+";
                                dgv[6, 0].Value = "--->";
                            }
                            else
                                return false;
                            return true;
                        }
                        catch(SqlException ex)
                        {
                            MessageBox.Show(ex.Message + "Datos erroneos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        //Caso nombre
                    case 1:
                        string _consulta1 = "select prod_id, nombre, precio_general, cantidad, tipo_acum from Producto where nombre = '" + parametro + "'";
                        _comando = new SqlCommand(_consulta1, _conexion);
                        _reader = _comando.ExecuteReader();
                        dgv.Rows.Clear();
                        int i = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            Producto _aux = new Producto();
                            for (int j = 0; j < 4; j++)
                                dgv[j, i].Value = _reader.GetValue(j).ToString();
                            _aux.Tipo = Convert.ToInt32(_reader.GetValue(4));
                            dgv[4, i].Value = _aux.Acumulacion[_aux.Tipo];
                            dgv[5, i].Value = "+";
                            dgv[6, i++].Value = "--->";
                        }
                        if (i == 0)
                            return false;
                        else
                            return true;
                }
                return false;
            }
        }

        //Buscar producto 2

        public bool BuscarProducto2(DataGridView dgv, Cliente cli, string parametro, int tipo, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                switch (tipo)
                {
                    //Caso ID
                    case 0:
                        try
                        {
                            string _consulta = "BuscarIdProducto";
                            _comando = new SqlCommand(_consulta, _conexion);
                            _comando.CommandType = CommandType.StoredProcedure;
                            _comando.Parameters.AddWithValue("@cli_id", cli.Id_Cliente);
                            _comando.Parameters.AddWithValue("@id", parametro);
                            _reader = _comando.ExecuteReader();
                            dgv.Rows.Clear();
                            int i = 0;
                            while(_reader.Read())
                            {
                                dgv.Rows.Add();
                                Producto _aux = new Producto();
                                for (int j = 0; j < 4; j++)
                                    dgv[j, i].Value = _reader.GetValue(j).ToString();
                                _aux.Tipo = Convert.ToInt32(_reader.GetValue(4));
                                dgv[4, i].Value = _aux.Acumulacion[_aux.Tipo];
                                dgv[5, i++].Value = "+";
                            }
                            if (i == 0)
                                return false;
                            else
                                return true;
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message + "Datos erroneos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    //Caso nombre
                    case 1:
                        string _consulta1 = "BuscarNombreProducto";
                        _comando = new SqlCommand(_consulta1, _conexion);
                        _comando.CommandType = CommandType.StoredProcedure;
                        _comando.Parameters.AddWithValue("@cli_id", cli.Id_Cliente);
                        _comando.Parameters.AddWithValue("@nombre", parametro);
                        _reader = _comando.ExecuteReader();
                        dgv.Rows.Clear();
                        int r = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            Producto _aux = new Producto();
                            for (int j = 0; j < 4; j++)
                                dgv[j, r].Value = _reader.GetValue(j).ToString();
                            _aux.Tipo = Convert.ToInt32(_reader.GetValue(4));
                            dgv[4, r].Value = _aux.Acumulacion[_aux.Tipo];
                            dgv[5, r++].Value = "+";
                        }
                        if (r == 0)
                            return false;
                        else
                            return true;
                }
                return false;
            }
        }

        //Modificar Poducto
        public bool ModificarProducto(Producto prod, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Producto set cantidad = "+prod.Cantidad+" , tipo_acum = "+prod.Tipo+", precio_general = @cantidad , descripcion = '"+prod.Descripcion+"' where prod_id = "+prod.Id_Producto;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.Parameters.Add("@cantidad", SqlDbType.Money).Value = Convert.ToDouble(prod.Precio_General);
                _comando.ExecuteNonQuery();
                res = "Producto Modificado";
                return true;
            }
        }

        //Agregar Producto
        public bool AgregarProducto(Producto prod, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_producto";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@nombre", prod.Nombre);
                _comando.Parameters.AddWithValue("@precio", prod.Precio_General);
                _comando.Parameters.AddWithValue("@cantidad", prod.Cantidad);
                _comando.Parameters.AddWithValue("@tipo_acum", prod.Tipo);
                _comando.Parameters.AddWithValue("@descripcion", prod.Nombre);
                _comando.Parameters.AddWithValue("@usr_id", prod.Id_User);
                _comando.ExecuteNonQuery();
                res = "Producto Agregado";
                return true;
            }
        }

        //Agregar cantidad Producto
        public bool AgregarCantidadProducto(Producto prod, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Producto set cantidad = cantidad + " + prod.Cantidad + " where prod_id = " + prod.Id_Producto;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Cantidad Agregada";
                return true;
            }
        }

        //Mostrar Repartidores
        public bool MostrarRepartidores(DataGridView dgv, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select repar_id, nombre, apellido,  telefono, usr_id from Repartidor";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 5; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[5, i++].Value = "...";
                }
                res = "exito";
                return true;
            }
        }


        //Atrapar Repartidor
        public bool AtraparRepartidor(string repar_id, ref Repartidor rep, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Repartidor where repar_id = '" + repar_id + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                rep.Id_Repartidor = repar_id;
                rep.Nombre = _reader.GetValue(1).ToString();
                rep.Apellido = _reader.GetValue(2).ToString();
                rep.Direccion = _reader.GetValue(3).ToString();
                rep.Telefono = _reader.GetValue(4).ToString();
                rep.Fecha_Alta = Convert.ToDateTime(_reader.GetValue(5).ToString());
                rep.Id_User = _reader.GetValue(6).ToString();
                res = "exito";
                return true;
            }
        }

        //Mostrar pedidos Repartidor
        public bool MostrarPedidosRepartidor(DataGridView dgv, Repartidor rep, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pedido_id, fecha_entrega, precio_total, cli_id, usr_id from Pedido where repar_id = '" + rep.Id_Repartidor + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 5; j++)
                        dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[5, i++].Value = "...";
                }
                res = "exito";
                return true;
            }
        }

        //Modificar Repartidor
        public bool ModificarRepartidor(Repartidor rep, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Repartidor set nombre = '" + rep.Nombre + "', apellido = '" + rep.Apellido + "', direccion = '" + rep.Direccion + "', telefono = '" + rep.Telefono + "' where repar_id = '" + rep.Id_Repartidor + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Repartidor Modificado";
                return true;
            }
        }

        //Filtrar fecha entrega
        public bool FiltrarFechaEntrega(DataGridView dgv, DateTime  fecha, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "PedidosFechaEntrega";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@fecha_entrega", fecha.ToShortDateString());
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    int j = 0;
                    dgv[j++, i].Value = _reader.GetValue(0).ToString();
                    dgv[j++, i].Value = _reader.GetValue(1).ToString();
                    dgv[j++, i].Value = _reader.GetValue(2).ToString();
                    dgv[j++, i].Value = _reader.GetValue(3).ToString();
                    dgv[j++, i].Value = _reader.GetValue(5).ToString() == "0" ? "SI" : "NO";
                    dgv[j++, i].Value = _reader.GetValue(8).ToString();
                    dgv[j++, i].Value = _reader.GetValue(6).ToString();
                    dgv[j++, i].Value = "...";
                    dgv[j++, i++].Value = "***";
                }
                res = "exito";
                return true;
            }
        }

        //Filtrar fecha pedido
        public bool FiltrarFechaPedido(DataGridView dgv, DateTime fecha, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "PedidosFechaPedido";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@fecha_pedido", fecha.ToShortDateString());
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    int j = 0;
                    dgv[j++, i].Value = _reader.GetValue(0).ToString();
                    dgv[j++, i].Value = _reader.GetValue(1).ToString();
                    dgv[j++, i].Value = _reader.GetValue(2).ToString();
                    dgv[j++, i].Value = _reader.GetValue(3).ToString();
                    dgv[j++, i].Value = _reader.GetValue(5).ToString() == "0" ? "SI" : "NO";
                    dgv[j++, i].Value = _reader.GetValue(8).ToString();
                    dgv[j++, i].Value = _reader.GetValue(6).ToString();
                    dgv[j++, i].Value = "...";
                    dgv[j++, i++].Value = "***";
                }
                res = "exito";
                return true;
            }
        }

        //Mostrar todos los pedidos

        public bool MostrarTodosPedidos(DataGridView dgv, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pedido_id, fecha_ped, fecha_entrega, precio_total, contado, repar_id, usr_id from Pedido where liquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 7; j++)
                        if (j == 4)
                            dgv[j, i].Value = _reader.GetValue(j).ToString() == "0" ? "SI" : "NO";
                        else
                            dgv[j, i].Value = _reader.GetValue(j).ToString();
                    dgv[7, i].Value = "...";
                    dgv[8, i++].Value = "***";
                }
                return true;
            }
        }

        //Reportes fecha
        public bool ReportesFecha(DateTime fecha,ref double abonos, ref double pedidos,  ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "AbonosTotal";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@fecha", fecha.ToShortDateString());
                _reader = _comando.ExecuteReader();
                _reader.Read();
                if (_reader.GetValue(0) != DBNull.Value)
                    abonos = Convert.ToDouble(_reader.GetValue(0));
                else
                    abonos = 0;
                _consulta = "VentaTotalPedidos";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@fecha", fecha.ToShortDateString());
                _reader = _comando.ExecuteReader();
                _reader.Read();
                if (_reader.GetValue(0) != DBNull.Value)
                    pedidos = Convert.ToDouble(_reader.GetValue(0));
                else
                    pedidos = 0;
                res = "exito";
                return true;
            }
        }

        //Pedidos de repartidor
        public ArrayList PedidosRepartidor(Repartidor _rep)
        {
            if (!Conectada)
            {
                return new ArrayList();
            }
            else
            {
                string _consulta = "select pedido_id, fecha_ped, fecha_entrega, precio_total, contado from Pedido where repar_id = '" + _rep.Id_Repartidor + "' and liquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                ArrayList _pedidos = new ArrayList();
                while(_reader.Read())
                {
                    Pedido _aux = new Pedido();
                    _aux.Id_Pedido = Convert.ToInt32(_reader.GetValue(0));
                    _aux.Fecha_Pedido = Convert.ToDateTime(_reader.GetValue(1));
                    _aux.Fecha_Entrega = Convert.ToDateTime(_reader.GetValue(2));
                    _aux.Precio_Total = Convert.ToDouble(_reader.GetValue(3));
                    _aux.Contado = Convert.ToBoolean(_reader.GetValue(4));
                    _pedidos.Add(_aux);
                }
                return _pedidos;
            }
        }

        //Lista de productos
        public ArrayList Productos()
        {
            if (!Conectada)
            {
                return new ArrayList();
            }
            else
            {
                string _consulta = "select prod_id, nombre, precio_general, tipo_acum from Producto";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                ArrayList _productos = new ArrayList();
                while(_reader.Read())
                {
                    Producto _aux = new Producto();
                    _aux.Id_Producto = Convert.ToInt32(_reader.GetValue(0));
                    _aux.Nombre = _reader.GetValue(1).ToString();
                    _aux.Precio_General = Convert.ToDouble(_reader.GetValue(2));
                    _aux.Tipo = Convert.ToInt32(_reader.GetValue(3));
                    _productos.Add(_aux);
                }
                return _productos;
            }
        }

        //Atrapar nota Pedido
        public bool AtraparNotaPedido(int pedido_id, ref Nota _no)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "select nota_id, cantidad, fecha_inicio, fecha_vencimiento, semanas from Nota where pedido_id = "+pedido_id;
                _comando = new SqlCommand(_consulta,_conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                _no.Id_Nota = Convert.ToInt32(_reader.GetValue(0));
                _no.Cantidad = Convert.ToDouble(_reader.GetValue(1));
                _no.Fecha_Inicio = Convert.ToDateTime(_reader.GetValue(2));
                _no.Fecha_Vencimiento = Convert.ToDateTime(_reader.GetValue(3));
                _no.Semanas = Convert.ToInt32(_reader.GetValue(4));
                return true;
            }
        }

        //Agregar Proveedor
        public bool AgregarProveedor(Proveedor _prov)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "insert_proveedor";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@nombre", _prov.Nombre);
                _comando.Parameters.AddWithValue("@apellido", _prov.Apellido);
                _comando.Parameters.AddWithValue("@negocio", _prov.Negocio);
                _comando.Parameters.AddWithValue("@telefono", _prov.Telefono);
                _comando.Parameters.AddWithValue("@direccion", _prov.Direccion);
                _comando.ExecuteNonQuery();
                return true;
            }
        }

        //Mostrar Proveedores
        public bool MostrarProveedores(DataGridView dgv)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "select prov_id, nombre, appellido, negocio, telefono from Proveedor";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int  i = 0;
                dgv.Rows.Clear();
                while(_reader.Read())
                {
                    dgv.Rows.Add();
                    for (int j = 0; j < 5; j++)
                        dgv[j, i].Value = _reader.GetValue(j);
                    i++;
                }
                return true;
            }
        }
    }
}
