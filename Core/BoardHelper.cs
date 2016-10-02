using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
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

        #region With/WithoutCellIds

        public static IEnumerable<IEnumerable<Tuple<Cell, PlayerId?>>> WithCellIds(
            this IEnumerable<IEnumerable<PlayerId?>> source)
            => source.Zip(BoardHelper.AllRows, (row, rowId)
                => row.Zip(BoardHelper.AllCols, (player, colId)
                    => Tuple.Create(new Cell(rowId, colId), player)));

        public static IEnumerable<IEnumerable<PlayerId?>> WithoutCellIds(
            this IEnumerable<IEnumerable<Tuple<Cell, PlayerId?>>> source)
            => source.Select(row => row.Select(cell => cell.Item2));

        #endregion
    }
}