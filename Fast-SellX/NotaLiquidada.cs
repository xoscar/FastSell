using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fast_SellX
{
    public partial class NotaLiquidada : Form
    {
        public NotaLiquidada()
        {
            InitializeComponent();
        }
        Conexion _co;
        PantallaAdm _adm;
        Cliente _cli;
        Nota _aux;

        public void Inicializar(PantallaAdm adm, Conexion co, Cliente cli)
        {
            _co = co;
            _adm = adm;
            _cli = cli;
            this.Text = "Notas Liquidadas Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            MostrarNotas();
        }

        public void MostrarNotas()
        {
            string _res = "";
            _co.Abrir();
            _co.MostrarNotasLiquidadas(dgvPedido, _cli, ref _res);
            _co.Cerrar();
        }

        private void dgvPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)//Al dar clic Ver
        {
             DataGridView dgv = (DataGridView)sender;
             if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             {
                 _aux = new Nota();
                 _aux.Id_Nota = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                 string _res = "";

                 _co.Abrir();
                 _co.AtraparNota(_aux.Id_Nota, ref _aux, ref _res);
                 _co.Cerrar();

                 txtCantidad.Text = _aux.Cantidad.ToString("n2");
                 txtCliente.Text = _aux.Id_Cliente;
                 txtDesc.Text = _aux.Descripcion;
                 txtFechaInicio.Text = _aux.Fecha_Inicio.ToShortDateString();
                 txtFechaLiquidacion.Text = _aux.Fecha_Liquidacion.ToShortDateString();
                 txtFechaVencimiento.Text = _aux.Fecha_Vencimiento.ToShortDateString();
                 txtID.Text = _aux.Id_Nota.ToString();
                 txtPedido.Text = _aux.Id_Pedido.ToString();
                 txtUser.Text = _aux.Id_User;
                 try
                 {
                     pbNota.Load(_aux.Pagare);
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }
        }

        private void NotaLiquidada_FormClosing(object sender, FormClosingEventArgs e)//Al cerrar
        {
            _adm.Enabled = true;
        }
    }
}
