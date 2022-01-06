using System.Collections.Generic;
using System.Xml;
using Geo.KptData.KptReaders;
using UnityEngine.Assertions;


namespace Geo.KptData.KptReader.Kpt10Excerpt
{
    public class Kpt10ExcerptReader: KptReaderBase
    {
        public XmlDocument doc = new XmlDocument();
        public XmlNamespaceManager nsmgr;

        public override string KptVersionNumber => "10Excerpt";

        public Kpt10ExcerptReader(XmlDocument doc)
        { 
            Assert.IsNotNull(doc);
            this.doc = doc;
            
            nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("root", "urn://x-artefacts-rosreestr-ru/outgoing/kvzu/7.0.1");
            nsmgr.AddNamespace("adrs", "urn://x-artefacts-rosreestr-ru/commons/complex-types/address-output/4.0.1");
            nsmgr.AddNamespace("spa", "urn://x-artefacts-rosreestr-ru/commons/complex-types/entity-spatial/5.0.1");
            nsmgr.AddNamespace("ns3", "urn://x-artefacts-rosreestr-ru/commons/complex-types/entity-spatial/5.0.1");
            
        }
 
        public override List<IParcel> GetAllParcels()
        {
            List<IParcel> list = new List<IParcel>();
            
            XmlNode root   = doc.DocumentElement;
            XmlNode parcel = root.SelectSingleNode("root:Parcels/root:Parcel", nsmgr);

            var p       = parcel as XmlElement;
            var fParcel = new Kpt10ExcerptParcelReader(p, nsmgr);
            list.Add(fParcel);
            return list;
        }

    }
}