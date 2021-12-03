using UnityEngine; 
using UnityEngine.UI;

using SiberianWellness.NotNullValidation;

namespace Geo.UI
{
	public class FormatToggle : MonoBehaviour
	{
		[IsntNull]
		public Toggle toggle;

		[SerializeField, IsntNull]
		Text formatText;

		[SerializeField, IsntNull]
		GameObject userFormatIcon;
 
		public string Format { get; private set; }
 
		public void Draw(string format, bool isCustomUserFormat)
		{
			Format          = format;
			formatText.text = format;
			userFormatIcon.SetActive(isCustomUserFormat);
		}
	}
}