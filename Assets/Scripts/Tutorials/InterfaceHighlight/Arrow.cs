using SiberianWellness.NotNullValidation;
using UnityEngine;

namespace Game.ScenarioSystem.GuiHighlight
{
	public enum ArrowOrientation
	{
		FromDown,
		FromTop,
		FromLeft,
		FromRight
	}

	public class Arrow : MonoBehaviour
	{
		[SerializeField,IsntNull]
		Transform arrow;
		
		[SerializeField, IsntNull]
		Transform textPosition;

		public Vector2 TextPosition => textPosition.position;
		
		/// <summary>
		/// Показывает стрелочку указывающую на <paramref name="rectTransform"/>
		/// </summary>
		/// <param name="rectTransform">На что указывает стрелочка</param>
		/// <param name="orientation">Ориентация стрелочки</param>
		/// <param name="padding">Отступ стрелочки от объекта</param>
		public void ShowArrow(RectTransform rectTransform, ArrowOrientation orientation, float padding)
		{
			Rect rect = rectTransform.rect;

			//Углы квадрата переводим в мировые координаты
			Vector3 rectMinWorld = rectTransform.TransformPoint(rect.min);
			Vector3 rectMaxWorld = rectTransform.TransformPoint(rect.max);
			(rectMinWorld, rectMaxWorld) = Utils.GetMinMax(rectMinWorld, rectMaxWorld);

			float halfWidth  = (rectMaxWorld.x - rectMinWorld.x) / 2f + padding;
			float halfHeight = (rectMaxWorld.y - rectMinWorld.y) / 2f + padding;

			Vector3 rectCenterWorld = (rectMinWorld + rectMaxWorld) / 2f;

			Vector3 p = rectCenterWorld;
			if (orientation == ArrowOrientation.FromDown)
			{
				p.y            -= halfHeight;
				arrow.rotation =  Quaternion.Euler(0, 0, 0);
			}

			if (orientation == ArrowOrientation.FromLeft)
			{
				p.x            -= halfWidth;
				arrow.rotation =  Quaternion.Euler(0, 0, 270);
			}

			if (orientation == ArrowOrientation.FromTop)
			{
				p.y            += halfHeight;
				arrow.rotation =  Quaternion.Euler(0, 0, 180);
			}

			if (orientation == ArrowOrientation.FromRight)
			{
				p.x            += halfWidth;
				arrow.rotation =  Quaternion.Euler(0, 0, 90);
			}

			arrow.position = p;
			arrow.gameObject.SetActive(true);
		}

		public void Hide()
		{
			arrow.gameObject.SetActive(false);
		}
	}
}