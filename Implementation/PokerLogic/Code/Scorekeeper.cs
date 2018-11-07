using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomePokerGameSln.Code
{
    /// <summary>
    /// Class to keep track of and update various game metrics
    /// </summary>
    public class Scorekeeper
    {
        public int currentBet;
        public string blindTurn;
        public int playerCurrency;
        public int cpuCurrency;
        public int potValue;
        public int bigBlind;
        public int smallBlind;
        public string playerBlind;
        public string cpuBlind;
        private Random rnd = new Random();

      
        /// <summary>
        /// Constructor -> Sets default values for current implementation of game
        /// </summary>
        public Scorekeeper() {
            currentBet = 0;
            blindTurn = null;
            playerCurrency = 2000;
            cpuCurrency = 2000;
            potValue = 0;
            bigBlind = 100;
            smallBlind = 50;
        }


        /// <summary>
        /// Updates the pot and currencies at the beginning of the game to account for blinds
        /// </summary>
        public void startUpdatePot()
        {
            potValue = bigBlind + smallBlind;
            updateBlindTurn();
            if (blindTurn == "cpu")
            {
                cpuCurrency -= bigBlind;
                playerCurrency -= smallBlind;
            }
            if (blindTurn == "player")
            {
                playerCurrency -= bigBlind;
                cpuCurrency -= smallBlind;
            }
        }

        /// <summary>
        /// Updates the pot and currencies to account for bets
        /// </summary>
        public void endUpdatePot()
        {
            potValue = currentBet * 2;
            playerCurrency -= currentBet;
            cpuCurrency -= currentBet;
        }


        /// <summary>
        /// Updates whose turn it is to be the Big blind, the other player gets the small blind
        /// </summary>
        public void updateBlindTurn()
        {
            if (blindTurn == null)
            {
                int randInt = rnd.Next(0, 99);
                if (randInt < 50)
                {
                    blindTurn = "cpu";
                    cpuBlind = "Big Blind";
                    playerBlind = "Small Blind";
                }
            }
            else if (blindTurn == "cpu")
            {
                blindTurn = "player";
                playerBlind = "Big Blind";
                cpuBlind = "Small Blind";

            }
            else if (blindTurn == "player")
            {
                blindTurn = "cpu";
                cpuBlind = "Big Blind";
                playerBlind = "Small Blind";
            }

        }


        /// <summary>
        /// Placeholder function emulates the cpu betting first to test functionality of call and bet functions
        /// </summary>
        public void updateBet()
        {
            currentBet = 50;
            cpuCurrency -= 50;
            potValue += 50;
        }

        /// <summary>
        /// Function that is called to adjust point values when a player uses the call button
        /// </summary>
        public void callFunction()
        {
            potValue += currentBet;
            playerCurrency -= currentBet;
            currentBet = 0;
        }

        /// <summary>
        /// function that is called to adjust point values when a player uses the raise button
        /// </summary>
        /// <param name="raiseAmount"></param>
        public void raiseFunction(int raiseAmount)
        {
            playerCurrency -= raiseAmount;
            potValue += raiseAmount;
            currentBet = raiseAmount;
        }

    }
}
