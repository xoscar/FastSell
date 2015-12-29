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
    public partial class DetallesCargo : Form
    {
        Cargo _ca;
        Conexion _co;
        AdministradorMain _AdmMain;
        Usuario _usr;
        public DetallesCargo()
        {
            InitializeComponent();
        }

        public void Inicializa(Cargo ca, Conexion co, AdministradorMain adm, Usuario user)
        {
            _ca = ca;
            _co = co;
            _AdmMain = adm;
            if (_ca.Fecha_Vencimiento.CompareTo(DateTime.Now) < 0)
                MessageBox.Show("Cargo Vencido", "Aviso");
            this.Text = "Detalles cargo Cliente id: " + ca.Id_Cliente;
            _AdmMain.Enabled = false;
            MostrarCargo();
            _usr = user;
            if (_usr.NumTipo == 1)
                CHModificar.Enabled = false;
        }

        private void DetallesCargo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarCargos();
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
                txtSemanas.ReadOnly = false;
            }
            else
            {
                button1.Enabled = false;
                txtCantidad.ReadOnly = true;
                txtDesc.ReadOnly = true;
                txtFI.ReadOnly = true;
                txtFV.ReadOnly = true;
                txtSemanas.ReadOnly = true;
                _co.Abrir();
                string _res = "";
                _co.AtraparCargo(_ca.Id_Cargo, ref _ca, ref _res);
                _co.Cerrar();
                MostrarCargo();
            }

        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        public void MostrarCargo()
        {
            txtCantidad.Text = _ca.Cantidad.ToString();
            txtDesc.Text = _ca.Descripcion;
            txtFI.Text = _ca.Fecha_Inicio.ToString();
            txtFV.Text = _ca.Fecha_Vencimiento.ToString();
            txtSemanas.Text = _ca.Semanas.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CHModificar.Checked)
            {
                DialogResult _resultado = MessageBox.Show("Seguro modificar el cargo", "Aviso", MessageBoxButtons.YesNo);
                if (_resultado == DialogResult.Yes)
                {
                    if (txtCantidad.Text != "" && txtDesc.Text != "" && txtFI.MaskFull && txtFV.MaskFull && txtSemanas.Text != "")
                    {
                        try
                        {
                            _ca.Cantidad = Convert.ToDouble(txtCantidad.Text);
                            _ca.Descripcion = txtDesc.Text;
                            _ca.Fecha_Inicio = Convert.ToDateTime(txtFI.Text);
                            _ca.Fecha_Vencimiento = Convert.ToDateTime(txtFV.Text);
                            _ca.Semanas = Convert.ToInt32(txtSemanas.Text);
                           
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Datos erroneos", "Aviso");
                            return;
                        }
                        _co.Abrir();
                        string _res = "";
                        _co.ModificarCargo(_ca, ref _res);
                        MessageBox.Show(_res, "Aviso");
                        _co.Cerrar();
                        button1.Enabled = false;
                        txtCantidad.ReadOnly = true;
                        txtDesc.ReadOnly = true;
                        txtFI.ReadOnly = true;
                        txtFV.ReadOnly = true;
                        txtSemanas.ReadOnly = true;
                        _co.Abrir();
                        _co.AtraparCargo(_ca.Id_Cargo, ref _ca, ref _res);
                        MostrarCargo();
                        CHModificar.Checked = false;
                    }
                    else
                        MessageBox.Show("Falta especificar valores", "Aviso");
                }
            }
        }
    }
}
