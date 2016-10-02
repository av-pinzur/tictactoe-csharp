using System;

namespace AvP.TicTacToe.Core
{
    public abstract partial class GameStatus : IEquatable<GameStatus>
    {
        public sealed class Drawn : GameStatus
        {
            public override bool IsComplete { get { return true; } }

            public override int GetHashCodeImpl()
                => typeof(Drawn).GetHashCode();

            public override bool Equals(GameStatus other)
                => other is Drawn;

            public override string ToString()
                => string.Format("{0}",
                    nameof(Drawn));
        }
    }
}