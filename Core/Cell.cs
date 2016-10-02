using System;
using System.Linq;
using AvP.Joy;

namespace AvP.TicTacToe.Core
{
    public struct Cell : IComparable<Cell>, IEquatable<Cell>
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

        #region (Try)Parse, ToString

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

        public override string ToString()
            => Row.ToString() + ((int) Col).ToString();

        #endregion
        #region CompareTo, Equals, etc.

        public int CompareTo(Cell other)
        {
            var rowResult = Row.CompareTo(other.Row);
            return (rowResult != 0) ? rowResult
                : Col.CompareTo(other.Col);
        }

        public bool Equals(Cell other)
            => Row.Equals(other.Row) && Col.Equals(other.Col);

        public override bool Equals(object obj)
            => obj is Cell && Equals((Cell) obj);

        public override int GetHashCode()
            => Row.GetHashCode() ^ Col.GetHashCode();

        public static bool operator ==(Cell x, Cell y)
            => x.Equals(y);

        public static bool operator !=(Cell x, Cell y)
            => !(x == y);

        public static bool operator >(Cell x, Cell y)
            => x.CompareTo(y) > 0;

        public static bool operator <(Cell x, Cell y)
            => x.CompareTo(y) < 0;

        #endregion
    }
}