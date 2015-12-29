using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace PrestaMaxv2
{
    public partial class AdministradorMain : Form
    {
        
        Conexion _co;
        Usuario _user;
        Inicio _inicio;
        Cliente _cli;
        Pago _pago;
        Prestamo _pres;
        DetallesPrestamo _detallePres;
        Liquidar _liq;
        AgregarPrestamo _addpres;
        Liquidados _liquidados;
        AgregarCargo _addCargo;
        DetallesCargo _detalleCargo;
        Cargo _ca;
        EliminarUsuario _elus;
        bool _cerrarsesion = false;
        public AdministradorMain()
        {
            InitializeComponent();
            this.Location = new Point(0, 0);
            cbCantidad.SelectedIndex = 0;
        }

        private void AdministradorMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_cerrarsesion)
            {
                DialogResult _dialogresult = MessageBox.Show("Seguro que deseas salir: ", "Verificacion de salida", MessageBoxButtons.YesNo);
                if (_dialogresult == DialogResult.Yes)
                {
                    _inicio.Cerrar();
                    _inicio.Close();
                    if (_co.Conectada)
                        _co.Cerrar();
                }
                else
                    e.Cancel = true;
            }
        }

        public void Inicializa(Conexion co, Usuario us, Inicio ini)
        {
            _co = co;
            _user = us;
            _inicio = ini;
            this.Text +=" "+ _user.Nombre + " "+DateTime.Now.DayOfWeek.ToString() +" " +DateTime.Now.ToShortDateString();
            if(_user.NumTipo == 1)
            {
                gbCambiarContraseña.Enabled = false;
                gbAgregarUsuario.Enabled = false;
                CHModificar.Enabled = false;
                gbEliminarUsuario.Enabled = false;
                txtTotal.Visible = false;
                lblTotal.Visible = false;
                GenerarReporte.Visible = false;
            }
        }

        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                gbTarjeta.Visible = true;
                gbTarjeta.Text = "Tarjeta de Cliente: ";
                dgvPrestamo.Visible = true;
                this.Width = 1350;
                _co.Abrir();
                string _res = "";
                _co.TarjetaCliente(dgvPrestamo, dgv[0, e.RowIndex].Value.ToString(), ref _cli, ref _res);
                _co.MostrarCargos(dgvCargos, _cli.Id_Cliente, ref _res);
                _co.MostrarMultas(dgvMulta, _cli.Id_Cliente, ref _res);
                bool _encontro = false;
                _co.MostrarIDPrestamos(cbPrestamo, _cli.Id_Cliente, ref _res,  ref _encontro);
                if (_encontro)
                    cbPrestamo.SelectedIndex = 0;
                _co.Cerrar();
                gbTarjeta.Text += _cli.Nombre + " " + _cli.Apellido + " id: " + _cli.Id_Cliente; 
                txtNombre.Text = _cli.Nombre;
                txtApellido.Text = _cli.Apellido;
                txtDireccion.Text = _cli.Direccion;
                txtFecha.Text = _cli.Fecha_Alta.ToShortDateString();
                txtTelefono.Text = _cli.Telefono;
                txtUser.Text = _cli.Id_Usuario;
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            txtBusqueda.CharacterCasing = CharacterCasing.Upper;
            CBbusqueda.SelectedIndex = 0;
            CBordenar.SelectedIndex = 0;
            string _res = "";
            _co.Abrir();
            _co.ConsultarClientes(dgvCliente, ref _res);
            _co.Cerrar();
        }

        private void button1_Click(object sender, EventArgs e)//Buscar
        {
            _co.Abrir();
            string _parametro = txtBusqueda.Text;
            string _res = "";
            bool _encontrado = false;
            int _tipo = CBbusqueda.SelectedIndex;
            _encontrado = _co.BuscCliente(dgvCliente, _parametro, _tipo, ref _res);
            if (!_encontrado)
            {
                MessageBox.Show(_res, "Aviso");
                _co.Cerrar();
                _res = "";
                _co.Abrir();
                _co.ConsultarClientes(dgvCliente, ref _res);
                _co.Cerrar();
            }
            if(_co.Conectada)
                _co.Cerrar();
        }

        private void tabPage1_Leave(object sender, EventArgs e)
        {
            txtBusqueda.Clear();
        }

        private void button2_Click(object sender, EventArgs e)//Reinciar Lista
        {
            string _res = "";
            _co.Abrir();
            _co.ConsultarClientes(dgvCliente, ref _res);
            _co.Cerrar();
        }

        private void button3_Click(object sender, EventArgs e)//Ordenar
        {
            string _res = "";
            bool _succes = false;
            string _desc = "";
            if (CHBdesc.Checked)
                _desc = "desc";
            int _tipo = CBordenar.SelectedIndex;
            _co.Abrir();
            _succes = _co.Ordenar(dgvCliente, _tipo, ref _res, _desc);
            if(!_succes)
            {
                MessageBox.Show(_res,"Aviso");
            }
            _co.Cerrar();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gbTarjeta.Visible = false;
            dgvPrestamo.Visible = false;
            this.Width = 550;
        }

        private void dgvPrestamo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                 switch (e.ColumnIndex)
                 {
                     case 5:
                         _co.Abrir();
                         string _res = "";
                         _co.AtraparPrestamo(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _res, ref _pres,0);
                         _co.Cerrar();
                         _pago = new Pago();
                         _pago.Inicializar(this, _pres, _cli, _co, _user);
                         _pago.Show();
                         this.Enabled = false;
                         break;
                     case 6:
                         _co.Abrir();
                         _res = "";
                          _co.AtraparPrestamo(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _res, ref _pres,0);
                         _co.Cerrar();
                         _detallePres = new DetallesPrestamo();
                         _detallePres.Inicalizar(_co, _cli, this, _pres, _user);
                         _detallePres.Show();
                         this.Enabled = false;
                         break;
                     case 7:
                         if (_user.NumTipo == 0)
                         {
                             DialogResult _resultado = MessageBox.Show("Seguro que desea liquidar el prestamo", "Aviso", MessageBoxButtons.YesNo);
                             if (_resultado == DialogResult.Yes)
                             {
                                 _co.Abrir();
                                 _res = "";
                                 _co.AtraparPrestamo(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _res, ref _pres, 0);
                                 _co.Cerrar();
                                 _liq = new Liquidar();
                                 _liq.Inicializar(_cli, _co, this, _pres);
                                 _liq.Show();
                             }
                         }
                         else
                             MessageBox.Show("No tiene permisos de realizar la operacion", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         break;
                 }
             }
        }

        //actualizar prestamos
        public void ActualizarPrestamos()
        {
            _co.Abrir();
            string _res = "";
            _co.TarjetaCliente(dgvPrestamo, _cli.Id_Cliente, ref _cli, ref _res);
            _co.Cerrar();
        }
        public void ActualizarCargos()
        {
            _co.Abrir();
            string _res = "";
            _co.MostrarCargos(dgvCargos, _cli.Id_Cliente, ref _res);
            _co.Cerrar();
        }

        private void button6_Click(object sender, EventArgs e)//Agregar prestamo
        {
            _addpres = new AgregarPrestamo();
            _addpres.Inicializar(_cli, _co, this, _user);
            _addpres.Show();
        }

        private void button5_Click(object sender, EventArgs e)//Ver Liquidados
        {
            _liquidados = new Liquidados();
            _liquidados.Inicializa(_pres, _co, this, _cli);
            _liquidados.Show();
        }

        private void button9_Click(object sender, EventArgs e)//Agregar Multa
        {
            DialogResult _resultado = MessageBox.Show("Seguro que desea agregar multa", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                try
                {
                    string _res = "";
                    _co.Abrir();
                    _co.AgregarMulta(Convert.ToInt32(cbCantidad.Items[cbCantidad.SelectedIndex]), Convert.ToInt32(cbPrestamo.Items[cbPrestamo.SelectedIndex]), _cli.Id_Cliente, _user.Login, ref _res);
                    MessageBox.Show(_res, "Aviso");
                    _co.MostrarMultas(dgvMulta, _cli.Id_Cliente, ref _res);
                    _co.Cerrar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Datos incorrectos");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)//Agregar Cargo
        {
            _addCargo = new AgregarCargo();
            _addCargo.Inicializar(_cli, _user, _co, this);
            _addCargo.Show();
        }

        private void dgvCargos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                    case 2:
                        if (_user.NumTipo == 0)
                        {
                            DialogResult _resultado = MessageBox.Show("Seguro que desea pagar cargo", "Aviso", MessageBoxButtons.YesNo);
                            if (_resultado == DialogResult.Yes)
                            {
                                _co.Abrir();
                                string _res = "";
                                _co.PagarCargo(Convert.ToInt32(dgv[0, e.RowIndex].Value), _cli, ref _res);
                                _co.MostrarCargos(dgvCargos, _cli.Id_Cliente, ref _res);
                                _co.Cerrar();
                            }
                        }
                        else
                            MessageBox.Show("No tiene permisos de realizar la operacion", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case 3:
                        _co.Abrir();
                        string _res1 = "";
                        _co.AtraparCargo( Convert.ToInt32(dgvCargos[0,e.RowIndex].Value) ,ref _ca,ref _res1);
                        _co.Cerrar();
                        _detalleCargo = new DetallesCargo();
                        _detalleCargo.Inicializa(_ca, _co, this, _user);
                        _detalleCargo.Show();
                        break;
                }
            }
        }

        private void dgvMulta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                 DialogResult _resultado = MessageBox.Show("Seguro que desea pagar multa", "Aviso", MessageBoxButtons.YesNo);
                 if (_resultado == DialogResult.Yes)
                 {
                     _co.Abrir();
                     string _res = "";
                     _co.PagarMulta(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _res);
                     MessageBox.Show(_res, "Aviso");
                     _co.MostrarMultas(dgvMulta, _cli.Id_Cliente, ref _res);
                     _co.Cerrar();
                 }
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {

            this.Width = 530;
        }

        private void button8_Click(object sender, EventArgs e)//Agregar Cliente
        {
             DialogResult _resultado = MessageBox.Show("Confirmacion agregar cliente", "Aviso", MessageBoxButtons.YesNo);
             if (_resultado == DialogResult.Yes )
             {
                 if (txtNom.Text != "" && txtApe.Text != "")
                 {
                     Cliente _aCliente = new Cliente();
                     _aCliente.Nombre = txtNom.Text;
                     _aCliente.Telefono = txtTel.Text;
                     _aCliente.Apellido = txtApe.Text;
                     _aCliente.Direccion = txtDire.Text;
                     _co.Abrir();
                     string _res = "";
                     _co.AgregarCliente(_aCliente, _user.Login, ref _res);
                     MessageBox.Show(_res);
                     txtNom.Clear();
                     txtApe.Clear();
                     txtDire.Clear();
                     txtTel.Clear();
                 }
                 else
                     MessageBox.Show("Especificar almenos nombre y apellido", "Aviso");
             }
        }

        private void CHModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (CHModificar.Checked)
            {
                txtTelefono.ReadOnly = false;
                txtNombre.ReadOnly = false;
                txtDireccion.ReadOnly = false;
                txtApellido.ReadOnly = false;
                btnModify.Enabled = true;
            }
            else
            {
                btnModify.Enabled = false;
                txtTelefono.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDireccion.ReadOnly = true;
                txtApellido.ReadOnly = true;
                _co.Abrir();
                string _res = "";
                _co.TarjetaCliente(dgvPrestamo, _cli.Id_Cliente, ref _cli, ref _res);
                _co.Cerrar();
                txtNombre.Text = _cli.Nombre;
                txtApellido.Text = _cli.Apellido;
                txtDireccion.Text = _cli.Direccion;
                txtFecha.Text = _cli.Fecha_Alta.ToShortDateString();
                txtTelefono.Text = _cli.Telefono;
                txtUser.Text = _cli.Id_Usuario;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)//Modificar Cliente
        {   
            DialogResult _resultado = MessageBox.Show("Confirmacion agregar cliente", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                if (txtNombre.Text != "" && txtTelefono.Text != "" && txtDireccion.Text != "" && txtApellido.Text != "")
                {
                    try
                    {
                        _cli.Nombre = txtNombre.Text;
                        _cli.Apellido = txtApellido.Text;
                        _cli.Direccion = txtDireccion.Text;
                        _cli.Telefono = txtTelefono.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Datos erroneos", "Aviso");
                        return;
                    }
                    _co.Abrir();
                    string _res = "";
                    _co.ModificarCliente(_cli, ref _res);
                    _co.Cerrar();
                    MessageBox.Show(_res, "Aviso");
                    CHModificar.Checked = false;
                    btnModify.Enabled = false;
                    txtTelefono.ReadOnly = true;
                    txtNombre.ReadOnly = true;
                    txtDireccion.ReadOnly = true;
                    txtApellido.ReadOnly = true;
                    _co.Abrir();
                    _res = "";
                    _co.TarjetaCliente(dgvPrestamo, _cli.Id_Cliente, ref _cli, ref _res);
                    _co.ConsultarClientes(dgvCliente, ref _res);
                    _co.Cerrar();
                }
                else 
                    MessageBox.Show("Especificar los datos indicados", "Aviso");
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)//Agregar Usuario
        {
            DialogResult _resultado = MessageBox.Show("Confirmacion agregar cliente", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                if (txtC.Text == txtCC.Text && txtLogin.Text != "" && txtC.Text != "" && txtNU.Text != "")
                {
                    Usuario _aUser = new Usuario();
                    try
                    {
                        _aUser.Login = txtLogin.Text;
                        _aUser.Password = txtC.Text;
                        _aUser.NumTipo = cbTipo.SelectedIndex;
                        _aUser.Nombre = txtNU.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Datos Erroneos");
                        return;
                    }

                    bool _encontrado = false;
                    _co.Abrir();
                    _encontrado = _co.BuscarUsuario(_aUser.Login);
                    _co.Cerrar();
                    if (_encontrado)
                    {
                        MessageBox.Show("Usuario ya existente en el sistema");
                    }
                    else
                    {
                        _co.Abrir();
                        string _res = "";
                        _co.AgregarUsuario(_aUser, ref _res);
                        MessageBox.Show(_res, "Aviso");
                        txtNU.Clear();
                        txtLogin.Clear();
                        txtC.Clear();
                        txtCC.Clear();
                    }
                }
                else
                    MessageBox.Show("Datos erroneos, verifique si la contraseña en ambos campos es igual", "Aviso");
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {
            cbTipo.SelectedIndex = 0;
        }

        private void button11_Click(object sender, EventArgs e)//Cambiar contraseña
        {
            DialogResult _resultado = MessageBox.Show("Confirmacion agregar cliente", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                if (txtCA.Text == _user.Password)
                {
                    if (txtCN.Text == txtCCN.Text && txtCCN.Text != "")
                    {
                        _co.Abrir();
                        _co.CambiarContraseña(_user.Login, txtCCN.Text);
                        _co.Cerrar();
                        MessageBox.Show("Contraseña cambiada","Aviso");
                        txtCA.Clear();
                        txtCN.Clear();
                        txtCCN.Clear();
                    }
                    else
                        MessageBox.Show("Verifique que las contraseñas coincidan", "Aviso");
                }
                else
                    MessageBox.Show("Contraseña de usuario incorrecta", "Aviso");
            }
        }

        private void button12_Click(object sender, EventArgs e)//Eliminar usuario
        {
             DialogResult _resultado = MessageBox.Show("Confirmacion eliminar usuario", "Aviso", MessageBoxButtons.YesNo);
             if (_resultado == DialogResult.Yes)
             {
                 string _login = txtEliUs.Text;
                 if (_login != _user.Login)
                 {
                     _co.Abrir();
                     bool _encontrado = _co.BuscarUsuario(_login);
                     _co.Cerrar();
                     if (_encontrado)
                     {
                         Usuario _aux = new Usuario();
                         _aux.Login = _login;
                         _elus = new EliminarUsuario();
                         _elus.Inicializa(_co, this, _aux);
                         _elus.Show();
                         txtEliUs.Clear();
                     }
                     else
                         MessageBox.Show("Usuario no encontrado", "Aviso");
                 }
                 else
                     MessageBox.Show("No se puede autoeliminar", "Aviso");
             }
        }

        public void ActualizarClientes()
        {
            string _res = "";
            _co.Abrir();
            _co.ConsultarClientes(dgvCliente, ref _res);
            _co.Cerrar();
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            _co.Abrir();
            string _res = "";
            _co.DineroTotal(ref _res);
            _co.Cerrar();
            txtTotal.Text = _res;

            this.Width = 530;
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {
            DialogResult _resultado = MessageBox.Show("¿Seguro que desea cerrar sesion?", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                _cerrarsesion = true;
                _inicio.Show();
                this.Close();
            }

            this.Width = 530;
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {

            this.Width = 530;
        }

        private void button13_Click(object sender, EventArgs e)//Generar Reporte
        {
            if (MessageBox.Show("¿Desea Generar Reporte?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                double   SaldoActual = 0;
                double   SaldoVencido = 0;
                _co.Abrir();
                ArrayList _clientes = _co.Clientes();
                double[,] _saldos = new double[_clientes.Count, 2];
                for (int i = 0; i < _clientes.Count; i++)
                {
                    SaldoActual = 0;
                    SaldoVencido = 0;
                    Cliente _aux = ((Cliente)_clientes[i]);
                    _co.SaldosActuales(_aux, ref SaldoActual, ref SaldoVencido);
                    _saldos[i, 0] = SaldoActual;
                    _saldos[i, 1] = SaldoVencido;
                }
                Documento.ReporteCliente(_clientes, _saldos);
            }
        }

        private void AdministradorMain_Load(object sender, EventArgs e)
        {
        }
    }
}
