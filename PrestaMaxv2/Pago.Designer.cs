namespace PrestaMaxv2
{
    partial class Pago
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pago));
            this.dgvAbono = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button6 = new System.Windows.Forms.Button();
            this.txtCantidadAcordada = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CHModificar = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Abono = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbono)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abono)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAbono
            // 
            this.dgvAbono.AllowUserToAddRows = false;
            this.dgvAbono.AllowUserToDeleteRows = false;
            this.dgvAbono.AllowUserToResizeColumns = false;
            this.dgvAbono.AllowUserToResizeRows = false;
            this.dgvAbono.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvAbono.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAbono.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column7,
            this.Column8});
            this.dgvAbono.Location = new System.Drawing.Point(372, 12);
            this.dgvAbono.Name = "dgvAbono";
            this.dgvAbono.ReadOnly = true;
            this.dgvAbono.RowHeadersVisible = false;
            this.dgvAbono.Size = new System.Drawing.Size(545, 197);
            this.dgvAbono.TabIndex = 0;
            this.dgvAbono.TabStop = false;
            this.dgvAbono.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAbono_CellContentClick);
            // 
            // Column6
            // 
            this.Column6.HeaderText = "No.";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 30;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Cantidad";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Fecha";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "ID Prestamo";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 70;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "ID Usuario";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 70;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Quitar Abono";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column7.Width = 70;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Modificar Fecha";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column8.Width = 70;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(228, 115);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 60);
            this.button6.TabIndex = 1;
            this.button6.Text = "Agregar abono";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtCantidadAcordada
            // 
            this.txtCantidadAcordada.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCantidadAcordada.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidadAcordada.Location = new System.Drawing.Point(11, 115);
            this.txtCantidadAcordada.Name = "txtCantidadAcordada";
            this.txtCantidadAcordada.ReadOnly = true;
            this.txtCantidadAcordada.Size = new System.Drawing.Size(195, 30);
            this.txtCantidadAcordada.TabIndex = 29;
            this.txtCantidadAcordada.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(263, 25);
            this.label6.TabIndex = 30;
            this.label6.Text = "Cantidad semanal acordada:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox1.Controls.Add(this.Abono);
            this.groupBox1.Controls.Add(this.CHModificar);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.txtCantidadAcordada);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 197);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opciones";
            // 
            // CHModificar
            // 
            this.CHModificar.AutoSize = true;
            this.CHModificar.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CHModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHModificar.Location = new System.Drawing.Point(17, 167);
            this.CHModificar.Name = "CHModificar";
            this.CHModificar.Size = new System.Drawing.Size(168, 24);
            this.CHModificar.TabIndex = 34;
            this.CHModificar.Text = "Activar Modificacion";
            this.CHModificar.UseVisualStyleBackColor = false;
            this.CHModificar.CheckedChanged += new System.EventHandler(this.CHModificar_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 25);
            this.label1.TabIndex = 33;
            this.label1.Text = "Cantidad a abonar:";
            // 
            // Abono
            // 
            this.Abono.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Abono.Location = new System.Drawing.Point(11, 54);
            this.Abono.Name = "Abono";
            this.Abono.Size = new System.Drawing.Size(174, 30);
            this.Abono.TabIndex = 35;
            // 
            // Pago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(929, 222);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvAbono);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(945, 260);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(945, 260);
            this.Name = "Pago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar Abono:";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Pago_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbono)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Abono)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAbono;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtCantidadAcordada;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CHModificar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn Column7;
        private System.Windows.Forms.DataGridViewButtonColumn Column8;
        private System.Windows.Forms.NumericUpDown Abono;
    }
}