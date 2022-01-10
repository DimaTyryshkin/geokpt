using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using Geo.KptData.KptReader.Kpt10Excerpt;
using Geo.KptData.KptReaders.Kpt10;
using Geo.KptData.KptReaders.Kpt11; 
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Geo.KptData.KptReaders
{
	public class KptFile
	{ 
		FileInfo file;
		
		List<IParcel> allParcels = new List<IParcel>();

		public IReadOnlyList<IParcel> Parcels => allParcels;
		public bool SaveUnzippedFile { get; set; }

		public event UnityAction<Exception> exceptionOnLoading;
 
		public KptFile(FileInfo file)
		{
			Assert.IsNotNull(file);
			Assert.IsTrue(file.Exists, $"File '{file.FullName}' not exist");
			
			this.file = file;
		}

		public static string TrimEndKptExtensions(string fileName)
		{
			return fileName
				.Replace(".zip", "")
				.Replace(".xml", "")
				.Replace(".sig", "");
		}

		public void LoadAllParcels()
		{
			UnityAction<TextReader, string> loadParcels     = null;
			bool                    catchExceptions = true;

#if UNITY_EDITOR
			catchExceptions = false;
#endif
			if (catchExceptions)
				loadParcels = LoadParcels_CatchExceptionDecorator;
			else
				loadParcels = LoadParcels;


			if (file.Extension == ".xml")
			{
				using (var stream = file.OpenText())
				{
					string allText = stream.ReadToEnd();
					loadParcels(new StringReader(allText), file.Name);
				}
			}
			else if (file.Extension == ".zip")
			{
				using (FileStream fs = file.OpenRead())
				using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read))
				{
					OpenZipRecursively(zip, loadParcels);
				}
			}
			else
			{
				var e = new NotSupportedException($"File '{file.Name}' with Extension '{file.Extension}' not supported");
				exceptionOnLoading?.Invoke(e);
			}
		}

		void OpenZipRecursively(ZipArchive zip, UnityAction<TextReader, string> loadParcels)
		{ 
			foreach (var entry in zip.Entries)
			{
				if (Path.GetExtension(entry.FullName) == ".xml")
				{
					using (StreamReader sr = new StreamReader(entry.Open()))
					{
						string xml = sr.ReadToEnd();
						if (SaveUnzippedFile)
						{
							string fullName = file.Directory.FullName + "/" + entry.FullName;
							File.WriteAllText(fullName, TryFormatXml(xml));
						}

						loadParcels(new StringReader(xml), entry.FullName);
					}
				}
				else if (Path.GetExtension(entry.FullName) == ".zip")
				{ 
					using (var nestedZip = entry.Open())
					{
						OpenZipRecursively(new ZipArchive(nestedZip, ZipArchiveMode.Read), loadParcels);
					}
				}
			}
		}

		void LoadParcels_CatchExceptionDecorator(TextReader textReader, string fileName)
		{
			try
			{
				LoadParcels(textReader, fileName);
			}
			catch (Exception e)
			{
				exceptionOnLoading?.Invoke(e);
				Debug.LogError(e); 
			}
		}

		void LoadParcels(TextReader textReader, string fileName)
		{ 
			XmlDocument doc = GetXml(textReader);
			IKpt        kpt = GetKpt(doc, fileName);
			LoadParcels(kpt); 
		}

		XmlDocument GetXml(TextReader textReader)
		{
			DateTime t = DateTime.Now;
			var xmlDoc = new XmlDocument();
			
			//TODO что если xml не парсится
			xmlDoc.Load(textReader);
			textReader.Close();
			textReader.Dispose();
			
			Debug.Log("Load xml time="+(DateTime.Now -t).ToString("g"));
			return xmlDoc;
		}

		IKpt GetKpt(XmlDocument xmlDoc, string fileName)
		{   
			IKpt   kptReader;
			XmlElement docElement = xmlDoc.DocumentElement;
			string rootName = docElement.Name;
			if (rootName == "KPT")
			{
				string attribute = docElement.GetAttribute("xmlns");
				if (attribute.Contains("x-artefacts-rosreestr-ru/outgoing/kpt/9"))
					kptReader = new Kpt09Reader(xmlDoc);
				else if(attribute.Contains("x-artefacts-rosreestr-ru/outgoing/kpt/10"))
					kptReader = new Kpt10Reader(xmlDoc);
				else
					throw new NotSupportedException($"File '{fileName}' with docName '{xmlDoc.Name}' with xml root name '{rootName}' and root namespace '{attribute}' not supported");
			}
			else if (rootName == "KVZU")//Кадастрвая выписка
			{
				string attribute = docElement.GetAttribute("xmlns");
				if (attribute.Contains("urn://x-artefacts-rosreestr-ru/outgoing/kvzu/7.0.1"))
					kptReader = new Kpt10ExcerptReader(xmlDoc);
				else
					throw new NotSupportedException($"File '{fileName}' with docName '{xmlDoc.Name}' with xml root name '{rootName}' and root namespace '{attribute}' not supported");
			}
			else if (rootName == "extract_cadastral_plan_territory")
				kptReader = new Kpt11Reader(xmlDoc);
			else
				throw new NotSupportedException($"File '{fileName}' with docName '{xmlDoc.Name}' with xml root name '{rootName}' not supported"); //TODO 

			return kptReader; 
		}

		void LoadParcels(IKpt kpt)
		{
			var allParcelsList = kpt.GetAllParcels();

			foreach (var p in allParcelsList)
				allParcels.Add(p);

			//TODO что если открыли левый файл и в нет нет парселей вообще, надо бы сказать пользователю об этом. 
		}

		string TryFormatXml(string xml)
		{
			try
			{
				XDocument doc = XDocument.Parse(xml);
				return doc.ToString();
			}
			catch (Exception)
			{
				// Handle and throw if fatal exception here; don't just ignore them
				return xml;
			}
		}
	}
}