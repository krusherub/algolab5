using System;
using System.Collections.Generic;

namespace AlgoLab5
{
    public enum ColorPlaying
    {
        White,
        Black
    }
    class Game
    {
        public Board currentBoard;
        public ColorPlaying turn;
        public int turns;
        public ColorPlaying userColor;
        public void Start(ColorPlaying userColor, int boardSize)
        {
            currentBoard = GenerateInitialBoard(boardSize);
            turns = 0;
            turn = ColorPlaying.White;
            this.userColor = userColor;
            BotPlayer bot = null;

            PrintBoard();

            if (userColor is ColorPlaying.White)
            {
                UserTurn();
                bot = new BotPlayer(ColorPlaying.Black);
            }

            if (userColor is ColorPlaying.Black)
            {
                bot = new BotPlayer(ColorPlaying.White);
            }

            while (currentBoard.state == BoardState.Playing)
            {
                BotTurn(bot);
                PrintBoard();
                if (CheckIfGameFinished())
                {
                    break;
                }
                UserTurn();
                PrintBoard();
                if (CheckIfGameFinished())
                {
                    break;
                }
            }
            Console.WriteLine("game finished. " + currentBoard.state.ToString());
        }

        public bool CheckIfGameFinished()
        {
            if (currentBoard.state !=BoardState.Playing)
            {
                return true;
            }
            return false;
        }
        public void BotTurn (BotPlayer bot)
        {
            Console.WriteLine("Computer turn:");
            var turnInfo = bot.CalculateTurn(currentBoard, turns, lastTurn);
            MakeTurn(turnInfo.column, turnInfo.row, turnInfo.stone);
        }

        public void UserTurn()
        {
            var userInput = AskForTurn();
            Fild stone;
            if (userColor is ColorPlaying.White)
            {
                stone = new WhiteStone();
            }
            else
            {
                stone = new BlackStone();
            }

            MakeTurn(userInput.column, userInput.row, stone);
        }

        public Turn lastTurn;
        public void MakeTurn(int column, int row, Fild fild)
        {
            turns++;
            currentBoard.PutStone(row, column, fild);
            lastTurn = new Turn();
            lastTurn.Column = column;
            lastTurn.Row = row;
            if (fild is WhiteStone)
            {
                lastTurn.color = ColorPlaying.White;
            }
            else
            {
                lastTurn.color = ColorPlaying.Black;
            }
        }

        public (int column, int row) AskForTurn()
        {
            Console.WriteLine("It`s your turn, enter coordinates:");
            Console.Write("Row:");
            var row = Console.ReadLine();
            Console.WriteLine("Column:");
            var column = Console.ReadLine();
            int y;
            int x;
            int.TryParse(row,out y);
            int.TryParse(column, out x);
            return (y, x);
        }
        public void PrintBoard()
        {
            Console.Write('*');
            for (int i = 0; i < currentBoard.boardSideLength; i++)
            {
                Console.Write(i);
            }
            Console.WriteLine();
            for (int i = 0; i < currentBoard.boardSideLength; i++)
            {
                Console.Write(i);
                for (int j = 0; j < currentBoard.boardSideLength; j++)
                {
                    Console.Write(currentBoard.board[i ,j].ToString());
                }
                Console.WriteLine();
            }
        }
        
        public Board GenerateInitialBoard(int boardSize)
        {
            var board = new Board(boardSize);
            int middleIndex =(new Random(23)).Next(0,boardSize-1);
            board.PutStone(middleIndex,middleIndex,new BlackStone());
            return board;
        }
    }
}