using System.Xml;

namespace SiberianWellness.Common
{
	public static class XmlNodeExtension
	{
		public static string FullPath(this XmlNode node)
		{
			if (node.ParentNode == null)
				return node.Name;
			else
				return FullPath(node.ParentNode) + "/" + node.Name;
		}
	}
}