using System.Collections.Generic;
using System.Linq;
using Geo.Data;
using Geo.KptData.Converters;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class DecimalSeparatorsPanel : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Toggle[] separatorToggles;

		AccountData.ContourToTxtConverterPreferences preferences;
		IStorage                                     storage;

		public event UnityAction preferencesChanged; 

		void Start()
		{
			for (int n = 0; n < separatorToggles.Length; n++)
			{
				int    index  = n;
				Toggle toggle = separatorToggles[index];
				toggle.onValueChanged.AddListener(isOn => OnToggleChanged(isOn, index));
			}
		}
		
		public void InitAndDraw(AccountData.ContourToTxtConverterPreferences preferences, IStorage storage)
		{
			Assert.IsNotNull(preferences);
			Assert.IsNotNull(storage);
			
			Assert.AreEqual(separatorToggles.Length, DecimalSeparatorsList.decimals.Select(s=>s.separator).Count());
			this.preferences = preferences;
			this.storage     = storage;
			
			Draw();
		}

		void Draw()
		{   
			for (int n = 0; n < separatorToggles.Length; n++)
			{
				Toggle toggle = separatorToggles[n];
				toggle.isOn = preferences.decimalSeparator == n;
			} 
		}

		void OnToggleChanged(bool isOn, int index)
		{
			if (isOn)
			{
				bool needUpdate = preferences.decimalSeparator != index;
				if (needUpdate)
				{
					preferences.decimalSeparator = index;
					storage.Save();
					preferencesChanged?.Invoke();
				}
			}
			
			Draw();
		}

	}
}