using System;
using System.Linq;
using Geo.Data;
using Geo.KptData;
using Geo.KptData.Converters;
using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class FormatsListPanel : MonoBehaviour
	{
		[SerializeField, IsntNull]
		FormatToggle toggleTemplate;

		[SerializeField, IsntNull]
		Transform togglesRoot;
		
		[SerializeField, IsntNull]
		Button addButton;
		
		AccountData.ContourToTxtConverterPreferences preferences;
		IStorage                                     storage;
		string[]                                     defaultFormat;

		public event UnityAction preferencesChanged;

		void Start()
		{
			addButton.onClick.AddListener(OnClickAdd);
		}

		void OnClickAdd()
		{
			preferences.userFormats.Add("{i} {n} {x}");
			Draw();
		}

		public void InitAndDraw(AccountData.ContourToTxtConverterPreferences preferences, string[] defaultFormat, IStorage storage)
		{
			Assert.IsNotNull(preferences);
			Assert.IsNotNull(storage);
			Assert.IsNotNull(defaultFormat);
			this.preferences   = preferences;
			this.storage       = storage;
			this.defaultFormat = defaultFormat;
 
			Draw();
		}

		void Draw()
		{
			togglesRoot.DestroyChilds();

			string[] reverseUserFormats = preferences.userFormats
				.Reverse<string>()//Переворачиваем, чтобы самые свежие были сверху
				.ToArray();
			
			foreach (string userFormat in reverseUserFormats)
				AddToggle(userFormat, true, userFormat == preferences.format);

			foreach (string format in defaultFormat)
				AddToggle(format, false, format == preferences.format);
		}

		void AddToggle(string format, bool isCustomUserFormat, bool isOnByDefault)
		{
			FormatToggle toggle = togglesRoot.InstantiateAsChild(toggleTemplate);
			toggle.gameObject.SetActive(true);
			toggle.Draw(format, isCustomUserFormat);
			toggle.toggle.isOn = isOnByDefault;
			toggle.toggle.onValueChanged.AddListener(isOn => OnToggleChanged(isOn, format));
		}

		void OnToggleChanged(bool isOn, string format)
		{
			if (isOn)
			{
				bool needUpdate = preferences.format != format;
				if (needUpdate)
				{
					preferences.format = format;
					storage.Save();
					preferencesChanged?.Invoke();
				}
			}
			
			Draw();
		}

	}
}