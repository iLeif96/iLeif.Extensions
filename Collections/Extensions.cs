using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Arrays
{
    public static class Extensions
    {
        public static T? TryGetByIndex<T>(this IList<T> list, int index) where T : class
        {
            if (list == null)
            {
                return null;
            }

            if (index > list.Count)
            {
                return null;
            }

            try
            {
                return list[index];
            }
            catch
            {
                return null;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (action is null) return;
            foreach (T obj in enumerable)
            {
                if (obj == null) continue;
                action.Invoke(obj);
            }
        }

        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> enumerable, Action<TKey, TValue> action)
        {
            if (action is null) return;
            foreach (KeyValuePair<TKey, TValue> obj in enumerable)
            {
                action.Invoke(obj.Key, obj.Value);
            }
        }

        public static void ForEach<T>(this List<T> list, Action<T> action)
        {
            if (action is null) return;
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] == null) continue;
                action.Invoke(list[i]);
            }
        }

        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0) return;
            ArrayTraverse walker = new ArrayTraverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }

        internal class ArrayTraverse
        {
            public int[] Position;
            private int[] maxLengths;

            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    maxLengths[i] = array.GetLength(i) - 1;
                }
                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < Position.Length; ++i)
                {
                    if (Position[i] < maxLengths[i])
                    {
                        Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
