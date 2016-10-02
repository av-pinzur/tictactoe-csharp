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
                .Play(CellId.Parse("A1"))
                .Play(CellId.Parse("C1"))
                .Play(CellId.Parse("A3"))
                .Play(CellId.Parse("A2"))
                .Play(CellId.Parse("C3"))
                .Play(CellId.Parse("b2"));

            Assert.AreEqual(
                new GameStatus.Ready(
                    PlayerId.X),
                setup.Status);

            Assert.AreEqual(
                new GameStatus.Won(
                    PlayerId.X,
                    new[] { CellId.Parse("A3"), CellId.Parse("B3"), CellId.Parse("C3") }),
                setup.Play(CellId.Parse("b3"))
                    .Status);

            Assert.AreEqual(
                new GameStatus.Drawn(),
                setup.Play(CellId.Parse("b1"))
                    .Play(CellId.Parse("b3"))
                    .Play(CellId.Parse("c2"))
                    .Status);
        }
    }
}
