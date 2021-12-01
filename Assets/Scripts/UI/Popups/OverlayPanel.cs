using System.Collections;
using SiberianWellness.NotNullValidation;
using UnityEngine; 
using UnityEngine.UI;

namespace Geo.UI
{
	public class OverlayPanel : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Text text;

		[SerializeField]
		float timeShow = 2;

		public void Show(string text)
		{ 
			gameObject.SetActive(true);
			this.text.text = text;
			
			StopAllCoroutines();
			StartCoroutine(Hide(timeShow));
		}

		IEnumerator Hide(float delay)
		{
			yield return  new WaitForSeconds(delay);
			gameObject.SetActive(false);
		}
	}
}