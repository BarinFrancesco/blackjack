using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            changeMoney();
            GraphicsPath palla = new GraphicsPath();
            palla.AddEllipse(0, 0, btn_dealCards.Width, btn_dealCards.Height);
            btn_dealCards.Region = new Region(palla);
            btn_cancelBet.Region = new Region(palla);
            btn_lastBet.Region = new Region(palla);
            btn_addNewMoney.Region = new Region(palla);
            btn_call.Region = new Region(palla);
            btn_doubleDown.Region = new Region(palla);
            btn_stop.Region = new Region(palla);
            btn_split.Region = new Region(palla);
        }

        //random per generare le carte in posti casuali
        Random random = new Random(Environment.TickCount);

        //credito della persona
        int credito = 0;

        //array con i valori della carte per generarle
        int[] valoricarte = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10,10,10,10 };
        Ccarta[] mazzi = new Ccarta[156];
        int cartapuntata;
        List<Ccarta> manoGiocatore = new List<Ccarta>();
        int valoreManoGiocatore = 0;
        List<Ccarta> manoBanco = new List<Ccarta>();
        int valoreManoBanco = 0;
        List<Control> carteSulTavolo = new List<Control>();


        int puntata = 0; //puntate e lista puntate per annullarle
        int ultimaPuntata = 0;
        List<int> listapuntate = new List<int>();
        int indexpuntate = -1;

        int spostamentoGiocatore = 0;
        int spostamentoBanco = 0;

        private void pnlFiches10_MouseClick(object sender, MouseEventArgs e)
        {
            increaseBet(10);
        }

        private void pnlFiches20_MouseClick(object sender, MouseEventArgs e)
        {
            increaseBet(20);
        }

        private void pnlFiches100_MouseClick(object sender, MouseEventArgs e) 
        {
            increaseBet(100);
        }
        private void pnlFiches500_MouseClick(object sender, MouseEventArgs e)
        {
            increaseBet(500);
        }

        private void pnlFiches1000_MouseClick(object sender, MouseEventArgs e)
        {
            increaseBet(1000);
        }

        private void btn_cancelBet_MouseClick(object sender, MouseEventArgs e)//cancella l'ultima scommessa fatta, per farlo (prima) le salva in un elenco e lo legge al contrario
        {
            if (indexpuntate >= 0)
            {
                puntata -= listapuntate[indexpuntate];
                listapuntate.RemoveAt(indexpuntate);
                indexpuntate--;
                lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
            } else
            {
                MessageBox.Show("non ci sono altri soldi da ritirare");
            }
        }

        private void btn_distribuisci_Click(object sender, EventArgs e)//fa iniziare il turno
        {
            /*ultimaPuntata = puntata;
            puntata = 0;
            lblPuntata.Text = $"Puntata: {puntata.ToString()}$";*/
            
            showPlayingButtons(); //all'inizio del turno si mostrano i pultanti di gioco e si danno le prime 4 carte 2 al giocatore e 2 al banco
            if (cartapuntata > 155)
            {
                shuffledeck();
            }
            addCard(true);
            addCard(true);
            addCard(false);
            addCard(false);
            lbl_playersHandValue.Text = valoreManoGiocatore.ToString();//si mostrano in output i valori
            lbl_dealersHandValue.Text = valoreManoBanco.ToString();
        }

        private void btn_addNewMoney_Click(object sender, EventArgs e)
        {
            changeMoney();//lancia lo stesso form dell'inizio
        }

        private void btn_lastBet_Click(object sender, EventArgs e)//lascia come puntata quella del turno precedente
        {
            puntata = ultimaPuntata;
            lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
        }

        private void btn_doubleDown_Click(object sender, EventArgs e)
        {

        }

        private void btn_call_Click(object sender, EventArgs e)
        {
            addCard(true);
            if (valoreManoGiocatore > 21)//se il giocatore ha sballato controlliamo che abbia delgi assi per poterne abbassare il valore
            {
                bool abbassato = true;
                while (abbassato) //si inizializza un ciclo ceh va a vedere se abbiamo abbassato almeno un asso durante il ciclo
                {
                    abbassato = false;
                    valoreManoGiocatore = 0;
                    for (int i = 0; i < manoGiocatore.Count; i++) //con un ciclo facciamo la somma di tutte le carte, ed abbassiamo il primo asso con valore 11 che troviamo
                    {
                        if (manoGiocatore[i].special == "asso" && manoGiocatore[i].value == 11 && !abbassato)
                        {
                            manoGiocatore[i].abbassaAsso();
                            abbassato = true;
                        }
                        valoreManoGiocatore += manoGiocatore[i].value;
                    }
                    if (valoreManoGiocatore <= 21) // se la nuova somma non supera 21 allora chiudiamo il ciclo
                    {
                        lbl_playersHandValue.Text = valoreManoGiocatore.ToString();
                        return;
                    }
                }//il ciclo si ripete empre purchè almleno un asso venga abbassato
                lbl_playersHandValue.Text = "Hai perso";//se nessun' asso viene abbassato o se il totale è comunque sopra dopo aver abbassato tutti gli assi allora il giocatore ha perso
                return;
            }
            lbl_playersHandValue.Text = valoreManoGiocatore.ToString();// se è minore di 21 il giocatore puù continuare a giocare
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            showBettingButtons();
        }

        private void btn_split_Click(object sender, EventArgs e)
        {

        }

        private void increaseBet(int aggiunta)//funzione che fa aumentare la puntata, se il giocatore ha abbastanza soldi
        {
            if (credito - puntata - aggiunta >= 0)
            {
                puntata += aggiunta;
                lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
                listapuntate.Add(aggiunta);//aggiungo la puntata alla lista
                indexpuntate++;//incremento l'indice lalle lista
            }
            else
            {
                MessageBox.Show("Non hai tutti questi soldi da puntare");
            }
        }

        private void addCard(bool scelta)//funzione che aggiunge la carta al giocatore
        {
            if (scelta)
            {
                manoGiocatore.Add(mazzi[cartapuntata]);
                valoreManoGiocatore += mazzi[cartapuntata].value;
            } else
            {
                manoBanco.Add(mazzi[cartapuntata]);
                valoreManoBanco += mazzi[cartapuntata].value;
            }

            CreatePanel(mazzi[cartapuntata], scelta);
            cartapuntata++;
        }

        private void shuffledeck()//mescolo randomicamente i mazzi
        {
            for (int i = 0; i < 156; i++) 
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
            cartapuntata = 0;
        }

        private void showPlayingButtons()//funzione che fa muovere su i bottoni per giocare e giù quelli per puntare
        {
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 1000);//sposriamo i bottoni che non servoo fuori dalla portata dell'utente
            lbl_dealCards.Location = new Point(lbl_dealCards.Location.X, 1000);
            btn_cancelBet.Location = new Point(btn_cancelBet.Location.X, 1000);
            lbl_cancelBet.Location = new Point(lbl_cancelBet.Location.X, 1000);
            btn_addNewMoney.Location = new Point(btn_addNewMoney.Location.X, 1000);
            lbl_addNewMoney.Location = new Point(lbl_addNewMoney.Location.X, 1000);
            btn_lastBet.Location = new Point(btn_lastBet.Location.X, 1000);
            lbl_lastBet.Location = new Point(lbl_lastBet.Location.X, 1000);
            pnlFiches10.Location = new Point(pnlFiches10.Location.X, 1000);
            pnlFiches20.Location = new Point(pnlFiches20.Location.X, 1000);
            pnlFiches100.Location = new Point(pnlFiches100.Location.X, 1000);
            pnlFiches500.Location = new Point(pnlFiches500.Location.X, 1000);
            pnlFiches1000.Location = new Point(pnlFiches1000.Location.X, 1000);

            btn_call.Location = new Point(btn_call.Location.X, 400);
            lbl_call.Location = new Point(lbl_call.Location.X, 475);
            btn_doubleDown.Location = new Point(btn_doubleDown.Location.X, 400);
            lbl_doubleDown.Location = new Point(lbl_doubleDown.Location.X, 475);
            btn_stop.Location = new Point(btn_stop.Location.X, 400);
            lbl_stop.Location = new Point(lbl_stop.Location.X, 475);
            btn_split.Location = new Point(btn_split.Location.X, 400);
            lbl_split.Location = new Point(lbl_split.Location.X, 475);
        }

        private void showBettingButtons()//funzione che fa muovere su i bottoni per giocare e giù quelli per puntare
        {
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 400);//spostiamo i bottoni che non servoo fuori dalla portata dell'utente
            lbl_dealCards.Location = new Point(lbl_dealCards.Location.X, 475);
            btn_cancelBet.Location = new Point(btn_cancelBet.Location.X, 400);
            lbl_cancelBet.Location = new Point(lbl_cancelBet.Location.X, 475);
            btn_lastBet.Location = new Point(btn_lastBet.Location.X, 400);
            lbl_lastBet.Location = new Point(lbl_lastBet.Location.X, 475);
            btn_addNewMoney.Location = new Point(btn_addNewMoney.Location.X, 450);
            lbl_addNewMoney.Location = new Point(lbl_addNewMoney.Location.X, 525);
            pnlFiches10.Location = new Point(pnlFiches10.Location.X, 450);
            pnlFiches20.Location = new Point(pnlFiches20.Location.X, 450);
            pnlFiches100.Location = new Point(pnlFiches100.Location.X, 450);
            pnlFiches500.Location = new Point(pnlFiches500.Location.X, 450);
            pnlFiches1000.Location = new Point(pnlFiches1000.Location.X, 450);

            btn_call.Location = new Point(btn_call.Location.X, 1000);
            lbl_call.Location = new Point(lbl_call.Location.X, 1000);
            btn_doubleDown.Location = new Point(btn_doubleDown.Location.X, 1000);
            lbl_doubleDown.Location = new Point(lbl_doubleDown.Location.X, 1000);
            btn_stop.Location = new Point(btn_stop.Location.X, 1000);
            lbl_stop.Location = new Point(lbl_stop.Location.X, 1000);
            btn_split.Location = new Point(btn_split.Location.X, 1000);
            lbl_split.Location = new Point(lbl_split.Location.X, 1000);
        }

        private void changeMoney()
        {
            using (formCambioSoldi form = new formCambioSoldi())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    credito += form.money;
                    lblCredito.Text = $"credito: {credito.ToString()}$";
                }
            }
        }

        private void CreatePanel(Ccarta carta, bool scelta /*int valore, string seme, string speciale*/)//funzione per aggiungere la carta visivamente
        {
            string seme = carta.seed;
            int valore = carta.value;
            Panel nuovoPanel = new Panel();
            nuovoPanel.Size = new Size(67, 95);
            if (scelta)
            {
                nuovoPanel.Location = new Point((390 + spostamentoGiocatore), 300);
                spostamentoGiocatore += 20;
            } else
            {
                nuovoPanel.Location = new Point((400 + spostamentoBanco), 80);
                spostamentoBanco += 20;
            }

                nuovoPanel.BackColor = Color.Transparent;
            switch (carta.special)
            {
                case "no":
                    nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/{valore}-{seme}.jpg");
                    break;
                case "asso":
                    nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/asso-{seme}.jpg");
                    break;
                case "jack":
                    nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/jack-{seme}.jpg");
                    break;
                case "donna":
                    nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/donna-{seme}.jpg");
                    break;
                case "re":
                    nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/re-{seme}.jpg");
                    break;
            }
            
            nuovoPanel.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(nuovoPanel);
            nuovoPanel.BringToFront();
            carteSulTavolo.Add(nuovoPanel);
        }
    }
}
/* aggiungi quando premo bottone
moveAnimation(btn_distribuisci, 1000);
moveAnimation(lbl_distribuisci, 1080);
moveAnimation(btn_cancelBet, 1000);
moveAnimation(lbl_cancelBet, 1080);
moveAnimation(btn_lastBet, 1000);
moveAnimation(lbl_lastBet, 1080);
*/

/* aggiungi come funzione
private async void moveAnimation(Control btn, int endingpoint) //animazione per lo scorrimento orizzonatale
{
    while (btn.Location.Y <= endingpoint)
    {
        btn.Location = new Point(btn.Location.X, btn.Location.Y+20);
        await Task.Delay(1);
    }

}*/