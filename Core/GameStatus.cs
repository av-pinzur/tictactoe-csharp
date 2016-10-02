using System;

namespace AvP.TicTacToe.Core
{
    public abstract partial class GameStatus : IEquatable<GameStatus>
    {
        private GameStatus() { }  // Prevent non-nested inheritors.
        public abstract bool IsComplete { get; }

        public abstract int GetHashCodeImpl();
        public abstract bool Equals(GameStatus other);

        public sealed override int GetHashCode()
            => GetHashCodeImpl();

        public sealed override bool Equals(object obj)
        {
            var objAs = obj as GameStatus;
            return objAs != null & Equals(objAs);
        }

        public static bool operator == (GameStatus x, GameStatus y)
            => Equals(x, y);

        public static bool operator != (GameStatus x, GameStatus y)
            => !(x == y);
    }
}