using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AvP.TicTacToe.Core;

namespace AvP.TicTacToe.UI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game()
                .Play(Cell.Parse("A1"))
                .Play(Cell.Parse("C1"))
                .Play(Cell.Parse("A3"))
                .Play(Cell.Parse("A2"))
                .Play(Cell.Parse("C3"))
                .Play(Cell.Parse("b2"))
                .Play(Cell.Parse("b3"));
            System.Console.WriteLine(GameRenderer.Render(game));
            System.Console.ReadLine();
        }
    }
}
