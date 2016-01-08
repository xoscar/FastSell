using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;

namespace Fast_SellX
{
    public partial class PantallaTarjetaCliente : Form
    {
        public PantallaTarjetaCliente()
        {
            InitializeComponent();
            cbBuscarProd.SelectedIndex = 0;
        }

        PantallaAdm _adm;
        Conexion _co;
        Cliente _cli;
        int id = -1;

        public void Inicializar(PantallaAdm adm, Conexion co, Cliente cli, Usuario usr)//Para inicializar la pantalla
        {
            _adm = adm;
            _co = co;
            _cli = cli;
            MostrarCliente();
            if (usr.NumTipo == 1)
                chModificar.Enabled = false;
        }

        private void PantallaTarjetaCliente_FormClosing(object sender, FormClosingEventArgs e)//Al cerrar
        {
            _adm.Enabled = true;
            _adm.Refresh();
            _adm.MostrarClientes();
        }

        private void chModificar_CheckedChanged(object sender, EventArgs e)//si modificar activo
        {
            if (chModificar.Checked)
            {
                txtNombre.ReadOnly = false;
                txtApellido.ReadOnly = false;
                txtDireccion.ReadOnly = false;
                txtTelefono.ReadOnly = false;
                btnModify.Enabled = true;
                txtNegocio.ReadOnly = false;
            }
            else
            {
                txtNombre.ReadOnly = true;
                txtApellido.ReadOnly = true;
                txtDireccion.ReadOnly = true;
                txtTelefono.ReadOnly = true;
                MostrarCliente();
                btnModify.Enabled = true;
                txtNegocio.ReadOnly = true;
            }
        }

        private void txtNombre_ReadOnlyChanged(object sender, EventArgs e)//Cambiar colores textbox
        {
            TextBox txt = (TextBox)sender;
            if (txt.ReadOnly == false)
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
            else
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
        }

        private void button3_Click(object sender, EventArgs e)//Modificar
        {
            if (MessageBox.Show("¿Seguro que desea modificar cliente?", "Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _cli.Nombre = txtNombre.Text;
                    _cli.Apellido = txtApellido.Text;
                    _cli.Direccion = txtDireccion.Text;
                    _cli.Telefono = txtTelefono.Text;
                    _cli.Negocio = txtNegocio.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string _res = "";
                _co.Abrir();
                _co.ModificarCliente(_cli, ref _res);
                _co.Cerrar();
                MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _co.Abrir();
                _co.AtraparCliente(_cli.Id_Cliente, ref _cli, ref _res);
                _co.Cerrar();

                MostrarCliente();
                chModificar.Checked = false;
                
            }
            else
                MostrarCliente();
        }

        public void MostrarCliente()
        {
            this.Text = "Tarjeta del Cliente: " + _cli.Nombre + " " + _cli.Apellido + " con ID: " + _cli.Id_Cliente;
            txtNombre.Text = _cli.Nombre;
            txtDireccion.Text = _cli.Direccion;
            txtApellido.Text = _cli.Apellido;
            txtFechaAlta.Text = _cli.Fecha_Alta.ToShortDateString();
            txtID.Text = _cli.Id_Cliente;
            txtUser.Text = _cli.Id_Usuario;
            txtTelefono.Text = _cli.Telefono;
            txtNegocio.Text = _cli.Negocio;
            dgvProducto.Rows.Clear();
            for (int i = 0; i < _cli.Precios.Count; i++)
            {
                dgvProducto.Rows.Add();
                dgvProducto[0, i].Value = ((Producto)_cli.Precios[i]).Id_Producto;
                dgvProducto[1, i].Value = ((Producto)_cli.Precios[i]).Nombre;
                dgvProducto[2, i].Value = ((Producto)_cli.Precios[i]).Precio_General;
                dgvProducto[3, i].Value = ((Producto)_cli.Precios[i]).Cantidad;
                dgvProducto[4, i].Value = ((Producto)_cli.Precios[i]).Acumulacion[((Producto)_cli.Precios[i]).Tipo];
                dgvProducto[5, i].Value = "+";
            }
        }

        private void txtTelefono_ReadOnlyChanged(object sender, EventArgs e)//Cambio de modify para masked text
        {
            MaskedTextBox txt = (MaskedTextBox)sender;
            if (txt.ReadOnly == false)
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
            else
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)//Agregar Precio
        {
            if (txtPrecio.Text != "" && txtNombreProd.Text != "")
            {
                if(MessageBox.Show("¿Seguro que desea Agregar el Precio?","Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //if (txtPrecio.Text.IndexOf('.') <= 0)
                    //{
                        string _nombre = "";
                        double _pres = 0.0;
                        try
                        {
                            _pres = double.Parse(txtPrecio.Text  , System.Globalization.CultureInfo.InvariantCulture);
                            _nombre = txtNombreProd.Text;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + " Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string _res = "";
                        _co.Abrir();
                        bool a = _co.AgregarPrecio(_cli, _nombre, id, _pres, ref _res);
                        _co.Cerrar();

                        if (a)
                        {
                            MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            _co.Abrir();
                            _co.AtraparCliente(_cli.Id_Cliente, ref _cli, ref _res);
                            _co.Cerrar();

                            MostrarCliente();
                            txtPrecio.Clear();
                        }
                        else
                            MessageBox.Show(" Error en los datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    /*}
                    else
                        MessageBox.Show("Utilie ',' en vez de '.' para los decimales", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);*/

                }
            }
            else
                MessageBox.Show("Elija el Producto e Ingrese el Precio");
        }

        private void dgvProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if( dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                id = Convert.ToInt32(dgv[0, e.RowIndex].Value); 
                txtNombreProd.Text = dgv[1, e.RowIndex].Value.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)//Buscar
        {
            string _res = "";
            _co.Abrir();
            _co.BuscarProducto2(dgvProducto, _cli, txtBuscarProd.Text, cbBuscarProd.SelectedIndex, ref _res);
            _co.Cerrar();
        }

        private void button17_Click(object sender, EventArgs e)//Reiniciar
        {
            MostrarCliente();
        }
    }
}
