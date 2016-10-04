using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvP.TicTacToe.Core.Test
{
    [TestClass]
    public class EnumerableSquaredExtensionsTest
    {
        [TestMethod]
        public void TestSymmetries()
        {
            var baseline = new[] { new[] { "a1", "a2", "a3" }, new[] { "b1", "b2", "b3" }, new[] { "c1", "c2", "c3" } };

            Assert.AreEqual(
                new[] { new[] { "c1", "b1", "a1" }, new[] { "c2", "b2", "a2" }, new[] { "c3", "b3", "a3" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.Rotate90().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] { new[] { "c3", "c2", "c1" }, new[] { "b3", "b2", "b1" }, new[] { "a3", "a2", "a1" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.Rotate180().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] { new[] { "a3", "b3", "c3" }, new[] { "a2", "b2", "c2" }, new[] { "a1", "b1", "c1" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.Rotate270().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] {
                    new[] { new[] { "c1", "b1", "a1" }, new[] { "c2", "b2", "a2" }, new[] { "c3", "b3", "a3" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c3", "c2", "c1" }, new[] { "b3", "b2", "b1" }, new[] { "a3", "a2", "a1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "a3", "b3", "c3" }, new[] { "a2", "b2", "c2" }, new[] { "a1", "b1", "c1" } }.ToListDeep().AsValueListDeep() }
                    .ToList().AsValueList(),
                baseline.Rotations().Select(o => o.ToListDeep().AsValueListDeep()).ToList().AsValueList());

            Assert.AreEqual(
                new[] { new[] { "a3", "a2", "a1" }, new[] { "b3", "b2", "b1" }, new[] { "c3", "c2", "c1" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.ReflectHorizontal().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] { new[] { "c1", "c2", "c3" }, new[] { "b1", "b2", "b3" }, new[] { "a1", "a2", "a3" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.ReflectVertical().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] { new[] { "c3", "b3", "a3" }, new[] { "c2", "b2", "a2" }, new[] { "c1", "b1", "a1" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.ReflectDownhill().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] { new[] { "a1", "b1", "c1" }, new[] { "a2", "b2", "c2" }, new[] { "a3", "b3", "c3" } }
                    .ToListDeep().AsValueListDeep(),
                baseline.ReflectUphill().ToListDeep().AsValueListDeep());

            Assert.AreEqual(
                new[] {
                    new[] { new[] { "a3", "a2", "a1" }, new[] { "b3", "b2", "b1" }, new[] { "c3", "c2", "c1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c1", "c2", "c3" }, new[] { "b1", "b2", "b3" }, new[] { "a1", "a2", "a3" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c3", "b3", "a3" }, new[] { "c2", "b2", "a2" }, new[] { "c1", "b1", "a1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "a1", "b1", "c1" }, new[] { "a2", "b2", "c2" }, new[] { "a3", "b3", "c3" } }.ToListDeep().AsValueListDeep() }
                    .ToListDeep().AsValueListDeep(),
                baseline.Reflections().Select(o => o.ToListDeep().AsValueListDeep()).ToList().AsValueList());

            Assert.AreEqual(
                new[] {
                    new[] { new[] { "a1", "a2", "a3" }, new[] { "b1", "b2", "b3" }, new[] { "c1", "c2", "c3" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c1", "b1", "a1" }, new[] { "c2", "b2", "a2" }, new[] { "c3", "b3", "a3" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c3", "c2", "c1" }, new[] { "b3", "b2", "b1" }, new[] { "a3", "a2", "a1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "a3", "b3", "c3" }, new[] { "a2", "b2", "c2" }, new[] { "a1", "b1", "c1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "a3", "a2", "a1" }, new[] { "b3", "b2", "b1" }, new[] { "c3", "c2", "c1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c1", "c2", "c3" }, new[] { "b1", "b2", "b3" }, new[] { "a1", "a2", "a3" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "c3", "b3", "a3" }, new[] { "c2", "b2", "a2" }, new[] { "c1", "b1", "a1" } }.ToListDeep().AsValueListDeep(),
                    new[] { new[] { "a1", "b1", "c1" }, new[] { "a2", "b2", "c2" }, new[] { "a3", "b3", "c3" } }.ToListDeep().AsValueListDeep() }
                    .ToListDeep().AsValueListDeep(),
                baseline.Symmetries().Select(o => o.ToListDeep().AsValueListDeep()).ToList().AsValueList());
        }
    }
}
