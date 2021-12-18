using System;
using System.Collections.Generic;

namespace AlgoLab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            
            
            Constants.FieldsInRowToWin = 4;
            
            Constants.FieldsAroundStone = 1;
            
            Constants.TurnsToBlock = 1;

            game.Start(ColorPlaying.White, 7);
        }
    }
}