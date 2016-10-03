using System;

namespace AvP.TicTacToe.Core
{
    public abstract partial class GameStatus : IEquatable<GameStatus>
    {
        public sealed class Ready : GameStatus
        {
            public Ready(PlayerId nextPlayer)
            {
                NextPlayer = nextPlayer;
            }

            public override bool IsComplete { get { return false; } }
            public PlayerId NextPlayer { get; }

            public override int GetHashCodeImpl()  // TODO: Improve has code algorithm.
                => unchecked(typeof(Ready).GetHashCode()
                    * 397 ^ NextPlayer.GetHashCode());

            public override bool Equals(GameStatus other)
            {
                var otherAs = other as Ready;
                return otherAs != null
                    && NextPlayer.Equals(otherAs.NextPlayer);
            }

            public override string ToString()
                => string.Format("{0} {{ {1}: {2} }}", 
                    nameof(Ready), 
                    nameof(NextPlayer), NextPlayer);
        }
    }
}