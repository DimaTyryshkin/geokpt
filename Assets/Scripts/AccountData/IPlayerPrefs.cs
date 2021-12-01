using UnityEngine;

namespace Geo.Data
{
	public interface IPlayerPrefs
	{
		string GetString(string key, string defaultValue);
		void   SetString(string key, string value);
		void   Save();
	}

	public class PlayerPrefsWrapper : IPlayerPrefs
	{
		public string GetString(string key, string defaultValue)
		{
			if (!PlayerPrefs.HasKey(key)) //удалять нельзя! UnityEngine.PlayerPrefs не умеет нормально работать если defaultValue==null 
				return defaultValue;

			return PlayerPrefs.GetString(key);
		}

		public void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public void Save()
		{
			PlayerPrefs.Save();
		}
	}
}