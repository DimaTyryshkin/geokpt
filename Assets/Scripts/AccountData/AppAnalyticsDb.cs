using System;

namespace Geo.Data.AnalyticsDb
{
	public enum UserSegment
	{
		firstRun           = 0,
		noOneExport        = 1,
		severalTimesExport = 2,
		manyExport         = 3,
	}

	[Serializable]
	public class AppAnalyticsDb
	{
		public UserSegment userSegment = UserSegment.firstRun;
		public int successExportNumber = 0;
		public int sessionNumber = 0;
	}
}