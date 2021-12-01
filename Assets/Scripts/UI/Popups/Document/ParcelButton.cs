using Geo.KptData;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class ParcelButton : MonoBehaviour
	{
		IParcel parcel;

		[SerializeField, IsntNull]
		Button button;
		
		[SerializeField, IsntNull]
		Text cadastralNumberText;
		
		[SerializeField, IsntNull]
		Text addressText;

		public event UnityAction<IParcel> click; 
		
		void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			click?.Invoke(parcel);
		}

		public void Draw(IParcel p)
		{
			Assert.IsNotNull(p);
			parcel = p;

			cadastralNumberText.text = parcel.GetCadastralNumber();
			addressText.text         = parcel.GetReadableAddress();
		}
	}
}