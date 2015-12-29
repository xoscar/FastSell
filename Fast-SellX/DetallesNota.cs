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
    public partial class DetallesNota : Form
    {
        public DetallesNota()
        {
            InitializeComponent();
        }
        Conexion _co;
        PantallaAdm _adm;
        Cliente _cli;
        Nota _nota;
        string _pagare = "";
        public void Inicializar(PantallaAdm adm, Conexion co, Cliente cli, Nota no)
        {
            _co = co;
            _adm = adm;
            _cli = cli;
            this.Text = "Detalles Nota del Cliente: " + _cli.Nombre + " " + _cli.Apellido;
            _nota = no;
            MostrarNota();
            MostrarAbonos();
        }

        public void MostrarAbonos()
        {
            string _res = "";
            _co.Abrir();
            _co.MostrarAbonos(dgvPedido,_nota, ref _res);
            _co.Cerrar();
        }

        public void MostrarNota()
        {
            txtCantidad.Text = _nota.Cantidad.ToString();
            txtCliente.Text = _nota.Id_Cliente;
            txtDesc.Text = _nota.Descripcion;
            txtFechaInicio.Text = _nota.Fecha_Inicio.ToShortDateString();
            txtFechaVencimiento.Text = _nota.Fecha_Vencimiento.ToShortDateString();
            txtID.Text = _nota.Id_Nota.ToString();
            txtPedido.Text = _nota.Id_Pedido.ToString();
            txtUser.Text = _nota.Id_Pedido.ToString();
            try
            {
                pbNota.Load(_nota.Pagare);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " No se pudo cargar la imagen", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DetallesNota_FormClosing(object sender, FormClosingEventArgs e)
        {
            _adm.Enabled = true;
            _adm.Refresh();
        }

        private void chModificar_CheckedChanged(object sender, EventArgs e)
        {
            if(chModificar.Checked)
            {
                txtFechaVencimiento.ReadOnly = false;
                txtDesc.ReadOnly = false;
                btnModify.Enabled = true;
            }
            else
            {
                txtFechaVencimiento.ReadOnly = true;
                txtDesc.ReadOnly = true;
                btnModify.Enabled = false;
                MostrarNota();
                MostrarAbonos();
            }
        }

        private void pbNota_Click(object sender, EventArgs e)
        {
            if(chModificar.Checked)
            {
                OpenFileDialog _ofd = new OpenFileDialog();
                _ofd.DefaultExt = ".jpg";
                _ofd.RestoreDirectory = true;
                if(_ofd.ShowDialog() == DialogResult.OK)
                {
                    pbNota.Load(_ofd.FileName);
                    _pagare = _ofd.FileName;
                }
            }
        }

        private void txtDesc_ReadOnlyChanged(object sender, EventArgs e) //ReadOnlys
        {
            TextBox txt = (TextBox)sender;
            if(!txt.ReadOnly)
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
            }
            else
            {
                txt.BackColor = Color.Blue;
                txt.ForeColor = Color.White;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)//Moficiar
        {
            if(MessageBox.Show("¿Seguro que desea Moficiar la Nota?","Aviso",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (txtFechaVencimiento.Text != "")
                {
                    if(_pagare == "")
                    {
                        string _res = "";
                        _co.Abrir();
                        _co.ModificarNota(txtDesc.Text, txtFechaVencimiento.Text, _nota.Pagare, ref _nota, ref _res);
                        _co.Cerrar();
                        MostrarNota();
                        chModificar.Checked = false;
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string _res = "";
                        _co.Abrir();
                        _co.ModificarNota(txtDesc.Text, txtFechaVencimiento.Text,_pagare, ref _nota, ref _res);
                        _co.Cerrar();
                        MostrarNota();
                        chModificar.Checked = false;
                        MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show("Agregar una fecha de vencimiento correcta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (MessageBox.Show("¿Desea quitar este abono permanentemente?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Abono _abo = new Abono();
                    _abo.Id_Abono = Convert.ToInt32(dgv[0, e.RowIndex].Value);
                    string _res = "";
                    _co.Abrir();
                    _co.QuitarAbono(_abo, ref _res);
                    _co.Cerrar();
                    MostrarAbonos();
                    MessageBox.Show(_res, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        } 
    }
}
