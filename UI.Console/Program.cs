using System;
using Con = System.Console;
using AvP.TicTacToe.Core;
using AvP.Joy.Enumerables;
using AvP.Joy;

namespace AvP.TicTacToe.UI.Console {
    public static class Program {
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string NewLineX2 = NewLine + NewLine;

        public static void Main(string[] args) {
            Con.WriteLine("Welcome to Av's C# Tic-Tac-Toe!");

            do {
                Con.Write(NewLineX2 + $"First, please select a player type for {PlayerId.X}s (1 = Human; 2 = Dopey Computer; 3 = Naive Computer; 4 = Smart Computer): ");
                var playerX = GetPlayerType();
                Con.Write(NewLine + $"Great! Now for {PlayerId.O}s: ");
                var playerY = GetPlayerType();

                var game = new Game();
                using (var players = new[] { playerX, playerY }.Cycle().GetEnumerator())
                do {
                    Con.Write(NewLineX2 + game.Render());

                    players.MoveNext();
                    bool played = false;
                    do {
                        try {
                            var playerMove = players.Current(game);
                            game = game.Play(playerMove);
                            played = true;
                        }
                        catch (Exception e) {
                            Con.Write("Sorry! " + e.Message + " Please try again: ");
                        }
                    } while (!played);

                } while (!game.Status.IsComplete);

                Con.Write(NewLineX2 + game.Render());

                Con.Write(NewLineX2 + "Play again (y/n)? ");
            } while (Con.ReadLine().Trim().ToLowerInvariant() == "y");

            Con.WriteLine("Thanks for playing Av's C# Tic-Tac-Toe!");
        }

        private static Func<Game, CellId> GetPlayerType()
        {
            do {
                var playerType = Con.ReadLine().Trim();
                if (playerType.IsAmong("1", "2", "3", "4"))
                    return playerType == "1" ? _ => CellId.Parse(Con.ReadLine())
                        : playerType == "2" ? ComputerPlayer.RandomMove
                        : playerType == "3" ? ComputerPlayer.NaiveMove
                        : (Func<Game, CellId>) ComputerPlayer.SmartMove;

                Con.Write("We're going to need a real answer ;-). ");
            } while (true);
        }
    }
}