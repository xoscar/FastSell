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
    public partial class Eliminar : Form
    {
        public Eliminar()
        {
            InitializeComponent();
        }
        Conexion _co;
        Pedido _pe;
        PantallaAdm _adm;
        string _clave;
        int _tipo;
        Nota _no;
        Usuario _user;
        public void InicializarPedido(Conexion co, Pedido pe, PantallaAdm adm, string text, string clave)//Inicializar Para pedidos
        {
            _co = co;
            _pe = pe;
            _adm = adm;
            _tipo = 0;
            _clave = clave;
            label2.Text = text;
        }

        public void InicializarNota(Conexion co, Nota no, PantallaAdm adm, string text, string clave)//Inicalizar para Notas
        {
            _co = co;
            _no = no;
            _adm = adm;
            _tipo = 0;
            _clave = clave;
            label2.Text = text;
            _tipo = 1;
        }

        public void InicializarBloqueo(Conexion co, Usuario us, PantallaAdm adm, string text, string clave)//Inicializar para bloquear usuario
        {
            _co = co;
            _user = us;
            _adm = adm;
            label2.Text = text;
            _tipo = 2;
            _clave = clave;
            txtClave.PasswordChar = '*';
            txtClave.UseSystemPasswordChar = true;
        }

        public void InicializarDesbloquear(Conexion co, Usuario us, PantallaAdm adm, string text, string clave)//Inicializar para desbloquear usuario
        {
            _co = co;
            _user = us;
            _adm = adm;
            label2.Text = text;
            _tipo = 3;
            _clave = clave;
            txtClave.PasswordChar = '*';
            txtClave.UseSystemPasswordChar = true;
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)//Cerrar
        {
            if (e.KeyCode == Keys.Escape)
            {
                _adm.Enabled = true;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)//Cancelar
        {
            _adm.Enabled = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)//Aceptar
        {
            if( txtClave.Text == _clave )
            {
                switch (_tipo)
                {
                        //caso liquidar pedido
                    case 0:
                        string _res = "";
                        _co.Abrir();
                        _co.LiquidarPedido(_pe, ref _res);
                        _co.Cerrar();
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _adm.Enabled = true;
                        _adm.Refresh();
                        this.Close();
                        break;

                        //caso liquidar nota
                    case 1:
                        _res = "";
                        _co.Abrir();
                        _co.LiquidarNota(_no, ref _res);
                        _co.Cerrar();
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _adm.Enabled = true;
                        _adm.Refresh();
                        this.Close();
                        break;
                        
                        //caso bloquear usuario
                    case 2:
                        _res = "";
                        _co.Abrir();
                        _co.Bloquearusuario(_user, ref _res);
                        _co.Cerrar();
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _adm.Enabled = true;
                        this.Close();
                        break;

                        //caso desbloquear usuario
                    case 3:
                        _res = "";
                        _co.Abrir();
                        _co.DesbloquearUsuario(_user, ref _res);
                        _co.Cerrar();
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _adm.Enabled = true;
                        this.Close();
                        break;
                }
            }
            else
                MessageBox.Show("Error en la clave", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
