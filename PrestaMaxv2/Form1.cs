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
    public partial class Inicio : Form
    {
        Conexion _co = new Conexion();
        Usuario _user;
        AdministradorMain _AdmMain;
        bool _cerrarnorm = false;
        public Inicio()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)//Iniciar Sesiono
        {
            _co.Abrir();
            string res = "";
            bool _encontrado = _co.VerificarUsuario(txtLogin.Text, txtPass.Text, ref res, ref _user);
            MessageBox.Show(res,"Bienvenido");
            if (_encontrado)
            {
                switch (_user.NumTipo)
                {
                    case 0:
                        txtLogin.Clear();
                        txtPass.Clear();
                        _co.Cerrar();
                        _AdmMain = new AdministradorMain();
                        _AdmMain.Inicializa(_co, _user, this);
                        _AdmMain.Show();
                        this.Hide();
                        break;

                    case 1:
                        txtLogin.Clear();
                        txtPass.Clear();
                        _co.Cerrar();
                        _AdmMain = new AdministradorMain();
                        _AdmMain.Inicializa(_co, _user, this);
                        _AdmMain.Show();
                        this.Hide();
                        break;
                }
               
            }
            _co.Cerrar();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void Inicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!_cerrarnorm)
            {
                DialogResult _dialogresult = MessageBox.Show("Seguro que deseas salir: ", "Verificacion de salida", MessageBoxButtons.YesNo);
                if (_dialogresult == DialogResult.Yes)
                {
                    if (_co.Conectada)
                        _co.Cerrar();
                }
                else
                    e.Cancel = true;
            }
        }

        public void Cerrar()
        {
            _cerrarnorm = true;
        }
    }
}
