using System.IO;
using Geo.Data;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;

namespace Geo
{
	public class PrivacyPolicyAndTermsConditionsPresenter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		PrivacyPolicyAndTermsConditionsPopup popup;

		AccountData.PrivacyPolicyAndTermsConditions data;

		public event UnityAction Assepted;
		public event UnityAction Complete;

		public void Init(AccountData.PrivacyPolicyAndTermsConditions data)
		{
			this.data         =  data;
			popup.clickAccept += OnClickAccept;
		}

		public void StartUserAccepting()
		{
			if (data.NeedAccept)
			{
				popup.Show();
			}
			else
			{
				Complete?.Invoke();
			}
		}

		void OnClickAccept()
		{
			data.OnAccept();
			popup.Close();
			Assepted?.Invoke();
			Complete?.Invoke();
		}
	}
}