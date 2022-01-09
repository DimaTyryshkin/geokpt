using System; 

namespace Geo
{
	[Serializable]
	public class GeoConfig
	{
		public string[] defaultContourToTxtFormats;

		public CoordinateFormats2Config coordinateFormats2Config; 
		
		public GeoConfig()
		{
			defaultContourToTxtFormats = new[]
			{
				"(x), (y)",
				"pt(i), (x), (y)",
				"pt(i), (x), (y), 0",
			};

			coordinateFormats2Config = new CoordinateFormats2Config();
		}
	}
}