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

        private static double RankPlayOption(Game game, CellId option)
        {
            var playResult = game.Play(option);
            return playResult.Status is GameStatus.Won ? 5
                : playResult.Status is GameStatus.Drawn ? 0
                : -1 * playResult.PlayOptions
                    .Select(o => RankPlayOption(playResult, o))
                    .Max();
        }

        public static Func<Game, CellId> SmartPlay(Random random)
            => (Game game) 
            => game.PlayOptions
                .GroupBy(o => RankPlayOption(game, o))
                .MaxBy(g => g.Key)
                .RandomPer(random).Value;
    }
}