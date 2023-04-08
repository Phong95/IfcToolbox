using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitIFC.Extensions
{
    public static class CollectionExtensions
    {
        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        public static List<List<T>> Divide<T>(this List<T> array, int size)
        {
            List<List<T>> result = new List<List<T>>();
            int numbersInGroup = array.Count / size;
            int numbersInTheory = numbersInGroup * size;
            for (int i = 0; i < size; i++)
            {
                List<T> group = new List<T>();
                if (numbersInTheory < array.Count && i==size-1)
                {
                    group = array.Skip(i * numbersInGroup).Take(numbersInGroup + array.Count - numbersInTheory).ToList();

                }
                else
                {
                    group = array.Skip(i * numbersInGroup).Take(numbersInGroup).ToList();

                }
                result.Add(group);
            }
            return result;
        }
    }
}
