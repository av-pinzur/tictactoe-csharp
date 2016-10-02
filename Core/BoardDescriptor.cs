using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public static class BoardDescriptor
    {
        public static IEnumerable<RowId> RowIds { get; } 
            = Enum.GetValues(typeof(RowId)).Cast<RowId>();

        public static IEnumerable<ColumnId> ColumnIds { get; } 
            = Enum.GetValues(typeof(ColumnId)).Cast<ColumnId>();

        public static IEnumerable<IEnumerable<CellId>> CellIds { get; }
            = RowIds.Select(r => ColumnIds.Select(c => new CellId(r, c)));

        #region With/WithoutCellIds

        public static IEnumerable<IEnumerable<Tuple<CellId, PlayerId?>>> WithCellIds(
            this IEnumerable<IEnumerable<PlayerId?>> source)
            => CellIds.Zip(source, (cellRow, playerRow)
                => cellRow.Zip(playerRow, Tuple.Create));

        public static IEnumerable<IEnumerable<PlayerId?>> WithoutCellIds(
            this IEnumerable<IEnumerable<Tuple<CellId, PlayerId?>>> source)
            => source.Select(row => row.Select(cell => cell.Item2));

        #endregion
    }
}