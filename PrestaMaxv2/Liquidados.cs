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
    public partial class Liquidados : Form
    {
        Prestamo _pres;
        Cliente _cli;
        Conexion _co;
        AdministradorMain _AdmMain;
        public Liquidados()
        {
            InitializeComponent();
        }

        public void Inicializa(Prestamo pres, Conexion co, AdministradorMain adm, Cliente cli)
        {
            _pres = pres;
            _co = co;
            _AdmMain = adm;
            _AdmMain.Enabled = false;
            _cli = cli;
            this.Text = "Prestamos liquidados Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            string _res = "";
            _co.Abrir();
            _co.MostrarLiquidados(dgvLiq, _cli, ref  _res);
            co.Cerrar();
        }

        private void Liquidados_FormClosed(object sender, FormClosedEventArgs e)
        {
            _AdmMain.Enabled = true;
        }

        private void dgvLiq_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if(dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >=0 )
            {
                _co.Abrir();
                string _res = "";
                _co.AtraparPrestamo(Convert.ToInt32(dgv[0, e.RowIndex].Value), ref _res, ref _pres,1);
                _co.Cerrar();
                txtCantidad.Text = _pres.Cantidad.ToString();
                txtDescripcion.Text = _pres.Descripcion;
                txtPase.Text = _pres.Pago_Semanal.ToString();
                txtSemanas.Text = _pres.Semanas.ToString();
                try
                {
                    pbPagare.Load(_pres.Pagare);
                }
                catch (Exception ex) { }
            }
        }
    }
}
