using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            shuffledeck();
            using (formCambioSoldi form = new formCambioSoldi())//all'inizio chiedo all'utente quanti sildi punta
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    credito += form.money;
                    lblCredito.Text = $"credito: {credito.ToString()}$";
                }
            }
        }
        Random random = new Random(Environment.TickCount);//random per generare le carte in posti casuali

        //credito della persona
        int credito = 0;

        //array con i valori della carte per generarle
        int[] valoricarte = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10,10,10,10 };
        Ccarta[] mazzi = new Ccarta[156];
        int cartapuntata = 0;

        int puntata = 0; //puntate e lista puntate per annullarle
        List<int> listapuntate = new List<int>();
        int indexpuntate = -1;
        private void pnlFiches100_MouseClick(object sender, MouseEventArgs e)
        {
            if (credito - puntata - 100 >= 0)
            {
                puntata += 100;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
                listapuntate.Add(100);
                indexpuntate++;
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
                listapuntate.Add(20);
                indexpuntate++;
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
                listapuntate.Add(10);
                indexpuntate++;
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
                listapuntate.Add(500);
                indexpuntate++;
            }
            else
            {
                MessageBox.Show("Non hai altri soldi da puntare");
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            if (indexpuntate >= 0)
            {
                puntata -= listapuntate[indexpuntate];
                listapuntate.RemoveAt(indexpuntate);
                indexpuntate--;
                lblPuntata.Text = $"Puntata: {puntata.ToString()}$";
            } else
            {
                MessageBox.Show("non ci sono altri soldi da ritirare");
            }

        }


        public void shuffledeck()
        {   for(int i = 0; i < 156; i++)
            {
                mazzi[i] = null;
            }
            for (int i = 0; i < 3; i++)//for per generare tutti i mazzi
            {
                for (int j = 0; j < 4; j++)//for per i semi
                {
                    string seme = "";
                    switch (j)
                    {
                        case 0:
                            seme = "cuori";
                            break;
                        case 1:
                            seme = "quadri";
                            break;
                        case 2:
                            seme = "fiori";
                            break;
                        case 3:
                            seme = "picche";
                            break;
                    }

                    for (int x = 0; x < 13; x++)//for per il valore di ogni carta, se è figura o asso lo scriviamo perché poi servirà per il riconoscimento della carta
                    {
                        string speciale = "";
                        if (x > 0 && x <= 9)
                        {
                            speciale = "no";
                        }
                        switch (x)
                        {
                            case 0:
                                speciale = "asso";
                                break;
                            case 10:
                                speciale = "jack";
                                break;
                            case 11:
                                speciale = "donna";
                                break;
                            case 12:
                                speciale = "re";
                                break;
                        }

                        int index = random.Next(0, 156);
                        while (mazzi[index] != null)//va a controllare che la carta non sia già valorizzata
                        {
                            index = random.Next(0, 156);
                        }
 
                        mazzi[index] = new Ccarta(valoricarte[x], seme, speciale);
                    }
                }
 
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

        private void btn_distribuisci_Click(object sender, EventArgs e)
        {
            shuffledeck();
        }

        private void btn_addNewMoney_Click(object sender, EventArgs e)
        {
            using(formCambioSoldi form = new formCambioSoldi())
            {
                if(form.ShowDialog() == DialogResult.OK)
                {
                    credito += form.money;
                    lblCredito.Text = $"credito: {credito.ToString()}$";
                }
            }
        }
    }
}
