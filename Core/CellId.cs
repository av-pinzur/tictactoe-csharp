using System;
using System.Linq;
using AvP.Joy;

namespace AvP.TicTacToe.Core
{
    public struct CellId : IComparable<CellId>, IEquatable<CellId>
    {
        public RowId Row { get; }
        public ColumnId Col { get; }

        public CellId(RowId row, ColumnId col)
        {
            if (!row.IsDefined()) throw new ArgumentOutOfRangeException(nameof(row));
            if (!col.IsDefined()) throw new ArgumentOutOfRangeException(nameof(col));

            Row = row;
            Col = col;
        }

        #region (Try)Parse, ToString

        public static CellId Parse(string value)
        {
            CellId result;
            string explanation;
            if (TryParse(value, out result, out explanation))
                return result;
            else
                throw new ArgumentException(explanation);
        }

        public static bool TryParse(string value, out CellId result)
        {
            string explanation;
            return TryParse(value, out result, out explanation);
        }

        private static bool TryParse(string value, out CellId result, out string explanation)
        {
            // Discard whitespace & punctuation.
            var letters = value.Where(char.IsLetter).ToList();
            var digits = value.Where(char.IsDigit).ToList();

            RowId row;
            ColumnId col;
            if (letters.Count == 1 && digits.Count == 1 
                && TryParseRow(letters[0], out row) 
                && TryCastCol(digits[0].ToDigit(), out col))
            {
                result = new CellId(row, col);
                explanation = null;
                return true;
            }

            result = default(CellId);
            explanation = "The specified value does not seem to identify a cell.";
            return false;
        }

        private static bool TryParseRow(char value, out RowId result)
            => Enum.TryParse(value.ToString().ToUpperInvariant(), out result);

        private static bool TryCastCol(int value, out ColumnId result)
        {
            result = (ColumnId) value;
            return result.IsDefined();
        }

        public override string ToString()
            => Row.ToString() + ((int) Col).ToString();

        #endregion
        #region CompareTo, Equals, etc.

        public int CompareTo(CellId other)
        {
            var rowResult = Row.CompareTo(other.Row);
            return (rowResult != 0) ? rowResult
                : Col.CompareTo(other.Col);
        }

        public bool Equals(CellId other)
            => Row.Equals(other.Row) && Col.Equals(other.Col);

        public override bool Equals(object obj)
            => obj is CellId && Equals((CellId) obj);

        public override int GetHashCode()
            => unchecked (Row.GetHashCode()
                    * 397 ^ Col.GetHashCode());

        public static bool operator ==(CellId x, CellId y)
            => x.Equals(y);

        public static bool operator !=(CellId x, CellId y)
            => !(x == y);

        public static bool operator >(CellId x, CellId y)
            => x.CompareTo(y) > 0;

        public static bool operator <(CellId x, CellId y)
            => x.CompareTo(y) < 0;

        #endregion
    }
}