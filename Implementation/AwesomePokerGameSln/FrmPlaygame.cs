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
    }

    private void FrmPlaygame_FormClosed(object sender, FormClosedEventArgs e) {
      foreach (Form f in Application.OpenForms)
        f.Close();
    }

    private void FrmPlaygame_Load(object sender, EventArgs e) {
      scorekeeper = new Scorekeeper();
      // This is a temporary function to emulate the cpu making the first bet
      scorekeeper.updateBet();
      scorekeeper.startUpdatePot();
      updateValues();
      deck = new Deck();
      dealCards();
    }

    private void button1_Click(object sender, EventArgs e) {
      dealCards();
      scorekeeper.updateBet();
      scorekeeper.startUpdatePot();
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
    
        private void updateValues()
        {
            potValue.Text = scorekeeper.potValue.ToString();
            playerCurrency.Text = scorekeeper.playerCurrency.ToString();
            cpuCurrency.Text = scorekeeper.cpuCurrency.ToString();
            currentBet.Text = scorekeeper.currentBet.ToString();
            playerBlind.Text = scorekeeper.playerBlind;
            cpuBlind.Text = scorekeeper.cpuBlind;

        }

        private void callButton_Click(object sender, EventArgs e)
        {
            scorekeeper.callFunction();
            updateValues();
        }
    }
}
