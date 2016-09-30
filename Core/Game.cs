using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public sealed class Game
    {
        private const byte SIZE = 3;

        private readonly IEnumerable<Cell> rawMoveHistory;

        public Game(IEnumerable<Cell> rawMoveHistory = null)
        {
            this.rawMoveHistory = rawMoveHistory ?? Enumerable.Empty<Cell>();
            if (!rawMoveHistory.IsDistinct())
                throw new ArgumentException("Elements must be unique.", nameof(rawMoveHistory));
        }

        private PlayerId PlayerByMoveIndex(int moveIndex) => (PlayerId) (moveIndex % 2);

        public IEnumerable<Tuple<Cell, PlayerId>> MoveHistory
        {
            get
            {
                return rawMoveHistory.Select((o, i) => Tuple.Create(o, PlayerByMoveIndex(i)));
            }
        }

        public IDictionary<Cell, PlayerId?> Board
        {
            get
            {
                return new SparseDictionaryWrapper<Cell, PlayerId?>(
                    MoveHistory.Select(pair => Tuple.Create(pair.Item1, (PlayerId?) pair.Item2)).ToDictionary(), 
                    BoardHelper.AllCells);
            }
        }

        public bool TryFindWinner(out PlayerId result, out IEnumerable<Cell> winningCells)
        {
            throw new NotImplementedException("BUILD ME!");
        }

        public GameStatus Status {
            get {
                PlayerId winner;
                IEnumerable<Cell> winningCells;
                return TryFindWinner(out winner, out winningCells) ? new GameStatus.Won(winner, winningCells)
                    : Board.Values.Contains(null) ? new GameStatus.AwaitingMove(
                        PlayerByMoveIndex(rawMoveHistory.Count()))
                    : (GameStatus) new GameStatus.Drawn();
            }
        }

        public Game Play(Cell cell)
        {
            if (Status.IsComplete)
                throw new InvalidOperationException("The current game is complete.");

            if (rawMoveHistory.Contains(cell))
                throw new InvalidOperationException("The specified cell has already been played.");

            return new Game(rawMoveHistory.Concat(cell));
        }
    }

    public abstract class GameStatus
    {
        private GameStatus() { }
        public abstract bool IsComplete { get; }

        public sealed class AwaitingMove : GameStatus
        {
            public AwaitingMove(PlayerId nextPlayer) { NextPlayer = nextPlayer; }
            public override bool IsComplete { get { return false; } }
            public PlayerId NextPlayer { get; }
        }

        public sealed class Won : GameStatus
        {
            public Won(PlayerId winner, IEnumerable<Cell> winningCells) {
                Winner = winner;
                WinningCells = winningCells;
            }

            public override bool IsComplete { get { return true; } }
            public PlayerId Winner { get; }
            public IEnumerable<Cell> WinningCells { get; }
        }

        public sealed class Drawn : GameStatus
        {
            public override bool IsComplete { get { return true; } }
        }
    }

    public enum BoardRow { A, B, C }
    public enum BoardCol { One = 1, Two = 2, Three = 3 }

    public struct Cell : IComparable<Cell>
    {
        public BoardRow Row { get; }
        public BoardCol Col { get; }

        public Cell(BoardRow row, BoardCol col)
        {
            if (!row.IsDefined()) throw new ArgumentOutOfRangeException(nameof(row));
            if (!col.IsDefined()) throw new ArgumentOutOfRangeException(nameof(col));

            Row = row;
            Col = col;
        }

        public static readonly Cell A1 = new Cell(BoardRow.A, BoardCol.One);
        public static readonly Cell A2 = new Cell(BoardRow.A, BoardCol.Two);
        public static readonly Cell A3 = new Cell(BoardRow.A, BoardCol.Three);

        public static readonly Cell B1 = new Cell(BoardRow.B, BoardCol.One);
        public static readonly Cell B2 = new Cell(BoardRow.B, BoardCol.Two);
        public static readonly Cell B3 = new Cell(BoardRow.B, BoardCol.Three);

        public static readonly Cell C1 = new Cell(BoardRow.C, BoardCol.One);
        public static readonly Cell C2 = new Cell(BoardRow.C, BoardCol.Two);
        public static readonly Cell C3 = new Cell(BoardRow.C, BoardCol.Three);

        public static Cell Parse(string value)
        {
            Cell result;
            string explanation;
            if (TryParse(value, out result, out explanation))
                return result;
            else
                throw new ArgumentException(explanation);
        }

        public static bool TryParse(string value, out Cell result)
        {
            string explanation;
            return TryParse(value, out result, out explanation);
        }

        private static bool TryParse(string value, out Cell result, out string explanation)
        {
            // Discard whitespace & punctuation.
            var letters = value.Where(char.IsLetter).ToList();
            var digits = value.Where(char.IsDigit).ToList();

            BoardRow row;
            BoardCol col;
            if (letters.Count == 1 && digits.Count == 1 
                && TryParseRow(letters[0], out row) 
                && TryCastCol(digits[0].ToDigit(), out col))
            {
                result = new Cell(row, col);
                explanation = null;
                return true;
            }

            result = A1;
            explanation = "The specified value does not seem to identify a cell.";
            return false;
        }

        private static bool TryParseRow(char value, out BoardRow result)
            => Enum.TryParse(value.ToString().ToUpperInvariant(), out result);

        private static bool TryCastCol(int value, out BoardCol result)
        {
            result = (BoardCol) value;
            return result.IsDefined();
        }
        
        public int CompareTo(Cell other)
        {
            var rowResult = Row.CompareTo(other.Row);
            return (rowResult != 0) ? rowResult
                : Col.CompareTo(other.Col);
        }

        public override string ToString()
            => Row.ToString() + ((int) Col).ToString();
    }

    public enum PlayerId { X = 0, Y = 1 }

    public static class BoardHelper
    {
        public static IReadOnlyList<BoardRow> AllRows { get; } 
            = Enum.GetValues(typeof(BoardRow)).Cast<BoardRow>().ToList();

        public static IReadOnlyList<BoardCol> AllCols { get; } 
            = Enum.GetValues(typeof(BoardCol)).Cast<BoardCol>().ToList();

        public static IReadOnlyList<Cell> AllCells { get; }
            = AllRows.SelectMany(row => AllCols.Select(col => new Cell(row, col))).ToList();

        public static SortedDictionary<Cell, PlayerId?> NewBoard()
            => AllCells.Select(c => Tuple.Create(c, default(PlayerId?))).ToSortedDictionary();
    }
}