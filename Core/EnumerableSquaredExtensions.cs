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
        #region Rotate~, AllRotations

        public static IEnumerable<IEnumerable<T>> Rotate90<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Zip(o => o.Reverse());

        public static IEnumerable<IEnumerable<T>> Rotate180<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate90().Rotate90();

        public static IEnumerable<IEnumerable<T>> Rotate270<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Rotate90().Rotate180();

        public static IEnumerable<IEnumerable<IEnumerable<T>>> Rotations<T>(
            this IEnumerable<IEnumerable<T>> source)
            => new[] {
                source.Rotate90(),
                source.Rotate180(),
                source.Rotate270() };

        #endregion
        #region Reflect~, Reflections

        public static IEnumerable<IEnumerable<T>> ReflectHorizontal<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Select(o => o.Reverse());

        public static IEnumerable<IEnumerable<T>> ReflectVertical<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.Reverse();

        public static IEnumerable<IEnumerable<T>> ReflectDownhill<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.ReflectVertical().Rotate90();

        public static IEnumerable<IEnumerable<T>> ReflectUphill<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.ReflectHorizontal().Rotate90();

        public static IEnumerable<IEnumerable<IEnumerable<T>>> Reflections<T>(
            this IEnumerable<IEnumerable<T>> source)
            => new[] {
                source.ReflectHorizontal(),
                source.ReflectVertical(),
                source.ReflectDownhill(),
                source.ReflectUphill() };

        #endregion
        #region Symmetries

        public static IEnumerable<IEnumerable<IEnumerable<T>>> Symmetries<T>(
            this IEnumerable<IEnumerable<T>> source)
            => source.InSingleton()
                .Concat(source.Rotations())
                .Concat(source.Reflections());

        #endregion
    }
}