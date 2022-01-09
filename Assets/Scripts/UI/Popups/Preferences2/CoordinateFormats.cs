using System.Linq;
using Geo.Data;
using Geo.KptData.Converters;
using UnityEngine.Assertions;

namespace Geo.UI
{
	public class CoordinateFormats
	{
		readonly CoordinateFormats2Config                      config;
		readonly AccountData.ContourToTxtConverterPreferences2 data;
		
		public string[] GetAvailableSeparator()
		{
			return config.defaultSeparators
				.Select(s => s.separator)
				.Concat(data.userSeparators)
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

		public CoordinateFormats(CoordinateFormats2Config config, AccountData.ContourToTxtConverterPreferences2 data)
		{
			Assert.IsNotNull(config);
			Assert.IsNotNull(data);
			this.config = config;
			this.data   = data;
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