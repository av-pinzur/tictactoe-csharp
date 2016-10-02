using System;
using Con = System.Console;
using AvP.TicTacToe.Core;

namespace AvP.TicTacToe.UI.Console {
    public static class Program {
        private static readonly string N = Environment.NewLine;
        private static readonly string NN = N + N;

        public static void Main(string[] args) {
            Con.WriteLine("Welcome to Av's C# Tic-Tac-Toe!");
            
            do {
                var game = new Game();
                do {
                    Con.Write(NN + game.Render());

                    bool played = false;
                    do {
                        try {
                            game = game.Play(Cell.Parse(Con.ReadLine()));
                            played = true;
                        }
                        catch (Exception e) {
                            Con.Write("Sorry! " + e.Message + " Please try again: ");
                        }
                    } while (!played);

                } while (!game.Status.IsComplete);

                Con.Write(game.Render());
                Con.Write(NN + "Play again (y/n)? ");
            } while (Con.ReadLine().Trim().ToLowerInvariant() == "y");

            Con.WriteLine("Thanks for playing Av's C# Tic-Tac-Toe!");
        }
    }
}