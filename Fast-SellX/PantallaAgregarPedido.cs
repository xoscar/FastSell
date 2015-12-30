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
    public partial class PantallaAgregarPedido : Form
    {
        public PantallaAgregarPedido()
        {
            InitializeComponent();
            chContado.Checked = true;
            cbBuscarProd.SelectedIndex = 0;
        }
        PantallaAdm _adm;
        Conexion _co;
        Cliente _cli;
        Cantidad _ca;
        ArrayList _prod = new ArrayList();
        Producto _producto;
        Pedido _ped;
        Usuario _usr;
        Nota _no;
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        public void Inicializar(PantallaAdm adm, Conexion co, Cliente cli, Usuario usr)//Inicializa
        {
            _adm = adm;
            _co = co;
            _cli = cli;
            _usr = usr;
            this.Text = "Agregar Pedido a Cliente: " + _cli.Nombre + " " + _cli.Apellido + " con ID: " + _cli.Id_Cliente;
            MostrarProductosCliente();
            string _res = "";
            _co.Abrir();
            _co.RepartidorCombo(cbBusqueda, ref _res);
            _co.Cerrar();
            if(cbBusqueda.Items.Count > 0)
                cbBusqueda.SelectedIndex = 0;
        }

        private void PantallaAgregarPedido_FormClosing(object sender, FormClosingEventArgs e)//Al cerrar
        {
            _adm.Enabled = true;
            _adm.Refresh();
        }

        public void MostrarProductosCliente()//Mostrar los productos de un cliente
        {
            Ordenar(_cli.Precios, 0, _cli.Precios.Count - 1);
            dgvProducto.Rows.Clear();
            for (int i = 0; i < _cli.Precios.Count; i++)
            {
                dgvProducto.Rows.Add();
                Producto _aux = ((Producto)_cli.Precios[i]);
                dgvProducto[0, i].Value = _aux.Id_Producto;
                dgvProducto[1, i].Value = _aux.Nombre;
                dgvProducto[2, i].Value = _aux.Precio_General;
                dgvProducto[3, i].Value = _aux.Cantidad;
                dgvProducto[4, i].Value = _aux.Acumulacion[_aux.Tipo];
                dgvProducto[5, i].Value = "+";
            }
        }

        private void dgvProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)//Agregar Producto al pedido
        {
            DataGridView dgv = (DataGridView)sender;
            if( dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (Convert.ToInt32(dgv[3, e.RowIndex].Value) > 0)
                {
                    string _res = "";
                    _producto = new Producto();
                    _producto.Id_Producto = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                    _producto.Nombre = dgv[1, e.RowIndex].Value.ToString();
                    _producto.Precio_General = Convert.ToSingle(dgv[2, e.RowIndex].Value);
                    _producto.Cantidad = Convert.ToInt32(dgv[3, e.RowIndex].Value);
                    Producto _aux = new Producto();
                    _co.Abrir();
                    _co.AtraparProducto(_producto.Id_Producto, ref _aux, ref _res);
                    _co.Cerrar();
                    _producto.Tipo = _aux.Tipo;
                    bool _encontrado = false;
                    if (_prod.Count > 0)
                    {
                        Ordenar(_prod, 0, _prod.Count - 1);
                        _encontrado = Buscar(_prod, _producto, 0, _prod.Count - 1);
                    }
                    if (!_encontrado)
                    {
                        this.Enabled = false;
                        _ca = new Cantidad();
                        _ca.inicializar(Convert.ToInt32(dgv[3, e.RowIndex].Value), this, e.RowIndex);
                        _ca.Show();
                    }
                    else
                        MessageBox.Show("Este producto ya se encuentra en el pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Producto terminado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void MostrarProductos()//Mostrar Producto agregado al pedido
        {
            dgvPedido.Rows.Clear();
            int j = 0;
            double _total = 0;
            for (int i = 0; i < _prod.Count; i++)
            {
                txtPrecio.Clear();
                Producto producto = ((Producto)_prod[i]);
                if (producto.Id_Producto != 0)
                {
                    dgvPedido.Rows.Add();
                    dgvPedido[0, j].Value = producto.Id_Producto;
                    dgvPedido[1, j].Value = producto.Nombre;
                    dgvPedido[2, j].Value = producto.Precio_General;
                    dgvPedido[3, j].Value = producto.Cantidad;
                    dgvPedido[4, j].Value = producto.Acumulacion[_producto.Tipo];
                    dgvPedido[5, j].Value = producto.Cantidad * producto.Precio_General;
                    dgvPedido[6, j++].Value = "X";
                    _total += producto.Cantidad * producto.Precio_General;
                }
                txtPrecio.Text = _total.ToString("n2");
            }
        }
        public void EnviarDatos(int can, int _row)//Pantalla cantidad datos regresados
        {
            _producto.Cantidad = can;
            _prod.Add(_producto);
            Ordenar(_prod, 0, _prod.Count-1);
            Ordenar(_cli.Precios, 0, _cli.Precios.Count - 1);
            BuscarRestar(_cli.Precios, _producto, 0, _cli.Precios.Count - 1, can);
            MostrarProductosCliente();
            MostrarProductos();
        }

        private void dgvPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)//Quitar
        {
            DataGridView dgv = (DataGridView)sender;
            if(dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (MessageBox.Show("Desea remover del pedido", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Ordenar(_prod, 0, _prod.Count - 1);
                    Producto _aux = new Producto();
                    _aux.Id_Producto =  Convert.ToInt32(dgv[0, e.RowIndex].Value);
                    _aux.Cantidad = Convert.ToInt32(dgv[3, e.RowIndex].Value);
                    bool _removido = BuscarQuitar(_prod, _aux, 0, _prod.Count - 1);
                    if(_removido)
                    {
                        Ordenar(_cli.Precios, 0, _cli.Precios.Count-1);
                        BuscarAumentar(_cli.Precios,_aux,  0, _cli.Precios.Count - 1, _aux.Cantidad);
                        MessageBox.Show("Producto removido del pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MostrarProductosCliente();
                        MostrarProductos();
                    }
                    else
                        MessageBox.Show("No se pudo remover", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Ordenamiento y busqueda por ID
        public void Ordenar(ArrayList x, int lb, int ub)
        {
            if (lb >= ub)
                return;
            int j = 0;
            Partition(x, lb, ub, ref j);
            Ordenar(x, lb, j - 1);
            Ordenar(x, j + 1, ub);
        }
        public void Partition(ArrayList x, int lb, int ub, ref int j)
        {
            int down, up;
            Producto temp, a;
            a = (Producto)x[lb];
            up = ub;
            down = lb;
            while (down < up)
            {
                while ((Producto)x[down] <= a && down < ub)
                    down++;
                while ((Producto)x[up] > a)
                    up--;
                if (down < up)
                {
                    temp = (Producto)x[down];
                    x[down] = x[up];
                    x[up] = temp;
                }
            }
            x[lb] = x[up];
            x[up] = a;
            j = up;
        }
        public  bool BuscarQuitar(ArrayList x, Producto key, int low, int hi)
        {
            if (low <= hi)
            {
                int mid = (low + hi) / 2;
                if (key == (Producto)x[mid])
                {
                    ((Producto)x[mid]).Id_Producto = 0;
                    return true;
                }
                else if (key < (Producto)x[mid])
                    return BuscarQuitar(x, key, low, mid - 1);
                else
                    return BuscarQuitar(x, key, mid + 1, hi);
            }
            else
                return false;
        }

        public bool BuscarAumentar(ArrayList x, Producto key, int low, int hi, int can)
        {
            if (low <= hi)
            {
                int mid = (low + hi) / 2;
                if (key == (Producto)x[mid])
                {
                    ((Producto)x[mid]).Cantidad += can;
                    return true;
                }
                else if (key < (Producto)x[mid])
                    return BuscarAumentar(x, key, low, mid - 1, can);
                else
                    return BuscarAumentar(x, key, mid + 1, hi, can);
            }
            else
                return false;
        }
        
        public bool Buscar(ArrayList x, Producto key, int low, int hi)
        {
            if (low <= hi)
            {
                int mid = (low + hi) / 2;
                if (key == (Producto)x[mid])
                    return true;
                else if (key < (Producto)x[mid])
                    return Buscar(x, key, low, mid - 1);
                else
                    return Buscar(x, key, mid + 1, hi);
            }
            else
                return false;
        }

        public bool BuscarRestar(ArrayList x, Producto key, int low, int hi, int can)
        {
            if (low <= hi)
            {
                int mid = (low + hi) / 2;
                if (key == (Producto)x[mid])
                {
                    ((Producto)x[mid]).Cantidad -= can;
                    return true;
                }
                else if (key < (Producto)x[mid])
                    return BuscarRestar(x, key, low, mid - 1, can);
                else
                    return BuscarRestar(x, key, mid + 1, hi, can);
            }
            else
                return false;
        }

        public Producto BuscarID(ArrayList x, int key, int low, int hi)
        {
            if (low <= hi)
            {
                int mid = (low + hi) / 2;
                if (key == ((Producto)x[mid]).Id_Producto)
                    return ((Producto)x[mid]);
                else if (key < ((Producto)x[mid]).Id_Producto)
                    return BuscarID(x, key, low, mid - 1);
                else
                    return BuscarID(x, key, mid + 1, hi);
            }
            else
                return new Producto();
        }

        private void chContado_CheckedChanged(object sender, EventArgs e)
        {
            if (chContado.Checked)
            {
                txtSemanas.Clear();
                txtDescripcion.Clear();
                txtNota.Clear();
                txtDescripcion.ReadOnly = true;
                txtNota.ReadOnly = true;
                btnNota.Enabled = false;
                txtSemanas.ReadOnly = true;
            }
            else
            {
                btnNota.Enabled = true;
                txtDescripcion.ReadOnly = false;
                txtNota.ReadOnly = false;
                txtSemanas.ReadOnly = false;
                btnNota.Enabled = true;
            }
        }

        private void txtSemanas_ReadOnlyChanged(object sender, EventArgs e)//Cambio del los texbox por readonly
        {
            TextBox txt = (TextBox)sender;
            if (txt.ReadOnly == true)
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

        private void btnNota_Click(object sender, EventArgs e)//cargar Nota
        {
            OpenFileDialog _ofd = new OpenFileDialog();
            _ofd.Title = "Agregar Nota";
            _ofd.DefaultExt = ".jpg";
            _ofd.RestoreDirectory = true;
            if (_ofd.ShowDialog() == DialogResult.OK)
            {
                txtNota.Text = _ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)//Agregar Pedido
        {
            if (MessageBox.Show("¿Seguro que desea agregar pedido?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (chContado.Checked)
                {
                    if (txtPrecio.Text != "")
                    {
                        if (dateTimePicker1.Value.Date.CompareTo(DateTime.Now.Date) >= 0)
                        {
                            if (cbBusqueda.SelectedIndex >= 0)
                            {
                                bool _agregado = false;
                                _ped = new Pedido();
                                _ped.Fecha_Entrega = dateTimePicker1.Value;
                                _ped.Precio_Total = Convert.ToSingle(txtPrecio.Text);
                                _ped.Id_Repartidor = cbBusqueda.Items[cbBusqueda.SelectedIndex].ToString();
                                _ped.Productos = _prod;
                                _ped.Contado = true;
                                _ped.Id_Usuario = _usr.Login;
                                _ped.Id_Cliente = _cli.Id_Cliente;
                                string _res = "";
                                int _pedId = 0;
                                _co.Abrir();

                                _agregado = _co.AgregarPedido(_ped, ref _res, ref _pedId);
                                _co.Cerrar();

                                Conexion _conAux = new Conexion();
                                for (int i = 0; i < _cli.Precios.Count; i++)
                                {
                                    Producto _aux = ((Producto)_cli.Precios[i]);

                                    _conAux.Abrir();
                                    _agregado = _conAux.ModificarProductoCantidad(_aux, ref _res);
                                    _conAux.Cerrar();

                                }
                                if (_agregado)
                                {
                                    Pedido _aux = new Pedido();
                                    _res = "";

                                    _co.Abrir();
                                    _co.AtraparPedido(_pedId, ref _aux, ref _res);
                                    _co.Cerrar();

                                    Documentos.GenerarPedido(_aux, _cli);
                                    MessageBox.Show("Pedido Agregado a Cliente: " + _cli.Nombre + " " + _cli.Apellido, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    _prod = new ArrayList();
                                    MostrarProductos();
                                    txtDescripcion.Clear();
                                    txtNota.Clear();
                                    txtPrecio.Clear();
                                    txtSemanas.Clear();
                                    chContado.Checked = true;
                                }

                            }
                            else
                                MessageBox.Show("Seleccione a un Repartidor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            MessageBox.Show("Fecha pasada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Ingresar al Menos un Producto al Pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (txtPrecio.Text != "")
                    {
                        if (dateTimePicker1.Value.Date.CompareTo(DateTime.Now.Date) >= 0)
                        {
                            if (cbBusqueda.SelectedIndex >= 0 && txtSemanas.Text != "")
                            {
                                _no = new Nota();
                                try
                                {
                                    _no.Descripcion = txtDescripcion.Text;
                                    _no.Semanas = Convert.ToInt32(txtSemanas.Text);
                                    _no.Pagare = txtNota.Text;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message + " Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                bool _agregado = false;
                                _ped = new Pedido();
                                _ped.Fecha_Entrega = dateTimePicker1.Value;
                                _ped.Precio_Total = Convert.ToSingle(txtPrecio.Text);
                                _no.Cantidad = _ped.Precio_Total;
                                _ped.Id_Repartidor = cbBusqueda.Items[cbBusqueda.SelectedIndex].ToString();
                                _ped.Productos = _prod;
                                _ped.Contado = false;
                                _ped.Id_Usuario = _usr.Login;
                                _no.Id_Cliente = _cli.Id_Cliente;
                                _no.Id_User = _usr.Login;
                                _ped.Id_Cliente = _cli.Id_Cliente;
                                string _res = "";
                                int _pedId = 0;
                                _co.Abrir();
                                _agregado = _co.AgregarPedido(_ped, ref _res, ref _pedId);
                                _co.Cerrar();
                                _no.Id_Pedido = _pedId;
                                Conexion _conAux = new Conexion();
                                for (int i = 0; i < _cli.Precios.Count; i++)
                                {
                                    Producto _aux = ((Producto)_cli.Precios[i]);

                                    _conAux.Abrir();
                                    _agregado = _conAux.ModificarProductoCantidad(_aux, ref _res);
                                    _conAux.Cerrar();
                                }

                                Conexion _coNota = new Conexion();
                                _coNota.Abrir();
                                _agregado = _coNota.AgregarNota(_no, ref _res);
                                _coNota.Cerrar();

                                if (_agregado)
                                {
                                    Pedido _aux = new Pedido();
                                    _res = "";

                                    _co.Abrir();
                                    _co.AtraparPedido(_pedId, ref _aux, ref _res);
                                    _co.Cerrar();

                                    _co.Abrir();
                                    _co.AtraparNotaPedido(_pedId, ref _no);
                                    _co.Cerrar();

                                    Documentos.GenerarPedido(_aux, _cli);
                                    Documentos.GenerarNota(_no, _cli);

                                    MessageBox.Show("Pedido Agregado a Cliente: " + _cli.Nombre + " " + _cli.Apellido + " Con su respectiva nota", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    _prod = new ArrayList();
                                    MostrarProductos();
                                    txtDescripcion.Clear();
                                    txtNota.Clear();
                                    txtPrecio.Clear();
                                    txtSemanas.Clear();
                                    chContado.Checked = true;
                                }
                            }
                            else
                                MessageBox.Show("Seleccione a un Repartidor y las semanas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            MessageBox.Show("Fecha pasada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Ingresar al Menos un Producto al Pedido", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)//Buscar
        {
            int id = 0;
            string nombre = "";
            if (txtBuscarProd.Text != "" && cbBuscarProd.SelectedIndex >= 0)
            {
                switch (cbBuscarProd.SelectedIndex)
                {
                    //Id
                    case 0:
                        try
                        {
                            id = Convert.ToInt32(txtBuscarProd.Text);
                        }
                        catch(Exception  ex)
                        {
                            MessageBox.Show("Error en los datos","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                        Producto _aux = BuscarID(_cli.Precios, id, 0, _cli.Precios.Count - 1);
                        if (_aux.Nombre != "")
                        {
                            dgvProducto.Rows.Clear();
                            dgvProducto.Rows.Add();
                            dgvProducto[0, 0].Value = _aux.Id_Producto;
                            dgvProducto[1, 0].Value = _aux.Nombre;
                            dgvProducto[2, 0].Value = _aux.Precio_General;
                            dgvProducto[3, 0].Value = _aux.Cantidad;
                            dgvProducto[4, 0].Value = _aux.Acumulacion[_aux.Tipo];
                            dgvProducto[5, 0].Value = "+";
                        }
                        else
                            MessageBox.Show("No encontrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    //Nombre
                    case 1:
                        try
                        {
                            nombre = txtBuscarProd.Text;
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error en los datos", "Aviso", MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }
                        ArrayList _aux1 = new ArrayList();
                        bool _flag = false;
                        for (int i = 0; i < _cli.Precios.Count; i++)
                            if (((Producto)_cli.Precios[i]).Nombre == nombre)
                           {
                                _aux1.Add(_cli.Precios[i]);
                                _flag = true;
                            }
                        if(_flag)
                        {
                            dgvProducto.Rows.Clear();
                            for (int i = 0; i < _aux1.Count; i++)
                            {
                                Producto _aux2 = ((Producto)_aux1[i]);
                                dgvProducto.Rows.Add();
                                dgvProducto[0, i].Value = _aux2.Id_Producto;
                                dgvProducto[1, i].Value = _aux2.Nombre;
                                dgvProducto[2, i].Value = _aux2.Precio_General;
                                dgvProducto[3, i].Value = _aux2.Cantidad;
                                dgvProducto[4, i].Value = _aux2.Acumulacion[_aux2.Tipo];
                                dgvProducto[5, i].Value = "+";
                            }
                        }
                        break;
                }
            }
            else
                MessageBox.Show("Elija la opcion de busqueda e ingrese el parametro", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void button17_Click(object sender, EventArgs e)//Reiniciar
        {
            MostrarProductosCliente();
        }

        private void PantallaAgregarPedido_Load(object sender, EventArgs e)
        {

        }
    }
}
