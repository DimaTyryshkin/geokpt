using UnityEngine;

namespace Game.ScenarioSystem.GuiHighlight
{
	public static class Utils
	{
		public static (Vector3, Vector3) GetMinMax(Vector3 a, Vector3 b)
		{
			if (a.x > b.x)
			{
				float temp = a.x;
				a.x = b.x;
				b.x = temp;
			}

			if (a.y > b.y)
			{
				float temp = a.y;
				a.y = b.y;
				b.y = temp;
			}

			return (a, b); //(min, max)
		}
	}
}