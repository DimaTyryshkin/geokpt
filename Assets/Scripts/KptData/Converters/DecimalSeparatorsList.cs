using System.Linq;
using UnityEngine;

namespace Geo.KptData.Converters
{
	public static class DecimalSeparatorsList
	{ 
		public static readonly SeparatorInfo[] decimals = new SeparatorInfo[]
		{
			new SeparatorInfo(".", "'.' (точка)"),
			new SeparatorInfo(",", "',' (запятая)")
		};
		
		public static string[] GetAvailableDecimalSeparator()
		{
			return decimals.Select(s => s.separator)
				.ToArray();
		}
		
		public static string GetDecimalSeparator(int decimalSeparator)
		{
			return GetDecimalSeparatorSafe(decimalSeparator).separator;
		}
		public static string GetDecimalSeparatorDescription(string decimalSeparator)
		{
			foreach (SeparatorInfo separatorInfo in decimals)
			{
				if (decimalSeparator == separatorInfo.separator)
					return separatorInfo.label;
			}

			return $"'{decimalSeparator}'";
		}	
		
		static SeparatorInfo GetDecimalSeparatorSafe(int decimalSeparator)
		{
			decimalSeparator = Mathf.Clamp(decimalSeparator, 0, decimals.Length - 1);
			return decimals[decimalSeparator];
		}
	}
}