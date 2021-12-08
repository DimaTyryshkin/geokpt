using Geo.Data;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Geo
{
	public class PreferencesPresenter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		SettingsPopup settingsPopup;

		[SerializeField, IsntNull]
		UserFormatPopup userFormatPopup;
		
		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;

		AccountData.ContourToTxtConverterPreferences preferences;
		IStorage                                     storage;
		string[]                                     defaultContourToTxtFormats;

		
		public event UnityAction cancel;

		public void Init(AccountData.ContourToTxtConverterPreferences preferences, string[] defaultContourToTxtFormats, IStorage storage)
		{
			Assert.IsNotNull(preferences);
			Assert.IsNotNull(storage);
			Assert.IsNotNull(defaultContourToTxtFormats);
			
			this.preferences                = preferences;
			this.storage                    = storage;
			this.defaultContourToTxtFormats = defaultContourToTxtFormats;

			settingsPopup.Init(preferences, defaultContourToTxtFormats, storage);
			userFormatPopup.Init(preferences, defaultContourToTxtFormats, storage);
			
			settingsPopup.clickAddUserFormatButton += OnClickAddUserFormatButton;
			settingsPopup.cancel                   += Cancel;
				
			userFormatPopup.cancel      += OnUserFormatCancel;
			userFormatPopup.successSave += OnSuccessSaveUserFormat;
		}

		void OnSuccessSaveUserFormat(string  format)
		{
			preferences.format = format;
			
			userFormatPopup.Close();
			settingsPopup.Show();
			
			overlayPanel.Show("Формат успешно сохранён");
		}

		void OnUserFormatCancel()
		{
			userFormatPopup.Close();
			settingsPopup.Show();
			
			//...
		}

		void OnClickAddUserFormatButton()
		{
			settingsPopup.Close();
			userFormatPopup.Show();
		}

		public void Show()
		{
			userFormatPopup.Close();
			settingsPopup.Show();
		}

		public void Close()
		{
			userFormatPopup.Close();
			settingsPopup.Close();
		}

		void Cancel()
		{ 
			cancel?.Invoke();
		}
	}
}