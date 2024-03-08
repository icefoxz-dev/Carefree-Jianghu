using System;
using System.Collections.Generic;
using System.Linq;

namespace _Data
{
    /// <summary>
    /// 权重接口
    /// </summary>
    public interface IWeightElement
    {
        double Weight { get; }
    }

    public static class WeightedRandomSelectionExtensions
    {
        private static readonly Random random = new();
        private static double GetRandom()
        {
            lock (random)
                return random.NextDouble();
        }

        public static IEnumerable<T> SelectWeightedRandom<T>(this IEnumerable<T> elements, int numberOfItems) where T : IWeightElement
        {
            var filteredElements = elements
                .Where(e => e.Weight > 0)
                .OrderBy(_ => GetRandom()) // 随机打乱元素顺序
                .ToList();

            if (!filteredElements.Any() || numberOfItems <= 0)
            {
                return Enumerable.Empty<T>(); // 如果过滤后没有元素或请求的元素数量不合理，则返回空集合
            }

            var weightedList = filteredElements
                .Select((e, index) => (item: e, accumulatedWeight: filteredElements.Take(index + 1).Sum(x => x.Weight)))
                .ToList();

            var totalWeight = weightedList.Last().accumulatedWeight;
            var selectedItems = new List<T>();

            for (var i = 0; i < numberOfItems; i++)
            {
                var r = GetRandom() * totalWeight;
                var selected = weightedList.FirstOrDefault(wl => wl.accumulatedWeight >= r).item;

                if (selected != null)
                {
                    selectedItems.Add(selected);
                }
            }

            return selectedItems;
        }
    }

}