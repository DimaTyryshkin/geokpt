using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class ShowMoreButton : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Text text;

		[SerializeField, IsntNull]
		Button button;

		public event UnityAction click;

		void Start()
		{
			button.onClick.AddListener(click);
		}

		public void Draw(int count)
		{
			text.text = $"Показать еще '{count}'";
		}
	}
}