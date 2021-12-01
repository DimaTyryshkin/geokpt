using System;
using System.Collections;
using Geo.KptData.Converters;
using NaughtyAttributes;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class SettingsPopup:Popup
	{
		[SerializeField, IsntNull]
		SeparatorPanel separatorPanel;
		
		[SerializeField, IsntNull]
		SeparatorPanel decimalPanel;
  
		
		public event UnityAction cancel;
		
		public override void Show()
		{
			base.Show();
			
			ContourToTxtConverterWrapper w = new ContourToTxtConverterWrapper();
			
			separatorPanel.InitAndDraw(w.separator);
			decimalPanel.InitAndDraw(w.decimals);
			RebuildLayout();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}

		//[Button()]
		void RebuildLayout()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(separatorPanel.GetComponent<RectTransform>());
			LayoutRebuilder.ForceRebuildLayoutImmediate(decimalPanel.GetComponent<RectTransform>());
			
			gameObject.SetActive(false);
			gameObject.SetActive(true);
		}
	}
}