using System.IO;
using Geo.KptData.KptReaders;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Geo.UI
{
	public class RecentFileButton : MonoBehaviour
	{
		[SerializeField, IsntNull]
		Text nameText;
		
		[SerializeField, IsntNull]
		Text dateText;
		
		[SerializeField, IsntNull]
		Button button;
		
		[SerializeField, IsntNull]
		RectTransform root;
		
		string path;

		public RectTransform Root => root;
		
        
		public event UnityAction<string> click; 
		
		
		void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			click?.Invoke(path);
		}

		public void Draw(string path)
		{
			Assert.IsFalse(string.IsNullOrWhiteSpace(path));
			
			this.path = path;
 
			FileInfo file = new FileInfo(path); 
			string fileName = file.Name;
			
			nameText.text = KptFile.TrimEndKptExtensions(fileName);
			dateText.text = "Кадастровый план территории";

			if (fileName.StartsWith("91"))
				dateText.text += " 'Севастополь'";
			
			if (fileName.StartsWith("70"))
				dateText.text += " 'Томск'";
			
			if (fileName.StartsWith("78"))
				dateText.text += " 'Питер'";
			
			if (fileName.StartsWith("77"))
				dateText.text += " 'Москва'";
		}
	}
}