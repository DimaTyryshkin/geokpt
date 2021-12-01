using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo.Data
{
	public sealed class FilesCache
	{
		static public readonly string cacheDirName = "kptCache";
		readonly List<string> cachedFiles;
		readonly IFileSystem  fileSystem; 
		readonly string cacheDirPath;

		public IReadOnlyList<string> Files => cachedFiles;

		public FilesCache(IFileSystem fileSystem, string persistentDataPath)
		{
			Assert.IsNotNull(fileSystem);
			this.fileSystem = fileSystem;
			cachedFiles     = new List<string>();

			cacheDirPath = Path.Combine(persistentDataPath, cacheDirName);
			fileSystem.CreateDirectoryIfNotExist(cacheDirPath);
			Debug.Log($"cacheDir '{cacheDirPath}'");
		}

		public void Load()
		{
			string[] files = fileSystem.GetFiles(cacheDirPath);
			cachedFiles.Clear();
			cachedFiles.AddRange(files);
		}

		public string CacheFile(string path)
		{
			Assert.IsFalse(string.IsNullOrWhiteSpace(path));
			
			Load();
			
			if (cachedFiles.Contains(path))
				return path; //Файл из кеша нельзя кешить 
			
			string fileName = Path.GetFileName(path);
			Assert.IsNotNull(fileName);
			
			string newFullFileName = Path.Combine(cacheDirPath, fileName);
			fileSystem.CopyFile(path, newFullFileName );
			cachedFiles.Add(newFullFileName);
			return newFullFileName;
		}

		public void ClearCache()
		{
			fileSystem.RemoveDir(cacheDirPath);
			fileSystem.CreateDirectoryIfNotExist(cacheDirPath);
		}
	}
}