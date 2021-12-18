using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoLab5
{
    public enum BoardState
    {
        Playing,
        WinWhite,
        WinBlack,
        Draw
    }
    abstract class Fild {
    }

    class WhiteStone : Fild
    {
        public override string ToString()
        {
            return "W";
        }
    }
    class BlackStone : Fild
    {
        public override string ToString()
        {
            return "B";
        }
    }
    class EmptyField : Fild
    {
        public override string ToString()
        {
            return " ";
        }
    }



    class Board
    {
        public BoardState state { 
            get { return IdentifyState(); }
            private set { }
        }


        public Fild[,] board;

        public Board(int boardSize)
        {
            board = new Fild[boardSize, boardSize];
            var empty = new EmptyField();
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    board[i, j] = empty;
                }
            }
        }
        public Board(Board board)
        {
            Fild[,] newField = new Fild[board.boardSideLength, board.boardSideLength];
            for (int i = 0; i < board.boardSideLength; i++)
            {
                for (int k = 0; k < board.boardSideLength; k++)
                {
                    newField[i, k] = board.board[i,k];
                }
            }
            this.board = newField ;
        }

        public int DistanceToNearestFigure(int row, int column)
        {
            int distance = 1;
            for (; distance < boardSideLength; distance++)
            {
                for (int t = -1 * distance; t <= distance; t++)
                {
                    for (int k = -1 * distance; k <= distance; k++)
                    {
                        if (k == 0 && t == 0)
                        {
                            continue;
                        }

                        if (row + t < boardSideLength && row + t >= 0 && column + k < boardSideLength &&
                            column + k >= 0)
                        {
                            if (!(board[column + k, row + t] is EmptyField))
                            {
                                return distance;
                            }
                        }
                    }
                }
            }

            return boardSideLength;
        }

        public bool PutStone(int row, int column, Fild stone)
        {
            if (row > boardSideLength-1 || column > boardSideLength-1 || row <0 || column < 0)
            {
                throw new Exception();
                return false;
            }

            if (stone is EmptyField || stone is null)
            {
                throw new Exception();
                return false;
            }
            if (stone is WhiteStone)
            {
                board[column, row] = (WhiteStone)stone;
                return true;
            }
            if (stone is BlackStone)
            {
                board[column, row] = (BlackStone)stone;
                return true;
            }
            return false;
        }

        public int boardSideLength
        {
            get {
                return board.GetUpperBound(0) + 1;
            }
        }


        public BoardState IdentifyState()
        {
            bool drawChecker = true;
            for (int i = 0; i < boardSideLength; i++)
            {
                if (drawChecker == false )
                {
                    break;
                }
                for (int k = 0; k < boardSideLength; k++)
                {
                    if (board[i, k] is EmptyField || board[i, k] is null)
                    {
                        drawChecker = false;
                        break;
                    }
                }
            }

            if (drawChecker)
            {
                return BoardState.Draw;
            }

            // вертикаль 
            for (int i = 0; i < boardSideLength; i++)
            {
                int whiteStonesChain = 0;
                int blackStonesChain = 0;
                for (int j = 0; j < boardSideLength; j++)
                {
                    if (whiteStonesChain == Constants.FieldsInRowToWin )
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }

                    if (board[i, j] is WhiteStone)
                    {
                        whiteStonesChain++;
                        blackStonesChain = 0;
                    }

                    if (board[i, j] is BlackStone)
                    {
                        blackStonesChain++;
                        whiteStonesChain = 0;
                    }
                    if (board[i, j] is EmptyField)
                    {
                        whiteStonesChain = 0;
                        blackStonesChain = 0;
                    }
                }
            }


            // Горизонталь 
            for (int i = 0; i < boardSideLength; i++)
            {
                int whiteStonesChain = 0;
                int blackStonesChain = 0;
                for (int j = 0; j < boardSideLength; j++)
                {
                    if (whiteStonesChain == Constants.FieldsInRowToWin)
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }

                    if (board[j,i] is WhiteStone)
                    {
                        whiteStonesChain++;
                        blackStonesChain = 0;
                    }

                    if (board[j, i] is BlackStone)
                    {
                        blackStonesChain++;
                        whiteStonesChain = 0;
                    }
                    if (board[j, i] is EmptyField)
                    {
                        whiteStonesChain = 0;
                        blackStonesChain = 0;
                    }
                }
            }


            //Диагонали

                 // лево-низ
                for (int i = 0; i < boardSideLength-5; i++)
                {
                    int whiteStonesChain = 0;
                    int blackStonesChain = 0;
                    for (int j = 0, k = i; j < boardSideLength && k < boardSideLength; j++, k++)
                    {
                        if (board[j, k] is WhiteStone)
                        {
                            whiteStonesChain++;
                            blackStonesChain = 0;
                        }

                        if (board[j, k] is BlackStone)
                        {
                            blackStonesChain++;
                            whiteStonesChain = 0;
                        }
                        if (board[j, k] is EmptyField)
                        {
                            whiteStonesChain = 0;
                            blackStonesChain = 0;
                        }
                    if (whiteStonesChain == Constants.FieldsInRowToWin)
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }
                }
                }

            for (int i = boardSideLength-1; i >=0 ; i--)
            {
                int whiteStonesChain = 0;
                int blackStonesChain = 0;
                for (int j = 0, k = i; j < boardSideLength && k < boardSideLength; j++, k++)
                {
                    if (board[k,j] is WhiteStone)
                    {
                        whiteStonesChain++;
                        blackStonesChain = 0;
                    }

                    if (board[k,j] is BlackStone)
                    {
                        blackStonesChain++;
                        whiteStonesChain = 0;
                    }
                    if (board[k,j] is EmptyField)
                    {
                        whiteStonesChain = 0;
                        blackStonesChain = 0;
                    }
                    if (whiteStonesChain == Constants.FieldsInRowToWin)
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }
                }
            }



            for (int i = boardSideLength-1; i >= 0; i--)
            {
                int whiteStonesChain = 0;
                int blackStonesChain = 0;
                for (int j = 0, k = i; j < boardSideLength && k >= 0; j++, k--)
                {
           

                    if (board[j, k] is WhiteStone)
                    {
                        whiteStonesChain++;
                        blackStonesChain = 0;
                    }

                    if (board[j, k] is BlackStone)
                    {
                        blackStonesChain++;
                        whiteStonesChain = 0;
                    }
                    if (board[j, k] is EmptyField)
                    {
                        whiteStonesChain = 0;
                        blackStonesChain = 0;
                    }
                    if (whiteStonesChain == Constants.FieldsInRowToWin)
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }
                }
            }


            for (int i = boardSideLength - 1; i >= 0; i--)
            {
                int whiteStonesChain = 0;
                int blackStonesChain = 0;
                for (int j = boardSideLength - 1, k = i; k < boardSideLength && j >= 0; j--, k++)
                {


                    if (board[k,j] is WhiteStone)
                    {
                        whiteStonesChain++;
                        blackStonesChain = 0;
                    }

                    if (board[k,j] is BlackStone)
                    {
                        blackStonesChain++;
                        whiteStonesChain = 0;
                    }
                    if (board[k,j] is EmptyField)
                    {
                        whiteStonesChain = 0;
                        blackStonesChain = 0;
                    }
                    if (whiteStonesChain == Constants.FieldsInRowToWin)
                    {

                        state = BoardState.WinWhite;
                        return BoardState.WinWhite;
                    }
                    if (blackStonesChain == Constants.FieldsInRowToWin)
                    {
                        state = BoardState.WinBlack;
                        return BoardState.WinBlack;
                    }
                }
            }


            return BoardState.Playing;
        }
    }
}