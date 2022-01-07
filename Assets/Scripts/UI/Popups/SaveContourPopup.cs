﻿using System.IO;
using System.Linq;
using Geo.Data;
using Geo.KptData;
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

		public Button SaveToFileButton => saveToFileButton;

		AccountData.ContourToTxtConverterPreferences preferences;
		ContourToTxtConverterFactory                 converterFactory;
		IContour                                     contour;
		IParcel                                      parcel;
		string                                       folderToSaveFile;

		public int PointCount => contour.GetPoints().Count;
		
		public event UnityAction cancel; 
		public event UnityAction<string> saved;

		void Start()
		{
			saveToFileButton.onClick.AddListener(OnClickSaveToFile);
			baskButton.onClick.AddListener(OnClickCancel);
		}

		void Update()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
				OnClickCancel();
		}
		
		public void Show(IParcel parcel, IContour contour, AccountData.ContourToTxtConverterPreferences preferences ,  string folderToSaveFile)
		{
			Assert.IsTrue(new DirectoryInfo(folderToSaveFile).Exists);
			Assert.IsNotNull(parcel);
			Assert.IsNotNull(contour);
			Assert.IsNotNull(preferences);
			
			this.parcel           = parcel;
			this.contour          = contour;
			this.preferences      = preferences;
			this.folderToSaveFile = folderToSaveFile;
			
			ShowAndRedraw(); 
		}

		public void ShowAndRedraw()
		{ 
			converterFactory                = new ContourToTxtConverterFactory(preferences);
			cadastralNumberText.text = parcel.GetCadastralNumber();
			areaText.text            = parcel.GetArea();
			addressText.text         = parcel.GetReadableAddress();
			previewResultText.text   = converterFactory.Creat().ConvertToString(contour, parcel);
			Show();
		} 
		
		void OnClickSaveToFile()
		{
			string fullName = converterFactory.Creat().ConvertToFile(folderToSaveFile, contour, parcel);
			Debug.Log($"save to '{fullName}'");

			saved?.Invoke(fullName);
		}

		void OnClickCancel()
		{
			cancel?.Invoke();
		} 
	}
}