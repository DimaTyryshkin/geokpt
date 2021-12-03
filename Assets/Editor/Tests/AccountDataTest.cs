using System.IO;

using UnityEngine;
using Geo.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class AccountDataTest
	{
		//string testJsonFileOutput = "GeoData/DataForTest/AccountData/output.txt";
		string testJsonFileInput1 = "GeoData/DataForTest/AccountData/input1.txt";
		//string testJsonFileInput2 = "GeoData/DataForTest/AccountData/input2.txt";
		string testJsonFileInput_forDif = "GeoData/DataForTest/AccountData/forDif.txt";
 
		

		[Test]
		public void ValidJson()
		{
			string      json = File.ReadAllText(testJsonFileInput1);
			AccountData data = AccountDataStorage.LoadFromJson(json);

			Assert.IsNotNull(data);
			//Assert.IsNotNull(data.lastOpenedFilePaths);
			//Assert.AreEqual(2, data.lastOpenedFilePaths.Count);

		}
		
		[Test]
		public void JsonToDataToJson()
		{
			string      json1 = File.ReadAllText(testJsonFileInput1);
			AccountData data = AccountDataStorage.LoadFromJson(json1);
			string json2 = AccountDataStorage.ToJson(data);

			if(json1!=json2)
				File.WriteAllText(testJsonFileInput_forDif,json2);
			
			Assert.AreEqual(json1, json2);
		}

		[Test]
		public void EmptyJson()
		{
			AccountData data1 = AccountDataStorage.LoadFromJson("{}");
			Assert.IsNotNull(data1);

			AccountData data2 = new AccountData();
			//data2.Validate();

			string json1 = AccountDataStorage.ToJson(data1);
			string json2 = AccountDataStorage.ToJson(data2);

			Debug.Log(json1);
			Assert.AreEqual(json1, json2);
		}

		[Test]
		public void FileCache()
		{
			string path1 = "test1/test1";
			string path2 = "test2/test2";
			 
			FakeFileSystem fileSystem = new FakeFileSystem(); 
			fileSystem.AddAvailablePath(path1, path2);
			
			//Добавляем пару файликов 
			FilesCache fileCache = new FilesCache(fileSystem, "cachePath");
			fileCache.Load();

			fileCache.CacheFile(path1);
			fileCache.CacheFile(path2);
			Assert.IsTrue(fileCache.Files.Count == 2);
  
			
			//Перезагружаем
			fileCache = new FilesCache(fileSystem, "cachePath");
			fileCache.Load();
			Assert.AreEqual(2,fileCache.Files.Count);
			Assert.AreEqual(path1, fileCache.Files[0]);
			Assert.AreEqual(path2, fileCache.Files[1]);

			//Првоеряем что удаленный файл исчез из списка
			fileSystem.paths.Remove(path1);
			fileCache.Load();
			Assert.AreEqual(1,fileCache.Files.Count);
			Assert.AreEqual(path2, fileCache.Files[0]);
		}
	}
}