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
    public partial class AgregarPrestamo : Form
    {
        Cliente _cli;
        Conexion _co;
        Usuario _user;
        AdministradorMain _AdmMain;
        Prestamo _pres;
        string _ruta = "";
        public AgregarPrestamo()
        {
            InitializeComponent();
        }

        public void Inicializar(Cliente cli, Conexion co, AdministradorMain adm, Usuario us)
        {
            _cli = cli;
            _co = co;
            _AdmMain = adm;
            _user = us;
            this.Text = "Agregar Prestamo Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            _AdmMain.Enabled = false;
        }

        private void AgregarPrestamo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarPrestamos();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnPagare_Click(object sender, EventArgs e)//Agregar Pagare
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".jpg";
            fd.Title = "Agregar Pagare";
            fd.FilterIndex = 2;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                _ruta = fd.FileName;
                pbPagare.Load(_ruta);
                txtPagare.Text = _ruta;
            }
        }

        private void button1_Click(object sender, EventArgs e)//Agregar
        {
             DialogResult _resultado = MessageBox.Show("Confirmacion agregar cargo", "Aviso", MessageBoxButtons.YesNo);
             if (_resultado == DialogResult.Yes)
             {
                 if (txtCantidad.Text != "" && txtSemanas.Text != "" && txtPase.Text != "")
                 {
                     try
                     {
                         _pres = new Prestamo();
                         _pres.Cantidad = Convert.ToDouble(txtCantidad.Text);
                         _pres.Pago_Semanal = Convert.ToInt32(txtPase.Text);
                         _pres.Semanas = Convert.ToInt32(txtSemanas.Text);
                         _pres.Pagare = _ruta;
                         _pres.Descripcion = txtDescripcion.Text;
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show("Datos erroneos", "Aviso");
                         return;
                     }
                     _co.Abrir();
                     string _res = "";
                     _co.AgregarPrestamo(_pres, _cli, _user, ref _res);
                     _co.Cerrar();
                     MessageBox.Show(_res, "Aviso");
                 }
                 else
                 {
                     MessageBox.Show("Especificar al menos cantidad, semanas y pago semanal", "Aviso");
                 }
             }
        }
    }
}
