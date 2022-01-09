using System;
using System.Collections.Generic;
using Geo.Data.AnalyticsDb;
using UnityEngine;
using UnityEngine.Serialization;

namespace Geo.Data
{
	public class AccountData
	{ 
		/// <summary>
		/// Никогда нельзя изменять имя переменной, ее тип или удалять ее!
		/// </summary>
		public Metadata metadata = new Metadata();

		public PrivacyPolicyAndTermsConditions  privacyPolicy                    = new PrivacyPolicyAndTermsConditions();
		public List<string>                     completedTutorialsId             = new List<string>();
		public ContourToTxtConverterPreferences contourToTxtConverterPreferences = new ContourToTxtConverterPreferences();
		public ContourToTxtConverterPreferences2 contourToTxtConverterPreferences2 = new ContourToTxtConverterPreferences2();
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
			
			if (contourToTxtConverterPreferences2 == null)
				contourToTxtConverterPreferences2 = new ContourToTxtConverterPreferences2();
			
			if (privacyPolicy == null)
				privacyPolicy = new PrivacyPolicyAndTermsConditions();
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
		
		/// <summary>
		/// Данные для универсального формата
		/// </summary>
		[Serializable]
		public class ContourToTxtConverterPreferences
		{
			public int    decimalSeparator;
			public string format;
			public List<string> userFormats;

			public ContourToTxtConverterPreferences()
			{
				decimalSeparator = 0;
				format           = "pt(i), (x), (y)";
				userFormats      = new List<string>();
			}
		}
		
		/// <summary>
		/// Данные для понятного формата
		/// </summary>
		[Serializable]
		public class ContourToTxtConverterPreferences2
		{
			public int          decimalSeparator;
			public bool         height;
			public string       separator;
			public string       pointIndexFormat;
			public List<string> userSeparators;

			public ContourToTxtConverterPreferences2()
			{
				decimalSeparator = 0;
				height           = false;
				separator        = ", ";
				pointIndexFormat = "pt(i)";
				userSeparators   = new List<string>();
			}
		}

		[Serializable]
		public class PrivacyPolicyAndTermsConditions
		{
			public int acceptedVersion;

			/// <summary>
			/// Текущая редакция 'Privacy Policy' и 'Terms and Conditions'
			/// </summary>
			public static readonly int actualVersion = 1;

			public bool NeedAccept => acceptedVersion != actualVersion;

			public PrivacyPolicyAndTermsConditions()
			{
				acceptedVersion = 0;
			}

			public void OnAccept()
			{
				acceptedVersion = actualVersion;
			}
		}
	}
}