using UnityEngine;
using UnityEngine.UI;

namespace Geo.UI
{
	public static class GeoLayoutRebuilder
	{
		public static void ForceRebuildLayoutImmediate(RectTransform rect)
		{  
			var allRects = rect.gameObject.GetComponentsInChildren<RectTransform>(true);
			foreach (var r in allRects)
				LayoutRebuilder.ForceRebuildLayoutImmediate(r);
		}
	}
}