using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class SelectorPopupButton : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Button button;

		[SerializeField, IsntNull]
		Text text;

		[SerializeField, IsntNull]
		GameObject selectIcon;

		public event UnityAction click;

		void Start()
		{
			button.onClick.AddListener(() => click?.Invoke());
		}

		public void Draw(string label, bool isSelected)
		{
			text.text = label;
			selectIcon.SetActive(isSelected);
		}
	}
}