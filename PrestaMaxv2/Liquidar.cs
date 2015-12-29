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
    public partial class Liquidar : Form
    {
        Cliente _cli;
        Conexion _co;
        AdministradorMain _AdmMain;
        Prestamo _pres;
        public Liquidar()
        {
            InitializeComponent();
        }

        public void Inicializar(Cliente cli, Conexion co, AdministradorMain adm, Prestamo pres)
        {
            _cli = cli;
            _co = co;
            _AdmMain = adm;
            _pres = pres;
            _AdmMain.Enabled = false;
            lbl1.Text = "Liquidar Prestamo Cliente: " + _cli.Id_Cliente;
            lbl2.Text = "Nombre: " + _cli.Nombre + " " + _cli.Apellido;
        }

        private void button1_Click(object sender, EventArgs e)//Liquidar
        {
            string _liq = txtLiq.Text;
            if (_liq == "LIQUIDAR")
            {
                _co.Abrir();
                string _res = "";
                _co.LiquidarPrestamo(_pres.Id_Pres, ref _res);
                _co.Cerrar();
                MessageBox.Show(_res, "Aviso");
                this.Close();
                
            }
            else
                MessageBox.Show("Error en palabra clave", "Aviso");
        }

        private void Liquidar_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.ActualizarPrestamos();
            _AdmMain.Enabled = true;
            _AdmMain.ActualizarClientes();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
