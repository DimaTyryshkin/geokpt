using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt10
{
	public abstract class BaseKpt10ContourReader : Contour
	{
		readonly protected XmlNode             node;
		readonly protected XmlNamespaceManager nsmgr;
 
		protected BaseKpt10ContourReader(XmlNode node, XmlNamespaceManager nsmgr)
		{
			Assert.IsNotNull(node);
			Assert.IsNotNull(nsmgr);
			this.node  = node;
			this.nsmgr = nsmgr;
		}
		
		public sealed override List<Point> GetPoints()
		{
			var nodes = node.SelectNodes("root:EntitySpatial/ns3:SpatialElement/ns3:SpelementUnit", nsmgr)
				.Cast<XmlNode>()
				.ToArray();


			List<Point> points = new List<Point>();
			foreach (var n in nodes)
			{
				var    ordinateNode = n.SelectSingleNode("ns3:Ordinate", nsmgr);
				string x            = ordinateNode.SelectSingleNode("@X").Value;
				string y            = ordinateNode.SelectSingleNode("@Y").Value;
				points.Add(new Point(x, y));
			}

			return points;
		}
	}

	public class Kpt10ContourReader : BaseKpt10ContourReader
	{
		public override string ID => node.SelectSingleNode("@NumberRecord").Value;

		public Kpt10ContourReader(XmlNode node, XmlNamespaceManager nsmgr):base(node,nsmgr )
		{ 
		} 
	}
	
	public class Kpt10AloneContourReader : BaseKpt10ContourReader
	{
		public override string ID => defaultId;

		public Kpt10AloneContourReader(XmlNode node, XmlNamespaceManager nsmgr):base(node,nsmgr )
		{ 
		} 
	}
}