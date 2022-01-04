using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.UI;

namespace Geo.Tutorials
{
	public class TutorialCharacterSpeech : MonoBehaviour
	{
		[SerializeField, IsntNull]
		GameObject[] charcters;

		[SerializeField, IsntNull]
		Text text;

		[SerializeField, IsntNull]
		RectTransform root;

		public RectTransform Root => root;
		
		public void ShowCharacterAndText(int id, string msg)
		{
			ShowText(msg);
			charcters[id].SetActive(true);
		}
		
		public void ShowText(string msg)
		{
			HideAllCharacters();
			text.text = msg;
			root.gameObject.SetActive(true);
		}

		void HideAllCharacters()
		{
			foreach (GameObject character in charcters)
				character.SetActive(false);
		}

		public void Hide()
		{
			root.gameObject.SetActive(false);
		}
	}
}