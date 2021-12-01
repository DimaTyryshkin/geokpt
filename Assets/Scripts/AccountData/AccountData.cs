using System;
using System.Collections.Generic;
using Geo.Data.AnalyticsDb;

namespace Geo.Data
{
	public class AccountData
	{
		[Serializable]
		public class Metadata
		{
			public string appVersion;
			public string writeLocalTime;

			public void FillData()
			{
				appVersion = appVersion;
				writeLocalTime = DateTime.Now.ToString("G");
			}
		}

		

		/// <summary>
		/// Никогда нельзя изменять имя переменной, ее тип или удалять ее!
		/// </summary>
		public Metadata metadata = new Metadata();

		public List<string>   lastOpenedFilePaths  = new List<string>();
		public List<string>   completedTutorialsId = new List<string>();
		public AppAnalyticsDb appAnalytics         = new AppAnalyticsDb();

		public void Validate()
		{
			if (metadata == null)
				metadata = new Metadata();
			
			if (lastOpenedFilePaths == null)
				lastOpenedFilePaths = new List<string>();
			
			if (completedTutorialsId == null)
				completedTutorialsId = new List<string>();

			if (appAnalytics == null)
				appAnalytics = new AppAnalyticsDb();
		}
	}
}