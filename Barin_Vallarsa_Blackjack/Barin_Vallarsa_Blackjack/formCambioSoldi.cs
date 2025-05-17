using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barin_Vallarsa_Blackjack
{
    public partial class formCambioSoldi : Form
    {
        public formCambioSoldi()
        {
            InitializeComponent();
        }
        public int money { get; set; }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            int soldi = int.Parse(input_soldi.Text);
            money = soldi;
            DialogResult = DialogResult.OK;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void btn_annulla_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
        }
    }
}
