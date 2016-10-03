using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{   // Greg started his test at 7:10.
    public static class ComputerPlayer
    {
        private static IEnumerable<CellId> PlayingOptions(this Game game)
            => BoardDescriptor.CellIds.SelectMany(F.Id)
                .Except(game.MoveHistory.Select(o => o.Item1))
                .ToList();

        private static IEnumerable<CellId> WinningOptions(this Game game)
            => game.PlayingOptions().Where(o 
                    => game.Play(o).Status is GameStatus.Won)
                .ToList();

        private static CellId? RandomPer(this IEnumerable<CellId> options, Random random)
            => random.NextFromOrDefault(options.Cast<CellId?>());

        public static Func<Game, CellId> RandomMove(Random random)
            => (Game game)
            => game.PlayingOptions().RandomPer(random).Value;

        public static Func<Game, CellId> NaiveMove(Random random)
            => (Game game) 
            => game.WinningOptions().RandomPer(random) 
                ?? game.PlayingOptions().Where(o 
                    => game.Play(o).WinningOptions().None()).RandomPer(random)
                ?? game.PlayingOptions().RandomPer(random).Value;

        private static double RankOption(Game game, CellId option)
        {
            var result = game.Play(option);
            return result.Status is GameStatus.Won ? 5
                : result.Status is GameStatus.Drawn ? 0
                : -result.PlayingOptions()
                    .Select(o => RankOption(result, o))
                    .Max();
        }

        public static Func<Game, CellId> SmartMove(Random random)
            => (Game game) 
            => game.PlayingOptions().GroupBy(o => RankOption(game, o)).MaxBy(g => g.Key).RandomPer(random).Value;
    }
}