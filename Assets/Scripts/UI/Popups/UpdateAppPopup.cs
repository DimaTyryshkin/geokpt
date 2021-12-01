using System;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine; 
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo
{
	public class UpdateAppPopup : Popup
	{
		[SerializeField, IsntNull]
		Button updateNowButton;

		[SerializeField, IsntNull]
		Button skipUpdateButton;

		[SerializeField, IsntNull]
		Text headerText;
		
		[SerializeField, IsntNull]
		Text infoText;
		
		[SerializeField]
		string headerFormat;

		bool canSkip;

		public event UnityAction clickUpdateNow;
		public event UnityAction clickSkipUpdate;

		void Start()
		{
			updateNowButton.onClick.AddListener(() => clickUpdateNow?.Invoke());
			skipUpdateButton.onClick.AddListener(() => clickSkipUpdate?.Invoke());
		}

		public void Show(string text, bool canSkip, Version newVersion)
		{
			headerText.text = string.Format(headerFormat, newVersion.ToString());
			this.canSkip  = canSkip;
			infoText.text = text.Replace("<br>", Environment.NewLine);
			skipUpdateButton.gameObject.SetActive(canSkip);
			
			Show();
		}

		void Update()
		{
			if (canSkip && Input.GetKeyDown(KeyCode.Escape))
				clickSkipUpdate?.Invoke();
		}
	}
}