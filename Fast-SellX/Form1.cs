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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        bool _cerrar = false;
        Conexion _co = new Conexion();
        Usuario _user;
        PantallaAdm _adm;
        private void button1_Click(object sender, EventArgs e)//Ingresar al sistema
        {
            ValidarUsuario();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void Inicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_cerrar)
            {
                DialogResult _resultado = MessageBox.Show("¿Seguro que desea salir?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (_resultado == DialogResult.Yes)
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
            _cerrar = true;
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ValidarUsuario();
            else
                if(e.KeyCode == Keys.Escape)
                    this.Close();
        }

        public void ValidarUsuario()
        {
            if (txtContraseña.Text != "" && txtUsuario.Text != "")
            {
                _co.Abrir();
                string _res = "";
                bool _encontrado = false;
                _encontrado = _co.VerificarUsuario(txtUsuario.Text, txtContraseña.Text, ref _res, ref _user);
                _co.Cerrar();
                if (_encontrado)
                {
                    switch (_user.CharTipo[_user.NumTipo])
                    {
                        case 'A':
                            this.Hide();
                            _adm = new PantallaAdm();
                            MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _adm.Inicializa(this, _user, _co);
                            _adm.Show();
                            txtContraseña.Clear();
                            txtUsuario.Clear();
                            break;

                        case 'U':
                            MessageBox.Show(_res, "Aviso");
                            this.Hide();
                            _adm = new PantallaAdm();
                            MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _adm.Inicializa(this, _user, _co);
                            _adm.Show();
                            txtContraseña.Clear();
                            txtUsuario.Clear();
                            break;
                    }
                }
                else
                    MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Ingrese Usuario y Contraseña", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)//
        {
            MessageBox.Show("Programa creado por Oscar Reyes, e-mail: oscar-rreyes1@hotmail.com", "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
