using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public static class ComputerPlayer
    {
        public static CellId RandomMove(Game game)
        {
            var allCellIds = BoardDescriptor.CellIds.SelectMany(F.Id);
            var remainingCellIds = allCellIds.Except(game.MoveHistory.Select(o => o.Item1));

            return remainingCellIds.Nth(new Random().Next(remainingCellIds.Count()));
        }

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
