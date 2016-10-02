using System;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvP.TicTacToe.Core.Test
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestAllAxes()
        {
            var emptyBoard = BoardHelper.AllRows.Select(row => BoardHelper.AllCols.Select(col => Tuple.Create(new Cell(row, col), default(PlayerId?))));

            Assert.AreEqual(
                new[]{
                        new[] { "A1", "B1", "C1" },
                        new[] { "A2", "B2", "C2" },
                        new[] { "A3", "B3", "C3" },

                        new[] { "A1", "B2", "C3" },

                        new[] { "A1", "A2", "A3" },
                        new[] { "B1", "B2", "B3" },
                        new[] { "C1", "C2", "C3" },

                        new[] { "A3", "B2", "C1" }}
                    .Select(axis => axis.Select(o => Tuple.Create(Cell.Parse(o), default(PlayerId?))))
                    .ToListDeep()
                    .AsValueSetDeep(),
                EnumerableSquaredExtensions.AllVectors(emptyBoard)
                    .ToListDeep()
                    .AsValueSetDeep());
        }
    }
}