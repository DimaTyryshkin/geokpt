using Geo.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Geo.Tutorials
{
	public class WaitForPopupOpened<T> : CustomYieldInstruction where T : Popup
	{
		PopupsRoot root;

		public          T    Popup       { get; private set; }
		public override bool keepWaiting => !Popup;

		public WaitForPopupOpened(PopupsRoot root)
		{
			this.root   =  root;
			root.Opened += OnPopupOpen;
		}

		void OnPopupOpen(Popup popup)
		{
			Debug.Log("[tutorial] "+popup.name);
			if (popup is T p)
			{
				root.Opened -= OnPopupOpen;
				Popup       =  p;
			}
		}
	}
	
	public class WaitForButtonClick : CustomYieldInstruction
	{
		Button button;
		bool   clickHandled;

		public override bool keepWaiting => !clickHandled;

		public WaitForButtonClick(Button button)
		{
			this.button   =  button;
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			clickHandled = true;
			button.onClick.RemoveListener(OnClick);
		}
	}
	
	public class WaitForEvent : CustomYieldInstruction
	{
		bool   isComplete;

		public override bool keepWaiting => !isComplete;
	
		public void Complete()
		{
			isComplete = true; 
		}
	}
	
	public class WaitForEvent<T> : WaitForEvent
	{ 
		public void Complete(T arg)
		{
			base.Complete();  
		}
	}
}