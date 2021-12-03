using System;
using System.Collections.Generic; 
using System.IO;
using System.Linq;
using System.Xml;
using Geo.KptData;
using Geo.KptData.Converters;
using NUnit.Framework;
using SiberianWellness.Common;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{ 
	public abstract class KptVersionTest
	{
		readonly static string dataFolderTemplate = "GeoData/DataForTest/KptExamples/Kpt{0}/";
		readonly        string tempFolder         = "GeoData/DataForTest/Temp/";

		string DataFolder => string.Format(dataFolderTemplate, KptVersionName);
		
		protected abstract string KptVersionName              { get; }
		protected abstract string KptFileName                 { get; }
		protected abstract string KptTestDataFileName         { get; }
		protected abstract string ReferenceParcelDataFileName { get; }
		protected abstract string CadastralNumber             { get; }

		protected abstract IKpt GetReader(XmlDocument doc);

		protected KptTestData GetTestData()
		{
			string[] jsonLines = File.ReadAllLines(DataFolder + KptTestDataFileName);
			string json = "";
			foreach (var line in jsonLines)
			{
				int index = line.IndexOf("//");
				if (index >= 0)
					json += line.Substring(0, index) + Environment.NewLine;
				else
					json += line + Environment.NewLine;
			}

			//string json = File.ReadAllText(DataFolder + KptTestDataFileName);
			KptTestData testData = JsonUtility.FromJson<KptTestData>(json);
			return testData;
		}

		XmlDocument GetDocument()
		{
			string KptFileFullName = DataFolder + KptFileName;
			var    doc             = new XmlDocument();
			doc.Load(KptFileFullName);
			return doc;
		}

		IKpt GetReader()
		{
			var doc = GetDocument();
			return GetReader(doc);
		}

		[Test]
		public void TT()
		{

			var doc = GetDocument();
			var kpt = GetReader(doc);
			foreach (var parcel in kpt.GetAllParcels())
			{
				var contours = parcel.GetContours();
				if (contours.Count == 1) 
					continue;

				foreach (var contour in contours)
				{
				
						Debug.Log(contour.ID);
				}
			}
		}

		[Test]
		public void PrintAllPaths()
		{
			Dictionary<string, int> allPaths = new Dictionary<string, int>();
			Dictionary<string, string> allNamespaces = new Dictionary<string, string>();

			void Add(string key)
			{
				if (allPaths.ContainsKey(key))
					allPaths[key]++;
				else
					allPaths[key] = 1;
			}

			var doc = GetDocument(); 
			foreach (var node in doc.GetElementsByTagName("*"))
			{
				var n = node as XmlNode;

				string fullPath = n.FullPath();

				Add(fullPath);

				foreach (var a in n.Attributes)
				{
					XmlAttribute attribute     = a as XmlAttribute;
					string       attributePath = $"{fullPath} [{attribute.Name}]";
					Add(attributePath);

					if (attribute.Name.StartsWith("xmlns"))
						allNamespaces.Add(attribute.Name, attribute.Value);
				}
			}

			
			string msgNamespaces = allNamespaces
				.ToStringMultiline(s => $"{s.Key}={s.Value}");
			
			string msg = allPaths
				.OrderBy(a => a.Key)
				.ToStringMultiline(s => $"{s.Key}={s.Value}");
			File.WriteAllText(DataFolder + "allXmlPaths.txt", msgNamespaces + msg);
		}
		
		[Test]
		public void PrintContourCount()
		{
			IKpt kptReader = GetReader();
			
			List<IParcel> parcels = kptReader.GetAllParcels();
			string        msg     =  $"Файл '{KptFileName}'" + Environment.NewLine;
			
			int           n       = 0;
			int           n2       = 0;
			 
			foreach (var parcel in parcels)
			{
				List<IContour> countors = parcel.GetContours();
				//if (countors.Count!=0 && countors[0].GetPoints().Count == 0)
				if (countors.Count !=1)
				{
					msg += parcel.GetCadastralNumber() + " contour.count="+ countors.Count+ Environment.NewLine;
					n++;
					n2 += Math.Max(0, countors.Count - 1);
				}
			}

			if (n > 0)
				Debug.Log(msg + Environment.NewLine + "additional contours count ="+ n2);
		}

		[Test]
		public void Structure()
		{
			var  testData  = GetTestData();
			IKpt kptReader = GetReader();
			testData.AssertIt(kptReader);
		}
		
		//TODO добавить больше тестов в пкт09

		[Test]
		/// <summary>
		/// Полная проверка конвертации в текстовый файл
		/// </summary>   
		public void TxtConverter()
		{
			Assert.IsFalse(CadastralNumber.Contains("_"), "Замени в кадастровом номере символ '_' на символ ':'");
			
			IKpt kptReader = GetReader();
			
			string referenceTxtDataFullName = DataFolder + ReferenceParcelDataFileName;

			CreateTempFolder(tempFolder);

			IParcel parcel = kptReader.FindParcelByCadastralNumber(CadastralNumber);
			Assert.IsNotNull(parcel, CadastralNumber);


			string referenceTxt = File.ReadAllText(referenceTxtDataFullName);

			string format = "pt{i},{x},{y}";
			//В качестве разделителя дробной части 'точка' 
			ContourToTxtConverter converter = new ContourToTxtConverter();
			Assert.AreEqual(referenceTxt, converter.ConvertToString(parcel.GetContours()[0], parcel,0, format));

			string fileName = converter.ConvertToFile(tempFolder,parcel.GetContours()[0], parcel,0, format);
			string result   = File.ReadAllText(fileName);
			Assert.AreEqual(referenceTxt, result);
		}

		void CreateTempFolder(string tempFolder)
		{
			DirectoryInfo d = new DirectoryInfo(tempFolder);

			if (!d.Exists)
				d.Create();
		}
	}
}