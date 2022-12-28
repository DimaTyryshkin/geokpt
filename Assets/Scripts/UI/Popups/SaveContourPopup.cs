using System.IO;
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
	public class SaveContourPopup : Popup
	{
		[SerializeField, IsntNull]
		Text cadastralNumberText;

		[SerializeField, IsntNull]
		Text areaText;

		[SerializeField, IsntNull]
		Text addressText;

		[SerializeField, IsntNull]
		Text previewResultText;

		[SerializeField, IsntNull]
		Button saveToFileButton;

		[SerializeField, IsntNull]
		Button baskButton;

		[SerializeField, IsntNull]  
		Button shareButton;

		public Button SaveToFileButton => saveToFileButton;
		public Button ShareButton => shareButton;
 
		ContourToTxtConverterFactory                 converterFactory;
		IContour                                     contour;
		IParcel                                      parcel;
		string                                       folderToSaveFile;

		public int PointCount => contour.GetPoints().Count;

		public event UnityAction                                    cancel;
		public event UnityAction<string, ContourToTxtConverterBase> saved;

		void Start()
		{
			saveToFileButton.onClick.AddListener(OnClickSaveToFile);
			baskButton.onClick.AddListener(OnClickCancel);
			shareButton.onClick.AddListener(OnClickShareFile);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				OnClickCancel();
		}

		public void Show(IParcel parcel, IContour contour, ContourToTxtConverterFactory converterFactory, string folderToSaveFile)
		{
			Assert.IsTrue(new DirectoryInfo(folderToSaveFile).Exists);
			Assert.IsNotNull(parcel);
			Assert.IsNotNull(contour);
			Assert.IsNotNull(converterFactory);

			this.parcel           = parcel;
			this.contour          = contour;
			this.converterFactory = converterFactory;
			this.folderToSaveFile = folderToSaveFile;

			ShowAndRedraw();
		}

		public void ShowAndRedraw()
		{ 
			cadastralNumberText.text = parcel.GetCadastralNumber();
			areaText.text            = parcel.GetArea();
			addressText.text         = parcel.GetReadableAddress();
			previewResultText.text   = converterFactory.Creat().ConvertToString(contour, parcel);
			Show();
		}

		void OnClickSaveToFile()
		{
			ContourToTxtConverterBase converter = converterFactory.Creat();
			string                    fullName  = converter.ConvertToFile(folderToSaveFile, contour, parcel);
			Debug.Log($"save to '{fullName}'");

			saved?.Invoke(fullName, converter);
		}

		void OnClickShareFile()
		{
			NativeShare nativeShare = new NativeShare();
			nativeShare.Clear();
			
			ContourToTxtConverterBase converter = converterFactory.Creat();
			string fullName = converter.ConvertToTemporaryFile(contour, parcel);
			
			Debug.Log($"Share file '{fullName}'");
			nativeShare.AddFile(fullName).Share();
		}

		void OnClickCancel()
		{
			cancel?.Invoke();
		} 
	}
}