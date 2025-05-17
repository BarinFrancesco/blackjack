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
            this.input_soldi = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_annulla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // input_soldi
            // 
            this.input_soldi.Location = new System.Drawing.Point(287, 85);
            this.input_soldi.Name = "input_soldi";
            this.input_soldi.Size = new System.Drawing.Size(249, 20);
            this.input_soldi.TabIndex = 0;
            this.input_soldi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(286, 111);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(173, 37);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "CONVERTI SOLDI";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_annulla
            // 
            this.btn_annulla.Location = new System.Drawing.Point(465, 111);
            this.btn_annulla.Name = "btn_annulla";
            this.btn_annulla.Size = new System.Drawing.Size(71, 37);
            this.btn_annulla.TabIndex = 2;
            this.btn_annulla.Text = "annulla operazione";
            this.btn_annulla.UseVisualStyleBackColor = true;
            this.btn_annulla.Click += new System.EventHandler(this.btn_annulla_Click);
            // 
            // formCambioSoldi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_annulla);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.input_soldi);
            this.Name = "formCambioSoldi";
            this.Text = "formCambioSoldi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox input_soldi;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_annulla;
    }
}