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
    public partial class Form1 : Form
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
        int[] valoricarte = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        Ccarta[] mazzi = new Ccarta[156];
        int cartaPuntata;
        List<Control> carteSulTavolo = new List<Control>();

        int puntata = 0; //puntate e lista puntate per annullarle
        int ultimaPuntata = 0;
        List<int> listapuntate = new List<int>();
        int indexpuntate = -1;

        CPersonealbanco Giocatore = new CPersonealbanco();
        CPersonealbanco Banco = new CPersonealbanco();

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
            }
            else
            {
                croupierSpeaking("Non ci sono altri soldi da ritirare");
            }
        }

        private void btn_distribuisci_Click(object sender, EventArgs e)//fa iniziare il turno
        {


            if (puntata != 0)
            {
                if (cartaPuntata > 155)
                {
                    shuffledeck();
                }

                showPlayingButtons();//all'inizio del turno si mostrano i pulsanti di gioco e si danno le prime 4 carte 2 al giocatore e 2 al banco
                addCard(true, false);
                addCard(true, false);
                addCard(false, false);
                addCard(false, true);

                if ((Giocatore.Mano[0].value == 11 && Giocatore.Mano[1].value == 10) || (Giocatore.Mano[0].value == 10 && Giocatore.Mano[1].value == 11))
                {
                    lbl_playersHandValue.Text = "BLACKJACK";
                    Giocatore.Blackjack = true;
                    checkifwin();
                }
            }
            else if (credito > 0)
            {
                croupierSpeaking("Devi puntare dei soldi per giocare amico");
            }
            else
            {
                croupierSpeaking("Hai finito i soldi, caricane altri");
            }

        }

        private void btn_addNewMoney_Click(object sender, EventArgs e)
        {
            changeMoney();//lancia lo stesso form dell'inizio
        }

        private void btn_lastBet_Click(object sender, EventArgs e)//lascia come puntata quella del turno precedente
        {
            if(credito - ultimaPuntata >= 0)
            {
                puntata = ultimaPuntata;
                lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
            } else
            {
                croupierSpeaking("Non ha abbastanza soldi per piazzare questa puntata");
            }

        }

        private void btn_doubleDown_Click(object sender, EventArgs e) //raddoppio
        {
            if (Giocatore.Mano.Count == 2)
            {
                if (credito - (puntata * 2) >= 0)
                {
                    addCard(true, false);
                    puntata += puntata;
                    lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
                    dealersturn();
                }
                else
                {
                    croupierSpeaking("Non ha abbastanza soldi per raddoppiare");
                }
            }
            else
            {
                croupierSpeaking("Può raddoppiare solo qunando ha le prime 2 carte");
            }

        }

        private void btn_call_Click(object sender, EventArgs e)
        {
            Task.Delay(500).GetAwaiter().GetResult();
            addCard(true, false);
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            dealersturn();
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
                croupierSpeaking("Non hai tutti questi soldi da puntare");
            }
        }

        private void addCard(bool scelta, bool hiddencard)//funzione che aggiunge la carta al giocatore
        {
            if (scelta)
            {
                Giocatore.Mano.Add(mazzi[cartaPuntata]);
                Giocatore.ValoreMano += mazzi[cartaPuntata].value;

                if (Giocatore.ValoreMano > 21) //se il giocatore ha sballato controlliamo che abbia degli assi per poterne abbassare il valore
                {
                    bool abbassato = true;
                    while (abbassato) //si inizializza un ciclo che va a vedere se abbiamo abbassato almeno un asso durante il ciclo
                    {
                        abbassato = false;
                        Giocatore.ValoreMano = 0;

                        for (int i = 0; i < Giocatore.Mano.Count; i++) //con un ciclo facciamo la somma di tutte le carte, ed abbassiamo il primo asso con valore 11 che troviamo
                        {
                            if (Giocatore.Mano[i].special == "asso" && Giocatore.Mano[i].value == 11 && !abbassato)
                            {
                                Giocatore.Mano[i].abbassaAsso();
                                abbassato = true;
                            }
                            Giocatore.ValoreMano += Giocatore.Mano[i].value;
                        }

                        if (Giocatore.ValoreMano <= 21) // se la nuova somma non supera 21 allora chiudiamo il ciclo, altrimenti si ripete il ciclo
                        {
                            lbl_playersHandValue.Text = Giocatore.ValoreMano.ToString();
                            CreatePanel(mazzi[cartaPuntata], scelta, false);
                            cartaPuntata++;
                            return;
                        }

                    }//il ciclo si ripete sempre purchè almleno un asso venga abbassato

                    lbl_playersHandValue.Text = "Hai sballato";//se nessun asso viene abbassato ed il totale è comunque sopra il giocatore ha perso
                    CreatePanel(mazzi[cartaPuntata], scelta, false);
                    Giocatore.Sballa = true;
                    cartaPuntata++;
                    checkifwin();
                    return;
                }
                lbl_playersHandValue.Text = Giocatore.ValoreMano.ToString();// se è minore di 21 il giocatore può continuare a giocare
            }
            else
            {
                Banco.Mano.Add(mazzi[cartaPuntata]);
                Banco.ValoreMano += mazzi[cartaPuntata].value;

                if (Banco.ValoreMano > 21)
                {
                    bool abbassato = true;
                    while (abbassato)
                    {

                        abbassato = false;
                        Banco.ValoreMano = 0;

                        for (int i = 0; i < Banco.Mano.Count; i++)
                        {
                            if (Banco.Mano[i].special == "asso" && Banco.Mano[i].value == 11 && !abbassato)
                            {
                                Banco.Mano[i].abbassaAsso();
                                abbassato = true;
                            }
                            Banco.ValoreMano += Banco.Mano[i].value;
                        }

                        if (Banco.ValoreMano <= 21)
                        {
                            lbl_dealersHandValue.Text = Banco.ValoreMano.ToString();
                            CreatePanel(mazzi[cartaPuntata], scelta, hiddencard);
                            cartaPuntata++;
                            return;
                        }
                    }

                    lbl_dealersHandValue.Text = "Il banco ha sballato";
                    Banco.Sballa = true;
                    CreatePanel(mazzi[cartaPuntata], scelta, false);
                    cartaPuntata++;
                    return;
                }

                if (!hiddencard)
                {
                    lbl_dealersHandValue.Text = Banco.ValoreMano.ToString();
                }
                else
                {
                    lbl_dealersHandValue.Text = $"{Banco.ValoreMano - mazzi[cartaPuntata].value}";
                }

            }
            CreatePanel(mazzi[cartaPuntata], scelta, hiddencard);
            cartaPuntata++;
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


        private void CreatePanel(Ccarta carta, bool scelta, bool hiddencard)//funzione per aggiungere la carta visivamente
        {
            string seme = carta.seed;
            int valore = carta.value;
            Panel nuovoPanel = new Panel();
            nuovoPanel.Size = new Size(67, 95);
            if (scelta)
            {
                nuovoPanel.Location = new Point((390 + Giocatore.Spostamento), 300);
                Giocatore.Spostamento += 20;
            }
            else
            {
                nuovoPanel.Location = new Point((400 + Banco.Spostamento), 80);
                Banco.Spostamento += 20;
            }

            nuovoPanel.BackColor = Color.Transparent;
            if (!hiddencard)
            {
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
            }
            else
            {
                nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/carta-coperta.jpg");
            }


            nuovoPanel.BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(nuovoPanel);
            nuovoPanel.BringToFront();
            carteSulTavolo.Add(nuovoPanel);
        }


        private void dealersturn()
        {
            lbl_dealersHandValue.Text = Banco.ValoreMano.ToString();
            string path = "";
            switch (Banco.Mano[1].special)
            {
                case "asso":
                    path = $"asso";
                    break;
                case "jack":
                    path = $"jack";
                    break;
                case "donna":
                    path = $"donna";
                    break;
                case "re":
                    path = $"re";
                    break;
                default:
                    path = $"{Banco.Mano[1].value}";
                    break;
            }
            path = "../../Images/mazzo/" + path + $"-{Banco.Mano[1].seed}.jpg";
            carteSulTavolo[3].BackgroundImage = Image.FromFile(path);
            Task.Delay(1000).GetAwaiter().GetResult();

            if ((Banco.Mano[0].value == 11 && Banco.Mano[1].value == 10) || (Banco.Mano[0].value == 10 && Banco.Mano[1].value == 11))
            {
                lbl_dealersHandValue.Text = "BLACKJACK";
                Banco.Blackjack = true;
            }


            while (Banco.ValoreMano < 17)
            {
                Task.Delay(500).GetAwaiter().GetResult();
                addCard(false, false);
            }
            checkifwin();
        }


        private void checkifwin()
        {
            if (Giocatore.Blackjack)
            {
                credito += (puntata + puntata / 2);
                lblCredito.Text = credito.ToString();
                croupierSpeaking("Complimenti, hai vinto");
            }
            else if ((Giocatore.ValoreMano > Banco.ValoreMano || Banco.Sballa) && !Giocatore.Sballa)
            {
                credito += puntata;
                lblCredito.Text = credito.ToString();
                croupierSpeaking("Complimenti, hai vinto");
            }
            else if ((Giocatore.ValoreMano < Banco.ValoreMano || Giocatore.Sballa || Banco.Blackjack) && !Banco.Sballa)
            {
                credito -= puntata;
                lblCredito.Text = credito.ToString();
                croupierSpeaking("Peccato, hai perso");
            }
            else
            {
                croupierSpeaking("Avete pareggiato");
            }

            ultimaPuntata = puntata;
            puntata = 0;
            lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
            lbl_dealersHandValue.Text = "";
            lbl_playersHandValue.Text = "";
            listapuntate.Clear();
            indexpuntate = -1;

            Banco = new CPersonealbanco();
            Giocatore = new CPersonealbanco();

            for (int i = 0; i < carteSulTavolo.Count; i++)
            {
                carteSulTavolo[i].Dispose();
            }
            carteSulTavolo = new List<Control>();
            showBettingButtons();
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
            cartaPuntata = 0;
        }


        private void showPlayingButtons()//funzione che fa muovere su i bottoni per giocare e giù quelli per puntare
        {
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 1000);//spostiamo i bottoni che non servoo fuori dalla portata dell'utente
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


        private void croupierSpeaking(string message)
        {
            using (dealerSpeaking form = new dealerSpeaking(message))
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(650, 100);
                if (form.ShowDialog() == DialogResult.OK)
                {

                }
            }
        }


    }
}
// puoi aggiungere l'assicurazione se il dealer mostra un asso e lo split

// per l'assicurazione aggiungi 2 bottoni nascosti uno verde e uno rosso metti una variabile booleana globale che ti tiene memoria dell'assicurazione                        

// controlla sulla funzione btn_distribuisci_Click se il banco ha asso come prima carta (manoBanco[0].value == 11 || manoBanco[0].value == 1 ) perché magari il banco ha 2 assi e quindi il primo vale 1

// se il banco ha asso allora mostra i 2 bottoni e in base a quale preme metti la variabile booleana true o false, poi nascondi di nuovo i bottoni

// se metti true inserisci anche che l'utende deve pagare la puntata una'ltra volta , prima però controlla che abbia abbastanza soldi per farlo, sennò lancia messaggio di errore

// aggiungi una condizione alla fine dellla funzione chek if win dove vedi se questa variabile è attivata, se il banco ha blackjack e tu hai la variabile attivata allora ti ritornano indietro i soldi, se il banco ha blackjack e tu hai rifiutato perdi

// se il banco non ha blakcjack e tu non hai accettato non fare niente e continua col resto, se invece il banco non ha blakcjack e tu hai accettato i soldi non ti ritornano indietro
// infine resetta tutto alla fine delle condizioni di vittoria   