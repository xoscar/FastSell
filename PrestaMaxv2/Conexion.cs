using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;
using PrestaMaxv2.Properties;
using System.Configuration;

namespace PrestaMaxv2
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
            _conexion.ConnectionString = Settings.Default.pruebaV2ConnectionString; //"data source = OFICINA-PC\\OSCAR; initial catalog = pruebaV2; MultipleActiveResultSets = True; user id = sa; password = system62";
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
                return ex.Message+ "conexion no completada";
            }
        }

        public bool Cerrar()
        {
            try
            {
                _conexion.Close();
                _conectada = true;
                return true;
            }
            catch (Exception ex)
            {
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
                string _consulta = "select * from Usuario";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();

                while (_reader.Read())
                {
                    if (_reader.GetValue(1).ToString() == login && _reader.GetValue(2).ToString() == password)
                    {
                        switch(Convert.ToChar(_reader.GetValue(3)))
                        {
                            case 'A':
                                us = new Usuario(_reader.GetValue(1).ToString(), _reader.GetValue(0).ToString(), 0, _reader.GetValue(2).ToString());
                                break;
                            case 'U':
                                us = new Usuario(_reader.GetValue(1).ToString(), _reader.GetValue(0).ToString(), 1, _reader.GetValue(2).ToString());
                                break;
                        }
                        _encontrado = true;
                    }
                }
                if (_encontrado)
                {
                    res = "Sesion iniciada: "+us.Nombre;
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
        public bool ConsultarClientes(DataGridView dgv,ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                dgv.Rows.Clear();
                string _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    double _adeudo = 0.0;
                    string _cliId = _reader.GetValue(0).ToString();
                    string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId+"' and loquidado = 0";
                    SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                    SqlDataReader _readerAux = _comAux.ExecuteReader();
                    _readerAux.Read();
                    if (_readerAux.GetValue(0) != DBNull.Value)
                        _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                    dgv[0, i].Value = _cliId;
                    dgv[1, i].Value = _reader.GetValue(1);
                    dgv[2, i].Value = _reader.GetValue(2);
                    dgv[3, i].Value = _adeudo;
                    dgv[4, i].Value = _reader.GetValue(3);
                    dgv[5, i++].Value = "--->";
                }
                res = "datos exitosamente cargados";
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
                        string _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente where cli_id = '"+ parametro+"'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado = _reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _adeudo;
                            dgv[4, 0].Value = _reader.GetValue(3);
                            dgv[5, 0].Value = "--->";
                            res = "cliente encontrado";
                            return true;
                        }
                        else
                        {
                            res = "cliente no encontrado";
                            return false;
                        }

                    case 1:
                        _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente where nombre = '"+ parametro+"'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado =_reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _adeudo;
                            dgv[4, 0].Value = _reader.GetValue(3);
                            dgv[5, 0].Value = "--->";
                            res = "cliente encontrado";
                            return true;
                        }
                        else
                        {
                            res = "cliente no encontrado";
                            return false;
                        }

                    case 2:
                        _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente where apellido = '" + parametro + "'";
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        _encontrado = _reader.Read();
                        if (_encontrado)
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, 0].Value = _cliId;
                            dgv[1, 0].Value = _reader.GetValue(1);
                            dgv[2, 0].Value = _reader.GetValue(2);
                            dgv[3, 0].Value = _adeudo;
                            dgv[4, 0].Value = _reader.GetValue(3);
                            res = "cliente encontrado";
                            dgv[5, 0].Value = "--->";
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
                        string _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente order by cli_id "+ desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        int i = 0;
                        while(_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _adeudo;
                            dgv[4, i].Value = _reader.GetValue(3);
                            dgv[5, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;

                    case 1:
                         _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente order by nombre "+ desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        i = 0;
                        while(_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _adeudo;
                            dgv[4, i].Value = _reader.GetValue(3);
                            dgv[5, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;

                    case 2:
                        _consulta = "select cli_id,  nombre, apellido, usr_id from Cliente order by apellido "+ desc;
                        _comando = new SqlCommand(_consulta, _conexion);
                        _reader = _comando.ExecuteReader();
                        i = 0;
                        while (_reader.Read())
                        {
                            dgv.Rows.Add();
                            double _adeudo = 0.0;
                            string _cliId = _reader.GetValue(0).ToString();
                            string _consultaAdeudo = "select sum(saldo_actual) from Prestamo where cli_id = '" + _cliId + "'";
                            SqlCommand _comAux = new SqlCommand(_consultaAdeudo, _conexion);
                            SqlDataReader _readerAux = _comAux.ExecuteReader();
                            _readerAux.Read();
                            if (_readerAux.GetValue(0) != DBNull.Value)
                                _adeudo = Convert.ToDouble(_readerAux.GetValue(0));
                            dgv[0, i].Value = _cliId;
                            dgv[1, i].Value = _reader.GetValue(1);
                            dgv[2, i].Value = _reader.GetValue(2);
                            dgv[3, i].Value = _adeudo;
                            dgv[4, i].Value = _reader.GetValue(3);
                            dgv[5, i++].Value = "--->";
                        }
                        res = "Carga exitosa";
                        return true;
                }
                return false;
            }
        }


        //Mostrar Tarjeta Cliente
        public bool TarjetaCliente(DataGridView dgv, string cli_id, ref Cliente cli,  ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Cliente where cli_id = '" + cli_id + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                cli = new Cliente(_reader.GetValue(0).ToString(), _reader.GetValue(1).ToString(), _reader.GetValue(2).ToString(), _reader.GetValue(3).ToString(),
                    Convert.ToDateTime(_reader.GetValue(4)), _reader.GetValue(5).ToString(), _reader.GetValue(6).ToString());
                string _comandaux = "select pres_id, cantidad, saldo_actual, fecha_vencimiento, pago_semanal from Prestamo where cli_id = '" + cli_id + "' and loquidado = 0";
                SqlCommand _comandoAux = new SqlCommand(_comandaux, _conexion);
                SqlDataReader _readeraux = _comandoAux.ExecuteReader();
                dgv.Rows.Clear();
                int  i= 0;
                while (_readeraux.Read())
                {
                    dgv.Rows.Add();
                    dgv[0, i].Value = _readeraux.GetValue(0).ToString();
                    dgv[1, i].Value = _readeraux.GetValue(1).ToString();
                    if (Convert.ToInt32(_readeraux.GetValue(2)) == 0)
                         dgv[2, i].Style.BackColor = Color.Green;
                    dgv[2, i].Value = _readeraux.GetValue(2).ToString();
                    if (Convert.ToDateTime(_readeraux.GetValue(3)).CompareTo(System.DateTime.Now) < 0)
                        dgv[3, i].Style.BackColor = Color.Red;
                    else
                        dgv[3, i].Style.BackColor = Color.Cyan;
                    dgv[3, i].Value = _readeraux.GetValue(3).ToString();
                    dgv[4, i].Value = _readeraux.GetValue(4).ToString();
                    dgv[5, i].Value = "+";
                    dgv[6, i].Value = "...";
                    dgv[7, i++].Value = "***";
                }
                res = "cliente mostrado con exito";
                return true;
            }
        }

        //Atrapar Prestamo
        public bool AtraparPrestamo(int pres_id, ref string res, ref Prestamo _pres, int liq)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Prestamo where pres_id = " + pres_id.ToString() + "and loquidado = "+liq;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                _pres = new Prestamo(Convert.ToInt32(_reader.GetValue(0)),Convert.ToDouble(_reader.GetValue(1)) , Convert.ToDouble(_reader.GetValue(2)),
                    Convert.ToDateTime(_reader.GetValue(3)), Convert.ToDateTime(_reader.GetValue(4)), Convert.ToDouble(_reader.GetValue(5)), Convert.ToInt32(_reader.GetValue(6)),
                    Convert.ToInt32(_reader.GetValue(7)), _reader.GetValue(8).ToString(), _reader.GetValue(9).ToString(), Convert.ToBoolean(_reader.GetValue(10)),
                    _reader.GetValue(11).ToString(), _reader.GetValue(12).ToString());
                res = "Prestamo cargado con exito";
                return true;
            }
        }

        //Mostrar Abonos
        public bool MostrarAbonos(DataGridView dgv, int pres_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Pago where pres_id = " + pres_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int  i = 0;
                dgv.Rows.Clear();
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    dgv[0, i].Value = i + 1;
                    dgv[1, i].Value = _reader.GetValue(0).ToString();
                    dgv[2, i].Value = _reader.GetValue(1).ToString();
                    dgv[3, i].Value = _reader.GetValue(2).ToString();
                    dgv[4, i].Value = _reader.GetValue(3).ToString();
                    dgv[5, i].Value = _reader.GetValue(5).ToString();
                    dgv[6, i].Value = "X";
                    dgv[7, i++].Value = "////";
                    res = "exito";
                }
                return true;
            }
        }

        //Abonar
        public bool Abonar(double cantidad, Prestamo pres, Cliente cl, Usuario us, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_pago";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.Add( new SqlParameter("@cantidad",cantidad));
                _comando.Parameters.Add( new SqlParameter("@fecha",DateTime.Now));
                _comando.Parameters.Add( new SqlParameter("@pres_id",pres.Id_Pres));
                _comando.Parameters.Add( new SqlParameter("@cli_id",cl.Id_Cliente));
                _comando.Parameters.Add( new SqlParameter("@usr_login",us.Login));
                _comando.ExecuteNonQuery();
                res = "abono agregado con exito";
                return true;
            }
        }
        //Semanas restantes y semana actual
        public bool ManejoSemanas(int pres_id, ref int semanaActual, ref int semanaRestante, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "semanas_restantes_prestamo";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.Add(new SqlParameter("@id_prestamo",pres_id));
                _reader = _comando.ExecuteReader();
                _reader.Read();
                semanaRestante = Convert.ToInt32(_reader.GetValue(0));
                string _consultaAux = "semana_actual_prestamo";
                SqlCommand _comandoAux = new SqlCommand(_consultaAux, _conexion);
                _comandoAux.CommandType = CommandType.StoredProcedure;
                _comandoAux.Parameters.Add(new SqlParameter("@id_prestamo", pres_id));
                SqlDataReader _readerAux = _comandoAux.ExecuteReader();
                _readerAux.Read();
                semanaActual = Convert.ToInt32(_readerAux.GetValue(0)) + 1;
                res = "exito";
                return true;
            }
        }

        //Modificar prestamo

        public bool ModificarPrestamo(ref Prestamo pres, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Prestamo set cantidad = "
                    + pres.Cantidad.ToString() + ", fecha_inicio = '"
                    + pres.Fecha_Inicio.ToShortDateString() + "', fecha_vencimiento = '"
                    + pres.Fecha_Vencimiento.ToShortDateString() + "', pago_semanal = "
                    + pres.Pago_Semanal.ToString() + ", semanas = "
                    + pres.Semanas.ToString() + ", pagare = '"
                    + pres.Pagare + "', descripcion = '"
                    + pres.Descripcion + "' where pres_id = " + pres.Id_Pres;
                //string _consultaux = "update Prestamo set fecha_vencimiento = '" + DateTime.Now.ToShortDateString() + "' where pres_id = " + pres.Id_Pres; 
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Prestamo modificado con exito";
                return true;
            }
        }
        
        //liquidarPrestamo
        public bool LiquidarPrestamo(int pres_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Prestamo set loquidado = 1 where pres_id = " + pres_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Prestamo Liquidado";
                return true;
            }
        }

        //Agregar Prestamo
        public bool AgregarPrestamo(Prestamo pres, Cliente cli, Usuario user, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_prestamo";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@cantidad", pres.Cantidad);
                _comando.Parameters.AddWithValue("@pago_semanal", pres.Pago_Semanal);
                _comando.Parameters.AddWithValue("@semanas", pres.Semanas);
                _comando.Parameters.AddWithValue("@pagare", pres.Pagare);
                _comando.Parameters.AddWithValue("@descripcion", pres.Descripcion);
                _comando.Parameters.AddWithValue("@cli_id", cli.Id_Cliente);
                _comando.Parameters.AddWithValue("@user", user.Login);
                _comando.Parameters.AddWithValue("@liquidado", 0);
                _comando.ExecuteNonQuery();
                res = "Prestamo agregado a cliente " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }
        //Mostrar prestamos liquidados
        public bool MostrarLiquidados(DataGridView dgv, Cliente _cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pres_id, cantidad, fecha_inicio, fecha_vencimiento, usr_id from Prestamo where loquidado = 1 and cli_id = '" + _cli.Id_Cliente + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                int i = 0;
                dgv.Rows.Clear();
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    dgv[0, i].Value = _reader.GetValue(0).ToString();
                    dgv[1, i].Value = _reader.GetValue(1).ToString();
                    dgv[2, i].Value = _reader.GetValue(2).ToString();
                    dgv[3, i].Value = _reader.GetValue(3).ToString();
                    dgv[4, i].Value = _reader.GetValue(4).ToString();
                    dgv[5, i++].Value = "...";
                }
                res = "Liquidados cargados con exito";
                return true;
            }
        }
        //Agregar Cargo
        public bool AgregarCargo(ref Cargo ca, Usuario usr, Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_Cargo";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@cantidad", ca.Cantidad);
                _comando.Parameters.AddWithValue("@semanas", ca.Semanas);
                _comando.Parameters.AddWithValue("@descripcion", ca.Descripcion);
                _comando.Parameters.AddWithValue("@cli_id", cli.Id_Cliente);
                _comando.Parameters.AddWithValue("@user", usr.Login);
                _comando.ExecuteNonQuery();
                res = "Cargo agregado a Cliente: " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }

        //Mostrar Cargos
        public bool MostrarCargos(DataGridView dgv, string cli_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select cargo_id,cantidad from Cargo where cli_id = '" + cli_id + "' and loquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    dgv[0, i].Value = _reader.GetValue(0).ToString();
                    dgv[1, i].Value = _reader.GetValue(1).ToString();
                    dgv[2, i].Value = "+";
                    dgv[3, i++].Value = "...";
                    res = "exito mostrar cargos";
                }
                return true;
            }
        }

        //pagar cargo
        public bool PagarCargo(int cargo_id, Cliente cli, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Cargo set loquidado = 1 where cargo_id = " + cargo_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Cargo pagado del cliente: " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }

        //Capturar Cargo
        public bool AtraparCargo(int cargo_id, ref Cargo ca, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select * from Cargo where cargo_id =" + cargo_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                ca = new Cargo(cargo_id,Convert.ToDouble(_reader.GetValue(1).ToString()), Convert.ToDateTime(_reader.GetValue(2)),Convert.ToDateTime(_reader.GetValue(3)),
                    Convert.ToInt32(_reader.GetValue(4)), Convert.ToInt32(_reader.GetValue(5)), _reader.GetValue(6).ToString(), Convert.ToBoolean(_reader.GetValue(7)),
                    _reader.GetValue(8).ToString(), _reader.GetValue(9).ToString());
                res = "Atrapado Exitoso";
                return true;
            }
        }

        //Moficiar Cargo
        public bool ModificarCargo(Cargo ca, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "update Cargo set cantidad = " + ca.Cantidad
                    + ", semanas = " + ca.Semanas
                    + ", fecha_inicio = '" + ca.Fecha_Inicio.ToString()
                    + "', fecha_vencimiento = '" + ca.Fecha_Vencimiento.ToString()
                    + "', descripcion = '" + ca.Descripcion
                    + "' where cargo_id = " + ca.Id_Cargo;

                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "modificacion exitosa";
                return true;
            }
        }

        //Mostrar Multas
        public bool MostrarMultas(DataGridView dgv, string cli_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select multa_id, cantidad, fecha, pres_id from Multa where cli_id = '" + cli_id + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                dgv.Rows.Clear();
                int i = 0;
                while (_reader.Read())
                {
                    dgv.Rows.Add();
                    dgv[0, i].Value = _reader.GetValue(0).ToString();
                    dgv[1, i].Value = _reader.GetValue(1).ToString();
                    dgv[2, i].Value = _reader.GetValue(2).ToString();
                    dgv[3, i].Value = _reader.GetValue(3).ToString();
                    dgv[4, i++].Value = "+";
                    res = "Exito al cargar";
                }
                return true;
            }
        }

        //Pagar Multa
        public bool PagarMulta(int multa_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "delete from Multa where multa_id = " + multa_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Multa eliminada";
                return true;
            }
        }

        //Agregar Multa
        public bool AgregarMulta(int cantidad, int pres_id, string cli_id, string usr_id, ref string res)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "insert_multa";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.CommandType = CommandType.StoredProcedure;
                _comando.Parameters.AddWithValue("@cantidad", cantidad);
                _comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
                _comando.Parameters.AddWithValue("@semana", 1);
                _comando.Parameters.AddWithValue("@descripcion", "multa");
                _comando.Parameters.AddWithValue("@pres_id", pres_id);
                _comando.Parameters.AddWithValue("@cli_id", cli_id);
                _comando.Parameters.AddWithValue("@usr_login", usr_id);
                _comando.ExecuteNonQuery();
                res = "Multa agregada exitosamente";
                return true;
            }
        }

        //Mostrar Combo Id Prestamos
        public bool MostrarIDPrestamos(ComboBox cb, string cli_id, ref string res, ref bool encontro)
        {
            if (!Conectada)
            {
                res = "La conexión no ha sido abierta";
                return false;
            }
            else
            {
                string _consulta = "select pres_id from Prestamo where loquidado = 0 and cli_id = '" + cli_id + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                cb.Items.Clear();
                while (_reader.Read())
                {
                    cb.Items.Add(_reader.GetValue(0).ToString());
                    encontro = true;
                }
                return true;
            }
        }

        //Agregar Cliente
        public bool AgregarCliente(Cliente cli, string usr_id, ref string res)
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
                _comando.Parameters.AddWithValue("@fecha_alta", DateTime.Now.ToString());
                _comando.Parameters.AddWithValue("@tel", cli.Telefono);
                _comando.Parameters.AddWithValue("@user_id", usr_id);
                _comando.ExecuteNonQuery();
                res = "Cliente Agregado: " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }

        //Modificar cliente

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
                    + "', tel = '" + cli.Telefono + "' where  cli_id = '" + cli.Id_Cliente + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                res = "Cliente modificado: " + cli.Nombre + " " + cli.Apellido;
                return true;
            }
        }

        //Buscar Usuario
        public bool BuscarUsuario(string usr_id)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                bool _encontrado = false;
                string _consulta = "select * from Usuario";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();

                while (_reader.Read())
                {
                    if (_reader.GetValue(1).ToString() == usr_id)
                    {
                        _encontrado = true;
                    }
                }
                return _encontrado;
            }
        }

        //Ingresar Usuario
        public bool AgregarUsuario(Usuario us, ref string res)
        {
            if (!Conectada)
            {
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
                res = "Usuario agregado exitosamente, Nombre: " + us.Nombre + "  Login: " + us.Login + " Tipo: " + us.CharTipo[us.NumTipo];
                return true;
            }
        }
        //Cambiar Contraseña
        public bool CambiarContraseña(string login, string pass)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "update Usuario set usr_pwd = '" + pass + "' where usr_login = '" + login + "'";
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                return true;
            }
        }

        //Eliminar Usuario
        public bool EliminarUsuario(string login, ref string res)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "update Usuario set usr_pwd = '*****************' where usr_login = '"+login+"'";
                _comando = new SqlCommand(_consulta, _conexion);
                try
                {
                    _comando.ExecuteNonQuery();
                    res = "Usuario eliminado del sistema";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar usuario", "Aviso");
                }
                return true;
            }
        }

        //Eliminar Abono
        public bool EliminarAbono(int pago_id)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "delete from Pago where pago_id = " + pago_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                return true;
            }
        }

        //Dinero total

        public bool DineroTotal( ref string res)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "select sum(saldo_actual) from Prestamo where loquidado = 0";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                bool _readed = _reader.Read();
                if (_readed)
                    res = _reader.GetValue(0).ToString();
                else
                    res = "no hay prestamos activos";
                return true;
            }
        }

        //Abono modificar fecha
        public bool ModificarFeca(string fecha, string pago_id)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "update Pago set fecha = '" + fecha + "' where pago_id = " + pago_id;
                _comando = new SqlCommand(_consulta, _conexion);
                _comando.ExecuteNonQuery();
                return true;
            }
        }

        public bool SaldosActuales(Cliente cli,ref double SaldoActual,ref double SaldoVencido)
        {
            if (!Conectada)
            {
                return false;
            }
            else
            {
                string _consulta = "select saldo_actual, fecha_vencimiento from Prestamo where cli_id = '"+cli.Id_Cliente+"' and loquidado = 0";
                _comando = new SqlCommand(_consulta,_conexion);
                _reader = _comando.ExecuteReader();
                while(_reader.Read())
                {
                    SaldoActual += Convert.ToDouble(_reader.GetValue(0));
                    if (Convert.ToDateTime(_reader.GetValue(1)).CompareTo(DateTime.Now) < 0)
                        SaldoVencido += Convert.ToDouble(_reader.GetValue(0));
                }
                return true;
            }
        }

        public int NumClientes()
        {
            if (!Conectada)
            {
                return 0;
            }
            else
            {
                string _consulta = "select count(*) from Clientes";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                _reader.Read();
                return Convert.ToInt32(_reader.GetValue(0));
            }
        }

        public ArrayList Clientes()
        {
            if (!Conectada)
            {
                return new ArrayList();
            }
            else
            {
                string _consulta = "select cli_id, nombre, apellido from Cliente";
                _comando = new SqlCommand(_consulta, _conexion);
                _reader = _comando.ExecuteReader();
                ArrayList _clientes = new ArrayList();
                while(_reader.Read())
                {
                    Cliente _aux = new Cliente();
                    _aux.Id_Cliente = _reader.GetValue(0).ToString();
                    _aux.Nombre = _reader.GetValue(1).ToString();
                    _aux.Apellido = _reader.GetValue(2).ToString();
                    _clientes.Add(_aux);
                }
                return _clientes;
            }
        }
    }
}
