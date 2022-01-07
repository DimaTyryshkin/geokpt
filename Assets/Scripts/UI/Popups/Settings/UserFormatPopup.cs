using System;
using System.Linq;
using Geo.Data;
using Geo.KptData;
using Geo.KptData.Converters;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class UserFormatPopup : Popup
	{
		[SerializeField, IsntNull]
		InputField formatInput;
		
		[SerializeField, IsntNull]
		Text outputPreviewText;

		[SerializeField, IsntNull]
		Button saveButton;
		
		[SerializeField, IsntNull]
		Button cancelButton;
		
		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;
		
		AccountData.ContourToTxtConverterPreferences preferences;
		IStorage                                     storage;
		string[]                                     defaultContourToTxtFormats;
		
		public event UnityAction cancel;
		public event UnityAction<string> successSave;

		
		static string format;
		
		void Start()
		{
			saveButton.onClick.AddListener(OnClickSave);
			cancelButton.onClick.AddListener(OnClickCancel);
			formatInput.onValueChanged.AddListener(OnInputValueChanged);
		}

		void OnInputValueChanged(string arg0)
		{
			format = formatInput.text;
			DrawPreview(format);
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
				OnClickCancel();
		}

		public void Init(AccountData.ContourToTxtConverterPreferences preferences, string[] defaultContourToTxtFormats, IStorage storage)
		{
			Assert.IsNotNull(preferences); 
			Assert.IsNotNull(storage);
			Assert.IsNotNull(defaultContourToTxtFormats);
			
			this.preferences                = preferences;
			this.storage                    = storage;
			this.defaultContourToTxtFormats = defaultContourToTxtFormats;

			if (string.IsNullOrEmpty(format))
			{
				format           = preferences.format;
				formatInput.text = format;
			}
			
			DrawPreview(format);
		}
		
		void DrawPreview(string format)
		{
			ContourToTxtConverter converter        = new ContourToTxtConverter(preferences.decimalSeparator, format);
			Point                 p                = new Point("9999.99", "11111.11"); 
			outputPreviewText.text = converter.PointToString(0, p);
		}

		void OnClickSave()
		{
			Save(format);
		}

		void Save(string format)
		{
			if (defaultContourToTxtFormats.Contains(format) || preferences.userFormats.Contains(format))
			{
				overlayPanel.Show($"Не сохранён.\nФормат '{format}' уже существует");
				return;
			}

			preferences.userFormats.Add(format);
			storage.Save();
			successSave?.Invoke(format);
		}

		void OnClickCancel()
		{
			cancel?.Invoke();
		} 
	}
}