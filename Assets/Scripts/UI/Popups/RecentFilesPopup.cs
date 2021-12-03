using System;
using System.Collections.Generic;
using System.IO;
using Geo.Data;
using Geo.KptData.KptReaders;
using Geo.UI; 
using SiberianWellness.Common;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo
{
	public class RecentFilesPopup : Popup
	{ 
		[SerializeField, IsntNull]
		RecentFileButton fileButtonTempalte;

		[SerializeField, IsntNull]
		Transform buttonsRoot;
		
		[SerializeField, IsntNull]
		Button openNewButton;
  
		FilesCache kptFilesCache;

		[NonSerialized]
		List<RecentFileButton> buttons;

		
		public IReadOnlyList<RecentFileButton> Buttons => buttons;

		
		public event UnityAction<string> clickOpenFromCache;
		public event UnityAction clickNewFile;
		
		public event UnityAction cancel;

		
		void Start()
		{
			openNewButton.onClick.AddListener(OnClickNewFile);
		}

		void OnClickNewFile()
		{
			clickNewFile?.Invoke();
		}

		public void Show(FilesCache kptFilesCache)
		{
			Assert.IsNotNull(kptFilesCache);
			this.kptFilesCache = kptFilesCache;

			ShowAndRedraw();
		}

		void ShowAndRedraw()
		{
			buttons = new List<RecentFileButton>();
			
			buttonsRoot.DestroyChilds();
			foreach (string path in kptFilesCache.Files)
			{
				RecentFileButton button = buttonsRoot.InstantiateAsChild(fileButtonTempalte);
				button.Draw(path);
				button.click += OnClickOpenLastFile;
				button.gameObject.SetActive(true);
				buttons.Add(button);
			}

			Show();
		}
 
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				cancel?.Invoke();
		}
  
		void OnClickOpenLastFile(string path)
		{
			clickOpenFromCache?.Invoke(path); 
		} 
	}
}