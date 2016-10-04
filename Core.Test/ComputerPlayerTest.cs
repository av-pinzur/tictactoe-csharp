using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy;
using AvP.Joy.Enumerables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvP.TicTacToe.Core.Test
{
    [TestClass]
    public class ComputerPlayerTest
    {
        private static IDictionary<Maybe<PlayerId>, int> PlayGames(
            int gameCount, 
            Func<Game, CellId> xPlayer, 
            Func<Game, CellId> oPlayer)
        {
            return Enumerable.Range(0, gameCount)
                .AsParallel()
                .Select(_ => PlayGame(xPlayer, oPlayer))
                .DistinctCounted()
                .ToDictionary(
                    o => o.Value.HasValue ? o.Value.Value : Maybe<PlayerId>.None, 
                    o => o.Count);
        }

        private static PlayerId? PlayGame(
            Func<Game, CellId> xPlayer, 
            Func<Game, CellId> oPlayer)
        {
            var game = new Game();
            while (game.Status is GameStatus.Ready)
            {
                var playSelector = ((GameStatus.Ready) game.Status).NextPlayer == PlayerId.X ? xPlayer : oPlayer;
                game = game.Play(playSelector(game));
            }
            var wonStatus = game.Status as GameStatus.Won;
            return wonStatus != null ? wonStatus.Winner : default(PlayerId?);
        }

        const int deepIterations = 10000;

        [TestMethod]
        public void TestSmartVsRandomPlay()
        {
            var results = PlayGames(deepIterations,
                ComputerPlayer.SmartPlay(new Random()),
                ComputerPlayer.RandomPlay(new Random()));

            Assert.AreEqual(0, results.GetOrDefault(PlayerId.O, 0));
            Assert.IsTrue(.95 < results[PlayerId.X] / (double) deepIterations);
        }

        [TestMethod]
        public void TestRandomVsSmartPlay()
        {
            var results = PlayGames(deepIterations,
                ComputerPlayer.RandomPlay(new Random()),
                ComputerPlayer.SmartPlay(new Random()));

            Assert.AreEqual(0, results.GetOrDefault(PlayerId.X, 0));
            Assert.IsTrue(.75 < results[PlayerId.O] / (double) deepIterations);
        }
    }
}
