using Geo.Data;
using Geo.KptData;
using Geo.KptData.Converters;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo
{
	public class ContourToTxtConverterFactory
	{
		readonly AccountData.ContourToTxtConverterPreferences preferences;

		public ContourToTxtConverterFactory(AccountData.ContourToTxtConverterPreferences preferences)
		{
			this.preferences = preferences;
		}

		public ContourToTxtConverterBase Creat()
		{ 
			var converter = new ContourToTxtConverter(preferences.decimalSeparator, preferences.format);
			return converter;
		}
	}
}