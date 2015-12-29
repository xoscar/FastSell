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
    public partial class PedidoLiquidado : Form
    {
        public PedidoLiquidado()
        {
            InitializeComponent();
        }
        Conexion _co;
        PantallaAdm _adm;
        Cliente _cli;
        Pedido _aux;
        public void Inicializar(PantallaAdm adm, Conexion co, Cliente cli)//Inicializar forma
        {
            _co = co;
            _adm = adm;
            _cli = cli;
            this.Text = "Pedidos Liquidados del Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            MostrarPedidos();
        }


        public void MostrarPedidos()//Para mostrar pedidos liquidados
        {
            string _res = "";
            _co.Abrir();
            _co.MostrarPedidosLiquidados(dgvPedido, _cli, ref _res);
            _co.Cerrar();
        }

        private void PedidoLiquidado_FormClosing(object sender, FormClosingEventArgs e)//Al cerrar
        {
            _adm.Enabled = true;
        }

        private void dgvPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)//clic en ver
        {
            DataGridView dgv = (DataGridView)sender;
            if(dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                _aux = new Pedido();
                _aux.Id_Pedido = Convert.ToInt32(dgv[0,e.RowIndex].Value.ToString());
                string _res = "";
                _co.Abrir();
                _co.AtraparPedido(_aux.Id_Pedido, ref _aux, ref _res);
                _co.Cerrar();
                txtID.Text = _aux.Id_Pedido.ToString();
                txtCliente.Text = _aux.Id_Cliente.ToString();
                txtContado.Text = _aux.Contado == true ? "NO" : "SI";
                txtFecha.Text = _aux.Fecha_Pedido.ToShortDateString();
                txtFechaEntrega.Text = _aux.Fecha_Entrega.ToShortDateString();
                txtFechaLiquidacion.Text = _aux.Fecha_Liquidacion.ToShortDateString();
                txtPrecio.Text = _aux.Precio_Total.ToString("n2");
                txtUsuario.Text = _aux.Id_Usuario;
                txtRepartidor.Text = _aux.Id_Repartidor;
            }
        }
    }
}
