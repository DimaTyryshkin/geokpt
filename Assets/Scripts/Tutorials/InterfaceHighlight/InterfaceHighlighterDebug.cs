using NaughtyAttributes;
using UnityEngine;

namespace Game.ScenarioSystem.GuiHighlight
{
	public class InterfaceHighlighterDebug : MonoBehaviour
	{
#if UNITY_EDITOR

		[SerializeField]
		TutorialGui facade;
		 
		[SerializeField]
		RectTransform t1;

		[SerializeField]
		RectTransform t2;

		[SerializeField]
		RectTransform taregt;

		[Button()]
		void DebugHighlight()
		{
			facade.ShowFramesAndArrow(taregt, ArrowOrientation.FromLeft); 
		}

		[Button()]
		void ArrowFromDown()
		{
			facade.ShowArrow(taregt, ArrowOrientation.FromDown);
		}

		[Button()]
		void ArrowFromTop()
		{
			facade.ShowArrow(taregt, ArrowOrientation.FromTop);
		}

		[Button()]
		void ArrowFromLeft()
		{
			facade.ShowArrow(taregt, ArrowOrientation.FromLeft);
		}

		[Button()]
		void ArrowFromRight()
		{
			facade.ShowArrow(taregt, ArrowOrientation.FromRight, 40);
		}

		[Button()]
		void GetRect()
		{
			t1.position         = taregt.TransformPoint(taregt.rect.min);
			t2.anchoredPosition = taregt.anchoredPosition + taregt.rect.max;
			//PositioningPanel(taregt, t1.position, t2.position);
		}
#endif
	}
}