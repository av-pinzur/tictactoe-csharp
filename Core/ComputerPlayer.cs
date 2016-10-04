using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public static class ComputerPlayer
    {
        private static IEnumerable<CellId> WinningOptions(this Game game)
            => game.PlayOptions.Where(o 
                    => game.Play(o).Status is GameStatus.Won)
                .ToList();

        private static CellId? RandomPer(this IEnumerable<CellId> options, Random random)
            => random.NextFromOrDefault(options.Cast<CellId?>());

        public static Func<Game, CellId> RandomPlay(Random random)
            => (Game game)
            => game.PlayOptions.RandomPer(random).Value;

        public static Func<Game, CellId> NaivePlay(Random random)
            => (Game game) 
            => game.WinningOptions().RandomPer(random) 
                ?? game.PlayOptions.Where(o 
                    => game.Play(o).WinningOptions().None()).RandomPer(random)
                ?? game.PlayOptions.RandomPer(random).Value;

        private static Game Canonicalized(this Game game)
        {
            if (game.Status.IsComplete) return game;

            var canonicalizedBoard = game.Board.Symmetries()
                .MinBy(EnumerableComparer.ByElementsOrdered(
                    EnumerableComparer.ByElementsOrdered(
                        Comparer<PlayerId?>.Default)));

            var playsByPlayer = canonicalizedBoard.WithCellIds().SelectMany(F.Id)
                .Where(o => o.Item2.HasValue)
                .GroupBy(o => o.Item2, o => o.Item1).OrderBy(o => o.Key);
            return playsByPlayer.Interleave().Aggregate(new Game(), (_game, cellId) => _game.Play(cellId));
        }

        private const int defaultMaxDepth = 10;

        private static double RankPlayResult(Game game, int maxDepth = defaultMaxDepth, int curDepth = 0)
            => RankPlayResult_Memoized(game.Canonicalized(), maxDepth, curDepth);

        private static Func<Game, int, int, double> RankPlayResult_Memoized { get; }
            = F.Memoize((Func<Game, int, int, double>) RankPlayResult_Impl);

        private static double RankPlayResult_Impl(Game playResult, int maxDepth, int curDepth)
        {
            return playResult.Status is GameStatus.Won ? defaultMaxDepth - curDepth
                : playResult.Status is GameStatus.Drawn ? 0
                : -1 * playResult.PlayOptions
                    .Select(o => RankPlayResult(playResult.Play(o), maxDepth, curDepth + 1))
                    .Max();
        }

        public static Func<Game, CellId> SmartPlay(Random random)
            => (Game game) 
            => game.PlayOptions
                .GroupBy(o => RankPlayResult(game.Play(o)))
                .MaxBy(g => g.Key)
                .RandomPer(random).Value;
    }
}