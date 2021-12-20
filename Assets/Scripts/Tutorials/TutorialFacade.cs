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

		public bool EnableInput
		{
			get => rootCanvasGroup.blocksRaycasts;
			set => rootCanvasGroup.blocksRaycasts = value;
		}

		public WaitForPopupOpened<T> WaitForPopupOpened<T>() where T:Popup
		{
			return new WaitForPopupOpened<T>(root);
		}

		public IEnumerator WaitForSeconds(float time)
		{
			EnableInput = false;
			yield return new WaitForSeconds(time);
			EnableInput = true;
		} 
	}
}