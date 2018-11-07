using AwesomePokerGameSln.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CardType = System.Tuple<int, int>;

namespace AwesomePokerGameSln {
  public partial class FrmPlaygame : Form {
    private Deck deck;
    private PictureBox[] playerCardPics;
    private PictureBox[] dealerCardPics;
    private Hand playerHand;
    private Hand dealerHand;
    private Scorekeeper scorekeeper;

        const int AvatarPicBoxIndex = 6;
        DateTime startTime = DateTime.Now;
        Timer MyTimer = new Timer();



        public FrmPlaygame() {
      InitializeComponent();
      playerCardPics = new PictureBox[5];
      for (int c = 1; c <= 5; c++) {
        playerCardPics[c - 1] = this.Controls.Find("picCard" + c.ToString(), true)[0] as PictureBox;
      }
      dealerCardPics = new PictureBox[5];
      for (int c = 1; c <= 5; c++) {
        dealerCardPics[c - 1] = this.Controls.Find("pictureBox" + c.ToString(), true)[0] as PictureBox;
      }
    }

    private void dealCards() {
      deck.shuffleDeck();
      Tuple<int, int>[] cards = new Tuple<int, int>[5];
      int index = 0;
      foreach (PictureBox playerCardPic in playerCardPics) {
        CardType card = deck.nextCard();
        //CardType card = new CardType(index, inde);
        cards[index++] = card;
        playerCardPic.BackgroundImage = CardImageHelper.cardToBitmap(card);
      }
      dealerHand = new Hand(cards);
      cards = new CardType[5];
      index = 0;
      foreach (PictureBox dealerCardPic in dealerCardPics) {
        CardType card = deck.nextCard();
        //CardType card = new CardType(index, inde);
        cards[index++] = card;
        dealerCardPic.BackgroundImage = CardImageHelper.cardToBitmap(card);
      }
      playerHand = new Hand(cards);
      lblHandType.Text = playerHand.getHandType().ToString();

            // set up timer
            MyTimer.Stop();
            TimerLabel.ForeColor = System.Drawing.Color.Black;
            TimerLabel.Text = "30 seconds remaining";
            startTime = DateTime.Now;
            lblHandType.Text = playerHand.getHandType().ToString();
            // start timer
            MyTimer.Tick += (s, ev) => { TimerLabel.Text = String.Format("{0:00} seconds remaining", 30 - (DateTime.Now - startTime).Seconds); };
            MyTimer.Interval = 1000;
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }

    private void FrmPlaygame_FormClosed(object sender, FormClosedEventArgs e) {
      foreach (Form f in Application.OpenForms)
        f.Close();
    }

    private void FrmPlaygame_Load(object sender, EventArgs e) {
      scorekeeper = new Scorekeeper();
      scorekeeper.startUpdatePot();
      // This is a temporary function to emulate the cpu making the first bet
      scorekeeper.updateBet();
      deck = new Deck();
      dealCards();
      startBet();
      updateValues();
    }

    private void button1_Click(object sender, EventArgs e) {
      dealCards();
      scorekeeper.startUpdatePot();
      // This is a temporary function to emulate the cpu making the first bet
      scorekeeper.updateBet();
      startBet();
      updateValues();
    }

        /// <summary>
        /// Handles click of "Change Avatar" button and updates new avatar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            // get avatar picturebox
            var avatarPictureBox = this.Controls.Find("pictureBox" + AvatarPicBoxIndex.ToString(), true)[0] as PictureBox;

            // open file dialog   
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                avatarPictureBox.Image = new Bitmap(open.FileName);
                avatarPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TimerLabel_TextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Handles timer tick. Adjusts color to red when 10 seconds left. When timer is up then it is restarted and 
        /// hand is redealt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (TimerLabel.Text == "10 seconds remaining")
            {
                TimerLabel.ForeColor = System.Drawing.Color.Red;
            }
            if (TimerLabel.Text == "00 seconds remaining")
            {
                lblHandType.Text = "Times run out!\nYou've folded!";
                MyTimer.Stop();
                DateTime Tthen = DateTime.Now;

                startTime = DateTime.Now;
                MyTimer = new Timer();
                MyTimer.Tick += (s, ev) => { TimerLabel.Text = String.Format("{0:00} seconds remaining", 30 - (DateTime.Now - startTime).Seconds); };
            }
        }
    
        /// <summary>
        /// Updates labels indicating point values and blind selection when called.
        /// </summary>
        private void updateValues()
        {
            potValue.Text = scorekeeper.potValue.ToString();
            playerCurrency.Text = scorekeeper.playerCurrency.ToString();
            cpuCurrency.Text = scorekeeper.cpuCurrency.ToString();
            currentBet.Text = scorekeeper.currentBet.ToString();
            playerBlind.Text = scorekeeper.playerBlind;
            cpuBlind.Text = scorekeeper.cpuBlind;

        }

        /// <summary>
        /// Calls the callFunction and updates appropriate values when clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callButton_Click(object sender, EventArgs e)
        {
            scorekeeper.callFunction();
            updateValues();
            endBet();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        /// <summary>
        /// shows fields that allow player to choose an amount to raise when pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void raiseButton_Click(object sender, EventArgs e)
        {
            if (raiseInput.Visible == false)
            {
                raiseInput.Visible = true;
                enterRaise.Visible = true;
                raiseInputLabel.Visible = true;
            }
        }

            /// <summary>
            /// Stricts input to only numbers when text is changed
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void raiseInput_TextChanged(object sender, EventArgs e)
        {

            if (System.Text.RegularExpressions.Regex.IsMatch(raiseInput.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                raiseInput.Text = raiseInput.Text.Remove(raiseInput.Text.Length - 1);
            }
        }

        /// <summary>
        /// Calls the raiseFunction and updates appropriate values when button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enterRaise_Click(object sender, EventArgs e)
        {
            int raiseInt = Int32.Parse(raiseInput.Text);
            if (raiseInt > scorekeeper.playerCurrency) MessageBox.Show("The value entered is more than you have!\n Enter a lower value");
            else if (raiseInt <= scorekeeper.currentBet) MessageBox.Show("The amount you entered is lower than or equal to the current bet!\n Enter a higher value");
            else
            {
                scorekeeper.raiseFunction(raiseInt);
                updateValues();
                endBet();
            }
        }

        /// <summary>
        /// function to enable the call and raise button
        /// </summary>
        private void startBet()
        {
            callButton.Enabled = true;
            raiseButton.Enabled = true;
        }

        /// <summary>
        /// function to disable the raise and call button, as well as hide the raise input auxilliary fields
        /// </summary>
        private void endBet()
        {
            callButton.Enabled = false;
            raiseButton.Enabled = false;
            raiseInput.Visible = false;
            enterRaise.Visible = false;
            raiseInputLabel.Visible = false;
        }
    }
}
