using System;
using System.IO;
using Geo.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace Geo.Data
{
	public class DefaultKptFiles
	{
		string[] files = new[] 
		{
			"70_14_0109003_2020-09-23_kpt11.xml.sig.zip",
			"77_01_0003031_2020-05-14_kpt10.xml.zip",
			"78_34_0436501_2021-06-02_kpt11.xml.zip",
			"91_01_049001_2020-03-27_kpt10.xml.zip",
		};

		string dir = "DefaultKpt";
		
		public void CopyToCache(FilesCache cache)
		{
#if UNITY_EDITOR
			CopyToCacheInternal(cache);
#else
			try
			{
				CopyToCacheInternal(cache);
			}
			catch(Exception e)
			{
				Debug.LogError(e);
			}
#endif
		}

		void CopyToCacheInternal(FilesCache cache)
		{
			foreach (var file in files)
			{
				string dirPath = Path.Combine(Application.streamingAssetsPath, dir, file);

				string newFileName = CopyToTemporary(dirPath);
				if (!string.IsNullOrEmpty(newFileName))
					cache.CacheFile(newFileName);
			}
		}

		string CopyToTemporary(string fullPath)
		{ 
			Debug.Log(fullPath);
			var loadingRequest = UnityWebRequest.Get(fullPath);
			loadingRequest.SendWebRequest();
			while (!loadingRequest.isDone)
			{
				if (loadingRequest.isNetworkError || loadingRequest.isHttpError)
				{
					break;
				}
			}

			Debug.Log(loadingRequest.error);
			bool success = !loadingRequest.isNetworkError && !loadingRequest.isHttpError;
			if (success)
			{
				DirectoryInfo dir = new DirectoryInfo(Path.Combine(Application.temporaryCachePath, "kptTemp"));
				dir.Create();
				
				string newFileName = Path.Combine(dir.FullName, Path.GetFileName(fullPath));
				File.WriteAllBytes(newFileName, loadingRequest.downloadHandler.data);
				return newFileName;
			}
			else
			{
				return null;
			}
		}
	}
}