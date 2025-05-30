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
using System.Media;

namespace Barin_Vallarsa_Blackjack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            shuffledeck(); // quando inizio il programma moescolo il mazzo e chiedo subito i soldi
            changeMoney();
            

            // partegrafica che serve a rendere i bottoni rotondi
            GraphicsPath palla = new GraphicsPath(); // nuovo oggetto della classe GraphicsPath
            palla.AddEllipse(0, 0, btn_dealCards.Width, btn_dealCards.Height); // ogetto palla è un ellisse (per cambiare la forma dei bottoni)
            btn_dealCards.Region = new Region(palla); 
            btn_cancelBet.Region = new Region(palla);
            btn_lastBet.Region = new Region(palla);
            btn_addNewMoney.Region = new Region(palla);
            btn_call.Region = new Region(palla);
            btn_doubleDown.Region = new Region(palla);
            btn_stop.Region = new Region(palla);
            btn_accettaAssicurazione.Region = new Region(palla);
            btn_rifiutaAssicurazione.Region = new Region(palla);
        }

        public SoundPlayer suono;

        //random per generare le carte in posti casuali
        Random random = new Random(Environment.TickCount);

        //credito della persona
        int credito = 0;

        //array con i valori della carte per generarle
        int[] valoricarte = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        Ccarta[] mazzi = new Ccarta[156];
        int cartaPuntata;
        List<Control> carteSulTavolo = new List<Control>(); //lista di pannelli, per cancellare poi le carte presenti sul tavolo

        int puntata = 0; //puntate e lista puntate per annullarle
        int ultimaPuntata = 0;
        List<int> listapuntate = new List<int>();
        int indexpuntate = -1;

        CPersonealbanco Giocatore = new CPersonealbanco(); // nuovo oggetto per il giocatore e il crupier
        CPersonealbanco Banco = new CPersonealbanco(); // nuovo oggetto della classe banco

        //variabile globale per metodo assicurazione
        bool assicurazione = false;
        int soldiAssicurazione = 0; 

        private void pnlFiches10_MouseClick(object sender, MouseEventArgs e) //funzioni per puntare soldi, richiamano tutte la stessa funzione e gli passano come parametro il valore da aggiungere
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

        private void btn_cancelBet_MouseClick(object sender, MouseEventArgs e) //cancella l'ultima scommessa fatta, per farlo (prima) le salva in un stack e poi lo legge al contrario usando metodo LIFO 
        {
            if (indexpuntate >= 0) // se è stata fatta almeno una puntata, allora l'indice della lista puntate è >= 0
            {
                puntata -= listapuntate[indexpuntate]; //rimuove i soldi dell'ultima puntata
                listapuntate.RemoveAt(indexpuntate); //rimuove la puntata dalla lista
                indexpuntate--; //aggiorna l'indice 
                lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
            }
            else
            {
                croupierSpeaking("Non ci sono altri soldi da ritirare", 4);
            }
        }

        private void btn_distribuisci_Click(object sender, EventArgs e) //fa iniziare il turno
        {


            if (puntata != 0)
            {

                //all'inizio del turno si mostrano i pulsanti di gioco e si danno le prime 4 carte, 2 al giocatore e 2 al banco
                addCard(true, false);
                addCard(false, false);
                addCard(true, false);
                addCard(false, true);

                if (Banco.Mano[0].value == 11 || Banco.Mano[0].value == 1)
                {
                    ShowInsuranceButtons(); // mostra i bottoni dell'assicurazione
                }
                else
                {
                    showPlayingButtons(); // mostra i bottoni "normali" per giocare
                }

                if ((Giocatore.Mano[0].value == 11 && Giocatore.Mano[1].value == 10) || (Giocatore.Mano[0].value == 10 && Giocatore.Mano[1].value == 11))
                {  // condizione dell'if che verifica se le prime due carte sono una carta speciale e un asso
                    lbl_playersHandValue.Text = "BLACKJACK";
                    Giocatore.Blackjack = true;
                    checkifwin();

                    return;
                }

            }
            else if (credito > 0) // se l'utente non ha soldi o non ne ha caricati ne lancia il rispettivo messaggio
            {
                croupierSpeaking("Devi puntare dei soldi per giocare amico", 1);
            }
            else
            {
                croupierSpeaking("Hai finito i soldi, caricane altri", 5);
            }

        }


        private void btn_lastBet_Click(object sender, EventArgs e)//lascia come puntata quella del turno precedente
        {
            if(credito - ultimaPuntata >= 0) // controlla se il giocatore ha abbastanza soldi per rifare la puntata precedente 
            {
                //pulisce la lista e azzera l'indice
                listapuntate.Clear(); 
                indexpuntate = -1;
                puntata = ultimaPuntata;
                lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
                listapuntate.Add(puntata);
                indexpuntate++;
            } else
            {
                croupierSpeaking("Non ha abbastanza soldi per piazzare questa puntata", 6);
            }

        }

        private void btn_addNewMoney_Click(object sender, EventArgs e)//lancia lo stesso form dell'inizio se l'utente vuole giocare con altri soldi
        {
            changeMoney();
        }


        private void btn_doubleDown_Click(object sender, EventArgs e) //raddoppio, se l'utente raddoppia la puntata gli viena data una sola carta e la sua mano finisce 
        {                                                             //il raddoppio può essere fatto solo con 2 carte in tavola
            if (Giocatore.Mano.Count == 2) // verifica che abbia 2 carte
            {
                if (credito - (puntata * 2) >= 0) // verifica che abbia abbastanza credito per raddoppiare la puntata
                {
                    addCard(true, false); // do una carta sola
                    puntata += puntata;
                    lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
                    dealersturn();
                }
                else
                {
                    croupierSpeaking("Non ha abbastanza soldi per raddoppiare", 7);
                }
            }
            else
            {
                croupierSpeaking("Può raddoppiare solo quando ha le prime 2 carte", 8);
            }

        }

        private void btn_call_Click(object sender, EventArgs e) // se l'utente chiama carta viene aggiunta una carta alla sua mano con la funzione addcard
        {
            addCard(true, false);
        }

        private void btn_stop_Click(object sender, EventArgs e) //quando l'utente vuole finire la sua mano comincia quella del dealer
        {
            dealersturn();
        }

        private void increaseBet(int aggiunta)//funzione che fa aumentare la puntata
        {
            if (credito - puntata - aggiunta >= 0)
            {
                puntata += aggiunta;
                lbl_bet.Text = $"Puntata: {puntata}$";
                listapuntate.Add(aggiunta);//aggiungo la puntata alla lista
                indexpuntate++;//incremento l'indice lalle lista
            }
            else
            {
                croupierSpeaking("Non hai tutti questi soldi da puntare", 9);
            }
        }

        private void addCard(bool scelta, bool hiddencard)//funzione che aggiunge la carta alla persona che la richiede 
        {                                                 // per creare fisicamente la carta viene usata la funzione showCard  
            // scelta --> true la carta va al giocatore - false la carta va al banco
            // hiddencard --> true coperta - false scoperta
            if (cartaPuntata >= 155) //rimescola il mazzo se abbiamo finito le carte
            {
                shuffledeck(); 
                croupierSpeaking("Il mazzo sta venendo rimescolato", 13);
            }

            if (scelta) // scelta == true
            {
                Giocatore.Mano.Add(mazzi[cartaPuntata]); //aggiunge la carta alla mano del giocatore
                Giocatore.ValoreMano += mazzi[cartaPuntata].value; //aggiorna il valore alla mano

                if (Giocatore.ValoreMano > 21) //se il giocatore ha sballato controlliamo che abbia degli assi per poterne abbassare il valore
                {
                    bool abbassato = true;
                    while (abbassato) //si inizializza un ciclo che va a vedere se abbiamo abbassato almeno un asso durante il ciclo
                    {
                        abbassato = false;
                        Giocatore.ValoreMano = 0;

                        for (int i = 0; i < Giocatore.Mano.Count; i++)
                        {    //con un ciclo facciamo la somma di tutte le carte, ed abbassiamo il primo asso con valore 11 che troviamo
                            if (Giocatore.Mano[i].special == "asso" && Giocatore.Mano[i].value == 11 && !abbassato)
                            {
                                Giocatore.Mano[i].abbassaAsso();
                                abbassato = true;
                            }
                            Giocatore.ValoreMano += Giocatore.Mano[i].value; // ricalcola punteggio con l'asso abbassato
                        }

                        if (Giocatore.ValoreMano <= 21) // se la nuova somma non supera 21 allora chiudiamo il ciclo, altrimenti si ripete il ciclo
                        {
                            lbl_playersHandValue.Text = Giocatore.ValoreMano.ToString();
                            showCard(mazzi[cartaPuntata], scelta, false); // mostra la carta presa dal mazzo al giocatore (carta scoperta) 
                            cartaPuntata++; // incremento indice per passare alla carta successiva
                            return;
                        }

                    } //il ciclo si ripete finchè almeno un asso viene abbassato

                    lbl_playersHandValue.Text = "Hai sballato";//se nessun asso viene abbassato ed il totale è comunque sopra il giocatore ha perso
                    showCard(mazzi[cartaPuntata], scelta, false);
                    Giocatore.Sballa = true;
                    cartaPuntata++;
                    checkifwin();
                    return;
                }
                lbl_playersHandValue.Text = Giocatore.ValoreMano.ToString();// se è minore di 21 il giocatore può continuare a giocare
            }
            else // scelta == false --> gioca il banco
            {
                Banco.Mano.Add(mazzi[cartaPuntata]);
                Banco.ValoreMano += mazzi[cartaPuntata].value;

                if (Banco.ValoreMano > 21) // stessa logica come per il giocatore 
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
                            showCard(mazzi[cartaPuntata], scelta, hiddencard);
                            cartaPuntata++;
                            return;
                        }
                    }

                    lbl_dealersHandValue.Text = "Il banco ha sballato";
                    Banco.Sballa = true;
                    showCard(mazzi[cartaPuntata], scelta, false);
                    cartaPuntata++;
                    return;
                }

                if (!hiddencard) // il banco deve tenere una carta coperta, se è il caso allora non ne mostra neanche il valore
                {
                    lbl_dealersHandValue.Text = Banco.ValoreMano.ToString();
                }
                else
                {
                    lbl_dealersHandValue.Text = $"{Banco.ValoreMano - mazzi[cartaPuntata].value}";
                }

            }
            showCard(mazzi[cartaPuntata], scelta, hiddencard);
            cartaPuntata++;
        }


        private void showCard(Ccarta carta, bool scelta, bool hiddencard)//funzione per aggiungere la carta visivamente
        {
            string seme = carta.seed;
            int valore = carta.value;
            Panel nuovoPanel = new Panel(); // creazione della carta sotto forma di pannello
            nuovoPanel.Size = new Size(67, 95);
            if (scelta) // scelta == true --> carte giocatore
            { // posizionamento carte del giocatore
                nuovoPanel.Location = new Point((390 + Giocatore.Spostamento), 300);
                Giocatore.Spostamento += 20;
            }
            else
            { // posizionamento carte del dealer
                nuovoPanel.Location = new Point((400 + Banco.Spostamento), 80);
                Banco.Spostamento += 20;
            }

            nuovoPanel.BackColor = Color.Transparent;
            if (!hiddencard) 
            { // se la carta è scoperta, sceglie il tipo di carta in base al valore - seme - carta speciale
                switch (carta.special) 
                {
                    case "no":
                        nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/{valore}-{seme}.jpg");
                        // metodo della classe Image(percorso del file 3-cuori})
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
            else // se è coperta mostra immagine del retro della carta
            {
                nuovoPanel.BackgroundImage = Image.FromFile($"../../Images/mazzo/carta-coperta.jpg");
            }


            nuovoPanel.BackgroundImageLayout = ImageLayout.Stretch; // adatta immagine alla dimensione del pannello
            Controls.Add(nuovoPanel); // fa visualizzare il pannello sul banco
            nuovoPanel.BringToFront(); // porta pannello in primo piano 
            carteSulTavolo.Add(nuovoPanel); // salva la carta nella lista per poterla rimuovere successivamente
        }


        private void dealersturn() //quando tocca al dealer quest'ultimo rivela la carta coperta e chiama fino al 17, poi controlla chi ha vinto
        {
            lbl_dealersHandValue.Text = Banco.ValoreMano.ToString();
            string path = "";
            switch (Banco.Mano[1].special) // indice 1 = seconda carta del dealer
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
            carteSulTavolo[3].BackgroundImage = Image.FromFile(path); // sostituzione della carta coperta con quella scoperta
            Task.Delay(1000).GetAwaiter().GetResult(); // 1 secondo delay

            if ((Banco.Mano[0].value == 11 && Banco.Mano[1].value == 10) || (Banco.Mano[0].value == 10 && Banco.Mano[1].value == 11))
            {
                lbl_dealersHandValue.Text = "BLACKJACK";
                Banco.Blackjack = true;
            }

            while (Banco.ValoreMano < 17) 
            { // pesca un'altra carta scoperta
                Task.Delay(500).GetAwaiter().GetResult();
                addCard(false, false);
            }
            checkifwin();
        }


        private void checkifwin() //tramite le condizioni di vittoria si vede chi ha vinto e si dano i rispettivi soldi in base alla puntata ed al tipo di vincita
        {
            if (Giocatore.Blackjack)
            {
                credito += (puntata + puntata / 2);
                lblCredito.Text = " Credito:"+  credito.ToString() +"$";
                croupierSpeaking("Complimenti, hai vinto", 2);
            }
            else if ((Giocatore.ValoreMano > Banco.ValoreMano || Banco.Sballa) && !Giocatore.Sballa)
            {
                credito += puntata;
                lblCredito.Text = " Credito:" + credito.ToString() + "$";
                //messaggio vittoria
                croupierSpeaking("Complimenti, hai vinto", 2);

            }
            else if ((Giocatore.ValoreMano < Banco.ValoreMano || Giocatore.Sballa || Banco.Blackjack) && !Banco.Sballa)
            {
                credito -= puntata;
                if(assicurazione)
                {
                credito += soldiAssicurazione;
                }
                lblCredito.Text = " Credito:" + credito.ToString() + "$";
                //messaggio sconfitta
                croupierSpeaking("Peccato, hai perso", 3);
            }
            else
            {
                croupierSpeaking("Avete pareggiato", 10);
            }

            ultimaPuntata = puntata;// alla fine si resettano tutti i valori
            puntata = 0;
            lbl_bet.Text = $"Puntata: {puntata.ToString()}$";
            lbl_dealersHandValue.Text = "";
            lbl_playersHandValue.Text = "";
            listapuntate.Clear();
            indexpuntate = -1;
            soldiAssicurazione = 0;
            assicurazione = false;
            
            // pulisce il tavolo
            Banco = new CPersonealbanco();
            Giocatore = new CPersonealbanco();

            for (int i = 0; i < carteSulTavolo.Count; i++)
            {
                carteSulTavolo[i].Dispose();
            }
            carteSulTavolo = new List<Control>();
           // rimostra i bottoni per scommettere e nasconde quelli dell'assicurazione
            showBettingButtons();
            HideInsuranceButtons();
        }


        private void shuffledeck()//mescolo randomicamente i mazzi
        {   // 3 mazzi = 156 carte (3*52)
            for (int i = 0; i < 156; i++)
            {  // svuota l'array
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

                    for (int x = 0; x < 13; x++)//for per il valore di ogni carta, se è carta speciale o asso lo scriviamo perché poi servirà per il riconoscimento della carta
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

                        int index = random.Next(0, 156); // carte casuali
                        while (mazzi[index] != null)//se la carta generata capita in quella posizione (indice), genera di nuovo il random finchè non trova uno spazio libero
                        {
                            index = random.Next(0, 156);
                        }

                        mazzi[index] = new Ccarta(valoricarte[x], seme, speciale); // inserimento in quella posizione dell'oggeto della classe Ccarta
                    }
                }

            }
            cartaPuntata = 0; // dopo rimescolamento si riparte dall'inizio del mazzo
        }


        private void showPlayingButtons()//funzione che fa muovere su i bottoni per giocare e giù quelli per puntare
        {
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 1000);
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
        }


        private void showBettingButtons()//funzione che fa muovere giù i bottoni per giocare e su quelli per puntare
        {
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 400);
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
        }


        private void changeMoney() // lancia il form per aggiungere soldi al credito del giocatore
        {
            using (formCambioSoldi form = new formCambioSoldi())
            {
                if (form.ShowDialog() == DialogResult.OK) // se utente ha premuto ok
                {
                    if(credito + form.money <= 999999999) // limite credito massimo
                    {
                        credito += form.money;
                        lblCredito.Text = $"credito: {credito.ToString()}$";
                        
                        // suono da ricchi quando cadono le fishes
                        suono = new SoundPlayer("FishesSound.wav");
                        suono.Play();
                    } else
                    {
                        croupierSpeaking("Ci dispiace, ma non siamo capaci di gestire una quantità di denaro così grande", 11);
                    }

                }
            }
        }


        private void croupierSpeaking(string message, int scelta) // funzione per comunicare con l'utente alternativa al messagebox
        {
            using (dealerSpeaking form = new dealerSpeaking(message, scelta))
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(700, 100);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    //non ritorno niente perché devo solo dare un messaggio
                    if(suono != null)
                    {
                        suono.Stop();
                    }
                }
            }
        }

        private void ShowInsuranceButtons()
        {
            btn_accettaAssicurazione.Location = new Point(btn_accettaAssicurazione.Location.X, 350);
            btn_rifiutaAssicurazione.Location = new Point(btn_rifiutaAssicurazione.Location.X, 350);
            lbl_accettaAssicurazione.Location = new Point(lbl_accettaAssicurazione.Location.X, 430);
            lbl_rifiutaAssicurazione.Location = new Point(lbl_rifiutaAssicurazione.Location.X, 430);

            // nascondi tutti gli altri bottoni per le puntate
            btn_dealCards.Location = new Point(btn_dealCards.Location.X, 1000);
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
        }

        private void HideInsuranceButtons()
        {
            btn_accettaAssicurazione.Location = new Point(btn_accettaAssicurazione.Location.X, 1000);
            btn_rifiutaAssicurazione.Location = new Point(btn_rifiutaAssicurazione.Location.X, 1000);
            lbl_accettaAssicurazione.Location = new Point(lbl_accettaAssicurazione.Location.X, 1000);
            lbl_rifiutaAssicurazione.Location = new Point(lbl_rifiutaAssicurazione.Location.X, 1000);
        }

        private void btn_accettaAssicurazione_Click(object sender, EventArgs e)
        {
            if(credito - (2 * puntata) > 0)
            {
                assicurazione = true;
                soldiAssicurazione = puntata;
                HideInsuranceButtons();
                showPlayingButtons();

            }
            else
            {
                croupierSpeaking("Non hai abbastanza soldi per giocare l'assicurazione!!!", 12);
            }
        }

        private void btn_rifiutaAssicurazione_Click(object sender, EventArgs e)
        {
            HideInsuranceButtons();
            showPlayingButtons();
        }
    }
}  