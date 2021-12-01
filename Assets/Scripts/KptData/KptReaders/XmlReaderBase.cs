using System.Xml;
using UnityEngine;

namespace Geo.KptData.KptReaders
{
	public class XmlReaderBase
	{
		protected void Log(XmlNode e )
		{
			if(e!=null)
				Debug.Log(e.Name);
			else
				Debug.Log("null"); 
		}
        
		protected void PrintChild(XmlNode element)
		{
			foreach (var c in element.ChildNodes)
			{
				var e = c as XmlElement;
				Debug.Log(e.Name);
			}
		}
	}
}