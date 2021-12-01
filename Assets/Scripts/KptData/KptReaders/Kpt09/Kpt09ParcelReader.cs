using System.Collections.Generic;
using System.Xml;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt10
{
	public class Kpt09ParcelReader : ParcelReaderBase
	{
		XmlElement          _node;
		XmlNamespaceManager nsmgr;

		public Kpt09ParcelReader(XmlElement node, XmlNamespaceManager nsmgr)
		{
			Assert.IsNotNull(node);
			Assert.IsNotNull(nsmgr);

			_node      = node;
			this.nsmgr = nsmgr;
		}

		public override string GetCadastralNumber()
		{
			if (cadastralNumberCache == null)
				cadastralNumberCache = _node.GetAttribute("CadastralNumber");

			return cadastralNumberCache;
		}

		public override string GetArea()
		{
			return _node.SelectSingleNode("root:Area/root:Area", nsmgr).InnerText;
		}

		public override string GetReadableAddress()
		{
			if (readableAddressCache == null)
			{
				var addressNOde = _node.SelectSingleNode("root:Location/root:Address/ns2:Note", nsmgr);
				if (addressNOde != null)
					readableAddressCache = addressNOde.InnerText;
				else
					readableAddressCache = "";
			}

			return readableAddressCache;
		}

		public override List<IContour> GetContours()
		{
			List<IContour> contours     = new List<IContour>();
			XmlNodeList    contourNodes = _node.SelectNodes("root:Contours/root:Contour", nsmgr);
			if (contourNodes != null && contourNodes.Count > 0)
			{
				foreach (object contourNode in contourNodes)
				{
					XmlNode n = contourNode as XmlNode;
					contours.Add(new Kpt10ContourReader(n, nsmgr));//контур кпт09 такой же как в кпт10  
				}
			}

			XmlNode nodes = _node.SelectSingleNode("root:EntitySpatial", nsmgr);
			if (nodes != null)
			{
				var contour = new Kpt10AloneContourReader(_node, nsmgr); //контур кпт09 такой же как в кпт10
				contours.Add(contour);
			}

			return contours;
		}
	}
}