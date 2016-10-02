using System;
using System.Collections.Generic;
using AvP.Joy;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public abstract partial class GameStatus : IEquatable<GameStatus>
    {
        public sealed class Won : GameStatus
        {
            public Won(PlayerId winner, IEnumerable<CellId> winningCells)
            {
                if (null == winningCells) throw new ArgumentNullException(nameof(winningCells));

                Winner = winner;
                WinningCells = winningCells.ToSortedSet().AsValueSet();
            }

            public override bool IsComplete { get { return true; } }
            public PlayerId Winner { get; }
            public ValueSet<CellId> WinningCells { get; }

            public override int GetHashCodeImpl()  // TODO: Improve has code algorithm.
                => typeof(Won).GetHashCode() 
                    ^ Winner.GetHashCode()
                    ^ WinningCells.GetHashCode();

            public override bool Equals(GameStatus other)
            {
                var otherAs = other as Won;
                return otherAs != null 
                    && Winner.Equals(otherAs.Winner)
                    && WinningCells.Equals(otherAs.WinningCells);
            }

            public override string ToString()
                => string.Format("{0} {{ {1}: {2}, {3}: {4} }}", 
                    nameof(Won), 
                    nameof(Winner), Winner, 
                    nameof(WinningCells), WinningCells);
        }
    }
}