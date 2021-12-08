using System;
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
	public class SettingsPopup:Popup
	{
		[SerializeField, IsntNull]
		Text outputPreviewText;
		
		[SerializeField, IsntNull]
		DecimalSeparatorsPanel decimalSeparatorPanel;

		[SerializeField, IsntNull]
		FormatsListPanel formatsListPanel;
 
		[SerializeField, IsntNull]
		Button addUserFormatButton;
		
		AccountData.ContourToTxtConverterPreferences preferences;
		IStorage                                     storage;
		string[]                                     defaultContourToTxtFormats;


		public event UnityAction cancel;
		public event UnityAction clickAddUserFormatButton;

		void Start()
		{
			decimalSeparatorPanel.preferencesChanged += DrawPreview;
			formatsListPanel.preferencesChanged      += DrawPreview;
			addUserFormatButton.onClick.AddListener(clickAddUserFormatButton);
		}

		public void Init(AccountData.ContourToTxtConverterPreferences preferences, string[] defaultContourToTxtFormats, IStorage storage)
		{
			Assert.IsNotNull(preferences);
			Assert.IsNotNull(storage);
			Assert.IsNotNull(defaultContourToTxtFormats);
			
			this.preferences                = preferences;
			this.storage                    = storage;
			this.defaultContourToTxtFormats = defaultContourToTxtFormats;
		}

		public override void Show()
		{
			decimalSeparatorPanel.InitAndDraw(preferences, storage );
			formatsListPanel.InitAndDraw(preferences, defaultContourToTxtFormats, storage);
			DrawPreview();
			
			base.Show();
			RebuildLayout();
		}

		void DrawPreview()
		{
			ContourToTxtConverter converter        = new ContourToTxtConverter();
			Point                 p                = new Point("9999.99", "11111.11");
			string                decimalSeparator = ContourToTxtConverter.GetDecimalSeparatorSafe(preferences.decimalSeparator);
			outputPreviewText.text = converter.PointToString(0, p, decimalSeparator, preferences.format);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}

		//[Button()]
		void RebuildLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(formatsListPanel.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(decimalSeparatorPanel.GetComponent<RectTransform>());
			
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
	}
}