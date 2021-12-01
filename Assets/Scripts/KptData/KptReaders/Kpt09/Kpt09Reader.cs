using System.Collections.Generic;
using System.Xml;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt10
{
	public class Kpt09Reader : KptReaderBase
	{
		XmlNamespaceManager nsmgr;
		XmlDocument         doc;

		XmlNodeList _parcels;

		public readonly static string kptVersionNumber = "09";
		public override string KptVersionNumber => kptVersionNumber;
		
		XmlNodeList Parcels
		{
			get
			{
				if (_parcels == null)
					_parcels = doc.DocumentElement.SelectNodes("root:CadastralBlocks/root:CadastralBlock/root:Parcels/root:Parcel", nsmgr);

				return _parcels;
			}
		}

		public Kpt09Reader(XmlDocument doc)
		{
			Assert.IsNotNull(doc);

			this.doc = doc;

			nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("root", "urn://x-artefacts-rosreestr-ru/outgoing/kpt/9.0.3");
			nsmgr.AddNamespace("ns2", "urn://x-artefacts-rosreestr-ru/commons/complex-types/address-output/3.0.1");
			nsmgr.AddNamespace("ns3", "urn://x-artefacts-rosreestr-ru/commons/complex-types/entity-spatial/2.0.1");
		}

		public override List<IParcel> GetAllParcels()
		{
			List<IParcel> r = new List<IParcel>();

			foreach (var percel in Parcels)
			{
				var e = percel as XmlElement;
				r.Add(new Kpt09ParcelReader(e, nsmgr));
			}

			return r;
		}

	}
}