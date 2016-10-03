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

        private readonly IEnumerable<Tuple<CellId, TimeSpan>> rawPlayHistory;

        public Game(IEnumerable<Tuple<CellId, TimeSpan>> rawPlayHistory = null)
        {
            rawPlayHistory = rawPlayHistory.OrEmpty();
            if (!rawPlayHistory.IsDistinct())
                throw new ArgumentException("Elements must be unique.", nameof(rawPlayHistory));

            this.rawPlayHistory = rawPlayHistory;

            PlayHistory = rawPlayHistory.Select((play, index)
                => Tuple.Create(play.Item1, PlayerByPlayIndex(index), play.Item2))
                .ToList().AsValueList();

            Board = BoardDescriptor.CellIds.Select(cellRow 
                => cellRow.Select(cell 
                    => PlayHistory
                        .Where(play => cell.Equals(play.Item1))
                        .Select(play => (PlayerId?) play.Item2)
                        .SingleOrDefault()))
                .ToListDeep().AsValueListDeep();

            var win = FindWin(Board);
            Status = win.HasValue 
                        ? new GameStatus.Won(win.Value.Item1, win.Value.Item2)
                : Board.Any(row => row.Contains(null)) 
                        ? new GameStatus.Ready(PlayerByPlayIndex(rawPlayHistory.Count()))
                : (GameStatus) new GameStatus.Drawn();
        }

        public ValueList<Tuple<CellId, PlayerId, TimeSpan>> PlayHistory { get; }

        public ValueList<ValueList<PlayerId?>> Board { get; }

        public GameStatus Status { get; }

        private static PlayerId PlayerByPlayIndex(int playIndex) => (PlayerId) (playIndex % 2);

        private static Maybe<Tuple<PlayerId, IEnumerable<CellId>>> FindWin(
            IReadOnlyList<IReadOnlyList<PlayerId?>> board)
            => board.WithCellIds().AllVectors()
                .Select(vector =>  // TODO: Craving fmap.
                    {
                        Maybe<PlayerId?> winner = vector.Select(o => o.Item2).Consensus();
                        return Maybe.If(
                            winner.HasValue  // Was the vector uniform?
                                && winner.Value.HasValue,  // Discard vectors of empty cells.
                            () => Tuple.Create(winner.Value.Value, vector.Select(o => o.Item1)));
                    })
                .FirstOrDefault(win => win.HasValue);

        public Game Play(CellId cell, TimeSpan thinkTime = default(TimeSpan))
        {
            if (Status.IsComplete)
                throw new InvalidOperationException("The current game is complete.");

            if (rawPlayHistory.Any(play => cell == play.Item1))
                throw new InvalidOperationException("The specified cell has already been played.");

            return new Game(rawPlayHistory.Concat(Tuple.Create(cell, thinkTime)));
        }
    }
}