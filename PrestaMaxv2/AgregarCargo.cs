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
    public partial class AgregarCargo : Form
    {
        Cliente _cli;
        Usuario _user;
        Conexion _co;
        AdministradorMain _AdmMain;
        Cargo _ca;
        public AgregarCargo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//Agregar Cargo
        {
            DialogResult _resultado = MessageBox.Show("Confirmacion agregar cargo", "Aviso", MessageBoxButtons.YesNo);
            if (_resultado == DialogResult.Yes)
            {
                if (txtCantidad.Text != "" && txtSemanas.Text != "")
                {
                    try
                    {
                        _ca = new Cargo();
                        _ca.Cantidad = Convert.ToDouble(txtCantidad.Text);
                        _ca.Descripcion = txtDescripcion.Text;
                        _ca.Semanas = Convert.ToInt32(txtSemanas.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Datos Erroneos", "Aviso");
                        return;
                    }
                    _co.Abrir();
                    string _res = "";
                    _co.AgregarCargo(ref _ca, _user, _cli, ref _res);
                    _co.Cerrar();
                    MessageBox.Show(_res, "Aviso");
                    txtCantidad.Clear();
                    txtDescripcion.Clear();
                    txtSemanas.Clear();
                }
                else
                    MessageBox.Show("Especificar almenos cantidad y semanas", "Aviso");
            }
        }

        public void Inicializar(Cliente cli, Usuario us, Conexion co, AdministradorMain adm)
        {
            _cli = cli;
            _user = us;
            _co = co;
            _AdmMain = adm;
            _AdmMain.Enabled = false;
            this.Text = "Agregar cargo Cliente: " + _cli.Nombre + " " + _cli.Apellido;
        }

        private void AgregarCargo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarCargos();
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
