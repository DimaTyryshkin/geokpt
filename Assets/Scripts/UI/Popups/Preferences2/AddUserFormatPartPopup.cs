using NaughtyAttributes;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class AddUserFormatPartPopup : Popup
	{
		[SerializeField, IsntNull]
		Text headerText;
		
		[SerializeField, IsntNull]
		Text previewText;

		[SerializeField, IsntNull]
		InputField inputField;

		[SerializeField, IsntNull]
		Button saveButton;
		
		[SerializeField, IsntNull]
		Button cancelButton;
  
		public event UnityAction<string> save;
		public event UnityAction         cancel;

		void Start()
		{
			saveButton.onClick.AddListener(() => save?.Invoke(inputField.text));
			cancelButton.onClick.AddListener(() => cancel?.Invoke());
			
			inputField.onValueChanged.AddListener(OnValueChanged);
			OnValueChanged("");
		}
		
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}

		void OnValueChanged(string arg0)
		{
			previewText.text = $"'{arg0}'";
		}

		public void Show(string header)
		{
			headerText.text = header;
			base.Show();
		}
	}
}