using System;

namespace Geo.KptData.Converters
{
	[Serializable]
	public struct SeparatorInfo
	{
		public string separator;
		public string description;
		
		public SeparatorInfo(string separator, string description)
		{
			this.separator   = separator;
			this.description = description;
		}
		
	}
}