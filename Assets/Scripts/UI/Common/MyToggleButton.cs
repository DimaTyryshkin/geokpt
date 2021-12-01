using System;
using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class MyToggleButton : MonoBehaviour
	{
		[SerializeField, IsntNull]
		GameObject onGo;

		[SerializeField, IsntNull]
		GameObject offGo;

		[SerializeField, IsntNull]
		Button button;

		[SerializeField, IsntNull]
		CanvasGroup canvasGroup;

		public UnityAction<MyToggleButton> setToOn;
		bool                               isOn;

		public bool IsOn => isOn;

		void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			if (!isOn)
			{
				Set(true);
				setToOn?.Invoke(this);
			}
		}

		public void SetInteractable(bool isInteractable)
		{
			canvasGroup.interactable = isInteractable;

			canvasGroup.alpha = isInteractable ?
				1.0f :
				0.15f;
		}

		public void Set(bool isOn)
		{
			this.isOn = isOn;

			onGo.SetActive(isOn);
			offGo.SetActive(!isOn);
		}
  
#if UNITY_EDITOR
		void Reset()
		{
			transform.ForAllHierarchy(t =>
			{
				if (t.name.ToLower().Contains("on"))
					onGo = t.gameObject;

				if (t.name.ToLower().Contains("off"))
					offGo = t.gameObject;
			});

			button = GetComponentInChildren<Button>();
		}
#endif
	}
}