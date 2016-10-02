using System;
using System.Collections.Generic;
using System.Linq;
using AvP.Joy.Enumerables;

namespace AvP.TicTacToe.Core
{
    public static class EnumerableSquaredExtensions
    {
        #region ~Vector(s)

        public static IEnumerable<IEnumerable<T>> HorizontalVectors<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source;

        public static IEnumerable<T> DownhillVector<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.HorizontalVectors()
                .Select((o, i) => o.Nth(i));

        private static IEnumerable<IEnumerable<T>> HorizontalAndDownhillVectors<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.HorizontalVectors()
                .Concat(source.DownhillVector());

        public static IEnumerable<IEnumerable<T>> AllVectors<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.HorizontalAndDownhillVectors()
                .Concat(source.Rotate90().HorizontalAndDownhillVectors());

        #endregion
        #region Rotate~

        public static IEnumerable<IEnumerable<T>> Rotate90<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Zip(o => o.Reverse());

        public static IEnumerable<IEnumerable<T>> Rotate180<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate90().Rotate90();

        public static IEnumerable<IEnumerable<T>> Rotate270<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate90().Rotate180();

        public static IEnumerable<IEnumerable<T>> RotateBack90<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate270();

        public static IEnumerable<IEnumerable<T>> RotateBack270<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate90();

        #endregion
        #region Flip~

        public static IEnumerable<IEnumerable<T>> FlipHorizontal<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Select(o => o.Reverse());

        public static IEnumerable<IEnumerable<T>> FlipVertical<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Reverse();

        #endregion
    }
}