using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;
using AvP.TicTacToe.Core;

namespace AvP.TicTacToe.UI.Console
{
    public static class GameRenderer
    {
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string NewLineX2 = NewLine + NewLine;

        public static string Render(this Game game)
        {
            var boardLines = RenderBoard(game.Board).Lines();
            var boardFiller = new string(' ', boardLines.First().Length);

            var historyLines = RenderMoveHistory(game.MoveHistory).Lines();
            
            return boardLines.ZipAll(historyLines, (bLine, hLine) 
                    => bLine.ValueOrDefault(boardFiller)
                        + "   "
                        + hLine.ValueOrDefault(string.Empty))
                    .Join(NewLine)
                + NewLineX2 + RenderStatus(game.Status);
        }

        private static string RenderMoveHistory(IEnumerable<Tuple<CellId, PlayerId>> moveHistory)
            => string.Join(NewLine,
                moveHistory.Select(RenderMove));

        private static string RenderMove(Tuple<CellId, PlayerId> move)
            => move.Item2 + " played at " + move.Item1 + ".";

        private static string RenderBoard(IReadOnlyList<IReadOnlyList<PlayerId?>> board)
            => string.Join(NewLine,
                RenderBoardHeader(board.First().Count).FollowedBy(
                    board.Select((o, i) => RenderBoardRow(o, i))
                        .Interpose(RenderBoardRowSeparator(board.First().Count))));

        private static string RenderBoardHeader(int columnCount)
            => "   " 
                + string.Join(" ", 
                    BoardDescriptor.ColumnIds.Select(id => $" {id.GetDescription()} ")) 
                + " ";

        private static string RenderBoardRow(IEnumerable<PlayerId?> row, int offset)
            => $" {BoardDescriptor.RowIds.Nth(offset)} " 
                + string.Join("|", 
                    row.Select(id => id == null ? "   " : $" {id} ")) 
                + " ";

        private static string RenderBoardRowSeparator(int columnCount)
            => "   " 
                + string.Join("+", 
                    Enumerable.Repeat("---", columnCount)) 
                + " ";

        private static string RenderStatus(GameStatus status)
        {
            var asReady = status as GameStatus.Ready;
            var asWon = status as GameStatus.Won;
            return asReady != null ?
                        $"It's your move, {asReady.NextPlayer}'s. What'll it be (e.g., A2/C3)? "
                : asWon != null ?
                        $"That's a win, {asWon.Winner}'s. Congrats!{NewLine}Better luck next time, {asWon.Winner.Opponent()}'s."
                : // GameStatus.Drawn
                        "Cat's game! Srsly?";
        }
    }
}