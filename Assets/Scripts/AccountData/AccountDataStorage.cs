using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo.Data
{
	public interface IStorage
	{
		void Save();
	}

	public class AccountDataStorage:IStorage
	{
		readonly string bsdKey = "accountJsonData"; 
		
		AccountData     data;
		IPlayerPrefs playerPrefs;

		

		public AccountDataStorage(IPlayerPrefs playerPrefs)
		{
			Assert.IsNotNull(playerPrefs);
			this.playerPrefs = playerPrefs;
		}

		public string GetRawSaveData()
		{
			return playerPrefs.GetString(bsdKey, "");
		}

		public static AccountData LoadFromJson(string json)
		{
			var data = JsonUtility.FromJson<AccountData>(json);
			Assert.IsNotNull(data);

			return data;
		}

		public static string ToJson(AccountData data)
		{
			return JsonUtility.ToJson(data, true);
		}

		AccountData LoadFromJson_CatchExceptionDecorator(string json)
		{
			try
			{
				return LoadFromJson(json);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				return new AccountData();
			}
		}

		public AccountData GetInst()
		{ 
			if (data == null)
			{
				var json = playerPrefs.GetString(bsdKey, "{}");
#if UNITY_EDITOR
				data = LoadFromJson(json);
#else
				data = LoadFromJson_CatchExceptionDecorator(json);
#endif
			}

			data.Validate();
			return data;
		}

		public void Save()
		{
			if (data == null)
				return;

			data.metadata = new AccountData.Metadata();
			data.metadata.FillData();
			
			string json = ToJson(data);
			Debug.Log("SaveData"+ Environment.NewLine + json);
			playerPrefs.SetString(bsdKey, json); 
			playerPrefs.Save();
		}
	}
}