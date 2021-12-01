using UnityEngine;
using UnityEngine.Events;

namespace Geo.UI
{
	public class PopupsRoot : MonoBehaviour
	{
		public event UnityAction<Popup> Opened;

		public void Init()
		{
			var allPopups = GetComponentsInChildren<Popup>(true);
			foreach (var popup in allPopups)
				popup.Opened += p => Opened?.Invoke(p);
		}
	}
}