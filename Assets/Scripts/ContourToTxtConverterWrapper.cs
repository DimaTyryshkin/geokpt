using Geo.Data;
using Geo.KptData;
using Geo.KptData.Converters;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo
{
	public class ContourToTxtConverterWrapper
	{
		readonly AccountData.ContourToTxtConverterPreferences preferences;
		
		public int PreferredDecimalSeparatorIndex
		{
			get => preferences.decimalSeparator;
			set
			{
				if (value >= ContourToTxtConverter.decimals.Length || value < 0)
				{
					preferences.decimalSeparator = 0;
				}
				else
				{
					preferences.decimalSeparator = value;
				}
			}
		}

		public ContourToTxtConverterWrapper(AccountData.ContourToTxtConverterPreferences preferences)
		{
			this.preferences               = preferences;
			PreferredDecimalSeparatorIndex = preferences.decimalSeparator;//вызываем валидацю
		}

		public string ConvertToString(IContour contour, IParcel parcel)
		{
			Assert.IsNotNull(contour);
			Assert.IsNotNull(parcel);
			 
			var converter = new ContourToTxtConverter(); 
			return converter.ConvertToString(contour, parcel, PreferredDecimalSeparatorIndex, preferences.format);
		}

		public string ConvertToFile(string folderToSaveFile, IContour contour, IParcel parcel)
		{ 
			Assert.IsNotNull(contour);
			Assert.IsNotNull(parcel);
			
			var converter = new ContourToTxtConverter(); 
			return converter.ConvertToFile(folderToSaveFile, contour, parcel, PreferredDecimalSeparatorIndex, preferences.format);
		}
	}
}