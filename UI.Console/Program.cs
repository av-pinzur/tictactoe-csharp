using System;
using System.Collections.Generic;
using System.Threading;
using Con = System.Console;
using AvP.TicTacToe.Core;
using AvP.Joy;

namespace AvP.TicTacToe.UI.Console {
    public static class Program {
        private static readonly string NewLine = Environment.NewLine;
        private static readonly string NewLineX2 = NewLine + NewLine;

        public static void Main(string[] args) {
            Con.WriteLine("Welcome to Av's C# Tic-Tac-Toe!");

            do {
                Con.Write(NewLineX2 + $"First, please select a player type for {PlayerId.X}s{NewLine} (1 = Human; 2 = Dopey Computer; 3 = Naive Computer; 4 = Smart Computer): ");
                var playerX = GetPlayerType();
                Con.Write(NewLine + $"Great! Now for {PlayerId.O}s: ");
                var playerO = GetPlayerType();
                var players = new Dictionary<PlayerId, Func<Game, CellId>> {
                    { PlayerId.X, playerX },
                    { PlayerId.O, playerO } };

                var game = new Game();
                while (game.Status is GameStatus.Ready) {
                    var readyStatus = ((GameStatus.Ready) game.Status);
                    var playSelector = players[readyStatus.NextPlayer];

                    Con.Write(NewLineX2 + game.Render());
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    bool hasPlayed = false;
                    var thinkStart = DateTime.UtcNow;
                    do {
                        try {
                            game = game.Play(
                                cell: playSelector(game), 
                                thinkTime: DateTime.UtcNow - thinkStart);
                            hasPlayed = true;
                        }
                        catch (Exception e) {
                            Con.Write("Sorry! " + e.Message + " Please try again: ");
                        }
                    } while (!hasPlayed);
                };

                Con.Write(NewLineX2 + game.Render());

                Con.Write(NewLineX2 + "Play again (y/n)? ");
            } while (Con.ReadLine().Trim().ToLowerInvariant() == "y");

            Con.WriteLine("Thanks for playing Av's C# Tic-Tac-Toe!");
        }

        private static Func<Game, CellId> GetPlayerType() {
            do {
                var playerType = Con.ReadLine().Trim();
                if (playerType.IsAmong("1", "2", "3", "4"))
                    return playerType == "1" ? _ => CellId.Parse(Con.ReadLine())
                        : playerType == "2" ? ComputerPlayer.RandomPlay(new Random())
                        : playerType == "3" ? ComputerPlayer.NaivePlay(new Random())
                        : ComputerPlayer.SmartPlay(new Random());

                Con.Write("We're going to need a real answer ;-). ");
            } while (true);
        }
    }
}