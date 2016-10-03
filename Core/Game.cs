using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public sealed class Game
    {
        private readonly IEnumerable<Tuple<CellId, TimeSpan>> rawPlayHistory;

        public Game() : this(Enumerable.Empty<Tuple<CellId, TimeSpan>>()) { }

        private Game(IEnumerable<Tuple<CellId, TimeSpan>> rawPlayHistory)
        {
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

            PlayOptions = BoardDescriptor.CellIds.SelectMany(F.Id)
                .Except(PlayHistory.Select(o => o.Item1))
                .ToHashSet().AsValueSet();
        }

        public ValueList<Tuple<CellId, PlayerId, TimeSpan>> PlayHistory { get; }
        public ValueList<ValueList<PlayerId?>> Board { get; }
        public GameStatus Status { get; }
        public ValueSet<CellId> PlayOptions { get; }

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

            if (!PlayOptions.Contains(cell))
                throw new InvalidOperationException("The specified cell has already been played.");

            return new Game(rawPlayHistory.Concat(Tuple.Create(cell, thinkTime)));
        }
    }
}