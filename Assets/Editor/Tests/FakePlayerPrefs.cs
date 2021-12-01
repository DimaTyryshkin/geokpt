using System.Collections.Generic;
using Geo.Data;

namespace Tests
{
	class FakePlayerPrefs : IPlayerPrefs
	{
		public Dictionary<string, string> save = new Dictionary<string, string>();
		
		public string GetString(string key, string defaultValue)
		{
			if (save.ContainsKey(key))
				return save[key];

			return defaultValue;
		}

		public void SetString(string key, string value)
		{
			save[key] = value;
		}

		public void Save()
		{ 
		}
	}
}