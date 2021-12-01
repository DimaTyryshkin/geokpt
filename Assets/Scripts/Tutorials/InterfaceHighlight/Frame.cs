using UnityEngine;
using UnityEngine.UI;

namespace Game.ScenarioSystem.GuiHighlight
{
	public class Frame : MonoBehaviour
	{ 
		[SerializeField]
		RectTransform left;

		[SerializeField]
		RectTransform right;

		[SerializeField]
		RectTransform top;

		[SerializeField]
		RectTransform bottom;

		[SerializeField]
		RectTransform screenMin;

		[SerializeField]
		RectTransform screenMax;

        
		[Header("Optional")]
		[SerializeField]
		RectTransform middle;

		[SerializeField]
		bool debugMode;
  
		/// <summary>
		/// Подсветить элемент. Все остальное затемнить и сделать не доступным.
		/// </summary> 
		public void ShowBlackout(RectTransform rectTransform, float padding)
		{
			Vector3 screenMinWorld = screenMin.position;
			Vector3 screenMaxWorld = screenMax.position;

			Rect rect = rectTransform.rect;

			//Углы квадрата переводим в мировые координаты
			Vector3 rectMinWorld = rectTransform.TransformPoint(rect.min);
			Vector3 rectMaxWorld = rectTransform.TransformPoint(rect.max);
			(rectMinWorld, rectMaxWorld) = Utils.GetMinMax(rectMinWorld, rectMaxWorld);

			rectMinWorld -=new Vector3(padding,padding,0);
			rectMaxWorld +=new Vector3(padding,padding,0);

			Vector3 p  = Vector3.zero;
			Vector3 p2 = Vector3.zero;

			//left
			p   = rectMinWorld;
			p.y = screenMaxWorld.y; 
			PositioningPanel(left, screenMinWorld, p);

			//right
			p   = rectMaxWorld;
			p.y = screenMinWorld.y; 
			PositioningPanel(right, p, screenMaxWorld);

			//top
			p   = rectMinWorld;
			p.y = rectMaxWorld.y;

			p2   = rectMaxWorld;
			p2.y = screenMaxWorld.y;
			PositioningPanel(top, p, p2);

			//bot
			p   = rectMinWorld;
			p.y = screenMinWorld.y;

			p2   = rectMaxWorld;
			p2.y = rectMinWorld.y;
			PositioningPanel(bottom, p, p2);
            
			//middle
			if (middle)
			{
				PositioningPanel(middle, rectMinWorld, rectMaxWorld);
			}

			SetEnableAll(true);
		}

		public void Hide()
		{
			SetEnableAll(false);
		}

		void SetEnableAll(bool enable)
		{
			SetEnable(left, enable);
			SetEnable(right, enable);
			SetEnable(top, enable);
			SetEnable(bottom, enable);
			 
			if(middle)
				SetEnable(middle, enable);  
		}

		void SetEnable(RectTransform r, bool enable)
		{
			r.gameObject.SetActive(enable);
			if (debugMode)
				r.GetChild(0).gameObject.SetActive(enable);
		}

		void PositioningPanel(RectTransform panel, Vector3 minWorld, Vector3 maxWorld)
		{
			//(Vector3 minWorld, Vector3 maxWorld) = GetMinMax(a, b);

			Vector3 centerWorld = (minWorld + maxWorld) / 2f;
			panel.position = centerWorld;

			//Переводим из мировых координат в локальные
			Vector3 p1 = panel.InverseTransformPoint(minWorld);
			Vector3 p2 = panel.InverseTransformPoint(maxWorld);

			var size = p2 - p1;
			panel.sizeDelta = size;
		} 

		/// <summary>
		/// Set frame alpha
		/// </summary>
		/// <param name="alpha">alpha parameter from 0 to 1</param>
		public void SetAlpha(float alpha)
		{
			SetAlpha(left, alpha);
			SetAlpha(right, alpha);
			SetAlpha(top, alpha);
			SetAlpha(bottom, alpha);
			SetAlpha(middle, alpha);
		}
		
		private void SetAlpha(RectTransform rect, float alpha)
		{
			Color color;
			Image image;

			image = rect.GetComponent<Image>();
			color = image.color;
			color.a = alpha;
			image.color = color;
		}
	}
}