using System;
using System.IO; 
using System.Collections;
using System.Collections.Generic;

using UnityEngine; 
using UnityEngine.UI;
using UnityEngine.Events;

using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;

using Geo.KptData;
using Geo.KptData.KptReaders;
using UnityEngine.EventSystems;

namespace Geo.UI
{
	public class DocumentPopup : Popup
	{
		[SerializeField, IsntNull]
		GameObject loadingPanel;

		[SerializeField, IsntNull]
		InputField inputCadastralNumberField;

		[SerializeField, IsntNull]
		Transform buttonsRoot;

		[SerializeField, IsntNull]
		ParcelButton parcelButtonTemplate;
		
		[SerializeField, IsntNull]
		ShowMoreButton showMoreButtonTemplate;
		
		[SerializeField, IsntNull]
		GameObject noElementLabelTemplate;

		[SerializeField]
		int defaultElementCountLimit = 50;
		
		[SerializeField]
		int minHidedElementCount = 10;

		IReadOnlyList<IParcel> parcels;
		ParcelsCollection parcelsCollection;
		List<IParcel> filteredParcels;
		
		int elementCountLimit;
		Exception lastExceptionInCurrentDocument;


		public event UnityAction<IParcel>   selectParcel;
		public event UnityAction<Exception> exceptionOnLoading;
		public event UnityAction<string>    successLoad;
		public event UnityAction            cancel;

		public string        CurrentFilter  => inputCadastralNumberField.text;
		public RectTransform InputFieldRect => inputCadastralNumberField.GetComponent<RectTransform>();
		public int           ParcelsCount   => parcels.Count;
		
		void Start()
		{
			inputCadastralNumberField.onValueChanged.AddListener(OnInputValueChanged);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}
 
		void OnInputValueChanged(string arg0)
		{
			var    t    = DateTime.Now;
			elementCountLimit = defaultElementCountLimit;
			
			filteredParcels = parcelsCollection.FilterParcels(arg0);
			DrawFilteredParcels();
		}
		
		public void DeselectInput()
		{
			inputCadastralNumberField.OnDeselect(null);
		}
		
		void DrawFilteredParcels()
		{
			var t = DateTime.Now;
			buttonsRoot.DestroyChilds();

			if (filteredParcels.Count == 0)
			{
				var label = buttonsRoot.InstantiateAsChild(noElementLabelTemplate);
				label.SetActive(true);
			}
			else
			{
				//Сколько будем показывать элементов. ПОказываем так, чобы в спяртаных было не меньше minHidedElementCount элементов
				int elementCountToShow = filteredParcels.Count < elementCountLimit + minHidedElementCount ?
					filteredParcels.Count :
					elementCountLimit;

				for (int i = 0; i < elementCountToShow; i++)
				{
					AddButton(filteredParcels[i]); 
				}

				int hidedElementCount = filteredParcels.Count - elementCountToShow;
				if (hidedElementCount > 0)
				{
					ShowMoreButton showMoreButton = buttonsRoot.InstantiateAsChild(showMoreButtonTemplate);
					showMoreButton.Draw(hidedElementCount);
					showMoreButton.click += OnPressShowMore;
					showMoreButton.gameObject.SetActive(true);
				}
			}

			Debug.Log("DrawFilteredParcels time=" + (DateTime.Now - t).TotalSeconds);
		}

		void OnPressShowMore()
		{
			elementCountLimit = filteredParcels.Count;
			DrawFilteredParcels();
		}

		void OnApplicationFocus(bool hasFocus)
		{
			//if (!hasFocus)
			//	SelectInput();
		}

		void AddButton(IParcel parcel)
		{
			var button = buttonsRoot.InstantiateAsChild(parcelButtonTemplate);
			button.gameObject.SetActive(true);
			button.Draw(parcel);
			button.click += selectParcel;
		}

		public void Show(string filePath)
		{ 
			Show();
			StartCoroutine(LoadAndDraw(filePath));
		}
 
		IEnumerator LoadAndDraw(string filePath)
		{
			DateTime t      = DateTime.Now;
		
			buttonsRoot.DestroyChilds();
			loadingPanel.gameObject.SetActive(true);
			float t1 = Time.realtimeSinceStartup;
			yield return null;
			
			elementCountLimit = defaultElementCountLimit;

			lastExceptionInCurrentDocument = null;
			FileInfo file    = new FileInfo(filePath);
			KptFile  kptFile = new KptFile(file);
#if UNITY_EDITOR
			//kptFile.SaveUnzippedFile = true;
#endif
			kptFile.exceptionOnLoading += OnException;
			kptFile.LoadAllParcels();
 
			parcels = kptFile.Parcels;
			parcelsCollection = new ParcelsCollection(parcels);
			
			
			float delay = Time.realtimeSinceStartup - t1;
			float minDelay = 0.5f;
			if(delay < minDelay)
				yield return new WaitForSeconds(minDelay - delay);//ждем немного чтобы пользователь успел прочитать надпись "загрузка..."
			
			loadingPanel.gameObject.SetActive(false);
			OnInputValueChanged(inputCadastralNumberField.text);

			Debug.Log("Load and Draw time="+(DateTime.Now -t).ToString("g"));
			if (lastExceptionInCurrentDocument == null)
			{
				SelectInput();

				if (parcels.Count > 0)
				{ 
					successLoad?.Invoke(filePath);
				}
			}
			else
			{ 
				exceptionOnLoading?.Invoke(lastExceptionInCurrentDocument);
			}
		}

		void OnException(Exception e)
		{
			
			lastExceptionInCurrentDocument = e;
		}

		void SelectInput()
		{
			inputCadastralNumberField.Select();
		}
	}
}