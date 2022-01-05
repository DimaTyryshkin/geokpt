using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Geo.UI
{
	public class UrlButton : MonoBehaviour
	{
		[SerializeField]
		string url;

		[SerializeField, IsntNull]
		Button button;
		 
		void Start()
		{
			button.onClick.AddListener(OnClick);
		}

		void OnClick()
		{
			Application.OpenURL(url);
		}

#if UNITY_EDITOR
		void Reset()
		{
			Undo.RegisterCompleteObjectUndo(this, "reset");
			button = GetComponent<Button>();
		}
#endif
	}
}