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
        public static string Render(this Game game)
        {
            var boardLines = RenderBoard(game.Board).Lines();
            var boardFiller = new string(' ', boardLines.First().Length);

            var historyLines = RenderMoveHistory(game.MoveHistory).Lines();
            
            return boardLines.ZipAll(historyLines, (bLine, hLine) 
                    => bLine.ValueOrDefault(boardFiller)
                        + "  "
                        + hLine.ValueOrDefault(string.Empty))
                    .Join(Environment.NewLine)
                + Environment.NewLine
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
            var asReady = status as GameStatus.Ready;
            var asWon = status as GameStatus.Won;
            return asReady != null ?
                        string.Format("It's your move, {0}'s. What'll it be (e.g., A2/C3)? ", asReady.NextPlayer)
                : asWon != null ?
                        string.Format("That's a win, {0}'s. Congrats!{1}Better luck next time, {2}'s.", 
                            asWon.Winner, 
                            Environment.NewLine, 
                            asWon.Winner.Opponent())
                : // GameStatus.Drawn
                        "Cat's game! Srsly?";
        }
    }
}