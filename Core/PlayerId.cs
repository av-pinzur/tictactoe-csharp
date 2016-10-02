namespace AvP.TicTacToe.Core
{
    public enum PlayerId { X = 0, O = 1 }

    public static class PlayerIdExtensions
    {
        public static PlayerId Opponent(this PlayerId value) 
            => value == PlayerId.X ? PlayerId.O : PlayerId.X;
    }
}