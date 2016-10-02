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
            rawMoveHistory = rawMoveHistory.OrEmpty();
            if (!rawMoveHistory.IsDistinct())
                throw new ArgumentException("Elements must be unique.", nameof(rawMoveHistory));

            this.rawMoveHistory = rawMoveHistory;

            MoveHistory = rawMoveHistory.Select((cell, index)
                => Tuple.Create(cell, PlayerByMoveIndex(index))).ToList();

            Board = BoardHelper.AllRows
                .Select(row => BoardHelper.AllCols
                    .Select(col => MoveHistory
                        .Where(move => new Cell(row, col).Equals(move.Item1))
                        .Select(move => (PlayerId?) move.Item2)
                        .SingleOrDefault())
                    .ToList())
                .ToList();

            var win = FindWin(Board);
            Status = win.HasValue 
                        ? new GameStatus.Won(win.Value.Item1, win.Value.Item2)
                : Board.Any(row => row.Contains(null)) 
                        ? new GameStatus.Ready(PlayerByMoveIndex(rawMoveHistory.Count()))
                : (GameStatus) new GameStatus.Drawn();
        }

        public IReadOnlyList<Tuple<Cell, PlayerId>> MoveHistory { get; }

        public IReadOnlyList<IReadOnlyList<PlayerId?>> Board { get; }

        public GameStatus Status { get; }

        private static PlayerId PlayerByMoveIndex(int moveIndex) => (PlayerId) (moveIndex % 2);

        private static Maybe<Tuple<PlayerId, IEnumerable<Cell>>> FindWin(
            IReadOnlyList<IReadOnlyList<PlayerId?>> board)
            => board.WithCellIds().AllVectors()
                .Select(axis =>
                    {
                        Maybe<PlayerId?> winner = axis.Select(o => o.Item2).Uniform();
                        return Maybe.If(
                            winner.HasValue  // Was the vector uniform?
                                && winner.Value.HasValue,  // Discard vectors of empty cells.
                            () => Tuple.Create(winner.Value.Value, axis.Select(o => o.Item1)));
                    })
                .SingleOrDefault(win => win.HasValue);

        public Game Play(Cell cell)
        {
            if (Status.IsComplete)
                throw new InvalidOperationException("The current game is complete.");

            if (rawMoveHistory.Contains(cell))
                throw new InvalidOperationException("The specified cell has already been played.");

            return new Game(rawMoveHistory.Concat(cell));
        }
    }
}