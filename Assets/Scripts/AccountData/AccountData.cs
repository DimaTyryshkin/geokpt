using System;
using System.Collections.Generic;
using Geo.Data.AnalyticsDb;
using UnityEngine;

namespace Geo.Data
{
	public class AccountData
	{ 
		/// <summary>
		/// Никогда нельзя изменять имя переменной, ее тип или удалять ее!
		/// </summary>
		public Metadata metadata = new Metadata();

		public List<string>                     completedTutorialsId             = new List<string>();
		public ContourToTxtConverterPreferences contourToTxtConverterPreferences = new ContourToTxtConverterPreferences();
		public AppAnalyticsDb                   appAnalytics                     = new AppAnalyticsDb();

		public void Validate()
		{
			if (metadata == null)
				metadata = new Metadata();
			
			if (completedTutorialsId == null)
				completedTutorialsId = new List<string>();

			if (appAnalytics == null)
				appAnalytics = new AppAnalyticsDb();

			if (contourToTxtConverterPreferences == null)
				contourToTxtConverterPreferences = new ContourToTxtConverterPreferences();
		}
		
		[Serializable]
		public class Metadata
		{
			public string appVersion;
			public string writeLocalTime;

			public void FillData()
			{
				appVersion     = Application.version;
				writeLocalTime = DateTime.Now.ToString("G");
			} 
		}
		
		[Serializable]
		public class ContourToTxtConverterPreferences
		{
			public int    decimalSeparator;
			public string format;
			public List<string> userFormats;

			public ContourToTxtConverterPreferences()
			{
				decimalSeparator = 0;
				format           = "pt{i}, {x}, {y}";
				userFormats      = new List<string>();
			}
		}
	}
}