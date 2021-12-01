using System;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;

namespace Geo.UI
{
	public class NavigationMenu:MonoBehaviour
	{
		public enum  State
		{
			Resent=0,
			Document,
			Settings
		}
		
		[SerializeField, IsntNull]
		MyToggleButton resentFilesToggle;
		
		[SerializeField, IsntNull]
		MyToggleButton documentToggle;
		
		[SerializeField, IsntNull]
		MyToggleButton settingsToggle;

		public event UnityAction clickResent;
		public event UnityAction clickDocument;
		public event UnityAction clickSettings;

		void Start()
		{
			resentFilesToggle.setToOn += OnClickResent;
			documentToggle.setToOn    += OnClickDocument;
			settingsToggle.setToOn    += OnClickSettings;
		}

		public void Draw(State state)
		{
			SetOn(GetToggle(state));
		}

		public void SetDocumentInteractable(bool isInteractable)
		{
			documentToggle.SetInteractable(isInteractable);
		}

		MyToggleButton GetToggle(State state)
		{
			switch (state)
			{
				case State.Resent:
					return resentFilesToggle;
				
				case State.Document:
					return documentToggle;
				
				case State.Settings:
					return settingsToggle;
				
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		void OnClickSettings(MyToggleButton toggle)
		{
			SetOn(toggle);
			clickSettings?.Invoke();
		}

		void OnClickDocument(MyToggleButton toggle)
		{
			SetOn(toggle);
			clickDocument?.Invoke();
		}

		void OnClickResent(MyToggleButton toggle)
		{
			SetOn(toggle);
			clickResent?.Invoke();
		}

		void SetOn(MyToggleButton toggle)
		{
			OffAll();
			toggle.Set(true);
		}

		void OffAll()
		{
			resentFilesToggle.Set(false);
			documentToggle.Set(false);
			settingsToggle.Set(false);
		}
	}
}