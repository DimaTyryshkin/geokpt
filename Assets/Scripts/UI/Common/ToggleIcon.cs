using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.UI;

namespace Geo.UI.Common
{ 
	public class ToggleIcon:MonoBehaviour
	{
		[SerializeField, IsntNull]
		Toggle toggle;

		[SerializeField, IsntNull]
		GameObject isOnIcon;

		void Start()
		{
			toggle.onValueChanged.AddListener(OnToggleChanged);
			Draw();
		}

		void OnToggleChanged(bool arg0)
		{
			Draw();
		}

		void Draw()
		{
			isOnIcon.SetActive(toggle.isOn);
		}
	}
}