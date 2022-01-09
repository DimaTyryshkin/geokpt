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
	public class FormatPreferences2Popup : Popup
	{
		[SerializeField, IsntNull]
		Text outputPreviewText;

		[SerializeField, IsntNull]
		Button decimalSeparatorButton;

		[SerializeField, IsntNull]
		Button pointIndexButton;

		[SerializeField, IsntNull]
		Button separatorButton;

		[SerializeField, IsntNull]
		Button heightButton;

		CoordinateFormats                             coordinateFormats;
		AccountData.ContourToTxtConverterPreferences2 preferences2;
		ContourToTxtConverterFactory                  converterFactory;

		public event UnityAction<FormatPart> FormatPartClick; 

		public event UnityAction cancel;

		void Start()
		{
			decimalSeparatorButton.onClick.AddListener(() => FormatPartClick?.Invoke(coordinateFormats.DecimalSeparatorFormatPart));
			pointIndexButton.onClick.AddListener(() => FormatPartClick?.Invoke(coordinateFormats.PointIndexFormatsFormatPart));
			separatorButton.onClick.AddListener(() => FormatPartClick?.Invoke(coordinateFormats.SeparatorFormatPart));
			heightButton.onClick.AddListener(() => FormatPartClick?.Invoke(coordinateFormats.HeightFormatPart));
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}

		public void Init(ContourToTxtConverterFactory converterFactory, CoordinateFormats coordinateFormats, AccountData.ContourToTxtConverterPreferences2 preferences2)
		{
			Assert.IsNotNull(converterFactory);
			Assert.IsNotNull(coordinateFormats);
			Assert.IsNotNull(preferences2);
			this.converterFactory  = converterFactory;
			this.coordinateFormats = coordinateFormats;
			this.preferences2      = preferences2;
		}

		public override void Show()
		{
			decimalSeparatorButton.GetComponentInChildren<Text>().text = coordinateFormats.DecimalSeparatorFormatPart.GetCurrentLabel();
			pointIndexButton.GetComponentInChildren<Text>().text       = coordinateFormats.PointIndexFormatsFormatPart.GetCurrentLabel();
			separatorButton.GetComponentInChildren<Text>().text        = coordinateFormats.SeparatorFormatPart.GetCurrentLabel();
			heightButton.GetComponentInChildren<Text>().text           = coordinateFormats.HeightFormatPart.GetCurrentLabel();

			DrawPreview();
			base.Show();
		}

		void DrawPreview()
		{
			ContourToTxtConverter2 converter = new ContourToTxtConverter2(
				preferences2.decimalSeparator,
				preferences2.separator,
				preferences2.pointIndexFormat,
				preferences2.height);

			Point p = new Point("9999.99", "11111.11");
			outputPreviewText.text = converter.PointToString(0, p);
		}
	}
}