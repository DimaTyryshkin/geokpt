using System;
using Geo.KptData.Converters;

namespace Geo
{
	[Serializable]
	public class CoordinateFormats2Config
	{
		public SeparatorInfo[] defaultPointIndexFormats = new SeparatorInfo[]
		{
			new SeparatorInfo("pt(i)", "pt(i)"),
			new SeparatorInfo("", "не показывать")
		};
		
		public SeparatorInfo[] defaultSeparators = new SeparatorInfo[]
		{
			new SeparatorInfo(",", "',' (запятая)"),
			new SeparatorInfo(" ", "' ' (пробел)"),
			new SeparatorInfo(", ", "', ' (запятая пробел)")
		};
		
		public SeparatorInfo[] defaultHeight = new SeparatorInfo[]
		{
			new SeparatorInfo("0", "0"),
			new SeparatorInfo("", "не показывать")
		};
	}
}