using System;
using System.Security.Permissions;
using Geo.UI;
using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo
{
	public class AndroidPathTestPopup : Popup
	{
		[SerializeField, IsntNull]
		RectTransform root;

		[SerializeField, IsntNull]
		Button templateButton;

		string[] values = new string[]
		{
			"getDataDirectory",
			"getDownloadCacheDirectory",
			"getRootDirectory",
			"getStorageDirectory"
		};

		public event UnityAction<string> pathSelected; 

		public void Show()
		{
			SpawnButtons();
			gameObject.SetActive(true);
		}

		void SpawnButtons()
		{
			root.DestroyChilds();
			 
			foreach (string value in values)
			{
				Debug.Log($"f'{value}'");
				var    path   = GetFolder(value);
				Debug.Log($"f'{value}'=>'{path}'");
				AddButton(value, path);
			}

			AddButton("T1", GetPath());
			AddButton("T2", GetPath2());
			
			LayoutRebuilder.ForceRebuildLayoutImmediate(root);
			LayoutRebuilder.ForceRebuildLayoutImmediate(root);
		}

		void AddButton(string key, string path)
		{
			Button button = root.InstantiateAsChild(templateButton);
			button.onClick.AddListener(() =>
			{
				Permission.RequestUserPermission(Permission.ExternalStorageRead);
				Permission.RequestUserPermission(Permission.ExternalStorageWrite);
				pathSelected?.Invoke(path);
			});

			button.GetComponentInChildren<Text>().text = key + "=" + path;
				
			button.gameObject.SetActive(true);
		}

		string GetFolder(string dir)
		{
#if UNITY_EDITOR
			return "C:/Users/Yabloko/Downloads/Гео";
#endif

			AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
			string path = jc
				.CallStatic<AndroidJavaObject>(dir)
				.Call<string>("getAbsolutePath");

			return path;
		}
		
		string GetPath()
		{
#if  UNITY_EDITOR
			//return Directory.GetCurrentDirectory();
			return "C:/Users/Yabloko/Downloads/Гео";
#endif
			//storage/emulated/0 //статья про это дерьмо https://qastack.ru/android/205430/what-is-storage-emulated-0
			
			AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
			string path = jc
				.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DOWNLOADS"))
				.Call<AndroidJavaObject>("getParentFile") 
				.Call<string>("getAbsolutePath");
			
			return path;
		}
		
		string GetPath2()
		{
#if  UNITY_EDITOR
			//return Directory.GetCurrentDirectory();
			return "C:/Users/Yabloko/Downloads/Гео";
#endif
			//storage/emulated/0 //статья про это дерьмо https://qastack.ru/android/205430/what-is-storage-emulated-0
			
			AndroidJavaClass jc = new AndroidJavaClass("android.os.Environment");
			string path = jc
				.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", jc.GetStatic<string>("DIRECTORY_DOWNLOADS"))
				.Call<string>("getAbsolutePath");
			
			return path;
		}
	}
}