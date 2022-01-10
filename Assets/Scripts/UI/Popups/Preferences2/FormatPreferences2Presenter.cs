using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using SiberianWellness.NotNullValidation;
using Geo.Data;

namespace Geo.UI
{
	public class FormatPreferences2Presenter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		FormatPreferences2Popup formatPreferences2Popup;

		[SerializeField, IsntNull]
		SelectorFormatPartPopup selectorFormatPartPopup;

		[SerializeField, IsntNull]
		AddUserFormatPartPopup addUserFormatPartPopup;

		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;


		IStorage   storage;
		FormatPart formatPart;

		public event UnityAction cancel;

		void Start()
		{
			formatPreferences2Popup.FormatPartClick += OnFormatPartClick;
			formatPreferences2Popup.cancel          += OnFormatPreferences2PopupCancel;

			selectorFormatPartPopup.cancel += OnSelectorFormatPartPopupCancel;
			addUserFormatPartPopup.save    += OnSaveNewUserPartValue;
			addUserFormatPartPopup.cancel  += OnAddUserFormatPartPopupCancel;
		}

		public void Init(
			ContourToTxtConverterFactory                  converterFactory,
			CoordinateFormats                             formats,
			AccountData.ContourToTxtConverterPreferences2 preferences2,
			IStorage                                      storage)
		{
			Assert.IsNotNull(converterFactory);
			Assert.IsNotNull(formats);
			Assert.IsNotNull(preferences2);
			Assert.IsNotNull(storage);

			this.storage = storage;
			formatPreferences2Popup.Init(converterFactory, formats, preferences2);
		}

		void ShowFormatPopup()
		{
			formatPreferences2Popup.Show();
		}

		void ShowAddUserFormatPartPopup(FormatPart formatPart)
		{
			Assert.IsNotNull(formatPart);
			this.formatPart = formatPart;

			addUserFormatPartPopup.Show(formatPart.Header);
		}

		void OnAddUserFormatPartPopupCancel()
		{
			addUserFormatPartPopup.Close();
			OnFormatPartClick(formatPart);
		}

		void OnSaveNewUserPartValue(string userFormatPartValue)
		{
			bool success = formatPart.AddUserValue(userFormatPartValue);
			if (success)
			{
				addUserFormatPartPopup.Close();
				formatPart.Value = userFormatPartValue;
				storage.Save();
				ShowFormatPopup();
			}
			else
			{
				overlayPanel.Show("Не сохранено");
			}
		}

		void OnFormatPreferences2PopupCancel()
		{
			formatPreferences2Popup.Close();
			cancel?.Invoke();
		}

		void OnSelectorFormatPartPopupCancel()
		{
			selectorFormatPartPopup.Close();
			ShowFormatPopup();
		}

		void OnFormatPartClick(FormatPart part)
		{
			formatPreferences2Popup.Close();

			selectorFormatPartPopup.Show(part.Header, part, (value) =>
			{
				part.Value = value;
				storage.Save();

				selectorFormatPartPopup.Close();
				ShowFormatPopup();
			}, () =>
			{
				selectorFormatPartPopup.Close();
				ShowAddUserFormatPartPopup(part);
			});
		}

		public void Show()
		{
			ShowFormatPopup();
		}

		public void Close()
		{
			formatPreferences2Popup.Close();
			selectorFormatPartPopup.Close();
		}
	}
}