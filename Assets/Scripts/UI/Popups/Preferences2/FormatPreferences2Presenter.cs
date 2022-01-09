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
		SelectorFormatPartPopup  selectorFormatPartPopup;

		IStorage storage;
		  
		public event UnityAction cancel;
		
		void Start()
		{
			formatPreferences2Popup.FormatPartClick += OnFormatPartClick;
			formatPreferences2Popup.cancel          += OnFormatPreferences2PopupCancel;
			
			selectorFormatPartPopup.cancel          += OnSelectorFormatPartPopupCancel;
		}
	 
		public void Init(
			ContourToTxtConverterFactory converterFactory,
			CoordinateFormats formats, 
			AccountData.ContourToTxtConverterPreferences2 preferences2, 
			IStorage storage)
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
				
				//TODO Показать менюшку для добавления новых элементов	
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