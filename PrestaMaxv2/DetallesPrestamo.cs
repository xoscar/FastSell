using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrestaMaxv2
{
    public partial class DetallesPrestamo : Form
    {
        Conexion _co;
        Cliente _cli;
        Prestamo _pres;
        AdministradorMain _AdmMain;
        bool _modify = false;
        string ruta = "";
        Usuario _usr;
        public DetallesPrestamo()
        {
            InitializeComponent();
        }

        public void Inicalizar(Conexion co, Cliente cli, AdministradorMain adm, Prestamo pres, Usuario user)
        {
            _co = co;
            _cli = cli;
            _pres = pres;
            _AdmMain = adm;
            int _semanaActual = 0, _semanaRestante = 0;
            string _res = "";
            _co.Abrir();
            _co.ManejoSemanas(_pres.Id_Pres, ref _semanaActual, ref  _semanaRestante, ref _res);
            _co.Cerrar();
            MostrarPrestamo(_semanaActual, _semanaRestante);
            ruta = _pres.Pagare;
            this.Text = "Detalles Prestamo Cliente: " + _cli.Id_Cliente + " Cantidad: " + _pres.Cantidad.ToString();
            _usr = user;
            if (_usr.NumTipo == 1)
                CHModificar.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)//Modificar
        {
            DialogResult _resultado = MessageBox.Show("Confirmacion modificacion prestamo", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                if (txtCantidad.Text != "" && txtFI.MaskFull && txtFV.MaskFull && txtPase.Text != "")
                {
                    try
                    {
                        _pres.Cantidad = Convert.ToDouble(txtCantidad.Text);
                        _pres.Descripcion = txtDesc.Text;
                        DateTime aux = Convert.ToDateTime(txtFI.Text);
                        if (aux.CompareTo(DateTime.Now) > 0)
                            MessageBox.Show("Fecha de inicio aun no alcanzada", "Aviso");
                        _pres.Fecha_Inicio = aux;
                        _pres.Fecha_Vencimiento = Convert.ToDateTime(txtFV.Text);
                        _pres.Pago_Semanal = Convert.ToDouble(txtPase.Text);
                        _pres.Pagare = ruta;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error en los datos", "Aviso");
                        return;
                    }
                    string _res = "";
                    _co.Abrir();
                    _co.ModificarPrestamo(ref _pres, ref _res);
                    _co.Cerrar();
                    button1.Enabled = false;
                    txtCantidad.ReadOnly = true;
                    txtDesc.ReadOnly = true;
                    txtFI.ReadOnly = true;
                    txtFV.ReadOnly = true;
                    txtPase.ReadOnly = true;
                    int _semanaActual = 0, _semanaRestante = 0;
                    _res = "";
                    _co.Abrir();
                    _co.AtraparPrestamo(_pres.Id_Pres, ref _res, ref _pres, 0);
                    _co.ManejoSemanas(_pres.Id_Pres, ref _semanaActual, ref  _semanaRestante, ref _res);
                    _co.Cerrar();
                    MostrarPrestamo(_semanaActual, _semanaRestante);
                    _modify = false;
                    CHModificar.Checked = false;
                }
                else
                    MessageBox.Show("Falta inidicar valores", "Aviso");
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void DetallesPrestamo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarPrestamos();
            _AdmMain.ActualizarClientes();
        }

        public void MostrarPrestamo(int semanaActual, int semanaRestante )
        {
            txtCantidad.Text = _pres.Cantidad.ToString();
            txtDesc.Text = _pres.Descripcion;
            txtFI.Text = _pres.Fecha_Inicio.ToString();
            txtFV.Text = _pres.Fecha_Vencimiento.ToString();
            txtPase.Text = _pres.Pago_Semanal.ToString();
            txtSaldo.Text = _pres.Saldo.ToString();
            txtSema.Text = semanaActual.ToString();
            txtSemRe.Text = semanaRestante.ToString();
            txtUser.Text = _pres.Id_User;
            try
            {
                pbPagare.Load(_pres.Pagare);
            }
            catch (Exception ex) 
            {
                Image img = pbPagare.ErrorImage;
                pbPagare.Image = img;
            }
        }

        private void CHModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (CHModificar.Checked)
            {
                button1.Enabled = true;
                txtCantidad.ReadOnly = false;
                txtDesc.ReadOnly = false;
                txtFI.ReadOnly = false;
                txtFV.ReadOnly = false;
                txtPase.ReadOnly = false;
                _modify = true;
            }
            else
            {
                button1.Enabled = false;
                txtCantidad.ReadOnly = true;
                txtDesc.ReadOnly = true;
                txtFI.ReadOnly = true;
                txtFV.ReadOnly = true;
                txtPase.ReadOnly = true;
                int _semanaActual = 0, _semanaRestante = 0;
                string _res = "";
                _co.Abrir();
                _co.AtraparPrestamo(_pres.Id_Pres, ref _res, ref _pres,0);
                _co.ManejoSemanas(_pres.Id_Pres, ref _semanaActual, ref  _semanaRestante, ref _res);
                _co.Cerrar();
                MostrarPrestamo(_semanaActual, _semanaRestante);
                _modify = false;
            }
        }

        private void pbPagare_Click(object sender, EventArgs e)
        {
            if (_modify)
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.DefaultExt = ".jpg";
                fd.Title = "Modificar Pagare";
                fd.FilterIndex = 2;
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    ruta = fd.FileName;
                    pbPagare.Load(ruta);
                }
            }
        }
    }
}
