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
        public static string Render(Game game)
        {
            var bLines = RenderBoard(game.Board).Lines();
            var boardFiller = new string(' ', bLines.First().Length);
            return bLines.ZipAll(
                    RenderMoveHistory(
                        game.MoveHistory).Lines(),
                        (bLine, hLine) 
                            => bLine.ValueOrDefault(boardFiller)
                                + "  "
                                + hLine.ValueOrDefault(string.Empty)
                                + Environment.NewLine)
                .Join(string.Empty)
                + Environment.NewLine
                + RenderStatus(game.Status);
        }

        private static string RenderMoveHistory(IEnumerable<Tuple<Cell, PlayerId>> moveHistory)
            => string.Join(Environment.NewLine,
                moveHistory.Select(RenderMove));

        private static string RenderMove(Tuple<Cell, PlayerId> move)
            => move.Item2 + " played at " + move.Item1 + ".";

        private static string RenderBoard(IReadOnlyList<IReadOnlyList<PlayerId?>> board)
            => string.Join(Environment.NewLine,
                board.Select(RenderBoardRow)
                    .Interpose(RenderBoardRowSeparator(board.First().Count)));

        private static string RenderBoardRow(IEnumerable<PlayerId?> row)
            => string.Join("|", row.Select(id => id == null ? "   " : $" {id} "));

        private static string RenderBoardRowSeparator(int columnCount)
            => string.Join("+", Enumerable.Repeat("---", columnCount));

        private static string RenderStatus(GameStatus status)
        {
            var asAwaiting = status as GameStatus.Ready;
            var asWon = status as GameStatus.Won;
            return asAwaiting != null ?
                        @"It's your move, {asAwaiting.NextPlayer}'s. What'll it be (e.g., x,y)?"
                : asWon != null ?
                        @"That's a win, {asWon.Winner}'s. Congrats! Better luck next time, {asWon.Winner.Opponent()}'s."
                : // GameStatus.Drawn
                        "Cat's game! Srsly?";
        }
    }
}