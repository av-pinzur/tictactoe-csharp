using System;
using AvP.TicTacToe.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvP.TicTacToe.Core.Test
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestStatus()
        {
            var setup = new Game()
                .Play(Cell.Parse("A1"))
                .Play(Cell.Parse("C1"))
                .Play(Cell.Parse("A3"))
                .Play(Cell.Parse("A2"))
                .Play(Cell.Parse("C3"))
                .Play(Cell.Parse("b2"));

            Assert.AreEqual(
                new GameStatus.Ready(
                    PlayerId.X),
                setup.Status);

            Assert.AreEqual(
                new GameStatus.Won(
                    PlayerId.X,
                    new[] { Cell.Parse("A3"), Cell.Parse("B3"), Cell.Parse("C3") }),
                setup.Play(Cell.Parse("b3"))
                    .Status);

            Assert.AreEqual(
                new GameStatus.Drawn(),
                setup.Play(Cell.Parse("b1"))
                    .Play(Cell.Parse("b3"))
                    .Play(Cell.Parse("c2"))
                    .Status);
        }
    }
}
