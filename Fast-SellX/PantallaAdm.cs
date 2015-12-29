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

namespace Fast_SellX
{
    public partial class PantallaAdm : Form
    {
        public PantallaAdm()
        {
            InitializeComponent();
            this.Location = new Point(0, 0);
        }

        Usuario _user;
        Conexion _co;
        Inicio _inicio;
        bool _cerrarSesion = false;
        Cliente _cli;
        PantallaTarjetaCliente _tarCliente;
        PantallaAgregarPedido _addPedido;
        NotaLiquidada _notaLiq;
        PedidoLiquidado _pedLiq;
        DetallesPedido _dePed;
        Pedido _ped;
        Eliminar _eli;
        Cantidad _cantidad;
        DetallesNota _deNota;
        Producto _aux;
        Repartidor _repartidor;
        bool _flag = false;
        public void Inicializa(Inicio ini, Usuario usr, Conexion co )
        {
            _inicio = ini;
            _user = usr;
            _co = co;
            this.Text = "Menu Principal Administrador, Nombre Usuario: " + _user.Nombre + " Login: " + _user.Login;
            if(_user.NumTipo == 1)
            {
                gbAgregarUsuario.Enabled = false;
                gbBloquearUsuario.Enabled = false;
                gbDesUsuario.Enabled = false;
                gbAgregarProducto.Enabled = false;
                chModificar.Enabled = false;
                chMoficiarRepartidor.Enabled = false;
                gbReportes.Visible = false;
            }
        }


        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)//Agregar Pedido
        {
            this.Enabled = false;
            _addPedido = new PantallaAgregarPedido();
            _addPedido.Inicializar(this, _co, _cli, _user);
            _addPedido.Show();
        }

        private void button6_Click(object sender, EventArgs e)//Ver Notas Liquidadas
        {
            this.Enabled = false;
            _notaLiq = new NotaLiquidada();
            _notaLiq.Inicializar(this, _co, _cli);
            _notaLiq.Show();
        }

        private void PantallaAdm_FormClosing(object sender, FormClosingEventArgs e)//Al Cerrar
        {
            if (!_cerrarSesion)
            {
                DialogResult _resultado = MessageBox.Show("¿Seguro que desea salir?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (_resultado == DialogResult.Yes)
                {
                    if (_co.Conectada)
                        _co.Cerrar();
                    _inicio.Cerrar();
                    _inicio.Close();
                }
                else
                    e.Cancel = true;
            }
            else
                _inicio.Show();
        }

        private void PantallaAdm_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Enter(object sender, EventArgs e)//Cargar clientes
        {

            gbRepartidor.Visible = false;
            if (this.Width != 1366)
                this.Width = 600;
            gbDetallesProducto.Visible = false;
            gbNota.Visible = true;
            gbPedido.Visible = true;
            cbBusqueda.SelectedIndex = 0;
            cbOrdenamiento.SelectedIndex = 0;
            MostrarClientes();
            lblCliente.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)//Buscar
        {
            if (txtBusqueda.Text != "" && cbBusqueda.SelectedIndex >= 0)
            {
                bool _encontrado = false;
                string _parametro = txtBusqueda.Text;
                string _res = "";
                _co.Abrir();
                _encontrado = _co.BuscCliente(dgvCliente,_parametro,cbBusqueda.SelectedIndex, ref _res);
                _co.Cerrar();
                if (!_encontrado)
                    MessageBox.Show("Cliente no Encontrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Determinar Forma de Busqueda y Parametro", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)//Ordenar
        {
            if (cbOrdenamiento.SelectedIndex >= 0)
            {

                int _tipo = cbOrdenamiento.SelectedIndex;
                string _res = "";
                _co.Abrir();
                _co.Ordenar(dgvCliente, _tipo, ref _res, chDesc.Checked ? "" : "desc");
                _co.Cerrar();
            }
            else
                MessageBox.Show("Determinar el Modo de Ordenamiento", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void MostrarClientes()
        {
            string _res = "";
            _co.Abrir();
            _co.MostrarClientes(dgvCliente, ref _res);
            _co.Cerrar();
        }

        private void button2_Click(object sender, EventArgs e)//Reinicar Lista
        {
            MostrarClientes();
        }

        private void dgvCliente_CellContentClick(object sender, DataGridViewCellEventArgs e)//Ver mas Cliente
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                this.Width = 1366;
                string _res = "";
                _co.Abrir();
                _co.AtraparCliente(dgv[0, e.RowIndex].Value.ToString(), ref _cli, ref _res);
                _co.MostrarPedidos(dgvPredido, _cli.Id_Cliente, ref _res);
                _co.MostrarNotas(dgvNota, _cli.Id_Cliente, ref _res);
                _co.Cerrar();
                lblCliente.Text = "Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            }
        }

        private void button13_Click(object sender, EventArgs e)//Tarjeta Cliente
        {
            this.Enabled = false;
            _tarCliente = new PantallaTarjetaCliente();
            _tarCliente.Inicializar(this, _co, _cli, _user);
            _tarCliente.Show();
        }

        private void dgvPredido_CellContentClick(object sender, DataGridViewCellEventArgs e)//Clic en Pedidos
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                switch (e.ColumnIndex)
                {
                        //caso Detalles
                    case 7:
                        this.Enabled = false;
                        _dePed = new DetallesPedido();
                        
                        string _res = "";
                        _co.Abrir();
                        _co.AtraparPedido(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _ped, ref _res);
                        _co.Cerrar();
                        _dePed.Inicializar(this, _co, _ped, _cli);
                        _dePed.Show();
                        break;

                        //caso Liquidar
                    case 8:
                        if (_user.NumTipo == 0)
                        {
                            this.Enabled = false;
                            _res = "";
                            _co.Abrir();
                            _co.AtraparPedido(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _ped, ref _res);
                            _co.Cerrar();
                            _eli = new Eliminar();
                            string _clave = "LIQUIDAR";
                            string _text = "Escriba \"LIQUIDAR\" para confirmar";
                            _eli.InicializarPedido(_co, _ped, this, _text, _clave);
                            _eli.Show();
                            break;
                        }
                        else
                            MessageBox.Show("el usuario no tiene permisos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)//Ver liquidados
        {
            _pedLiq = new PedidoLiquidado();
            _pedLiq.Inicializar(this, _co, _cli);
            this.Enabled = false;
            _pedLiq.Show();
        }

        private void dgvNota_CellContentClick(object sender, DataGridViewCellEventArgs e)//Notas
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                switch(e.ColumnIndex)
                {
                        //Abonar
                    case 5:
                        _cantidad = new Cantidad();
                        _cantidad.InicializarMain(this, Convert.ToInt32(dgv[2, e.RowIndex].Value), e.RowIndex, 1);
                        _cantidad.Show();
                        this.Enabled = false;
                        break;
                        //Detalles
                    case 6:
                        this.Enabled = false;
                        Nota _aux = new Nota();
                        _aux.Id_Nota = Convert.ToInt32(dgv[0,e.RowIndex].Value);
                        string _res = "";
                        _co.Abrir();
                        _co.AtraparNota(_aux.Id_Nota, ref _aux, ref _res);
                        _co.Cerrar();
                        _deNota = new DetallesNota();
                        _deNota.Inicializar(this, _co, _cli, _aux);
                        _deNota.Show();
                        break;

                        //Liquidar
                    case 7:
                        if (_user.NumTipo == 0)
                        {
                            Nota _no = new Nota();
                            _no.Id_Nota = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                            string _clave = "LIQUIDAR";
                            string _texto = "Escriba \"LIQUIDAR\" para Confirmar";
                            _eli = new Eliminar();
                            _eli.InicializarNota(_co, _no, this, _texto, _clave);
                            _eli.Show();
                            this.Enabled = false;
                            break;
                        }
                        else
                            MessageBox.Show("No se cuenta con permisos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)//Ocultar
        {
            this.Width = 600;
        }

        public void EnviarDatos(int cantidad, int row) //Abonar a nota
        {
            int nota_id = Convert.ToInt32(dgvNota[0,row].Value);
            Nota _no = new Nota();
            string _res = "";
            _co.Abrir();
            _co.AtraparNota(nota_id, ref _no, ref _res);
            _co.Cerrar();
            Abono _aux = new Abono();
            _aux.Cantidad = cantidad;
            _aux.Id_Nota = nota_id;
            _aux.Id_Pedido = _no.Id_Pedido;
            _aux.Id_User = _user.Login;
            _aux.Id_Cliente = _cli.Id_Cliente;
            _co.Abrir();
            bool _agregado = _co.AgregarAbono(_aux, ref _res);
            _co.Cerrar();
            if (_agregado)
                MessageBox.Show("Abono Agregado Nota Cliente: " + _cli.Nombre + " " + _cli.Apellido + " Nota ID " + _no.Id_Nota);
            else
                MessageBox.Show("No se pudo agregar abono","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Error);
            Refresh();
        }

        public void Refresh()
        {
            string _res = "";
            _co.Abrir();
            _co.AtraparCliente(_cli.Id_Cliente , ref _cli, ref _res);
            _co.MostrarPedidos(dgvPredido, _cli.Id_Cliente, ref _res);
            _co.MostrarNotas(dgvNota, _cli.Id_Cliente, ref _res);
            _co.Cerrar();
            MostrarClientes();
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)//Agregar Cliente
        {
            if (MessageBox.Show("¿Seguro que desea agregar cliente?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtApellidoCli.Text != "" && txtNombreCli.Text != "")
                {
                    Cliente _aux = new Cliente();
                    try
                    {
                        _aux.Nombre = txtNombreCli.Text;
                        _aux.Apellido = txtApellidoCli.Text;
                        _aux.Direccion = txtDireccionCli.Text;
                        _aux.Telefono = txtTelefonoCli.Text;
                        _aux.Id_Usuario = _user.Login;
                        _aux.Negocio = txtNegocioCliente.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " Datos erroneos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string _res = "";
                    _co.Abrir();
                    _co.AgregarCliente(_aux, ref _res);
                    _co.Cerrar();
                    txtNombreCli.Clear();
                    txtDireccionCli.Clear();
                    txtApellidoCli.Clear();
                    txtTelefonoCli.Clear();
                    MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Ingrese al menos nombre y apellido del cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)//Agregar Repartidor
        {
            if (MessageBox.Show("¿Seguro que desea agregar repartidor?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtApellidoRe.Text != "" && txtNombreRe.Text != "")
                {
                    Repartidor _aux = new Repartidor();
                    try
                    {
                        _aux.Nombre = txtNombreRe.Text;
                        _aux.Apellido = txtApellidoRe.Text;
                        _aux.Direccion = txtDireccionRe.Text;
                        _aux.Telefono = txtTelefonoRe.Text;
                        _aux.Id_User = _user.Login;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " Datos erroneos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string _res = "";
                    _co.Abrir();
                    _co.AgregarRepartidor(_aux, ref _res);
                    _co.Cerrar();
                    txtNombreRe.Clear();
                    txtDireccionRe.Clear();
                    txtApellidoRe.Clear();
                    txtTelefonoRe.Clear();
                    MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Ingrese al menos nombre y apellido del cliente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {

            gbRepartidor.Visible = false;
            this.Width = 600;
            lblCliente.Visible = false;
            cbTipoUser.SelectedIndex = 0;
        }

        private void button10_Click(object sender, EventArgs e)//Agregar usuario
        {
            if (MessageBox.Show("¿Seguro que desea agregar usuario?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtNombreUser.Text != "" && txtLogin.Text != "")
                {
                    string _res = "";
                    Usuario _aux = new Usuario();
                    _aux.Login = txtLogin.Text;
                    _co.Abrir();
                    bool _encontrado = _co.BuscarUsuario(_aux, ref _res);
                    _co.Cerrar();
                    if (!_encontrado)
                    {
                        if (txtContraseña.Text == txtConfirmarContraseña.Text && txtContraseña.Text != "")
                        {
                            _aux.Nombre = txtNombreUser.Text;
                            _aux.NumTipo = cbTipoUser.SelectedIndex;
                            _aux.Password = txtContraseña.Text;
                            _co.Abrir();
                            _co.AgregarUsuario(_aux, ref _res);
                            _co.Cerrar();
                            MessageBox.Show("Usuario: " + _aux.Nombre + " con login: " + _aux.Login);
                        }
                        else
                            MessageBox.Show("Verifique las contraseñas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Ingrese Nombre y login", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)//Bloquear usuario
        {
            if (MessageBox.Show("¿Seguro que desea bloquear el usuario?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtBloqueo.Text != "" && txtBloqueo.Text != _user.Login)
                {
                    string _res = "";
                    Usuario _aux = new Usuario();
                    _aux.Login = txtBloqueo.Text;
                    _co.Abrir();
                    bool _encontrado = _co.BuscarUsuario(_aux, ref _res);
                    _co.Cerrar();
                    if (_encontrado)
                    {
                        this.Enabled = false;
                        _eli = new Eliminar();
                        string _texto = "Escriba su contraseña para confirmar";
                        _eli.InicializarBloqueo(_co, _aux, this, _texto, _user.Password);
                        _eli.Show();
                        txtBloqueo.Clear();
                    }
                    else
                        MessageBox.Show("El usuario no se encuentra en el sistema", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Error en el login", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button14_Click(object sender, EventArgs e)//Desbloquear usuario
        {
            if (MessageBox.Show("¿Seguro que desea desbloquear el cliente?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtDesbloquear.Text != "" && txtDesbloquear.Text != _user.Login)
                {
                    string _res = "";
                    Usuario _aux = new Usuario();
                    _aux.Login = txtDesbloquear.Text;
                    _co.Abrir();
                    bool _encontrado = _co.BuscarUsuario(_aux, ref _res);
                    _co.Cerrar();
                    if (_encontrado)
                    {
                        this.Enabled = false;
                        _eli = new Eliminar();
                        string _texto = "Escriba su contraseña para confirmar";
                        _eli.InicializarDesbloquear(_co, _aux, this, _texto, _user.Password);
                        _eli.Show();
                        txtDesbloquear.Clear();
                    }
                    else
                        MessageBox.Show("El usuario no se encuentra en el sistema", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Error en el login", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage4_Enter(object sender, EventArgs e)//Cagar Productos
        {
            gbRepartidor.Visible = false;
            cbBuscarProd.SelectedIndex = 0;
            gbPedido.Visible = false;
            gbNota.Visible = false;
            string _res = "";
            _co.Abrir();
            _co.MostrarProductos(dgvProducto, ref _res);
            _co.Cerrar();
            
                this.Width = 600;
            lblCliente.Visible = false;
        }

        private void dgvProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)//dgv Producto 
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                switch(e.ColumnIndex)
                {
                        //caso Agregar cantidad
                    case 5:
                        this.Width = 600;
                        string _res = "";
                        _cantidad = new Cantidad();
                        _cantidad.InicializarMain(this, 10000, e.RowIndex,2);
                        _cantidad.Show();
                        break;

                        //caso detalles prestamo
                    case 6:
                        this.Width = 1000;
                        gbDetallesProducto.Visible = true;
                        _res = "";
                        _aux = new Producto();
                        _aux.Id_Producto = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                        _co.Abrir();
                        _co.AtraparProducto(_aux.Id_Producto, ref _aux, ref _res);
                        _co.Cerrar();
                        txtUsuarioProducto.Text = _aux.Id_User;
                        txtIDProducto.Text = _aux.Id_Producto.ToString();
                        txtNombreProducto.Text = _aux.Nombre;
                        txtFechaAltaProducto.Text = _aux.Fecha_Ingreso.ToShortDateString();
                        txtCantidadProducto.Text = _aux.Cantidad.ToString();
                        txtPrecioProducto.Text = _aux.Precio_General.ToString("n2");
                        txtDescripcionProducto.Text = _aux.Descripcion;
                        cbTipoProducto.Items.Clear();
                        cbTipoProducto.Items.Add(_aux.Acumulacion[_aux.Tipo]);
                        cbTipoProducto.SelectedIndex = 0;
                        break;
                }
             }
        }

        private void button15_Click(object sender, EventArgs e)//Buscar producto
        {

            if (txtBuscarProd.Text != "" && cbBuscarProd.SelectedIndex >= 0)
            {
                bool _encontrado = false;
                string _parametro = txtBuscarProd.Text;
                string _res = "";
                _co.Abrir();
                _encontrado = _co.BuscarProducto(dgvProducto, txtBuscarProd.Text, cbBuscarProd.SelectedIndex, ref _res);
                _co.Cerrar();
                if (!_encontrado)
                    MessageBox.Show("Producto no Encontrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Determinar Forma de Busqueda y Parametro", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void chModificar_CheckedChanged(object sender, EventArgs e)
        {
            if(chModificar.Checked)
            {
                txtPrecioProducto.ReadOnly = false;
                txtDescripcionProducto.ReadOnly = false;
                txtCantidadProducto.ReadOnly = false;
                cbTipoProducto.Enabled = true;
                cbTipoProducto.Items.Clear();
                for (int i = 0; i < _aux.Acumulacion.Length; i++)
                    cbTipoProducto.Items.Add(_aux.Acumulacion[i]);
                btnModify.Enabled = true;
                cbTipoProducto.SelectedIndex = 0;
                cbTipoProd.SelectedIndex = 0;
            }
            else
            {
                cbTipoProducto.SelectedIndex = 0;
                txtPrecioProducto.ReadOnly = true;
                txtDescripcionProducto.ReadOnly = true;
                txtCantidadProducto.ReadOnly = true;
                cbTipoProducto.Enabled = false;
                txtCantidadProducto.Text = _aux.Cantidad.ToString();
                txtPrecioProducto.Text = _aux.Precio_General.ToString();
                txtDescripcionProducto.Text = _aux.Descripcion;
                cbTipoProducto.Items.Clear();
                cbTipoProducto.Items.Add(_aux.Acumulacion[_aux.Tipo]);
                cbTipoProducto.SelectedIndex = 0;
                btnModify.Enabled = false;
            }
        }

        private void txtDescripcionProducto_ReadOnlyChanged(object sender, EventArgs e)// para readonlys
        {
            TextBox txt = (TextBox)sender;
            if(txt.ReadOnly)
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
            else
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
        }

        private void cbTipoProducto_EnabledChanged(object sender, EventArgs e)// para readonlys
        {
            ComboBox cb = (ComboBox)sender;
            if(cb.Enabled)
            {
                cb.BackColor = Color.White;
                cb.ForeColor = Color.Black;
            }
            else
            {
                cb.ForeColor = Color.White;
                cb.BackColor = Color.Blue;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)//Modificar producto
        {
            if (MessageBox.Show("¿Modificar Producto?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtCantidadProducto.Text != "" && txtPrecioProducto.Text != "" && cbTipoProducto.SelectedIndex >= 0)
                {
                    if (txtPrecioProducto.Text.IndexOf('.') <= 0)
                    {
                        try
                        {
                            _aux.Tipo = cbTipoProducto.SelectedIndex;
                            _aux.Cantidad = Convert.ToInt32(txtCantidadProducto.Text);
                            _aux.Precio_General = double.Parse(txtPrecioProducto.Text);
                            _aux.Descripcion = txtDescripcionProducto.Text;
                            string _res = "";
                            _co.Abrir();
                            _co.ModificarProducto(_aux, ref _res);
                            MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _co.MostrarProductos(dgvProducto, ref _res);
                            _co.AtraparProducto(_aux.Id_Producto, ref _aux, ref _res);
                            chModificar.Checked = false;
                            _co.Cerrar();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                        MessageBox.Show("Utilice ',' en vez de '.' para los decimales", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Almenos ingresar Cantidad, Precio y Tipo de producto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)//Regresar tamaño forma
        {
            this.Width = 600;
            lblCliente.Visible = false;
        }

        private void button16_Click(object sender, EventArgs e)//Ocultar
        {
            this.Width = 600;
        }

        private void button12_Click(object sender, EventArgs e)//Agregar Producto
        {
            if (MessageBox.Show("¿Agregar Producto?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtNombreProd.Text != "" && txtCantidadProd.Text != "" && txtPrecioGeneralProd.Text != "" && cbTipoProd.SelectedIndex >= 0)
                {
                    if (txtPrecioGeneralProd.Text.IndexOf('.') <= 0)
                    {
                        Producto _aux1 = new Producto();
                        try
                        {
                            this.Width = 600;
                            _aux1.Nombre = txtNombreProd.Text;
                            _aux1.Precio_General = Convert.ToDouble(txtPrecioGeneralProd.Text);
                            _aux1.Tipo = cbTipoProd.SelectedIndex;
                            _aux1.Cantidad = Convert.ToInt32(txtCantidadProd.Text);
                            _aux1.Id_User = _user.Login;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string _res = "";
                        _co.Abrir();
                        _co.AgregarProducto(_aux1, ref _res);
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _co.MostrarProductos(dgvProducto, ref _res);
                        _co.Cerrar();
                    }
                    else
                        MessageBox.Show("Utilie ',' en vez de '.' para los decimales", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Ingrese los datos marcados", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EnviarDatosProducto(int cantidad, int row)//Datos para agregar producto
        {
            Producto _aux1 = new Producto();
            _aux1.Cantidad = cantidad;
            _aux1.Id_Producto = Convert.ToInt32(dgvProducto[0, row].Value);
            string _res = "";
            _co.Abrir();
            _co.AgregarCantidadProducto(_aux1, ref _res);
            MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _co.MostrarProductos(dgvProducto, ref _res);
            _co.Cerrar();
        }

        private void tabPage5_Enter(object sender, EventArgs e)//Repartidores
        {
            lblCliente.Visible = false;
            this.Width = 600;
            string _res = "";
            gbPedido.Visible = false;
            gbNota.Visible = false;
            _co.Abrir();
            _co.MostrarRepartidores(dgvRepartidor, ref  _res);
            _co.Cerrar();
            gbDetallesProducto.Visible = false;
            gbRepartidor.Visible = true;
        }

        private void dgvRepartidor_CellContentClick(object sender, DataGridViewCellEventArgs e)//Detalles Repartidor
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _flag = true;
                gbRepartidor.Visible = true;
                _repartidor = new Repartidor();
                _repartidor.Id_Repartidor = dgv[0, e.RowIndex].Value.ToString();
                string _res = "";
                _co.Abrir();
                _co.AtraparRepartidor(_repartidor.Id_Repartidor, ref _repartidor, ref _res);
                _co.Cerrar();
                txtNombreRepartidor.Text = _repartidor.Nombre;
                txtApellidoRepartidor.Text = _repartidor.Apellido;
                txtDireccionRepartidor.Text = _repartidor.Direccion;
                txtFechaAlta.Text = _repartidor.Fecha_Alta.ToShortDateString();
                txtTelefonoRepartidor.Text = _repartidor.Telefono;
                txtUsuarioRepartidor.Text = _repartidor.Id_User;
                txtIDRepartidor.Text = _repartidor.Id_Repartidor.ToString();
                _co.Abrir();
                _co.MostrarPedidosRepartidor(dgvPedidosRepartidor, _repartidor, ref _res);
                _co.Cerrar();
                this.Width = 1000;
            }
        }

        private void button18_Click(object sender, EventArgs e)//Ocultar
        {
            this.Width = 600;
        }

        private void chMoficiarRepartidor_CheckedChanged(object sender, EventArgs e)
        {
            if(chMoficiarRepartidor.Checked)
            {
                txtDireccionRepartidor.ReadOnly = false;
                txtTelefonoRepartidor.ReadOnly = false;
                txtNombreRepartidor.ReadOnly = false;
                txtApellidoRepartidor.ReadOnly = false;
                btnModificarRepartidor.Enabled = true;
            }
            else
            {
                txtDireccionRepartidor.ReadOnly = true;
                txtTelefonoRepartidor.ReadOnly = true;
                txtNombreRepartidor.ReadOnly = true;
                txtApellidoRepartidor.ReadOnly = true;
                btnModificarRepartidor.Enabled = false;
                if (_flag == true)
                {
                    txtNombreRepartidor.Text = _repartidor.Nombre;
                    txtApellidoRepartidor.Text = _repartidor.Apellido;
                    txtDireccionRepartidor.Text = _repartidor.Direccion;
                    txtFechaAlta.Text = _repartidor.Fecha_Alta.ToShortDateString();
                    txtTelefonoRepartidor.Text = _repartidor.Telefono;
                    txtUsuarioRepartidor.Text = _repartidor.Id_User;
                    txtIDRepartidor.Text = _repartidor.Id_Repartidor.ToString();
                }
            }
        }

        private void btnModificarRepartidor_Click(object sender, EventArgs e)//Modificar Repartidor
        {
            if (MessageBox.Show("¿Modificar Repartidor?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtNombreRepartidor.Text != "" && txtApellidoRepartidor.Text != "")
                {
                    _repartidor.Nombre = txtNombreRepartidor.Text;
                    _repartidor.Apellido = txtApellidoRepartidor.Text;
                    _repartidor.Telefono = txtTelefonoRepartidor.Text;
                    _repartidor.Direccion = txtDireccionRepartidor.Text;
                    string _res = "";
                    _co.Abrir();
                    _co.ModificarRepartidor(_repartidor, ref _res);
                    MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _co.MostrarRepartidores(dgvRepartidor,ref  _res);
                    _co.Cerrar();
                    chMoficiarRepartidor.Checked = false;
                }
                else
                    MessageBox.Show("Al menos agregue nombre y apellido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDireccionRepartidor_ReadOnlyChanged(object sender, EventArgs e)//Readonlys
        {
            TextBox txt = (TextBox)sender;
            if(txt.ReadOnly)
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
            else
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
        }

        private void txtTelefonoRepartidor_ReadOnlyChanged(object sender, EventArgs e)
        {
            MaskedTextBox txt = (MaskedTextBox)sender;
            if (txt.ReadOnly)
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
            else
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
        }

        private void dgvPedidosRepartidor_CellContentClick(object sender, DataGridViewCellEventArgs e)//Detalles Pedido 
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                 this.Enabled = false;
                 _ped = new Pedido();
                 _ped.Id_Pedido = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                 string _res = "";

                 _co.Abrir();
                 _co.AtraparPedido(_ped.Id_Pedido, ref _ped, ref _res);
                 _co.AtraparCliente(_ped.Id_Cliente, ref _cli, ref _res);
                 _co.Cerrar();

                 _dePed = new DetallesPedido();
                 _dePed.Inicializar(this, _co, _ped, _cli);
                 _dePed.Show();
             }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)//Filtrar por fecha de entrega
        {
            string _res = "";
            _co.Abrir();
            _co.FiltrarFechaEntrega(dgvPedidos, fecha_entrega.Value, ref _res);
            _co.Cerrar();
        }

        private void tabPage6_Enter(object sender, EventArgs e)
        {
            this.Width = 600;
            string _res = "";
            _co.Abrir();
            _co.MostrarTodosPedidos(dgvPedidos, ref _res);
            _co.Cerrar();
        }

        private void button17_Click(object sender, EventArgs e)//Reiniciar
        {
            string _res = "";
            _co.Abrir();
            _co.MostrarProductos(dgvProducto, ref _res);
            _co.Cerrar();
        }

        private void fecha_pedido_ValueChanged(object sender, EventArgs e)
        {
            string _res = "";
            _co.Abrir();
            _co.FiltrarFechaPedido(dgvPedidos, fecha_pedido.Value, ref _res);
            _co.Cerrar();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Width = 600;
            string _res = "";
            _co.Abrir();
            _co.MostrarTodosPedidos(dgvPedidos, ref _res);
            _co.Cerrar();
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                 switch(e.ColumnIndex)
                 {
                     case 7:
                         string _res = "";
                         _ped = new Pedido();
                         _ped.Id_Pedido = Convert.ToInt32(dgv[0,e.RowIndex].Value);
                         _co.Abrir();
                         _co.AtraparPedido(_ped.Id_Pedido, ref _ped, ref _res );
                         _co.AtraparCliente(_ped.Id_Cliente, ref _cli, ref _res);
                         _co.Cerrar();
                         this.Enabled = false;
                         _dePed = new DetallesPedido();
                         _dePed.Inicializar(this, _co, _ped, _cli);
                         _dePed.Show();
                         break;

                     case 8:
                         if (_user.NumTipo == 0)
                         {
                             _res = "";
                             _ped = new Pedido();
                             _ped.Id_Pedido = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                             _co.Abrir();
                             _co.AtraparPedido(_ped.Id_Pedido, ref _ped, ref _res);
                             _co.Cerrar();
                             this.Enabled = false;
                             _eli = new Eliminar();
                             _eli.InicializarPedido(_co, _ped, this, "Escribir \"LIQUIDAR\" para confirmar", "LIQUIDAR");
                             _eli.Show();
                             break;
                         }
                         else
                             MessageBox.Show("el usuario no tiene permisos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         break;
                 }
             }
        }

        private void tabPage7_Enter(object sender, EventArgs e)//Reportes
        {
            this.Width = 600;
            double abonos = 0, pedido = 0;
            string _res = "";
            _co.Abrir();
            _co.ReportesFecha(fecha_reporte.Value, ref abonos, ref pedido, ref _res);
            _co.Cerrar();
            txtCantidadAbonos.Text = abonos.ToString();
            txtNoPedidos.Text = pedido.ToString();
        }

        private void fecha_reporte_ValueChanged(object sender, EventArgs e)//Reportes
        {
            double abonos = 0, pedido = 0;
            string _res = "";
            _co.Abrir();
            _co.ReportesFecha(fecha_reporte.Value, ref abonos, ref pedido, ref _res);
            _co.Cerrar();
            txtCantidadAbonos.Text = abonos.ToString("n2");
            txtNoPedidos.Text = pedido.ToString("n2");
        }

        private void tabPage8_Enter(object sender, EventArgs e)//cerrar Sesion
        {
            _co.Abrir();
            _co.MostrarProveedores(dgvProveedor);
            _co.Cerrar();
        }

        private void button20_Click(object sender, EventArgs e)// Generar Reportes
        {
            _co.Abrir();
            ArrayList _pedidos =  _co.PedidosRepartidor(_repartidor);
            _co.Cerrar();
            Documentos.GenerarListaPedidos(_pedidos, _repartidor);
        }

        private void button21_Click(object sender, EventArgs e)//Generar Reporte
        {
            _co.Abrir();
            ArrayList Precios = _co.Productos();
            _co.Cerrar();
            Documentos.GenerrListaProductos(Precios);
        }

        private void tabPage9_Enter(object sender, EventArgs e)//Cerrar Sesion
        {
            if (MessageBox.Show("¿Desea Cerrar Sesion?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _cerrarSesion = true;
                this.Close();
            }
        }

        private void button22_Click(object sender, EventArgs e)//Agregar Proveedor
        {
            if(MessageBox.Show("¿Seguro Agregar Proveedor?","Aviso",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtNombreProv.Text != "" && txtApellidoProv.Text != "" && txtNegocioProv.Text != "")
                {
                    Proveedor _prov = new Proveedor();
                    try 
                    {
                        _prov.Nombre = txtNombreProv.Text;
                        _prov.Apellido = txtApellidoProv.Text;
                        _prov.Telefono = txtTelefonoProv.Text;
                        _prov.Direccion = txtDireccionProv.Text;
                        _prov.Negocio = txtNegocioProv.Text;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + "Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    _co.Abrir();
                    _co.AgregarProveedor(_prov);
                    _co.MostrarProveedores(dgvProveedor);
                    _co.Cerrar();
                    MessageBox.Show("Proveedor agregado con exito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Al menos indique nombre, apellido y nombre del negocio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
