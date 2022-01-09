using System;
using UnityEngine;
using UnityEditor;  

using Geo.Data;  

namespace Geo
{
	public static class EditorMenu
	{ 
		[MenuItem ("CONTEXT/MonoBehaviour/Set name to GO")]
		public static void SetNameToGO(MenuCommand command)
		{
			var     mb   = command.context as MonoBehaviour;
			if (mb)
			{
				string name = mb.GetType().Name;
				name = Char.ToLower(name[0]) + name.Substring(1);
				Undo.RecordObject(mb.gameObject, "rename");
				mb.gameObject.name = name;
			}
		}
		
		[MenuItem("Geo/Log Save")]
		public static void LogSave()
		{
			PlayerPrefsWrapper playerPrefsWrapper = new PlayerPrefsWrapper();
			AccountDataStorage storage            = new AccountDataStorage(playerPrefsWrapper);
			Debug.Log(storage.GetRawSaveData());
		}
		
		[MenuItem("Geo/Log GeoConfig")]
		public static void LogGeoConfig()
		{
			string geoConfigText = JsonUtility.ToJson(new GeoConfig(), true);
			Debug.Log(geoConfigText);
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