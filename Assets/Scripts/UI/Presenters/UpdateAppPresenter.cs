using System;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;

namespace Geo
{
	public class UpdateAppPresenter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		UpdateAppPopup updateAppPopup;

		string storeUrl;
		
		public event UnityAction skip;
		
		public void Show(bool canSkip, Version newVersion, string infoText, string storeUrl)
		{
			this.storeUrl                  =  storeUrl;
			updateAppPopup.clickUpdateNow  += OnClickUpdate;
			updateAppPopup.clickSkipUpdate += OnClickSkip;
			
			updateAppPopup.Show(infoText, canSkip, newVersion);
		}

		void OnClickSkip()
		{
			updateAppPopup.Close();
			skip?.Invoke();
		}

		void OnClickUpdate()
		{
			Application.OpenURL(storeUrl);
			Application.Quit();
		}
	}
}