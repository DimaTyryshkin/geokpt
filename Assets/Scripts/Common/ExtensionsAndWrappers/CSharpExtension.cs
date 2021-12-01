using System; 
using System.Collections.Generic;

namespace SiberianWellness.Common
{
	public static class EnumerableExtension
	{
		public static string ToStringMultiline<T>(this IEnumerable<T> collection, Func<T,string> func)
		{
			string s = "";

			foreach (var element in collection)
			{
				s += func(element) + Environment.NewLine;
			}

			return s;
		}

		public static IEnumerable<T> CastIfCan<TSource,T>(this IEnumerable<TSource> collection)
		{
			foreach (var element in collection)
			{
				if (element is T value)
					yield return value;
			}
		}
	}

	public static class DictionaryWrapper
	{
		public static T2 GetOrDefault<T1, T2>(this Dictionary<T1, T2> dic, T1 key, T2 defaultValue)
		{
			if (dic.TryGetValue(key, out T2 val))
				return val;
			else
				return defaultValue;
		}
		
		public static T2 GetOrCreate<T1, T2>(this Dictionary<T1, T2> dic, T1 key, Func<T2>  factory)
		{
			if (dic.TryGetValue(key, out T2 val))
				return val;
			else
			{
				 var defaultValue =factory();
				 dic.Add(key, defaultValue);
				 return defaultValue;
			}
		}
	}

	public static class ListExtension
	{
		private static readonly Random rng = new Random();
		
		public static T Random<T>(this List<T> list)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		public static int RandomIndex<T>(this List<T> list)
		{
			return UnityEngine.Random.Range(0, list.Count);
		}
		 
		/// <summary>
		/// Shuffles the specified list.
		/// </summary> 
		public static void Shuffle<T>(this IList<T> list)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k     = rng.Next(n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}

	public static class ArrayExtension
	{
		public static T Random<T>(this T[] array)
		{
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static int RandomIndex<T>(this T[] array)
		{
			return UnityEngine.Random.Range(0, array.Length);
		}

		public static int IndexOf<T>(this T[] array, T element)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(element))
					return i;
			}

			return -1;
		}

		public static int IndexOf<T>(this T[] array, Predicate<T> predicate)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (predicate(array[i]))
					return i;
			}

			return -1;
		}
	}
}