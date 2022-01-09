using UnityEngine.Assertions;

using Geo.UI;
using Geo.Data;
using Geo.KptData.Converters;

namespace Geo
{
	public class ContourToTxtConverterFactory
	{
		readonly AccountData.ContourToTxtConverterPreferences  preferences;
		readonly AccountData.ContourToTxtConverterPreferences2 preferences2;
		readonly CoordinateFormats2Config                       config;

		readonly CoordinateFormats2 formats;

		public CoordinateFormats2 Formats => formats;


		public ContourToTxtConverterFactory(AccountData.ContourToTxtConverterPreferences preferences, AccountData.ContourToTxtConverterPreferences2 preferences2, CoordinateFormats2Config config)
		{
			Assert.IsNotNull(preferences);
			Assert.IsNotNull(preferences2);
			Assert.IsNotNull(config);

			this.preferences  = preferences;
			this.preferences2 = preferences2;
			this.config       = config;

			formats = new CoordinateFormats2(config, preferences2);
		}

		public ContourToTxtConverterBase Creat()
		{
			var converter = new ContourToTxtConverter2(CoordinateFormats2.GetDecimalSeparator(preferences2.decimalSeparator), preferences2.separator, preferences2.pointIndexFormat, preferences2.height);
			return converter;
		}
	}
}