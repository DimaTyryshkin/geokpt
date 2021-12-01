using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Geo.UI
{
	public class SeparatorPanel : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Toggle[] separatorToggles;
		
		ContourToTxtConverterWrapper.SeparatorData data; 
		
		void Start()
		{
			for (int n = 0; n < separatorToggles.Length; n++)
			{
				int    index  = n;
				Toggle toggle = separatorToggles[index];
				toggle.onValueChanged.AddListener(isOn => OnToggleChanged(isOn, index));
			}
		}

		public void InitAndDraw(ContourToTxtConverterWrapper.SeparatorData data)
		{
			Assert.IsNotNull(data);
			Assert.AreEqual(separatorToggles.Length, data.Count);
			this.data = data;
			
			Draw();
		}

		void Draw()
		{   
			for (int n = 0; n < separatorToggles.Length; n++)
			{
				Toggle toggle = separatorToggles[n];
				toggle.isOn = data.Index == n;
			} 
		}

		void OnToggleChanged(bool isOn, int index)
		{
			if (isOn)
			{
				bool needUpdate = data.Index != index;
				if (needUpdate)
				{
					data.Index = index;
					PlayerPrefs.Save();
					Draw();
				}
			}
			else
			{
				
			}
		} 
	}
}