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
    public partial class Pago : Form
    {
        AdministradorMain _AdmMain;
        Prestamo _pres;
        Cliente _cli;
        Conexion _co;
        Usuario _user;
        public Pago()
        {
            InitializeComponent();
        }

        private void Pago_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarPrestamos();
            _AdmMain.ActualizarClientes();
        }

        public void Inicializar(AdministradorMain adm, Prestamo pres, Cliente cli, Conexion co, Usuario user)
        {
            _AdmMain = adm;
            _pres = pres;
            _cli = cli;
            _co = co;
            _user = user;
            if (pres.Vencido())
                MessageBox.Show("Prestamo Vencido", "Aviso");
            this.Text = "Agregar Abono Cliente: " + _cli.Nombre+ " "+_cli.Apellido + " Cantidad: " + _pres.Cantidad + " Saldo: "+ _pres.Saldo;
            txtCantidadAcordada.Text = _pres.Pago_Semanal.ToString();
            _co.Abrir();
            string _res = "";
            _co.MostrarAbonos(dgvAbono, _pres.Id_Pres, ref _res);
            _co.Cerrar();
            Abono.Maximum = Convert.ToDecimal(_pres.Saldo);
            Abono.Minimum = 1;
            if (_user.NumTipo == 1)
                CHModificar.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)//Abonar
        {
            DialogResult _result = MessageBox.Show("Seguro que desea agregar un abono?", "Aviso", MessageBoxButtons.YesNo);
            if (_result == DialogResult.Yes)
            {
                if (Abono.Value >= 1 && Abono.Value <= Convert.ToDecimal(_pres.Saldo))
                {
                    double _cantidad;
                    try
                    {
                        _cantidad = Convert.ToDouble(Abono.Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Datos Erroneos", "Aviso");
                        return;
                    }
                    _co.Abrir();
                    string _res = "";
                    _co.Abonar(_cantidad, _pres, _cli, _user, ref _res);
                    _co.AtraparPrestamo(_pres.Id_Pres, ref _res, ref _pres, 0);
                    _co.MostrarAbonos(dgvAbono, _pres.Id_Pres, ref _res);
                    _co.Cerrar();
                    Abono.Maximum = Convert.ToDecimal(_pres.Saldo);
                    this.Text = "Agregar Abono Cliente: " + _cli.Nombre + " " + _cli.Apellido + " Cantidad: " + _pres.Cantidad + " Saldo: " + _pres.Saldo;
                }
                else
                    MessageBox.Show("Especificar cantidad a abonar", "Aviso");
            }
        }

        private void txtAbono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(Char.IsDigit(e.KeyChar) || e.KeyChar  == '.' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void dgvAbono_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                 DialogResult _resultado = MessageBox.Show("Confirmacion para realizar operacion", "Aviso", MessageBoxButtons.YesNo);
                 if (_resultado == DialogResult.Yes)
                 {
                     switch(e.ColumnIndex)
                     {
                         case 6:
                            int pago_id = Convert.ToInt32(dgv[1, e.RowIndex].Value);
                            _co.Abrir();
                            _co.EliminarAbono(pago_id);
                            string _res = "";
                            _co.MostrarAbonos(dgvAbono,_pres.Id_Pres, ref _res);
                            _co.Cerrar();
                            MessageBox.Show("Abono eliminado", "Aviso");
                             break;

                         case 7:
                             if (CHModificar.Checked)
                             {
                                 DateTime _aux;
                                 string _pagoid;
                                 try
                                 {
                                     _aux = Convert.ToDateTime(dgv[3, e.RowIndex].Value);
                                     _pagoid = dgv[1, e.RowIndex].Value.ToString();
                                 }
                                 catch (Exception ex)
                                 {
                                     MessageBox.Show("Datos erroneos", "Aviso");
                                     return;
                                 }
                                 _co.Abrir();
                                 _co.ModificarFeca(_aux.ToString(), _pagoid);
                                 _res = "";
                                 _co.MostrarAbonos(dgvAbono, _pres.Id_Pres, ref _res);
                                 _co.Cerrar();
                             }
                             break;
                    }
                 }
             }
        }

        private void CHModificar_CheckedChanged(object sender, EventArgs e)
        {
            if (CHModificar.Checked)
            {
                dgvAbono.ReadOnly = false;
            }
            else
            {
                dgvAbono.ReadOnly = true;
                _co.Abrir();
                string _res = "";
                _co.MostrarAbonos(dgvAbono, _pres.Id_Pres, ref _res);
                _co.Cerrar();
            }
        }
    }
}
