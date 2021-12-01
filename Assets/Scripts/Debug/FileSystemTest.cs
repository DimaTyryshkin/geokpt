using System;
using System.IO; 
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine; 
using UnityEngine.Android;
using UnityEngine.UI;

namespace Geo
{
	public class FileSystemTest:Popup
	{ 
		[SerializeField, IsntNull]
		Button permissionsButton;

		[SerializeField, IsntNull]
		Text permissionsText;
		
		[SerializeField, IsntNull]
		Button xmlButton;
		
		[SerializeField, IsntNull]
		Button zipButton;
		
		[SerializeField, IsntNull]
		Button any1Button;
		
		[SerializeField, IsntNull]
		Button any2Button;
		
		[SerializeField, IsntNull]
		Button any3Button;
		
		[SerializeField, IsntNull]
		Button any4Button;
		
		[SerializeField, IsntNull]
		Button saveNewFileButton;
		
		[SerializeField, IsntNull]
		Button settingsButton;
		
		[SerializeField, IsntNull]
		Button redrawButton;

		[NonSerialized]
		string path = null;
		
	 
		
		//https://developer.android.com/training/data-storage
		//https://developer.android.com/training/data-storage/shared/media
		//https://answers.unity.com/questions/1133761/cant-access-android-mediastore.html
		
		//https://assetstore.unity.com/packages/tools/integration/native-share-for-android-ios-112731
		
		void Start()
		{
			//ApplicationChrome.navigationBarState = ApplicationChrome.States.TranslucentOverContent;
			//ApplicationChrome.navigationBarState = ApplicationChrome.States.VisibleOverContent;
			 
			xmlButton.onClick.AddListener(OnClickPickXml);
			zipButton.onClick.AddListener(OnClickPickZip);
			any1Button.onClick.AddListener(OnClickPickAny);
			any2Button.onClick.AddListener(OnClickPickAny2);
			any3Button.onClick.AddListener(OnClickPickAny3);
			any4Button.onClick.AddListener(OnClickPickAny4);
			
			permissionsButton.onClick.AddListener(OnClickPermissions);
			saveNewFileButton.onClick.AddListener(OnClickSaveNewFile);
			settingsButton.onClick.AddListener(OnClickSettings);
			redrawButton.onClick.AddListener(Draw);
			
			saveNewFileButton.gameObject.SetActive(false);
			
			Draw(); 
		}

		void OnClickPickXml()
		{
			string pdfFileType = NativeFilePicker.ConvertExtensionToFileType("xml");
			string[] fileTypes = new string[] {pdfFileType};
			PickFile(fileTypes);
		}
		
		void OnClickPickZip()
		{
			string   pdfFileType = NativeFilePicker.ConvertExtensionToFileType("zip");
			string[] fileTypes   = new string[] {pdfFileType};
			PickFile(fileTypes);
		}
		
		void OnClickPickAny()
		{
			string   pdfFileType = NativeFilePicker.ConvertExtensionToFileType("*");
			string[] fileTypes   = new string[] {pdfFileType};
			PickFile(fileTypes);
		}
		
		void OnClickPickAny2()
		{
			string   pdfFileType = NativeFilePicker.ConvertExtensionToFileType("*.*");
			string[] fileTypes   = new string[] {pdfFileType};
			PickFile(fileTypes);
		}
		
		void OnClickPickAny3()
		{ 
			string[] fileTypes   = new string[] {"*.*"};
			PickFile(fileTypes);
		}
		
		void OnClickPickAny4()//Вот это рботает!
		{ 
			string[] fileTypes = new string[] {"*/*"};
			PickFile(fileTypes);
		}


		void OnClickSettings()
		{
			NativeFilePicker.OpenSettings();
		}

		void OnClickSaveNewFile()
		{
			FileInfo f= new FileInfo(path);
			string name = f.Name;
			string newName = name.Replace(f.Extension, "_copy"+ f.Extension);

			string newPath = f.Directory.FullName;
			string newFullName = Path.Combine(newPath, newName);
			Debug.Log("newFullName="+newFullName);
			File.WriteAllText(newFullName, "it is copy of the origin file");
			Debug.Log("create new file success");
			
			// Export the file
			
			NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( newFullName, ( success ) => Debug.Log( "File exported: " + success ) );

			Debug.Log( "Permission result: " + permission );
		}

		void PickFile(string[] fileTypes)
		{
			NativeFilePicker.RequestPermission();
  
			if (NativeFilePicker.IsFilePickerBusy())
				return;
			
			// Pick a PDF file
			NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
			{
				if (path == null)
					Debug.Log("Operation cancelled");
				else
				{
					this.path = path;
					saveNewFileButton.gameObject.SetActive(true);
					Debug.Log("Picked file: " + path);
					CheckFileAsses(path);
				}

				//}, new string[] {pdfFileType});
			}, fileTypes);

			Debug.Log("Permission result: " + permission);
		}

		void CheckFileAsses(string path)
		{
			Debug.Log("CheckFileAsses "+ path);
			string msg = "";
			FileInfo f= new FileInfo(path);
			msg += f.Name + Environment.NewLine;
			msg += "lenght="+f.Length + Environment.NewLine;

			var stream = f.OpenRead();
			msg+="byte=" +stream.ReadByte() + Environment.NewLine;
			stream.Close();
			msg += "success";
			Debug.Log(msg);
		}

		public void Show()
		{
			gameObject.SetActive(true); 
		}
  

		void Draw()
		{
			//https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006?_ga=2.216730704.7727953.1593711564-521249394.1573670938#content
			permissionsText.text =  "";
			permissionsText.text += "ExternalStorageRead=" + Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) + Environment.NewLine;
			permissionsText.text += "ExternalStorageWrite=" + Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) + Environment.NewLine;
			permissionsText.text += "CanExportFiles=" + NativeFilePicker.CanExportFiles() + Environment.NewLine;
		}

		void OnClickPermissions()
		{  
			//Permission.RequestUserPermission(Permission.ExternalStorageRead);
			Permission.RequestUserPermission(Permission.ExternalStorageWrite);
			
			Draw();
		} 
	}
}