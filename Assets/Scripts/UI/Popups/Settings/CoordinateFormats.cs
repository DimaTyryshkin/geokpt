using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Geo.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo.UI
{
	[Serializable]
	public class CoordinateFormats2Config
	{
		public SeparatorInfo[] defaultPointIndexFormats = new SeparatorInfo[]
		{
			new SeparatorInfo("pt(i)", "pt(i)"),
			new SeparatorInfo("", "не показывать")
		};
		
		public SeparatorInfo[] defaultSeparators = new SeparatorInfo[]
		{
			new SeparatorInfo(",", "',' (запятая)"),
			new SeparatorInfo(" ", "' ' (пробел)"),
			new SeparatorInfo(", ", "', ' (запятая пробел)")
		};
		
		public SeparatorInfo[] defaultHeight = new SeparatorInfo[]
		{
			new SeparatorInfo("0", "0"),
			new SeparatorInfo("", "не показывать")
		};
	}

	[Serializable]
	public struct SeparatorInfo
	{
		public string separator;
		public string description;
		
		public SeparatorInfo(string separator, string description)
		{
			this.separator   = separator;
			this.description = description;
		}
	}
	
	public class CoordinateFormats2
	{
		readonly CoordinateFormats2Config config;
		readonly AccountData.ContourToTxtConverterPreferences2 data;

		public static readonly SeparatorInfo[] decimals = new SeparatorInfo[] 
		{
			new SeparatorInfo(".", "'.' (точка)"), 
			new SeparatorInfo(",", "',' (запятая)")
		};

		public string[] GetAvailableSeparator()
		{
			return config.defaultSeparators
				.Select(s => s.separator)
				.Concat(data.userSeparators)
				.ToArray();
		}

		public string[] GetAvailableDecimalSeparator()
		{
			return decimals.Select(s => s.separator)
				.ToArray();
		}

		public string[] GetAvailablePointIndexFormat()
		{
			return config.defaultPointIndexFormats
				.Select(s => s.separator)
				.ToArray();
		}

		public string[] GetAvailableHeight()
		{
			return config.defaultHeight
				.Select(s => s.separator)
				.ToArray();
		}

		public CoordinateFormats2(CoordinateFormats2Config config, AccountData.ContourToTxtConverterPreferences2 data)
		{
			Assert.IsNotNull(config);
			Assert.IsNotNull(data);
			this.config = config;
			this.data   = data;
		}

		public static string GetDecimalSeparator(int decimalSeparator)
		{
			return GetDecimalSeparatorSafe(decimalSeparator).separator;
		}
		public string GetDecimalSeparatorDescription(int decimalSeparator)
		{
			return GetDecimalSeparatorSafe(decimalSeparator).description;
		}	
		
		static SeparatorInfo GetDecimalSeparatorSafe(int decimalSeparator)
		{
			decimalSeparator = Mathf.Clamp(decimalSeparator, 0, decimals.Length - 1);
			return decimals[decimalSeparator];
		}
		
		public string GetPointIndexFormatDescription(string pointIndexFormat)
		{
			foreach (SeparatorInfo pointIndexFormatInfo in config.defaultPointIndexFormats)
			{
				if (pointIndexFormat == pointIndexFormatInfo.separator)
					return pointIndexFormatInfo.description;
			}

			return $"'{pointIndexFormat}'";
		}

		public string GetSeparatorDescription(string separator)
		{
			foreach (SeparatorInfo pointIndexFormatInfo in config.defaultPointIndexFormats)
			{
				if (separator == pointIndexFormatInfo.separator)
					return pointIndexFormatInfo.description;
			}

			return $"'{separator}'";
		}

		public string GetHeightDescription(string height)
		{
			foreach (SeparatorInfo pointIndexFormatInfo in config.defaultPointIndexFormats)
			{
				if (height == pointIndexFormatInfo.separator)
					return pointIndexFormatInfo.description;
			}
			
			return $"'{height}'";
		} 
	}
}