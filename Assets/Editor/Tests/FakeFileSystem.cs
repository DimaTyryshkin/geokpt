using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Geo.Data;
using NUnit.Framework;

namespace Tests
{
	class MytestSearch_max
	{
		[Test]
		public void TestSearch_max()
		{
			int[] data_1 = new int[]{1, 2, 3, 4, 5};
			int[] data_2 = new int[]{3, 5, 12, 17, 0};
			int[] data_3 = new int[]{17, 6, 9};
			int[] data_4 = new int[]{9, 3, 5, 7, 1, 11, 15};
			int[] data_5 = new int[]{1, 2, 3, -4, -125, 11, 99, 34, 0, 1};
			Assert.AreEqual(Max(data_1), 4);
			Assert.AreEqual(Max(data_2), 3);
			Assert.AreEqual(Max(data_3), 0);
			Assert.AreEqual(Max(data_4), 6);
			Assert.AreEqual(Max(data_5), 6);
		}
		
		int Max(int[] data)
		{
			int id = 0, max_size = data[0];
			for (int i = 1; i < data.Length; i++)
			{
				if (data[i] > max_size)
				{
					max_size = data[i];
					id = i;
				}
			}
			return id;
		}
	}
	class FakeFileSystem : IFileSystem
	{
		public List<string> paths = new List<string>();

		public void AddAvailablePath(params string[] paths)
		{
			this.paths.AddRange(paths);
		}
  
		public bool FileExists(string path)
		{
			return paths.Contains(path);
		}

		public bool DirExists(string path)
		{
			throw new System.NotImplementedException();
		}

		public string[] GetFiles(string path)
		{
			return paths.ToArray();
		}

		public void CreateDirectoryIfNotExist(string path)
		{
			
		}

		public void MoveFile(string path, string newFullFileName)
		{
			int index = paths.FindIndex(f => f == path);

			if (index >= 0)
			{
				paths[index] = newFullFileName;
			}
			else
			{
				throw new FileNotFoundException(path);
			}
		}

		public void CopyFile(string path, string newFullFileName)
		{
			if(paths.Contains(newFullFileName))
				throw  new InvalidOperationException();
			
			paths.Add(newFullFileName);
		}

		public void RemoveDir(string cacheDirPath)
		{
			throw new System.NotImplementedException();
		}
	}
}