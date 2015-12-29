namespace PrestaMaxv2
{
    partial class DetallesCargo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetallesCargo));
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSemanas = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CHModificar = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtFI = new System.Windows.Forms.MaskedTextBox();
            this.txtFV = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtDesc.Location = new System.Drawing.Point(12, 176);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(419, 119);
            this.txtDesc.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 25);
            this.label9.TabIndex = 37;
            this.label9.Text = "Descipcion:";
            // 
            // txtSemanas
            // 
            this.txtSemanas.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSemanas.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSemanas.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtSemanas.Location = new System.Drawing.Point(224, 39);
            this.txtSemanas.Name = "txtSemanas";
            this.txtSemanas.ReadOnly = true;
            this.txtSemanas.Size = new System.Drawing.Size(196, 30);
            this.txtSemanas.TabIndex = 1;
            this.txtSemanas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCantidad_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(219, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 25);
            this.label7.TabIndex = 36;
            this.label7.Text = "Semanas";
            // 
            // txtCantidad
            // 
            this.txtCantidad.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCantidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCantidad.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtCantidad.Location = new System.Drawing.Point(12, 39);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.ReadOnly = true;
            this.txtCantidad.Size = new System.Drawing.Size(196, 30);
            this.txtCantidad.TabIndex = 0;
            this.txtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCantidad_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 25);
            this.label3.TabIndex = 32;
            this.label3.Text = "Cantidad:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Controls.Add(this.CHModificar);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(12, 301);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 74);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            // 
            // CHModificar
            // 
            this.CHModificar.AutoSize = true;
            this.CHModificar.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CHModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHModificar.ForeColor = System.Drawing.SystemColors.Desktop;
            this.CHModificar.Location = new System.Drawing.Point(203, 30);
            this.CHModificar.Name = "CHModificar";
            this.CHModificar.Size = new System.Drawing.Size(205, 29);
            this.CHModificar.TabIndex = 6;
            this.CHModificar.Text = "Activar Modificacion";
            this.CHModificar.UseVisualStyleBackColor = false;
            this.CHModificar.CheckedChanged += new System.EventHandler(this.CHModificar_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.button1.Location = new System.Drawing.Point(14, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 49);
            this.button1.TabIndex = 5;
            this.button1.Text = "Modificar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFI
            // 
            this.txtFI.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtFI.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFI.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtFI.Location = new System.Drawing.Point(12, 115);
            this.txtFI.Mask = "00/00/0000 00:00";
            this.txtFI.Name = "txtFI";
            this.txtFI.ReadOnly = true;
            this.txtFI.Size = new System.Drawing.Size(196, 30);
            this.txtFI.TabIndex = 2;
            this.txtFI.ValidatingType = typeof(System.DateTime);
            // 
            // txtFV
            // 
            this.txtFV.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtFV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFV.ForeColor = System.Drawing.SystemColors.Desktop;
            this.txtFV.Location = new System.Drawing.Point(224, 115);
            this.txtFV.Mask = "00/00/0000 00:00";
            this.txtFV.Name = "txtFV";
            this.txtFV.ReadOnly = true;
            this.txtFV.Size = new System.Drawing.Size(196, 30);
            this.txtFV.TabIndex = 3;
            this.txtFV.ValidatingType = typeof(System.DateTime);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(219, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 25);
            this.label5.TabIndex = 35;
            this.label5.Text = "Fecha Vencimineto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 25);
            this.label2.TabIndex = 34;
            this.label2.Text = "Fecha Inicio";
            // 
            // DetallesCargo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(444, 392);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSemanas);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtFI);
            this.Controls.Add(this.txtFV);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(460, 430);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 430);
            this.Name = "DetallesCargo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalles Cargo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DetallesCargo_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSemanas;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox CHModificar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MaskedTextBox txtFI;
        private System.Windows.Forms.MaskedTextBox txtFV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
    }
}