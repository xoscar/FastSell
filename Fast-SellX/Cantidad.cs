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
    public partial class Cantidad : Form
    {
        public Cantidad()
        {
            InitializeComponent();
        }
        PantallaAgregarPedido _addPed;
        PantallaAdm _adm;
        int _max;
        int _row;
        int _tipo;
        public void inicializar(int max, PantallaAgregarPedido addPed, int rowIndex)
        {
            nudCantidad.Maximum = (_max = max);
            _addPed = addPed;
            _row = rowIndex;
            _tipo = 0;
        }
        private void button2_Click(object sender, EventArgs e)//Cerrar
        {
            switch(_tipo)
            {
                case 0:
                    _addPed.Enabled = true;
                    break;
                case 1:
                    _adm.Enabled = true;
                    break;
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)//Aceptar
        {
            if (nudCantidad.Value <= _max && nudCantidad.Value > 0)
            {
                switch(_tipo)
                { 
                        //llamado desde add Pedido
                    case 0:
                        _addPed.EnviarDatos(Convert.ToInt32(nudCantidad.Value), _row);
                        MessageBox.Show("Cantidad Agregada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _addPed.Enabled = true;
                        this.Close();
                        break;
                        //llamado desde main
                    case 1:
                        _adm.EnviarDatos(Convert.ToInt32(nudCantidad.Value), _row);
                        _adm.Enabled = true;
                        this.Close();
                        break;
                    case 2:
                        _adm.EnviarDatosProducto(Convert.ToInt32(nudCantidad.Value), _row);
                        _adm.Enabled = true;
                        this.Close();
                        break;
                }
            }
            else
                MessageBox.Show("El valor debe estar entre 1 y " + _max, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Cantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                switch(_tipo)
                {
                    case 0:
                        _addPed.Enabled = true;
                        break;

                    case 1:
                        _adm.Enabled = true;
                        break;
                }
                this.Close();
            }
        }

        public void InicializarMain(PantallaAdm adm, int max, int row, int tipo)//Inicializar desde el main
        {
            _adm = adm;
            nudCantidad.Maximum = (_max = max);
            _tipo = tipo;
            _row = row;
        }
    }
}
