using Geo.KptData;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class ContourButton : MonoBehaviour
	{
		IContour contour;

		[SerializeField, IsntNull]
		Button button;

		public event UnityAction<IContour> click; 
		
		void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			click?.Invoke(contour);
		}

		public void Draw(IContour contour)
		{
			Assert.IsNotNull(contour);
			this.contour = contour;

			button.GetComponentInChildren<Text>().text = contour.ID;
		}
	}
}