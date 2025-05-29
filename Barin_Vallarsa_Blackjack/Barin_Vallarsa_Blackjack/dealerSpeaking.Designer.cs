namespace Barin_Vallarsa_Blackjack
{
    partial class dealerSpeaking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dealerSpeaking));
            this.lbl_dealerVoice = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_dealerVoice
            // 
            this.lbl_dealerVoice.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbl_dealerVoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dealerVoice.Location = new System.Drawing.Point(203, 238);
            this.lbl_dealerVoice.Name = "lbl_dealerVoice";
            this.lbl_dealerVoice.Size = new System.Drawing.Size(402, 65);
            this.lbl_dealerVoice.TabIndex = 1;
            this.lbl_dealerVoice.Text = " ";
            this.lbl_dealerVoice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(198, 306);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(407, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dealerSpeaking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_dealerVoice);
            this.Name = "dealerSpeaking";
            this.Text = "dealerSpeaking";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_dealerVoice;
        private System.Windows.Forms.Button button1;
    }
}