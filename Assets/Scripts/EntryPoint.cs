using System;
using System.Collections.Generic;
using UnityEngine;

using Geo.Data;
using Geo.Tutorials;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine.Serialization;

namespace Geo
{
	public class EntryPoint : MonoBehaviour
	{
		[SerializeField, IsntNull]
		MainPresenter mainPresenter;
		
		[SerializeField, IsntNull]
		FileSystemTest fileSystemTest;

		[SerializeField, IsntNull]
		PrivacyPolicyAndTermsConditionsPresenter policyAndTermsConditionsPresenter;
		
		[SerializeField, IsntNull]
		UpdateAppPresenter updateAppPresenter;

		[SerializeField, IsntNull]
		TutorialStarter tutorialStarter;
		
		[SerializeField, IsntNull]
		PopupsRoot popupsRoot;
		
		[SerializeField, IsntNull]
		GameObject loadingPanel; 

		[SerializeField, IsntNull]
		AppMetrica yandexAppMetrica; 
			
		[SerializeField]
		bool isFileSystemTest;
		
		[SerializeField]
		bool cleanRun;
		
		[SerializeField]
		bool skipTutorials;
		
		UnityRemoteConfigIntegration remoteConfig;
		AccountDataStorage           storage;
		AccountData                  data;

		void Start()
		{
			yandexAppMetrica.Init();
			
#if !UNITY_EDITOR
			cleanRun      = false;
			skipTutorials = false;
#endif
			loadingPanel.SetActive(true);
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
			PlayerPrefsWrapper playerPrefs = new PlayerPrefsWrapper();
			storage = new AccountDataStorage(playerPrefs);
			data    = storage.GetInst();
			MigrateAccountData(data);
			
			if (isFileSystemTest)
			{
				loadingPanel.SetActive(false);
				fileSystemTest.Show();
			}
			else
			{
				PrivacyPolicyAndTermsConditions();
			}
		}

		void PrivacyPolicyAndTermsConditions()
		{
			policyAndTermsConditionsPresenter.Init(data.privacyPolicy);
			//policyAndTermsConditionsPresenter.Assepted += () => storage.Save();
			policyAndTermsConditionsPresenter.Complete += UpdatePopup;
			policyAndTermsConditionsPresenter.StartUserAccepting();
		}

		void UpdatePopup()
		{
			Version appVersion = new Version(Application.version);
			updateAppPresenter.skip += InitApp;

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
				InitApp();
			}
		}

		void MigrateAccountData(AccountData data)
		{ 
			List<string> completedTutorialsId = data.completedTutorialsId;
			completedTutorialsId.Remove("SettingsPopupTutorial"); 
			completedTutorialsId.Remove("UserFormatTutorial"); 
		}

		void InitApp()
		{  
			popupsRoot.Init();
			
			var                appAnalytics = new AppAnalytics(data.appAnalytics);
			appAnalytics.StartApp();
			
			IFileSystem fileSystem = new RealFileSystem();
			var kptFilesCache = new FilesCache(fileSystem, Application.persistentDataPath);
			
			if (appAnalytics.SessionNumber == 1)
			{
				DefaultKptFiles defaultKptFiles = new DefaultKptFiles();
				defaultKptFiles.CopyToCache(kptFilesCache);
			}
			 
			if(!skipTutorials)
				tutorialStarter.Init(storage);
			
			mainPresenter.Init(storage, appAnalytics, kptFilesCache, remoteConfig.Config);
			
			storage.Save();
			loadingPanel.SetActive(false);
			
			if(!skipTutorials)
				tutorialStarter.OnShowStartScreen();
		}
	}
}