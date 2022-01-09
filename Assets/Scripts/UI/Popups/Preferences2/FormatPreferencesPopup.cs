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
	public class FormatPreferencesPopup : Popup
	{
		// [SerializeField, IsntNull]
		// Text outputPreviewText;
		//
		// [SerializeField, IsntNull]
		// Button decimalSeparatorButton;
		//
		// [SerializeField, IsntNull]
		// Button pointIndexButton;
		//
		// [SerializeField, IsntNull]
		// Button separatorButton;
		//
		// [SerializeField, IsntNull]
		// Button heightButton;
		//
		// AccountData.ContourToTxtConverterPreferences2 preferences; 
		//
		// public event UnityAction decimalSeparatorClick;
		// public event UnityAction pointIndexClick;
		// public event UnityAction separatorClick;
		// public event UnityAction heightClick;
		//
		// public event UnityAction cancel;
		//
		// void Start()
		// {
		// 	decimalSeparatorButton.onClick.AddListener(() => decimalSeparatorClick?.Invoke());
		// 	pointIndexButton.onClick.      AddListener(() => pointIndexClick?.Invoke());
		// 	separatorButton.onClick.       AddListener(() => separatorClick?.Invoke());
		// 	heightButton.onClick.          AddListener(() => heightClick?.Invoke());
		// }
		//
		// void Update()
		// {
		// 	if (Input.GetKeyDown(KeyCode.Escape))
		// 		cancel?.Invoke();
		// }
		//
		// public void Init(AccountData.ContourToTxtConverterPreferences2 preferences)
		// {
		// 	Assert.IsNotNull(preferences);
		// 	this.preferences = preferences;
		// }
		//
		// public override void Show()
		// {
		// 	var a = new CoordinateFormats2();
		// 	decimalSeparatorButton.GetComponentInParent<Text>().text = a.GetDecimalSeparatorDescription(preferences.decimalSeparator);
		// 	pointIndexButton.GetComponentInParent<Text>().text       = a.GetPointIndexFormatDescription(preferences.pointIndexFormat);
		// 	separatorButton.GetComponentInParent<Text>().text        = a.GetSeparatorDescription(preferences.pointIndexFormat);
		// 	heightButton.GetComponentInParent<Text>().text           = a.GetHeightDescription(preferences.height);
		//
		// 	DrawPreview();
		// 	base.Show();
		// }
		//
		// void DrawPreview()
		// {
		// 	ContourToTxtConverter2 converter = new ContourToTxtConverter2(preferences.decimalSeparator, preferences.separator, preferences.pointIndexFormat, preferences.height);
		// 	Point                  p         = new Point("9999.99", "11111.11");
		// 	outputPreviewText.text = converter.PointToString(0, p);
		// }
	}
}