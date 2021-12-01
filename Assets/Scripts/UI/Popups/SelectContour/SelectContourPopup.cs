using System;
using System.Collections.Generic;

using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;

using Geo.KptData;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Geo.UI
{
	public class SelectContourPopup : Popup
	{
		[SerializeField, IsntNull]
		Transform buttonsRoot;

		[SerializeField, IsntNull]
		ContourButton contourButtonTemplate;
		
		[SerializeField, IsntNull]
		GameObject noElementLabelTemplate;
		
		[SerializeField, IsntNull]
		Button backButton;
 
		IList<IContour> contours;

		public event UnityAction<IContour> selectContour;
		public event UnityAction          cancel;

		void Start()
		{
			backButton.onClick.AddListener(OnClickBack);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				OnClickBack();
		}

		public override void Close()
		{
			buttonsRoot.DestroyChilds();
			base.Close();
		}

		void OnClickBack()
		{
			cancel?.Invoke();
		}

		void DrawContours()
		{
			buttonsRoot.DestroyChilds();

			if (contours.Count > 0)
			{
				foreach (IContour contour in contours)
					AddButton(contour);
			}
			else
			{
				GameObject noElementLabel = buttonsRoot.InstantiateAsChild(noElementLabelTemplate);
				noElementLabel.SetActive(true);
			} 
		}

		void AddButton(IContour contour)
		{
			var button = buttonsRoot.InstantiateAsChild(contourButtonTemplate);
			button.gameObject.SetActive(true);
			button.Draw(contour);
			button.click += selectContour;
		}

		public void Show(IParcel parcel)
		{ 
			contours = parcel.GetContours();
			DrawContours();
			Show();
		} 
	}
}