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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        float puntata = 0;
        float credito = 1000;
        private void pnlFiches100_MouseClick(object sender, MouseEventArgs e)
        {
            if (credito - puntata - 100 >= 0)
            {
                puntata += 100;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
            } else
            {
                MessageBox.Show("Non hai altri soldi da puntare");
            }

        }

        private void pnlFiches25_MouseClick(object sender, MouseEventArgs e)
        {
            if (credito - puntata - 20 >= 0)
            {
                puntata += 20;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
            }
            else
            {
                MessageBox.Show("Non hai altri soldi da puntare");
            }
        }

        private void pnlFiches10_MouseClick(object sender, MouseEventArgs e)
        {
            if (credito - puntata - 10 >= 0)
            {
                puntata += 10;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
            }
            else
            {
                MessageBox.Show("Non hai altri soldi da puntare");
            }
        }

        private void pnlFiches500_MouseClick(object sender, MouseEventArgs e)
        {
            if (credito - puntata - 500 >= 0)
            {
                puntata += 500;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
            }
            else
            {
                MessageBox.Show("Non hai altri soldi da puntare");
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            //crea un'immanige con il percorso dato, e la mette all'interno di panel 1, unato questo perché coì non ci sono bordi o margini brutti da vedere
            Image img = Image.FromFile("../../Images/mazzo/carta-coperta.jpg");
            e.Graphics.DrawImage(img, panel5.ClientRectangle);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Image img = Image.FromFile("../../Images/mazzo/re-fiori.jpg");
            e.Graphics.DrawImage(img, panel1.ClientRectangle);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            Image img = Image.FromFile("../../Images/mazzo/donna-quadri.jpg");
            e.Graphics.DrawImage(img, panel6.ClientRectangle);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            Image img = Image.FromFile("../../Images/mazzo/asso-picche.jpg");
            e.Graphics.DrawImage(img, panel7.ClientRectangle);
        }
    }
}
