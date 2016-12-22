using System;
using System.Text;
using System.Threading;

namespace TowersOfHanoi
{
    /*
     * Made by Krzysztof Grzeslak 21.12.2016
     * Game Object that represents the complete session of the Towers game
     */
    class TowersGame
    {

        enum moveType { source, destination }

        // Three poles of the game, discs starts on the left one, should finish on the middle one
        private GamePole Left
        {
            get; set;
        }

        private GamePole Middle
        {
            get; set;
        }

        private GamePole Right
        {
            get; set;
        }

        private GamePole Current
        {
            get; set;
        }

        private GamePole[] All
        {
            get; set;
        }

        // Placeholder for number of discs in the session
        private int DiscQtty
        {
            get; set;
        }

        // Player moves counter
        private int MoveCounter
        {
            get; set;
        }

        // Placeholder for current disc taken in the round
        private int CurrentDisc
        {
            get; set;
        }

        // Waits for player to abandon the game session
        private bool AbandonSession
        {
            get; set;
        }

        // Factory method for GamePole Objects
        private GamePole newPole(int discQtty, bool isStartPole = false)
        {
            GamePole tempPole = new GamePole(discQtty, isStartPole);
            return tempPole;
        }

        // Starts new session of the game
        public TowersGame(int discs, bool isGodMode = false)
        {
            DiscQtty = discs;
            MoveCounter = 0;
            CurrentDisc = 0;    // 0 means no disc currently taken
            AbandonSession = false;

            // initialize the game poles
            Left = newPole(DiscQtty, true);
            Middle = newPole(DiscQtty);
            Right = newPole(DiscQtty);

            // create collection of all game poles
            GamePole[] tempAll = { Left, Middle, Right };
            All = tempAll;

            if (isGodMode)
            {
                newGodMode(DiscQtty);
            }
            else
            {
                // play new round until the game is won
                do
                {
                    newRound();
                }
                while (!AbandonSession && !isGameWon());
            }

            // Ensure game has been won
            if(!AbandonSession)
            {
                gameWon();
            }
        }

        // Represents each round of the game
        private void newRound()
        {
            printGameState();
            string source = String.Empty;
            do
            {
                source = getNextMove(moveType.source);
            }
            while(!AbandonSession && !takeDisc(source));

            // Check if game session was abandoned
            if (AbandonSession)
            {
                return;
            }

            printGameState();
            string destination = String.Empty;
            do
            {
                destination = getNextMove(moveType.destination);
            }
            while (!AbandonSession &&  !placeDisc(destination));
            
            // Check if game session was abandoned
            if (AbandonSession)
            {
                return;
            }

            if (source != destination)
            {
                MoveCounter++;
            }
        }

        // End the current game session
        private void gameWon()
        {
            printGameState();
            Console.WriteLine("Game Complete! You have made {0} moves", MoveCounter);
            Console.WriteLine("Minimum moves required for {0} discs is {1}", DiscQtty, Math.Pow(2, DiscQtty) - 1);
        }

        // Returns true, if player wins the games
        private bool isGameWon()
        {
            int firstDisc = 0;
            int secondDisc = 0;
            for (int i = 0; i < DiscQtty; i++)
            {
                if(!int.TryParse(Right.Disc(i), out firstDisc) && !int.TryParse(Right.Disc(i + 1), out secondDisc)
                    && !(firstDisc > secondDisc))
                {
                    return false;
                }
            }

            return true;
        }

        // Graphical representation of the game round
        private void printGameState()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("Moves made: {0}", MoveCounter));
            sb.AppendLine();

            // Show current disc taken above its source pole
            printTakenDisc(ref sb);

            // Create new line for each disc in the game
            for (int i = DiscQtty - 1; i >= 0; i--)
            {
                //  add current position for each game pole
                foreach (GamePole pole in All)
                {
                    printDisc(pole, i, ref sb);
                }

                // start new line
                sb.AppendLine();
            }

            // clear previous game state
            Console.Clear();

            // Print the whole game state on the console
            Console.WriteLine(sb.ToString());
        }

        // represents the disc currently taken from the pole
        private void printTakenDisc(ref StringBuilder sb)
        {
            sb.AppendLine();
            foreach (GamePole pole in All)
            {
                int poleDistance = (pole == Current) ? getPoleDistance(CurrentDisc) : getPoleDistance(0);

                sb.Append(' ', poleDistance);

                if (pole == Current)
                {
                    sb.Append('_', getDiscSize(CurrentDisc));
                }
                else
                {
                    // compensate empty pole character
                    sb.Append(' ');
                }

                sb.Append(' ', poleDistance);
            }

            sb.AppendLine();
            sb.AppendLine();
        }
        
        // Determine the pole distance considering the current disc size
        private int getPoleDistance(int discNumber)
        {
            int poleDistance = 3 + DiscQtty - discNumber;
            return poleDistance;
        }

        // Determine graphical size of the disc
        private int getDiscSize(int discNumber)
        {
            int discSize = discNumber * 2 + 1;
            return discSize;
        }

        // Graphical representation of the disc on the pole
        private void printDisc(GamePole pole, int position, ref StringBuilder sb)
        {
            string disc = pole.Disc(position);

            // Determine the current disc; '0' means no disc at position
            int discNumber = 0;
            int.TryParse(disc, out discNumber);

            // Determine the pole distance for current disc size
            int poleDistance = getPoleDistance(discNumber);

            // Center all discs at the pole
            sb.Append(' ', poleDistance);

            if (discNumber > 0)
            {
                // disc representation
                sb.Append('_', getDiscSize(discNumber));
            }
            else
            {
                // empty pole representation
                sb.Append('|');
            }

            // Center all discs at the pole
            sb.Append(' ', poleDistance);
        }

        // ask player for his next move
        private string getNextMove(moveType mt)
        {
            // determine move type
            string move;
            switch (mt)
            {
                case moveType.destination:
                    move = "destination";
                    break;

                default:
                case moveType.source:
                    move = "source";
                    break;
            }

            // ask user and ensure players input is correct
            string nextMove = "";
            do
            {
                Console.Write("Please choose {0} Pole: A, B or C: ", move);
                nextMove = Console.ReadLine();
                
            }
            while (!isInputValid(nextMove));

            // Player wants to abandon the game session
            if (nextMove.ToUpper() == "Q")
            {
                AbandonSession = true;
            }
            return nextMove;
        }

        // Ensure the player input is correct
        private bool isInputValid(string input)
        {
            bool tempVal = false;

            // accept lower and upper case
            input = input.ToUpper();

            // Only letters A, B or C are accepted
            if (input == "A" || input == "B" || input == "C" || input == "Q")
            {
                tempVal = true;
            }

            return tempVal;
        }

        // Get the pole where the move will be made
        private GamePole getPole(string move)
        {
            GamePole tempPole = null;

            // accept lower and upper case
            move = move.ToUpper();

            switch (move)
            {
                case "A":
                    tempPole = Left;
                    break;
                case "B":
                    tempPole = Middle;
                    break;
                case "C":
                    tempPole = Right;
                    break;
            }

            return tempPole;
        }

        // Remove disc from the source pole
        private bool takeDisc(string srcMove)
        {
            GamePole srcPole = getPole(srcMove);
            if (srcPole.TopDisc().Equals("|"))
            {
                return false;
            }
            else
            {
                CurrentDisc = srcPole.PopDisc();
                Current = srcPole;
                return true;
            }

        }

        // Place the disk on the destination pole
        private bool placeDisc(string destMove)
        {
            GamePole destPole = getPole(destMove);

            // if push succeed, then clear current disc and pole
            if (destPole.PushDisc(CurrentDisc))
            {
                Current = null;
                CurrentDisc = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        // mode, where the game play itself
        private void newGodMode(int discQtty)
        {
            moveTower(discQtty, "A", "C", "B");
        }

        // recursive method for the solving the towers
        private void moveTower(int discQtty, string srcPole, string destPole, string sparePole)
        {
            if (discQtty >= 1)
            {
                moveTower(discQtty - 1, srcPole, sparePole, destPole);

                Thread.Sleep(250);
                printGameState();
                takeDisc(srcPole);

                Thread.Sleep(250);
                printGameState();
                placeDisc(destPole);
                MoveCounter++;

                moveTower(discQtty - 1, sparePole, destPole, srcPole);
            }
        }
    }
    
}