using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace AlgoLab5
{
    class  Turn
    {

        public int Row { get; set; }
        public int Column { get; set; }

        public ColorPlaying color;

        public Turn(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public Turn(int row, int column, Fild fild)
        {
            Row = row;
            Column = column;
            if (fild is WhiteStone)
            {
                color = ColorPlaying.White;
            }
            else
            {
               color = ColorPlaying.Black;
            }
        }


        public Turn()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Turn turn &&
                   Row == turn.Row &&
                   Column == turn.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
    class BoardNode
    {
        public Turn lastTurn;
        public Board currentBoard;
        public int depth;
        public List<BoardNode> childs;
        public BoardNode parent;
        public int localMinToWin = int.MaxValue;
        public BoardNode bestAnswer;
    }
    class BotPlayer
    {
        public ColorPlaying color;
        public BotPlayer(ColorPlaying color)
        {
            this.color = color;
        }
        public Fild Stone { 
            get {
                if (color is ColorPlaying.White)
                {
                    return new WhiteStone();
                }
            
                return new BlackStone();
                
            }
        }
        public (int column, int row) GeneratePosition(Board board)
        {
            Random random = new Random(23);
            var row = random.Next(board.boardSideLength);
            var column = random.Next(board.boardSideLength);
            while (!(board.board[column,row] is EmptyField && board.DistanceToNearestFigure(row,column) <= 1))
            {
                row = random.Next(board.boardSideLength);
                column = random.Next(board.boardSideLength);
            }
            return (column, row);
        }
        public (int column, int row, Fild stone) CalculateTurn(Board board, int turns, Turn lastTurn)
        {
            int column = 0;
            int row = 0;
            if (turns < 3)
            {
                var position = GeneratePosition(board);
                return (position.column, position.row, Stone);
            }


            GenerateGameTree(board, lastTurn);

            if (draw)
            {
                column = bestAnswer.lastTurn.Column;
                row = bestAnswer.lastTurn.Row;
                return (column, row, Stone);
            }
            
            var nextNodeStep = bestAnswer;
            var nextNodeStepParent = bestAnswer.parent;
            
            if (lossAnswer is not null && lossAnswer.depth == 2*Constants.TurnsToBlock && bestAnswer.depth > 1*Constants.TurnsToBlock)
            { 
                  bestAnswer = lossAnswer;
                 nextNodeStep = bestAnswer;
                 nextNodeStepParent = bestAnswer.parent.parent;
            }
            while (nextNodeStepParent.parent != null)
            {
                nextNodeStep = nextNodeStep.parent;
                nextNodeStepParent = nextNodeStepParent.parent;
            }
            column = nextNodeStep.lastTurn.Column;
            row = nextNodeStep.lastTurn.Row;
            bestAnswer = null;
            lossAnswer = null;
            localMinToWin = int.MaxValue;
             return (column, row, Stone);
        }

        public bool draw;
        public BoardNode GenerateRandomPostion(BoardNode node)
        {
            draw = true;
            Fild stone;
            if (node.lastTurn.color is ColorPlaying.White)
            {
                stone = new BlackStone();
            }
            else
            {
                stone = new WhiteStone();
            }
            for (int i = 0; i < node.currentBoard.boardSideLength ; i++)
            {
                for (int j = 0; j < node.currentBoard.boardSideLength ; j++)
                {
                    if (node.currentBoard.board[i,j] is EmptyField)
                    {
                        var newBoard = new Board(node.currentBoard);
                        newBoard.PutStone(j, i, stone);
                        var newBoardNode = new BoardNode();
                        newBoardNode.lastTurn = new Turn(j, i, stone);
                        newBoardNode.currentBoard = newBoard;
                        return newBoardNode;
                    }
                }
            }

            return null;
        }
        public void GenerateGameTree(Board board, Turn lastTurn)
        {
            var boardInitialNode = new BoardNode();
            boardInitialNode.currentBoard = board;
            boardInitialNode.lastTurn = lastTurn;
            boardInitialNode.depth = 0;
            bestAnswer = GenerateRandomPostion(boardInitialNode); // if draw
            boardInitialNode.childs = GenerateAllPossibleNextVariants(boardInitialNode);
        }

        public int localMinToWin = int.MaxValue;
        public BoardNode bestAnswer;
        public BoardNode lossAnswer;

        public List<BoardNode> GenerateAllPossibleNextVariants(BoardNode boardNode)
        {
            if (boardNode.depth >= localMinToWin)
            {
                return null;
            }
            if (boardNode.depth >= Constants.FieldsInRowToWin*2)
            {
                return null;
            }
            

            if (boardNode.currentBoard.state != BoardState.Playing)
            {
                //   Console.WriteLine(boardNode.currentBoard.state.ToString() + boardNode.depth);
                
                
                if (boardNode.currentBoard.state is BoardState.WinBlack)
                {
                    var bestDepth = boardNode.depth;
                    
                    localMinToWin = bestDepth;
                    bestAnswer = boardNode;
                    draw = false;
                    return null;
                }
   
                
                if (boardNode.currentBoard.state is BoardState.WinWhite)
                {
                    draw = false;
                    if (lossAnswer is not null)
                    {
                        if (boardNode.depth < lossAnswer.depth)
                        {
                            lossAnswer = boardNode;
                        }
                        
                    }
                    else
                    {
                        lossAnswer = boardNode;
                    }
                }
                return null;
            }

            List<BoardNode> childs = new List<BoardNode>();
            Fild stone;
            if (boardNode.lastTurn.color is ColorPlaying.White)
            {
                stone = new BlackStone();
            }
            else
            {
                stone = new WhiteStone();
            }

            for (int i = 0; i < boardNode.currentBoard.boardSideLength; i++)
            {
                for (int j = 0; j < boardNode.currentBoard.boardSideLength; j++)
                {
                    if (boardNode.currentBoard.board[i, j] is EmptyField &&
                        boardNode.currentBoard.DistanceToNearestFigure(j, i) <= Constants.FieldsAroundStone)
                    {
                        var newBoard = new Board(boardNode.currentBoard);
                        newBoard.PutStone(j, i, stone);
                        var newBoardNode = new BoardNode();
                        newBoardNode.lastTurn = new Turn(j, i, stone);
                        newBoardNode.currentBoard = newBoard;
                        newBoardNode.parent = boardNode;
                        newBoardNode.depth = boardNode.depth + 1;
                        newBoardNode.childs = GenerateAllPossibleNextVariants(newBoardNode);
                    }
                }
            }

            return childs;
        }
        
    }
}