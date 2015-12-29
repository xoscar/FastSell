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
    public partial class EliminarUsuario : Form
    {
        Conexion _co;
        AdministradorMain _AdmMain;
        Usuario _user;
        public EliminarUsuario()
        {
            InitializeComponent();
        }

        public void Inicializa(Conexion co, AdministradorMain adm, Usuario user)
        {
            _co = co;
            _AdmMain = adm;
            _user = user;
            this.Text = "Eliminar usuario nombre = " + _user.Nombre + " Login: " + _user.Login;
            _AdmMain.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _liq = txtLiq.Text;
            if (_liq == "ELIMINAR")
            {
                _co.Abrir();
                string _res = "";
                _co.EliminarUsuario(_user.Login, ref _res);
                _co.Cerrar();
                MessageBox.Show(_res, "Aviso");
                this.Close();
            }
            else
                MessageBox.Show("Error en palabra clave", "Aviso");
        }

        private void EliminarUsuario_FormClosing(object sender, FormClosingEventArgs e)
        {
            _AdmMain.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
