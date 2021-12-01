using System;
using UnityEngine;

using Game.UnityServices.RemoteConfig;
using Geo.Data;
using Geo.Tutorials;
using Geo.UI;
using SiberianWellness.NotNullValidation;

namespace Geo
{
	public class EntryPoint : MonoBehaviour
	{
		[SerializeField, IsntNull]
		MainPresenter mainPresenter;
		
		[SerializeField, IsntNull]
		FileSystemTest fileSystemTest;
		
		[SerializeField, IsntNull]
		UpdateAppPresenter updateAppPresenter;

		[SerializeField, IsntNull]
		TutorialStarter tutorialStarter;
		
		[SerializeField, IsntNull]
		PopupsRoot popupsRoot;

		[SerializeField, IsntNull]
		AppMetrica yandexAppMetrica; 
			
		[SerializeField]
		bool isFileSystemTest;
		
		[SerializeField]
		bool cleanRun;
		
		UnityRemoteConfigIntegration remoteConfig;

		void Start()
		{
			if(cleanRun)
				PlayerPrefs.DeleteAll();
			
			Debug.Log("temporaryCachePath=" + Application.temporaryCachePath);
			Debug.Log("persistentDataPath=" + Application.persistentDataPath);

			remoteConfig               =  new UnityRemoteConfigIntegration();
			remoteConfig.fetchComplete += OnRemoteConfigFetchComplete;
			remoteConfig.FetchDataAsync();
		}

		void OnRemoteConfigFetchComplete()
		{
			if (isFileSystemTest)
			{
				fileSystemTest.Show();
			}
			else
			{
				Version appVersion = new Version(Application.version);
				updateAppPresenter.skip += InitPresenters;

				string url = remoteConfig.GooglePlayMarketUrl;

#if UNITY_EDITOR
				url = remoteConfig.GooglePlayUrl;
#endif
				if (appVersion < remoteConfig.MinAndroidVersion)
				{
					updateAppPresenter.Show(false, remoteConfig.MaxAndroidVersion, remoteConfig.NewVersionInfo, url);
				}
				else if (appVersion < remoteConfig.MaxAndroidVersion)
				{
					updateAppPresenter.Show(true, remoteConfig.MaxAndroidVersion, remoteConfig.NewVersionInfo, url);
				}
				else
				{
					InitPresenters();
				}
			}
		}

		void InitPresenters()
		{ 
			yandexAppMetrica.Init();
			popupsRoot.Init();
			
			PlayerPrefsWrapper playerPrefs  = new PlayerPrefsWrapper();
			var                storage      = new AccountDataStorage(playerPrefs);
			AccountData        data         = storage.GetInst();
			var                appAnalytics = new AppAnalytics(data.appAnalytics);
			
			tutorialStarter.Init(storage, appAnalytics);
			mainPresenter.Init(storage, appAnalytics);
		}
	}
}