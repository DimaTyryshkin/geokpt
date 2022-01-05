using System.Collections;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo
{
	public class PrivacyPolicyAndTermsConditionsPopup : Popup
	{
		[SerializeField, IsntNull]
		Button acceptButton;
		
		[SerializeField, IsntNull]
		ScrollRect scrollRect;
 
		public event UnityAction clickAccept; 

		void Start()
		{
			acceptButton.onClick.AddListener(() => clickAccept?.Invoke());
			StartCoroutine(RebuildLayout());
		}
  
		IEnumerator RebuildLayout()
		{
			scrollRect.verticalNormalizedPosition = 1;
			GeoLayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
			yield return null;
			scrollRect.verticalNormalizedPosition = 1;
			GeoLayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
		}
	}
}