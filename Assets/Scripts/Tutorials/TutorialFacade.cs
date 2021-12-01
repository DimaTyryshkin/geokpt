using System.Collections; 
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;

namespace Geo.Tutorials
{
	public class TutorialFacade:MonoBehaviour
	{
		[SerializeField, IsntNull]
		PopupsRoot root;
		
		[SerializeField, IsntNull]
		CanvasGroup rootCanvasGroup;
 
		public WaitForPopupOpened<T> WaitForPopupOpened<T>() where T:Popup
		{
			return new WaitForPopupOpened<T>(root);
		}

		public IEnumerator WaitForSeconds(float time)
		{
			SetEnableInput(false);
			yield return new WaitForSeconds(time);
			SetEnableInput(true);
		}

		public void SetEnableInput(bool isEnable)
		{
			rootCanvasGroup.blocksRaycasts = isEnable;
		}
	}
}