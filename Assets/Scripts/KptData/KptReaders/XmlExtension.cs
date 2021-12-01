using System.Xml;

namespace Geo.KptData.KptReaders
{
	public static class XmlExtension
	{
		public static XmlElement GetNode(this XmlElement element, string childName)
		{
			foreach (var c in element.ChildNodes)
			{
				var e = c as XmlElement;
				if (e.Name == childName)
					return e;
			}

			return null;
		}
	}
}