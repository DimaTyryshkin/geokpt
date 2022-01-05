using System;
using UnityEngine;

using SiberianWellness.NotNullValidation;

using Geo.Data; 
using Geo.KptData;
using Geo.OsIntegration;
using Geo.UI;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace Geo
{
	public class MainPresenter : MonoBehaviour
	{ 
		[SerializeField, IsntNull]
		RecentFilesPopup recentFilesPopup;
 
		[SerializeField, IsntNull]
		PreferencesPresenter preferencesPresenter;
		
		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;
		
		[SerializeField, IsntNull]
		NavigationMenu navigationMenu;

		[SerializeField, IsntNull]
		DocumentPresenter documentPresenter;
		
		FilesCache kptFilesCache;
		AccountDataStorage storage;
		IParcel selectedParcel;
		AppAnalytics appAnalytics;
		  
		public void Init(AccountDataStorage storage, AppAnalytics appAnalytics, FilesCache filesCache, GeoConfig config)
		{
			Assert.IsNotNull(storage);
			Assert.IsNotNull(appAnalytics);
			Assert.IsNotNull(filesCache);
			Assert.IsNotNull(config);
			 
			this.storage      = storage;
			this.appAnalytics = appAnalytics;
			kptFilesCache     = filesCache;

			//string[] fileExtensions = new[] {"xml", "zip"}.Select(NativeFilePicker.ConvertExtensionToFileType).ToArray();
			//NativeFileBrowser fileBrowser = new NativeFileBrowser(fileExtensions);
			NativeFileBrowser fileBrowser = new NativeFileBrowser(new []{"*/*"});

			AccountData.ContourToTxtConverterPreferences contourToTxtConverterPreferences = storage.GetInst().contourToTxtConverterPreferences;
			documentPresenter.Init(fileBrowser, appAnalytics, contourToTxtConverterPreferences);
			preferencesPresenter.Init(contourToTxtConverterPreferences, config.defaultContourToTxtFormats, storage);

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

			//settingsPopup.cancel += ShowRecent;
			documentPresenter.cancel += ShowRecent;
			documentPresenter.successExport += storage.Save;
			documentPresenter.exceptionOnLoading += OnFileLoadFail;
			recentFilesPopup.cancel += QuiteApp;
			
			navigationMenu.SetDocumentInteractable(false);
			
			ShowRecent(); 
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
			navigationMenu.SetDocumentInteractable(documentPresenter.CanWakeup);
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
			navigationMenu.SetDocumentInteractable(documentPresenter.CanWakeup);
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
			recentFilesPopup.Show(kptFilesCache);
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
		}

		void ShowSettings()
		{
			HideAll();
			
			preferencesPresenter.Show();
			navigationMenu.Draw(NavigationMenu.State.Settings);
		} 
		
		void HideAll()
		{
			recentFilesPopup.Close();
			preferencesPresenter.Close();
			documentPresenter.Sleep();
		}
		
		void QuiteApp()
		{
			storage?.Save();
			Application.Quit();
			Debug.Log("<b>Application.Quit</b>");
		}

		void OnApplicationQuit()
		{
			storage?.Save();
		}
	}
}