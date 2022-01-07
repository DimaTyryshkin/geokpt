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
	public class FormatPreferencesPopup:Popup
	{
		// [SerializeField, IsntNull]
		// Text outputPreviewText;
		//
		// [SerializeField, IsntNull]
		// DecimalSeparatorsPanel decimalSeparatorPanel;
		//
		// [SerializeField, IsntNull]
		// Button decimalSeparatorButton;
  //
		// AccountData.ContourToTxtConverterPreferences2 preferences;
		// IStorage                                     storage;
		// string[]                                     defaultContourToTxtFormats;
		//
		// public event UnityAction cancel;
		//
		// void Start()
		// {
		// 	decimalSeparatorPanel.preferencesChanged += DrawPreview;
		// 	formatsListPanel.preferencesChanged      += DrawPreview;
		// 	addUserFormatButton.onClick.AddListener(clickAddUserFormatButton);
		// }
		//
		// void Update()
		// {
		// 	if (Input.GetKeyDown(KeyCode.Escape))
		// 		cancel?.Invoke();
		// } 
		//
		// public void Init(AccountData.ContourToTxtConverterPreferences2 preferences, string[] defaultContourToTxtFormats, IStorage storage)
		// {
		// 	Assert.IsNotNull(preferences);
		// 	Assert.IsNotNull(storage);
		// 	Assert.IsNotNull(defaultContourToTxtFormats);
		// 	
		// 	this.preferences                = preferences;
		// 	this.storage                    = storage;
		// 	this.defaultContourToTxtFormats = defaultContourToTxtFormats;
		// }
		//
		// public override void Show()
		// {
		// 	decimalSeparatorButton.GetComponentInParent<Text>().text = "" 
		// 	
		// 	DrawPreview(); 
		// 	base.Show(); 
		// }
		//
		// void DrawPreview()
		// {
		// 	ContourToTxtConverter converter        = new ContourToTxtConverter();
		// 	Point                 p                = new Point("9999.99", "11111.11");
		// 	string                decimalSeparator = ContourToTxtConverter.GetDecimalSeparatorSafe(preferences.decimalSeparator);
		// 	outputPreviewText.text = converter.PointToString(0, p, decimalSeparator, preferences.format);
		// }

		
	}
}