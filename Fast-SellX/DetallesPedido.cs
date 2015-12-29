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
    public partial class DetallesPedido : Form
    {
        public DetallesPedido()
        {
            InitializeComponent();
        }

        PantallaAdm _adm;
        Conexion _co;
        Pedido _ped;
        Cliente _cli;
        public void Inicializar(PantallaAdm adm, Conexion co, Pedido pe, Cliente cli)//Para iniciar al forma
        {
            _adm = adm;
            _co = co;
            _ped = pe;
            _cli = cli;
            MostrarPedido();
        }

        private void DetallesPedido_FormClosing(object sender, FormClosingEventArgs e)//Al cerrar
        {
            _adm.Enabled = true;
        }

        public void MostrarPedido()
        {
            this.Text = "Detalles Pedido Cliente: " + _cli.Nombre + " " + _cli.Apellido + " Pedido con ID: " + _ped.Id_Pedido;
            txtID.Text = _ped.Id_Pedido.ToString();
            txtCliente.Text = _cli.Nombre+ " "+_cli.Apellido;
            txtContado.Text = _ped.Contado == false? "SI":"NO";
            txtEntrega.Text = _ped.Fecha_Entrega.ToShortDateString();
            txtPedido.Text = _ped.Fecha_Pedido.ToShortDateString();
            txtPrecio.Text = _ped.Precio_Total.ToString("n2");
            txtRepartidor.Text = _ped.Id_Repartidor;
            txtUser.Text = _ped.Id_Usuario;
            dgvPedido.Rows.Clear();
            for (int i = 0; i < _ped.Productos.Count; i++)
            {
                dgvPedido.Rows.Add();
                Producto _aux = ((Producto)_ped.Productos[i]);
                dgvPedido[0, i].Value = _aux.Id_Producto;
                dgvPedido[1, i].Value = _aux.Nombre;
                dgvPedido[2, i].Value = _aux.Precio_General;
                dgvPedido[3, i].Value = _aux.Cantidad;
                dgvPedido[4, i].Value = _aux.Acumulacion[_aux.Tipo];
            }
        }
    }
}
