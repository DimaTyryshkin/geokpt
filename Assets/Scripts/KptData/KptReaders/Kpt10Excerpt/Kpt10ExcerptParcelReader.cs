using System.Collections.Generic;
using System.Xml;
using Geo.KptData.KptReaders;
using Geo.KptData.KptReaders.Kpt10;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReader.Kpt10Excerpt
{
    public class Kpt10ExcerptParcelReader : Kpt10ParcelReader
    {
        protected override string ReadableAddressXPath => "root:Location/root:Address/adrs:Note";

        public Kpt10ExcerptParcelReader(XmlElement node, XmlNamespaceManager nsmgr) : base(node, nsmgr)
        {
        }
    }
}