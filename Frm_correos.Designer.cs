namespace Parcial_ProgAVZ1
{
    partial class Frm_correos
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDestinatario = new System.Windows.Forms.TextBox();
            this.txtAsunto = new System.Windows.Forms.TextBox();
            this.txtCuerpoMensaje = new System.Windows.Forms.TextBox();
            this.lblValidacionEmail = new System.Windows.Forms.Label();
            this.lblInfoAdjuntos = new System.Windows.Forms.Label();
            this.lblEstudianteInfo = new System.Windows.Forms.Label();
            this.listAdjuntos = new System.Windows.Forms.ListView();
            this.btnAdjuntar = new System.Windows.Forms.Button();
            this.btnEliminarAdjunto = new System.Windows.Forms.Button();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1132, 100);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(401, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(311, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "Envio de correos";
            // 
            // txtDestinatario
            // 
            this.txtDestinatario.Location = new System.Drawing.Point(344, 123);
            this.txtDestinatario.Name = "txtDestinatario";
            this.txtDestinatario.Size = new System.Drawing.Size(100, 22);
            this.txtDestinatario.TabIndex = 1;
            // 
            // txtAsunto
            // 
            this.txtAsunto.Location = new System.Drawing.Point(354, 204);
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Size = new System.Drawing.Size(131, 22);
            this.txtAsunto.TabIndex = 2;
            // 
            // txtCuerpoMensaje
            // 
            this.txtCuerpoMensaje.Location = new System.Drawing.Point(344, 163);
            this.txtCuerpoMensaje.Name = "txtCuerpoMensaje";
            this.txtCuerpoMensaje.Size = new System.Drawing.Size(100, 22);
            this.txtCuerpoMensaje.TabIndex = 4;
            // 
            // lblValidacionEmail
            // 
            this.lblValidacionEmail.AutoSize = true;
            this.lblValidacionEmail.Location = new System.Drawing.Point(450, 126);
            this.lblValidacionEmail.Name = "lblValidacionEmail";
            this.lblValidacionEmail.Size = new System.Drawing.Size(113, 16);
            this.lblValidacionEmail.TabIndex = 5;
            this.lblValidacionEmail.Text = "Validacion correo";
            // 
            // lblInfoAdjuntos
            // 
            this.lblInfoAdjuntos.AutoSize = true;
            this.lblInfoAdjuntos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoAdjuntos.Location = new System.Drawing.Point(722, 161);
            this.lblInfoAdjuntos.Name = "lblInfoAdjuntos";
            this.lblInfoAdjuntos.Size = new System.Drawing.Size(112, 22);
            this.lblInfoAdjuntos.TabIndex = 6;
            this.lblInfoAdjuntos.Text = "info adjuntos";
            // 
            // lblEstudianteInfo
            // 
            this.lblEstudianteInfo.AutoSize = true;
            this.lblEstudianteInfo.Location = new System.Drawing.Point(460, 163);
            this.lblEstudianteInfo.Name = "lblEstudianteInfo";
            this.lblEstudianteInfo.Size = new System.Drawing.Size(90, 16);
            this.lblEstudianteInfo.TabIndex = 7;
            this.lblEstudianteInfo.Text = "Infoestudiante";
            // 
            // listAdjuntos
            // 
            this.listAdjuntos.HideSelection = false;
            this.listAdjuntos.Location = new System.Drawing.Point(580, 129);
            this.listAdjuntos.Name = "listAdjuntos";
            this.listAdjuntos.Size = new System.Drawing.Size(121, 97);
            this.listAdjuntos.TabIndex = 8;
            this.listAdjuntos.UseCompatibleStateImageBehavior = false;
            // 
            // btnAdjuntar
            // 
            this.btnAdjuntar.Location = new System.Drawing.Point(548, 245);
            this.btnAdjuntar.Name = "btnAdjuntar";
            this.btnAdjuntar.Size = new System.Drawing.Size(88, 38);
            this.btnAdjuntar.TabIndex = 9;
            this.btnAdjuntar.Text = "Adjuntar";
            this.btnAdjuntar.UseVisualStyleBackColor = true;
            // 
            // btnEliminarAdjunto
            // 
            this.btnEliminarAdjunto.Location = new System.Drawing.Point(642, 247);
            this.btnEliminarAdjunto.Name = "btnEliminarAdjunto";
            this.btnEliminarAdjunto.Size = new System.Drawing.Size(123, 36);
            this.btnEliminarAdjunto.TabIndex = 10;
            this.btnEliminarAdjunto.Text = "Eliminar Adjunto";
            this.btnEliminarAdjunto.UseVisualStyleBackColor = true;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(330, 253);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(75, 23);
            this.btnEnviar.TabIndex = 11;
            this.btnEnviar.Text = "Enviar";
            this.btnEnviar.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(411, 253);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 12;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(232, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 22);
            this.label3.TabIndex = 13;
            this.label3.Text = "Destinatario";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(259, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 22);
            this.label4.TabIndex = 14;
            this.label4.Text = "Asunto";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(260, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 22);
            this.label5.TabIndex = 15;
            this.label5.Text = "Mensaje";
            // 
            // Frm_correos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.btnEliminarAdjunto);
            this.Controls.Add(this.btnAdjuntar);
            this.Controls.Add(this.listAdjuntos);
            this.Controls.Add(this.lblEstudianteInfo);
            this.Controls.Add(this.lblInfoAdjuntos);
            this.Controls.Add(this.lblValidacionEmail);
            this.Controls.Add(this.txtCuerpoMensaje);
            this.Controls.Add(this.txtAsunto);
            this.Controls.Add(this.txtDestinatario);
            this.Controls.Add(this.panel1);
            this.Name = "Frm_correos";
            this.Size = new System.Drawing.Size(1132, 619);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestinatario;
        private System.Windows.Forms.TextBox txtAsunto;
        private System.Windows.Forms.TextBox txtCuerpoMensaje;
        private System.Windows.Forms.Label lblValidacionEmail;
        private System.Windows.Forms.Label lblInfoAdjuntos;
        private System.Windows.Forms.Label lblEstudianteInfo;
        private System.Windows.Forms.ListView listAdjuntos;
        private System.Windows.Forms.Button btnAdjuntar;
        private System.Windows.Forms.Button btnEliminarAdjunto;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
