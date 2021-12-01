using UnityEngine;
using UnityEditor;  

using Geo.Data;  

namespace Geo
{
	public static class EditorMenu
	{ 
		[MenuItem("Geo/Log Save")]
		public static void LogSave()
		{
			PlayerPrefsWrapper playerPrefsWrapper = new PlayerPrefsWrapper();
			AccountDataStorage storage            = new AccountDataStorage(playerPrefsWrapper);
			Debug.Log(storage.GetRawSaveData());
		}
		
		[MenuItem("Geo/Delete Save")]
		public static void DeleteSave()
		{
			PlayerPrefs.DeleteAll();
		}
		
		[MenuItem("Geo/Open Persistent")]
		public static void OpenPersistent()
		{
			Application.OpenURL(Application.persistentDataPath);
		}
	}
}