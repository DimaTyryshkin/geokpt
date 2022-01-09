using System;
using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class SelectorFormatPartPopup : Popup
	{
		[SerializeField, IsntNull]
		Text headerText;
		
		[SerializeField, IsntNull]
		SelectorPopupButton buttonTemplate;

		[SerializeField, IsntNull]
		Button addNewButton;
		
		[SerializeField, IsntNull]
		RectTransform root;

		public event UnityAction cancel;

		public void Show(string header, FormatPart formatPart, UnityAction<string> select, UnityAction addNew)
		{
			headerText.text = header;
			
			root.DestroyChilds();

			string[] allValues    = formatPart.GetAvailableValues();
			string   currentValue = formatPart.Value;
			foreach (string value in allValues)
			{
				string              valueCopy = value;
				string              label     = formatPart.GetLabel(value);
				SelectorPopupButton newButton = root.InstantiateAsChild(buttonTemplate);
				newButton.Draw(label, currentValue == value);
				newButton.click += () => @select?.Invoke(valueCopy);
				newButton.gameObject.SetActive(true);
			}

			if (formatPart.CanAddUserValues)
			{
				Button addButton = root.InstantiateAsChild(addNewButton);
				addButton.onClick.AddListener(addNew);
				addButton.gameObject .SetActive(true);
			}

			base.Show();
		}
		
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}
	}
}