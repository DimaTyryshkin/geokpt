using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Geo.OsIntegration
{
	public class NativeFileBrowser
	{
		string[] fileTypes;
		
		public NativeFileBrowser(string[] fileTypes)
		{
			this.fileTypes = fileTypes;
		}

		public void PicFile(UnityAction<string> callBack, UnityAction canceled)
		{
			NativeFilePicker.RequestPermission();

			if (NativeFilePicker.IsFilePickerBusy())
				return;

			NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
			{
				if (path == null)
					canceled();
				else
					callBack(path);
			}, fileTypes);

			if (permission != NativeFilePicker.Permission.Granted)
				Debug.LogError("Permission result: " + permission);
		}

		public void Export(string fileFullName, UnityAction<bool> result)
		{
			NativeFilePicker.RequestPermission();

			if (NativeFilePicker.IsFilePickerBusy())
				return;
			
			NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( fileFullName, (success)=>result(success));

			if (permission != NativeFilePicker.Permission.Granted)
				Debug.Log( "Permission result: " + permission );
		}
	}
}