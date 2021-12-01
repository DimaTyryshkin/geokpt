using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt11
{
	public class Kpt11ContourReader : Contour
	{
		readonly XmlNode node;

		public Kpt11ContourReader(XmlNode node)
		{
			Assert.IsNotNull(node);
			this.node = node;
		}

		public override string ID {
			get {
				XmlNode n = node.SelectSingleNode("number_pp");
				if (n != null)
					return n.InnerText;
				else
					return defaultId;
			}
		}

		public override List<Point> GetPoints()
		{
			var nodes = node.SelectNodes("entity_spatial/spatials_elements/spatial_element/ordinates/ordinate")
				.Cast<XmlNode>()
				.ToArray();
  
			List<Point> points = new List<Point>();
			foreach (var n in nodes)
			{
				string x = n.SelectSingleNode("x").InnerText;
				string y = n.SelectSingleNode("y").InnerText;
				points.Add(new Point(x, y));
			}

			return points;
		}
	}
}