using System;

namespace Geo.KptData.Converters
{
	[Serializable]
	public struct SeparatorInfo
	{
		public string separator;
		public string label;
		
		public SeparatorInfo(string separator, string label)
		{
			this.separator   = separator;
			this.label = label;
		}	
	}
}