using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public static class ComputerPlayer
    {
        public static Func<Game, CellId> RandomMove(Random rnd) 
            => (Game game) =>
        {
            var remainingCellIds = BoardDescriptor.CellIds.SelectMany(F.Id)
                .Except(game.MoveHistory.Select(o => o.Item1));

            return remainingCellIds.Nth(
                rnd.Next(remainingCellIds.Count()));
        };

        public static CellId NaiveMove(Game game)
        {
            throw new NotImplementedException();
        }

        public static CellId SmartMove(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
