using System;

namespace TowersOfHanoi
{
    class GamePole
    /*
     *  Made by Krzysztof Grzeslak 21.12.2016
     *  Game Object that represents the single Disc Pole
     */

    {
        private int maxDiscs;
        private int[] discStack;
        private int curPosition;

        // returns value of the top disc on the stack
        public string TopDisc()
        {
            return Disc(curPosition);
        }

        // returns disc number at given position or empty position sign
        public string Disc (int position)
        {
            if(curPosition >= 0 && position <= curPosition)
            {
                return discStack[position].ToString();
            }
            else
            {
                return "|";
            }
        }

        // Returns the top disc from the stack. Returns -1, if failed
        public int PopDisc ()
        {
            int tempdisc = -1;

            // Ensure stack is not empty
            if(curPosition >= 0)
            {
                // pop the disc from the stack
                tempdisc = discStack[curPosition];

                // Update current stack position
                curPosition--;
            }
            else
            {
                // do nothing
            }

            return tempdisc;
        }
        
        // Places new disc on the stack. Returns true, if success
        public bool PushDisc(int disc)
        {
            bool tempVal = false;

            // Ensure stack is not full and current disk is positive and is bigger than pushed one
            if (curPosition < maxDiscs && disc > 0)
            {
                if(curPosition == -1 || (curPosition >= 0 && disc < discStack[curPosition]))
                {
                    // Update current stack position
                    curPosition++;

                    // Place disc on top
                    discStack[curPosition] = disc;

                    tempVal = true;
                }
            }

            return tempVal;
        }
        
        public GamePole(int discQtty, bool isStartPole)
        {
            // Prepare the pole for the disc quantity in the game
            maxDiscs = discQtty;
            Array.Resize<int>(ref discStack, maxDiscs);

            // Populate stack with starting values
            for (int i = 0; i < maxDiscs; i++)
            {
                if(isStartPole)
                {
                    discStack[i] = maxDiscs - i;
                }
                else
                {
                    discStack[i] = 0;
                }
            }

            // Set initial stack position
            if (isStartPole)
            {
                curPosition = maxDiscs -1;     // Value equals Array size -1 means full stack
            }
            else
            {
                curPosition = -1;            // Value -1 means empty stack
            }
        }

    }
}