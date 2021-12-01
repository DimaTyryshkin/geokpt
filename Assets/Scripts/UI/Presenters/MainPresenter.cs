using System;
using UnityEngine;

using SiberianWellness.NotNullValidation;

using Geo.Data; 
using Geo.KptData;
using Geo.OsIntegration;
using Geo.Tutorials;
using Geo.UI;
using UnityEngine.Accessibility;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Geo
{
	public class MainPresenter : MonoBehaviour
	{ 
		[SerializeField, IsntNull]
		RecentFilesPopup recentFilesPopup;
 
		[SerializeField, IsntNull]
		SettingsPopup settingsPopup;
		
		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;
		
		[SerializeField, IsntNull]
		NavigationMenu navigationMenu;

		[SerializeField, IsntNull]
		DocumentPresenter documentPresenter;
		
		[SerializeField, IsntNull]
		TutorialsList tutorials;
		 
		FilesCache kptFilesCache;
		AccountDataStorage storage;
		IParcel selectedParcel;
		AppAnalytics appAnalytics;
		
		public event UnityAction ShowStartScreen;
		
		
		public void Init(AccountDataStorage storage, AppAnalytics appAnalytics)
		{
			Assert.IsNotNull(storage);
			Assert.IsNotNull(appAnalytics);
			 
			
			this.storage = storage;
			this.appAnalytics = appAnalytics;
			RealFileSystem fileSystem = new RealFileSystem();
			kptFilesCache = new FilesCache(fileSystem, Application.persistentDataPath);

			//string[] fileExtensions = new[] {"xml", "zip"}.Select(NativeFilePicker.ConvertExtensionToFileType).ToArray();
			//NativeFileBrowser fileBrowser = new NativeFileBrowser(fileExtensions);
			NativeFileBrowser fileBrowser = new NativeFileBrowser(new []{"*/*"});
			 
			documentPresenter.Init(fileBrowser, appAnalytics);

			navigationMenu.clickResent += ShowRecent;
			navigationMenu.clickDocument += ShowDocument;
			navigationMenu.clickSettings += ShowSettings;

			recentFilesPopup.clickOpenFromCache += TryOpenFileFromRecent;
			
			recentFilesPopup.clickNewFile += () =>
			{
				appAnalytics.OpenFilePicker();
				fileBrowser.PicFile(TryOpenFileFromPicker, () =>
				{
					appAnalytics.CancelFilePicker();
					ShowRecent();
				});
			};

			settingsPopup.cancel += ShowRecent;
			documentPresenter.cancel += ShowRecent;
			documentPresenter.successExport += storage.Save;
			documentPresenter.exceptionOnLoading += OnFileLoadFail;
			recentFilesPopup.cancel += QuiteApp;
			
			navigationMenu.SetDocumentInteractable(false);
			
			appAnalytics.StartApp();
			
			// ---
			this.storage.Save();

			if (this.appAnalytics.SessionNumber == 1)
			{
				DefaultKptFiles defaultKptFiles = new DefaultKptFiles();
				defaultKptFiles.CopyToCache(kptFilesCache);
			}

			ShowRecent();
			
			//---tutorials
			if (this.appAnalytics.SessionNumber == 1)
				StartCoroutine(tutorials.HelloTutorial());
		}
		
		void TryOpenFileFromPicker(string filePath)
		{ 
			appAnalytics.TryOpenFileByPicker(filePath);
			
			documentPresenter.ClearSuccessLoadEvent();
			documentPresenter.successLoad += OnSuccessLoadFromPicker;
			ShowDocument(filePath);
		}

		void OnSuccessLoadFromPicker(string filePath)
		{ 
			appAnalytics.SuccessLoadFile(filePath);
			
			documentPresenter.ClearSuccessLoadEvent();
			kptFilesCache.CacheFile(filePath);  
		}
		
		void TryOpenFileFromRecent(string filePath)
		{ 
			appAnalytics.TryOpenFileFromCache(filePath, kptFilesCache.Files.Count);
			
			documentPresenter.ClearSuccessLoadEvent();
			documentPresenter.successLoad += OnSuccessLoadFromRecent;
			ShowDocument(filePath); 
		}
		
		void OnSuccessLoadFromRecent(string filePath)
		{ 
			appAnalytics.SuccessLoadFile(filePath);
			documentPresenter.ClearSuccessLoadEvent();
		}
		
		void OnFileLoadFail(Exception e, string filePath)
		{
			appAnalytics.FailLoadFile(e, filePath);
			documentPresenter.ClearSuccessLoadEvent();
			overlayPanel.Show("Ошибка загрузки файла");
			ShowRecent();
		}
 
		void ShowRecent()
		{
			HideAll();
			navigationMenu.Draw(NavigationMenu.State.Resent);
			
			kptFilesCache.Load();
			recentFilesPopup.Show(kptFilesCache, storage);
		}

		void ShowDocument()
		{
			HideAll();

			documentPresenter.Wakeup();
			navigationMenu.Draw(NavigationMenu.State.Document);
			navigationMenu.SetDocumentInteractable(documentPresenter.CanWakeup);
		}

		void ShowDocument(string path)
		{
			HideAll();
			
			documentPresenter.Show(path);
			navigationMenu.Draw(NavigationMenu.State.Document);
			navigationMenu.SetDocumentInteractable(documentPresenter.CanWakeup);
		}

		void ShowSettings()
		{
			HideAll();
			
			settingsPopup.Show();
			navigationMenu.Draw(NavigationMenu.State.Settings);
		} 
		
		void HideAll()
		{
			recentFilesPopup.Close();
			settingsPopup.Close();
			documentPresenter.Sleep();
		}
		
		void QuiteApp()
		{
			Application.Quit();
			Debug.Log("<b>Application.Quit</b>");
		}

		void OnApplicationQuit()
		{
			storage?.Save();
		}
	}
}