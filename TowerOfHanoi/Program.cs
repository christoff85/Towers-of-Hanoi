using System;
using System.Text;

/*
 * Made by Krzysztof Grzeslak 21.12.2016
 * I made this classic puzzle game as a final project based on the course Effective Thinking Through Mathematics (by Professor Michael Starbird on edX platform).
 */

namespace TowersOfHanoi
{

    class Program
    {
        
        
        static void Main(string[] args)
        {

            // Big loop to ask player, whether he want to play another session
            string playAgain = String.Empty;
            do
            {
                // remove all the previous data from the console
                Console.Clear();

                greetPlayer();

                int discs = 0;
                do
                {
                    Console.Write("Please choose the number of discs: ");
                    int.TryParse(Console.ReadLine(), out discs);
                }
                while (!(discs > 0) || !(discs < 10));

                // Start new game
                if (args.Length == 1 && (args[0] == "-megahit" || args[0] == "-makinit")) // do  You recognize these passwords? :)
                {
                    TowersGame gameSession = new TowersGame(discs, true);
                }
                else
                {
                    TowersGame gameSession = new TowersGame(discs);
                }

                Console.WriteLine();
                do
                {
                    Console.Write("Do You want to play again? [y / n]: ");
                    playAgain = Console.ReadLine();
                }
                while (playAgain.ToUpper() != "Y" && playAgain.ToUpper() != "N");
            }
            while (playAgain.ToUpper() == "Y");
        }

        private static void greetPlayer()
        {
            StringBuilder sb = new StringBuilder();

            // Greet the player
            sb.AppendLine();
            sb.AppendLine("Welcome to the Towers of Hanoi Game!");
            sb.AppendLine();

            // Show instructions
            sb.AppendLine("The game consists of three poles: left - A, middle - B and right - C.");
            sb.AppendLine("The goal of the game is to move all discs from the left pole to the right pole.");
            sb.AppendLine("You can only move one disc at a time.");
            sb.AppendLine("You cannot place bigger disc on top of the smaller one.");
            sb.AppendLine();
            sb.AppendLine("You can quit the game anytime during the session by typing Q");
            sb.AppendLine();

            // Ask for number of disc
            sb.AppendLine("Minimum number of discs is 1 and maximum is 9");
            Console.Write(sb.ToString());
        }
    }
}
