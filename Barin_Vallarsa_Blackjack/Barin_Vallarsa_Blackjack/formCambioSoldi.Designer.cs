namespace Barin_Vallarsa_Blackjack
{
    partial class formCambioSoldi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formCambioSoldi));
            this.txt_input_soldi = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_annulla = new System.Windows.Forms.Button();
            this.lbl_dealerVoice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_input_soldi
            // 
            this.txt_input_soldi.Location = new System.Drawing.Point(373, 387);
            this.txt_input_soldi.Margin = new System.Windows.Forms.Padding(4);
            this.txt_input_soldi.Name = "txt_input_soldi";
            this.txt_input_soldi.Size = new System.Drawing.Size(331, 22);
            this.txt_input_soldi.TabIndex = 0;
            this.txt_input_soldi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_input_soldi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(371, 419);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(231, 46);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "CONVERTI SOLDI";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_annulla
            // 
            this.btn_annulla.Location = new System.Drawing.Point(610, 419);
            this.btn_annulla.Margin = new System.Windows.Forms.Padding(4);
            this.btn_annulla.Name = "btn_annulla";
            this.btn_annulla.Size = new System.Drawing.Size(95, 46);
            this.btn_annulla.TabIndex = 2;
            this.btn_annulla.Text = "annulla operazione";
            this.btn_annulla.UseVisualStyleBackColor = true;
            this.btn_annulla.Click += new System.EventHandler(this.btn_annulla_Click);
            // 
            // lbl_dealerVoice
            // 
            this.lbl_dealerVoice.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_dealerVoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dealerVoice.Location = new System.Drawing.Point(653, 51);
            this.lbl_dealerVoice.Name = "lbl_dealerVoice";
            this.lbl_dealerVoice.Size = new System.Drawing.Size(402, 65);
            this.lbl_dealerVoice.TabIndex = 0;
            this.lbl_dealerVoice.Text = " ";
            this.lbl_dealerVoice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_dealerVoice.Click += new System.EventHandler(this.lbl_dealerVoice_Click);
            // 
            // formCambioSoldi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.lbl_dealerVoice);
            this.Controls.Add(this.btn_annulla);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_input_soldi);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "formCambioSoldi";
            this.Text = "formCambioSoldi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_input_soldi;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_annulla;
        private System.Windows.Forms.Label lbl_dealerVoice;
    }
}