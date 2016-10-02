namespace AvP.TicTacToe.Core
{
    public enum PlayerId { X = 0, Y = 1 }

    public static class PlayerIdExtensions
    {
        public static PlayerId Opponent(this PlayerId value) 
            => value == PlayerId.X ? PlayerId.Y : PlayerId.X;
    }
}