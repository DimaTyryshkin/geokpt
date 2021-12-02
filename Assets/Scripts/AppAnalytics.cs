using System;
using System.Collections.Generic;
using System.IO; 
using Geo.Data.AnalyticsDb;
using SiberianWellness.Common;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Assertions;
  

namespace Geo
{
	public class AppAnalytics
	{
		AppAnalyticsDb data;

		bool enable;
		bool log = false;
		
		float timeOpenFilePicker;

		public int SessionNumber => data.sessionNumber;
 
		public AppAnalytics(AppAnalyticsDb data)
		{
			Assert.IsNotNull(data);
			this.data = data;

			enable = true;
#if UNITY_EDITOR
			enable = false;
#else
			enable = !Debug.isDebugBuild;
#endif
			Analytics.enabled             = enable;
			Analytics.initializeOnStartup = enable;
			Analytics.deviceStatsEnabled  = enable;
			PerformanceReporting.enabled  = enable;
		}

		public void StartApp()
		{
			data.sessionNumber++;
			Event("start_app", new Dictionary<string, object>()
			{
				{"user_segment", data.userSegment.ToString()},
				{"session_number",  data.sessionNumber}
			});

			if (data.userSegment == UserSegment.firstRun)
				data.userSegment = UserSegment.noOneExport;
		}

		public void OpenFilePicker()
		{
			timeOpenFilePicker = Time.realtimeSinceStartup;
			Step("open_file_picker",null);
		}

		public void CancelFilePicker()
		{
			float timeInPicker = Time.realtimeSinceStartup - timeOpenFilePicker;

			Step("cancel_file_picker",
				"time_in_picker", RoundTime(timeInPicker));
		}

		public void TryOpenFileByPicker(string fileName)
		{
			float    timeInPicker = Time.realtimeSinceStartup - timeOpenFilePicker;
			FileInfo f            = new FileInfo(fileName);

			Step("try_open_file_by_picker",
				"extension", f.GetFullExtension(),
				"time_in_picker", RoundTime(timeInPicker));
		}

		public void TryOpenFileFromCache(string fileName, int filesInCache)
		{
			FileInfo f = new FileInfo(fileName);

			Step("try_open_file_from_cache",
				"extension", f.GetFullExtension(),
				"files_in_cache", filesInCache);
		}

		public void FailLoadFile(Exception e, string filePath)
		{
			FileInfo f = new FileInfo(filePath);

			Step("fail_load_file",
				"extension", f.GetFullExtension(),
				"exception", e.Message);
		}

		public void SuccessLoadFile(string fileName)
		{
			FileInfo f = new FileInfo(fileName);

			Step("success_load_file",
				"extension", f.GetFullExtension(),
				"file_name", f.Name);
		}

		public void SelectParcel(string parcelFilter)
		{
			Step("select_parcel",
				"parcel_filter", parcelFilter,
				"parcel_filter_length", parcelFilter.Length
			);
		}

		public void TryExportContour()
		{
			ContourToTxtConverterWrapper converter            = new ContourToTxtConverterWrapper();
			string                       lastDecimalSeparator = converter.decimals.Separator.Replace(" ", "_");
			string                       lastSeparator        = converter.separator.Separator.Replace(" ", "_");

			Step("try_export_contour", new Dictionary<string, object>()
			{
				{"decimal_separator", $"'{lastDecimalSeparator}'"},
				{"separator", $"'{lastSeparator}'"},
				{"success_export_count", data.successExportNumber},
				{"user_segment", data.userSegment.ToString()}
			});
		}

		public void SuccessExportContour()
		{
			data.successExportNumber++;

			Step("success_export_contour",
				"success_export_count", data.successExportNumber);

			data.userSegment = UserSegment.severalTimesExport;

			if (data.successExportNumber >= 30)
				data.userSegment = UserSegment.manyExport;
		}
 
		void Step(string name, string key, object value)
		{
			Step(name, new Dictionary<string, object>()
				{
					{key, value}
				}
			);
		}

		void Step(string name, string key1, object value1, string key2, object value2)
		{
			Step(name, new Dictionary<string, object>()
				{
					{key1, value1},
					{key2, value2}
				}
			);
		}

		void Step(string name, Dictionary<string, object> data)
		{
			Event("step_" + name, data);
		}

		void Event(string name, Dictionary<string, object> data)
		{
			if (log)
			{
				string log = $"AppAnalytics '{name}'" + Environment.NewLine;
				if (data != null)
				{
					foreach (KeyValuePair<string, object> d in data)
						log += $"	{d.Key}: {d.Value}" + Environment.NewLine;
				}

				Debug.Log(log);
			}

			if (enable)
			{
				AnalyticsEvent.Custom(name, data);           //Unity аналитика
				AppMetrica.Instance.ReportEvent(name, data); //Yandex аналитика
			}
		}
 
		string RoundTime(float seconds)
		{
			return RoundingUtil.SecondTime(seconds);
		} 
	}
}