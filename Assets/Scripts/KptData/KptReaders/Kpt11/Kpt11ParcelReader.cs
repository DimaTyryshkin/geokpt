using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt11
{
	public class Kpt11ParcelReader : ParcelReaderBase
	{
		XmlNode node;
		 
		public Kpt11ParcelReader(XmlNode node)
		{
			Assert.IsNotNull(node);

			this.node = node;
		}

		public override string GetCadastralNumber()
		{
			if (cadastralNumberCache == null)
				cadastralNumberCache = node.SelectSingleNode("object/common_data/cad_number").InnerText;

			return cadastralNumberCache;
		}

		public override string GetArea()
		{
			return node.SelectSingleNode("params/area/value").InnerText;
		}
 
		public override string GetReadableAddress()
		{
			if (readableAddressCache == null)
			{
				var addressNode = node.SelectSingleNode("address_location/address/readable_address");
				if (addressNode != null)
					readableAddressCache = addressNode.InnerText;
				else
					readableAddressCache = "";
			}

			return readableAddressCache;
		}

		public override List<IContour> GetContours()
		{
			var nodes = node.SelectNodes("contours_location/contours/contour")
				.Cast<XmlNode>()
				.ToArray();

			List<IContour> contour = new List<IContour>();
			foreach (var n in nodes)
				contour.Add(new Kpt11ContourReader(n));

			return contour;
		}
	}
}